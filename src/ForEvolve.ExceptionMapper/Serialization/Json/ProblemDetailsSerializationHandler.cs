using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForEvolve.ExceptionMapper.Serialization.Json;

public class ProblemDetailsSerializationHandler : IExceptionSerializer
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ProblemDetailsSerializationOptions _options;
#if NET7_0_OR_GREATER
    private readonly IProblemDetailsService _problemDetailsService;
#endif
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ProblemDetailsSerializationHandler(
#if NET7_0_OR_GREATER
        IProblemDetailsService problemDetailsService,
#endif
        ProblemDetailsFactory problemDetailsFactory,
        IHostEnvironment hostEnvironment,
        ProblemDetailsSerializationOptions options,
        IOptions<JsonOptions> jsonOptions)
    {
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        _options = options ?? throw new ArgumentNullException(nameof(options));
#if NET7_0_OR_GREATER
        _problemDetailsService = problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));
#endif
        _jsonSerializerOptions = jsonOptions.Value.SerializerOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
    }

    public async Task ExecuteAsync(ExceptionHandlingContext ctx)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            ctx.HttpContext,
            title: ctx.Error.Message,
            statusCode: ctx.HttpContext.Response.StatusCode
        );

        // Add debug info
        var displayDebugInformation = _options.DisplayDebugInformation?.Invoke() ?? false;
        if (displayDebugInformation || _hostEnvironment.IsDevelopment())
        {
            var errorType = ctx.Error.GetType();
            problemDetails.Extensions.Add(
                FormatName("debug"),
                new
                {
                    type = new
                    {
                        name = errorType.Name,
                        fullName = errorType.FullName,
                    },
                    stackTrace = ctx.Error.StackTrace,
                }
            );
        }

        // Remove the default "traceId" property and
        // add it back with a key that is in line with the JSON serializer options.
        var traceId = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier;
        if (traceId != null)
        {
            var traceIdKey = "traceId";
            problemDetails.Extensions.Remove(traceIdKey);
            problemDetails.Extensions.Add(FormatName(traceIdKey),  traceId);
        }

        // Transfer non-excluded and non-JsonIgnored properties to the problem details.
        var properties = TypeDescriptor.GetProperties(ctx.Error);
        var propertiesToExclude = new string[] {
            nameof(Exception.StackTrace),
            nameof(Exception.Data),
            nameof(Exception.HResult),
            nameof(Exception.TargetSite),
            nameof(Exception.Message),
            nameof(Exception.Source),
            nameof(Exception.InnerException),
        };
        foreach (PropertyDescriptor property in properties)
        {
            if (propertiesToExclude.Contains(property.Name))
            {
                continue;
            }
            if (property.Attributes.OfType<JsonIgnoreAttribute>().Any())
            {
                continue;
            }

            var value = property.GetValue(ctx.Error);
            if (value != null)
            {
                problemDetails.Extensions.Add(FormatName(property.Name), value);
            }
        }

        // Output the problem details
#if NET7_0_OR_GREATER
        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = ctx.HttpContext,
#if NET8_0_OR_GREATER
            Exception = ctx.Error,
#endif
            ProblemDetails = problemDetails,
        };
        await _problemDetailsService.WriteAsync(problemDetailsContext);
#else
#pragma warning disable CS0618 // Type or member is obsolete
        ctx.HttpContext.Response.ContentType = _options.ContentType;
        await JsonSerializer.SerializeAsync(
            ctx.HttpContext.Response.Body,
            problemDetails,
            _jsonSerializerOptions,
            cancellationToken: ctx.HttpContext.RequestAborted
        );
#pragma warning restore CS0618 // Type or member is obsolete
#endif
    }

    private string FormatName(string name)
    {
        return _jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(name) ?? FormatToCamelCase(name);

        static string FormatToCamelCase(string name)
            => string.Concat(name[0].ToString().ToLower(), name.AsSpan(1));
    }
}

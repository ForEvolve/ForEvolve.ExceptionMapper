using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ForEvolve.ExceptionMapper.Serialization.Json
{
    public class ProblemDetailsSerializationHandler : IExceptionHandler
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ProblemDetailsSerializationOptions _options;

        public ProblemDetailsSerializationHandler(ProblemDetailsFactory problemDetailsFactory, IHostEnvironment hostEnvironment, ProblemDetailsSerializationOptions options)
        {
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public int Order => HandlerOrder.SerializerOrder;

        public async Task ExecuteAsync(ExceptionHandlingContext ctx)
        {
            var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                ctx.HttpContext,
                title: ctx.Error.Message,
                statusCode: ctx.HttpContext.Response.StatusCode
            );

            var displayDebugInformation = _options.DisplayDebugInformation?.Invoke() ?? false;
            if (displayDebugInformation || _hostEnvironment.IsDevelopment())
            {
                var errorType = ctx.Error.GetType();
                problemDetails.Extensions.Add(
                    "debug",
                    new {
                        type = new {
                            name = errorType.Name,
                            fullName = errorType.FullName,
                        },
                        stackTrace = ctx.Error.StackTrace,
                    }
                );
            }

            ctx.HttpContext.Response.ContentType = _options.ContentType;
            if(_options.JsonSerializerOptions is null)
            {
                await JsonSerializer.SerializeAsync(
                    ctx.HttpContext.Response.Body,
                    problemDetails
                );
            }
            else
            {
                await JsonSerializer.SerializeAsync(
                    ctx.HttpContext.Response.Body,
                    problemDetails,
                    _options.JsonSerializerOptions
                );
            }
        }

        public Task<bool> KnowHowToHandleAsync(Exception exception)
        {
            return Task.FromResult(_options.SerializeExceptions);
        }
    }
}

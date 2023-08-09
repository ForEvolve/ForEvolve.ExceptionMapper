using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace ForEvolve.ExceptionMapper.Serialization.Json
{
    public class ProblemDetailsSerializationHandler : IExceptionHandler
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ProblemDetailsSerializationOptions _options;
#if NET7_0_OR_GREATER
        private readonly IProblemDetailsService _problemDetailsService;
#endif

        public ProblemDetailsSerializationHandler(
#if NET7_0_OR_GREATER
            IProblemDetailsService problemDetailsService,
#endif
            ProblemDetailsFactory problemDetailsFactory,
            IHostEnvironment hostEnvironment,
            ProblemDetailsSerializationOptions options)
        {
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _options = options ?? throw new ArgumentNullException(nameof(options));
#if NET7_0_OR_GREATER
            _problemDetailsService = problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));
#endif
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
            ctx.HttpContext.Response.ContentType = _options.ContentType;
            if (_options.JsonSerializerOptions is null)
            {
                await JsonSerializer.SerializeAsync(
                    ctx.HttpContext.Response.Body,
                    problemDetails,
                    cancellationToken: ctx.HttpContext.RequestAborted
                );
            }
            else
            {
                await JsonSerializer.SerializeAsync(
                    ctx.HttpContext.Response.Body,
                    problemDetails,
                    _options.JsonSerializerOptions,
                    cancellationToken: ctx.HttpContext.RequestAborted
                );
            }
#endif

        }

        public Task<bool> KnowHowToHandleAsync(Exception exception)
        {
            return Task.FromResult(_options.SerializeExceptions);
        }
    }
}

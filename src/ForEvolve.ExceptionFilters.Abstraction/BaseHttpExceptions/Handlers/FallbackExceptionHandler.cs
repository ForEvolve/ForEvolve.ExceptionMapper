using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters.Handlers
{
    public class FallbackExceptionHandler : IExceptionHandler
    {
        public const int FallbackOrder = 50;
        public int Order => FallbackOrder;

        private readonly ExceptionFiltersOptions _options;
        public FallbackExceptionHandler(IOptionsMonitor<ExceptionFiltersOptions> options)
        {
            _options = options.CurrentValue;
        }

        public Task<bool> KnowHowToHandleAsync(Exception exception)
        {
            if (_options.FallbackStrategy == FallbackStrategy.Handle)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task ExecuteAsync(ExceptionHandlingContext context)
        {
            if (_options.FallbackStrategy == FallbackStrategy.Handle)
            {
                if (!context.Result.ExceptionHandled)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Result = new ExceptionHandledResult(context.Error);
                }
            }
            return Task.CompletedTask;
        }

    }

}

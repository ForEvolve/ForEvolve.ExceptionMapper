using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
namespace ForEvolve.ExceptionFilters
{
    public class HttpExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionHandlingManager _exceptionHandlingManager;

        public HttpExceptionHandlingMiddleware(IExceptionHandlingManager exceptionHandlingManager, RequestDelegate next)
        {
            _exceptionHandlingManager = exceptionHandlingManager ?? throw new ArgumentNullException(nameof(exceptionHandlingManager));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var result = await _exceptionHandlingManager.HandleAsync(context);
            if (result.ExceptionHandled)
            {
                return;
            }
            await _next(context);
        }
    }
}
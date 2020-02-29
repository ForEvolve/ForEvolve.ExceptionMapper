using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public class HttpExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionMapManager _exceptionMapManager;

        public HttpExceptionHandlingMiddleware(IExceptionMapManager exceptionMapManager, RequestDelegate next)
        {
            _exceptionMapManager = exceptionMapManager ?? throw new ArgumentNullException(nameof(exceptionMapManager));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerPathFeature?.Error != null) {
                var exceptionType = exceptionHandlerPathFeature.Error.GetType();
                if (await _exceptionMapManager.HasMapForExceptionAsync(exceptionType))
                {
                    var handler = await _exceptionMapManager.GetMapForExceptionAsync(exceptionType);
                    await handler.ExecuteAsync(context);
                }
                return;
            }
            await _next(context);
        }
    }
}

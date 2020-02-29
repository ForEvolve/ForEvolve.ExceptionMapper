using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
namespace ForEvolve.ExceptionFilters
{
    public class ExceptionHandlingManager : IExceptionHandlingManager
    {
        private readonly List<IExceptionHandler> _handlers;

        public ExceptionHandlingManager(IEnumerable<IExceptionHandler> handlers)
        {
            if (handlers == null) { throw new ArgumentNullException(nameof(handlers)); }

            _handlers = handlers
                .Reverse()
                .OrderBy(x => x.Order)
                .ToList();
        }

        public IReadOnlyCollection<IExceptionHandler> Handlers
            => new ReadOnlyCollection<IExceptionHandler>(_handlers);

        public async Task<ExceptionHandlingResult> HandleAsync(HttpContext context)
        {
            var exceptionHandled = false;
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                var exception = exceptionHandlerPathFeature.Error;
                foreach (var handler in _handlers)
                {
                    if (await handler.KnowHowToHandleAsync(exception))
                    {
                        exceptionHandled = true;
                        await handler.ExecuteAsync(context, exception);
                    }
                }
            }
            return new ExceptionHandlingResult(exceptionHandled);
        }
    }
}
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionMappingFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IExceptionMapManager _exceptionMapManager;
        private readonly IExceptionContextResultBuilder _exceptionContextResultBuilder;

        public ExceptionMappingFilterAttribute(IExceptionMapManager exceptionMapManager, IExceptionContextResultBuilder exceptionContextResultBuilder)
        {
            _exceptionMapManager = exceptionMapManager ?? throw new ArgumentNullException(nameof(exceptionMapManager));
            _exceptionContextResultBuilder = exceptionContextResultBuilder ?? throw new ArgumentNullException(nameof(exceptionContextResultBuilder));
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            if (await _exceptionMapManager.HasMapForExceptionAsync(exceptionType))
            {
                var handler = await _exceptionMapManager.GetMapForExceptionAsync(exceptionType);
                context.ExceptionHandled = true;
                context.Result = _exceptionContextResultBuilder.Create(context);
                await handler.ExecuteAsync(context.HttpContext);
                return;
            }
            await base.OnExceptionAsync(context);
        }
    }
}

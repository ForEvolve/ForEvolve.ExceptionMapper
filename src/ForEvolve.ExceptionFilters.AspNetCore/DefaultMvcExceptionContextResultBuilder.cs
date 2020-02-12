using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ForEvolve.ExceptionFilters
{
    public class DefaultMvcExceptionContextResultBuilder : IExceptionContextResultBuilder
    {
        private readonly IExceptionConverter<ProblemDetails> _exceptionConverter;

        public DefaultMvcExceptionContextResultBuilder(IExceptionConverter<ProblemDetails> exceptionConverter)
        {
            _exceptionConverter = exceptionConverter ?? throw new ArgumentNullException(nameof(exceptionConverter));
        }

        public IActionResult Create(ExceptionContext context)
        {
            var value = _exceptionConverter.Convert(context.Exception);
            return new ObjectResult(value);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionContextResultBuilder
    {
        IActionResult Create(ExceptionContext context);
    }
}

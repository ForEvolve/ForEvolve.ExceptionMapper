using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace ForEvolve.ExceptionFilters
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder WithExceptionMapping(this IMvcBuilder mvcBuilder, Action<IExceptionMappingBuilder> exceptionMappingBuilder = null)
        {
            mvcBuilder.Services.AddExceptionMapping(exceptionMappingBuilder);
            mvcBuilder.Services.TryAddSingleton<IExceptionConverter<ProblemDetails>, ExceptionToProblemDetailsConverter>();
            mvcBuilder.Services.TryAddSingleton<IExceptionContextResultBuilder, DefaultMvcExceptionContextResultBuilder>();
            mvcBuilder.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<ExceptionMappingFilterAttribute>();
            });
            return mvcBuilder;
        }
    }
}

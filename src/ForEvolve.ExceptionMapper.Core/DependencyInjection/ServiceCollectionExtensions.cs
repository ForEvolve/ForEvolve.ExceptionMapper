using ForEvolve.ExceptionMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExceptionFiltersExtensions
    {
        public static IServiceCollection AddExceptionMapper(this IServiceCollection services, Action<IExceptionMappingBuilder> exceptionMappingBuilder = null)
        {
            services.AddLogging();
            exceptionMappingBuilder?.Invoke(new ServiceCollectionWrapper(services));
            services.TryAddSingleton<IExceptionHandlingManager, ExceptionHandlingManager>();
            return services;
        }
    }
}
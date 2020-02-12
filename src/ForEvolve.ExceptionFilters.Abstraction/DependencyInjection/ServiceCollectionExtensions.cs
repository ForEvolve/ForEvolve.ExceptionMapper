using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace ForEvolve.ExceptionFilters
{
    public static class ServiceCollectionExtensions
    {
        private readonly static IExceptionMapManager _mapManager = new ExceptionMapManager();
        private static IExceptionMappingBuilder _builder;

        public static IServiceCollection AddExceptionMapping(this IServiceCollection services, Action<IExceptionMappingBuilder> exceptionMappingBuilder = null)
        {
            services.AddLogging();
            if (_builder == null)
            {
                _builder = new ExceptionMappingBuilder(services, _mapManager);
            }
            services.TryAddSingleton(_builder);
            services.TryAddSingleton(_mapManager);
            exceptionMappingBuilder?.Invoke(_builder);
            return services;
        }
    }
}

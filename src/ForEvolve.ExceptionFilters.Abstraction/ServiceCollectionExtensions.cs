using ForEvolve.ExceptionFilters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExceptionFiltersExtensions
    {
        public static IServiceCollection AddExceptionMapping(this IServiceCollection services, Action<IExceptionMappingBuilder> exceptionMappingBuilder = null)
        {
            services.AddLogging();
            exceptionMappingBuilder?.Invoke(new ServiceCollectionWrapper(services));
            services.TryAddSingleton<IExceptionHandlingManager, ExceptionHandlingManager>();
            return services;
        }
    }

    public interface IExceptionMappingBuilder
    {
        IServiceCollection Services { get; }
    }

    public class ServiceCollectionWrapper : IExceptionMappingBuilder
    {
        public ServiceCollectionWrapper(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }

    public static class ExceptionMappingBuilderExtensions
    {
        public static IExceptionMappingBuilder AddExceptionHandler<THandler>(this IExceptionMappingBuilder builder)
            where THandler : class, IExceptionHandler
        {
            builder.Services.AddSingleton<IExceptionHandler, THandler>();
            return builder;
        }
    }
}
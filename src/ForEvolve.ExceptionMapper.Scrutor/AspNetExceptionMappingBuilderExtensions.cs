using ForEvolve.ExceptionMapper;
using Scrutor;

namespace Microsoft.Extensions.DependencyInjection;

public static class AspNetExceptionMappingBuilderExtensions
{
    /// <typeparam name="T">The type in which assembly that should be scanned.</typeparam>
    public static IExceptionMappingBuilder ScanHandlersFromAssemblyOf<T>(this IExceptionMappingBuilder builder, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        return builder.ScanHandlersFrom(s => s.FromAssemblyOf<T>(), lifetime);
    }

    public static IExceptionMappingBuilder ScanHandlersFrom(this IExceptionMappingBuilder builder, Func<ITypeSourceSelector, IImplementationTypeSelector> typeSelector, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        if (typeSelector == null) { throw new ArgumentNullException(nameof(typeSelector)); }

        builder.Services.Scan(s => typeSelector(s)
            .AddClasses(f => f.AssignableTo<IExceptionHandler>())
            .As<IExceptionHandler>()
            .WithLifetime(lifetime)
        );
        return builder;
    }
}

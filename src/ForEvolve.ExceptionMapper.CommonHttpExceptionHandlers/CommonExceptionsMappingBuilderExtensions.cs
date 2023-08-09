using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.Handlers;
using ForEvolve.ExceptionMapper.Handlers.Fallback;

namespace Microsoft.Extensions.DependencyInjection;

public static class CommonHttpExceptionHandlersMappingBuilderExtensions
{
    /// <summary>
    /// Registers all <see cref="IExceptionHandler"/> found in the assembly <see cref="ForEvolve.ExceptionMapper"/>
    /// with singleton lifetime.
    /// </summary>
    public static IExceptionMappingBuilder MapCommonHttpExceptions(this IExceptionMappingBuilder builder)
    {
        return builder.ScanHandlersFrom(
            s => s
                .FromAssembliesOf(typeof(CommonHttpExceptionHandlersMappingBuilderExtensions))
                .AddClasses(x => x.InExactNamespaceOf<ConflictExceptionHandler>()),
            ServiceLifetime.Singleton
        );
    }

    /// <summary>
    /// Registers all <see cref="IExceptionHandler"/> found in the assembly <see cref="ForEvolve.ExceptionMapper"/>
    /// with singleton lifetime.
    /// </summary>
    public static IExceptionMappingBuilder MapHttpFallback(this IExceptionMappingBuilder builder, Action<FallbackExceptionHandlerOptions>? setup = null)
    {
        if (setup != null) { builder.Services.Configure(setup); }
        return builder.AddExceptionHandler<FallbackExceptionHandler>();
    }
}

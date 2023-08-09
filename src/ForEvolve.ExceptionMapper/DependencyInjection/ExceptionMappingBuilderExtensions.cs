using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.Handlers;
using ForEvolve.ExceptionMapper.Handlers.Fallback;

namespace Microsoft.Extensions.DependencyInjection;

public static class ExceptionMappingBuilderExtensions
{
    public static IExceptionMappingBuilder AddExceptionHandler<THandler>(this IExceptionMappingBuilder builder)
        where THandler : class, IExceptionHandler
    {
        builder.Services.AddSingleton<IExceptionHandler, THandler>();
        return builder;
    }

    public static IExceptionMappingBuilder AddExceptionHandler<THandler>(this IExceptionMappingBuilder builder, THandler handler)
        where THandler : class, IExceptionHandler
    {
        builder.Services.AddSingleton<IExceptionHandler>(handler);
        return builder;
    }

    /// <summary>
    /// Registers all <see cref="IExceptionHandler"/> found in the assembly <see cref="ForEvolve.ExceptionMapper"/>
    /// with singleton lifetime.
    /// </summary>
    public static IExceptionMappingBuilder MapCommonHttpExceptions(this IExceptionMappingBuilder builder)
    {
        return builder
            .AddExceptionHandler<BadRequestExceptionHandler>()
            .AddExceptionHandler<ConflictExceptionHandler>()
            .AddExceptionHandler<ForbiddenExceptionHandler>()
            .AddExceptionHandler<InternalServerErrorExceptionHandler>()
            .AddExceptionHandler<NotFoundExceptionHandler>()
            .AddExceptionHandler<NotImplementedExceptionHandler>()
            .AddExceptionHandler<UnauthorizedExceptionHandler>()
        ;
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
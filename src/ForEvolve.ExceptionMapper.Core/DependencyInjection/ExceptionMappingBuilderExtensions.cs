using ForEvolve.ExceptionMapper;

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
}
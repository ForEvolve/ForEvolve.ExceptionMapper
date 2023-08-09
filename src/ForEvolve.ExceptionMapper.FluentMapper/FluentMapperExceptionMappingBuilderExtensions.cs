using ForEvolve.ExceptionMapper.FluentMapper;

namespace Microsoft.Extensions.DependencyInjection;

public static class FluentMapperExceptionMappingBuilderExtensions
{
    public static IExceptionMappingBuilder Map<TException>(this IExceptionMappingBuilder builder, Action<FluentMapperBuilder<TException>> mapperAction)
       where TException : Exception
    {
        if (mapperAction == null) { throw new ArgumentNullException(nameof(mapperAction)); }

        var handler = new FluentExceptionHandler<TException>();
        var fluentMapper = new FluentMapperBuilder<TException>(builder, handler);
        builder.AddExceptionHandler(handler);
        mapperAction.Invoke(fluentMapper);
        return builder;
    }
}

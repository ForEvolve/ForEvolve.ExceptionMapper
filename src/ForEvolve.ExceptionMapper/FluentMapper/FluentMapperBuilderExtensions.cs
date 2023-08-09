using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.FluentMapper;

namespace Microsoft.Extensions.DependencyInjection;

public static class FluentMapperBuilderExtensions
{
    public static FluentMapperBuilder<TException> ToStatusCode<TException>(this FluentMapperBuilder<TException> fluentBuilder, int expectedStatusCode, FluentHandlerStrategy strategy = FluentHandlerStrategy.Replace)
        where TException : Exception
    {
        return fluentBuilder.To(
            context => StatusCodeHandler(context, expectedStatusCode),
            strategy
        );

        static Task StatusCodeHandler(ExceptionHandlingContext context, int statusCode)
        {
            context.HttpContext.Response.StatusCode = statusCode;
            return Task.CompletedTask;
        }
    }

    public static FluentMapperBuilder<TException> To<TException>(this FluentMapperBuilder<TException> fluentBuilder, Func<ExceptionHandlingContext, Task> handler, FluentHandlerStrategy strategy = FluentHandlerStrategy.Replace)
        where TException : Exception
    {
        switch (strategy)
        {
            case FluentHandlerStrategy.Append:
                fluentBuilder.Handler.AppendHandler(handler);
                break;
            case FluentHandlerStrategy.Prepend:
                fluentBuilder.Handler.PrependHandler(handler);
                break;
            case FluentHandlerStrategy.Replace:
                fluentBuilder.Handler.ReplaceHandler(handler);
                break;
            default:
                throw new NotSupportedException($"The strategy '{strategy}' is not supported.");
        }
        return fluentBuilder;
    }
}

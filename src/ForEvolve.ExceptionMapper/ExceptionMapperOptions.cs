using ForEvolve.ExceptionMapper;

namespace Microsoft.Extensions.DependencyInjection;

public class ExceptionMapperOptions
{
    public ExceptionMapperOptions(ExceptionHandlerCollection handlers, IExceptionSerializer serializer)
    {
        Handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public ExceptionHandlerCollection Handlers { get; }
    public IExceptionSerializer Serializer { get; }
}

using ForEvolve.ExceptionMapper;

namespace Microsoft.Extensions.DependencyInjection;

public interface IExceptionMappingBuilder
{
    IServiceCollection Services { get; }
    //IList<IExceptionHandler> Handlers { get; }
}

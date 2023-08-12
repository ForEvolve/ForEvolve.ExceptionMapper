using ForEvolve.ExceptionMapper;

namespace Microsoft.Extensions.DependencyInjection;

public class ExceptionMappingBuilder : IExceptionMappingBuilder
{
    public ExceptionMappingBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }
    public IList<IExceptionHandler> Handlers { get; } = new List<IExceptionHandler>();
}
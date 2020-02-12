using Microsoft.Extensions.DependencyInjection;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionMappingBuilder
    {
        IServiceCollection Services { get; }
        IExceptionMapManager Maps { get; }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionMappingBuilder : IExceptionMappingBuilder
    {
        public ExceptionMappingBuilder(IServiceCollection services, IExceptionMapManager mapManager)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Maps = mapManager ?? throw new ArgumentNullException(nameof(mapManager));
        }

        public IServiceCollection Services { get; }
        public IExceptionMapManager Maps { get; }
    }
}

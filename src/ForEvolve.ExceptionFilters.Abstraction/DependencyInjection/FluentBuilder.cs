using Microsoft.Extensions.DependencyInjection;
using System;

namespace ForEvolve.ExceptionFilters
{
    public class FluentBuilder<TException> : IExceptionMappingBuilder
        where TException : Exception
    {
        public FluentBuilder(IExceptionMappingBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public IExceptionMappingBuilder Builder { get; }

        public IServiceCollection Services => Builder.Services;

        public IExceptionMapManager Maps => Builder.Maps;
    }
}

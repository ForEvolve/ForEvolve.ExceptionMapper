using Microsoft.Extensions.DependencyInjection;

namespace ForEvolve.ExceptionMapper.FluentMapper
{
    public class FluentMapperBuilder<TException>
        where TException : Exception
    {
        public FluentMapperBuilder(IExceptionMappingBuilder builder, FluentExceptionHandler<TException> handler)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public IExceptionMappingBuilder Builder { get; }

        public IServiceCollection Services => Builder.Services;

        public FluentExceptionHandler<TException> Handler { get; }
    }
}

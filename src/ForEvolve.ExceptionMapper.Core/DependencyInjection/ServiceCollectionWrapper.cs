namespace Microsoft.Extensions.DependencyInjection
{
    public class ServiceCollectionWrapper : IExceptionMappingBuilder
    {
        public ServiceCollectionWrapper(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
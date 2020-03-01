namespace Microsoft.Extensions.DependencyInjection
{
    public interface IExceptionMappingBuilder
    {
        IServiceCollection Services { get; }
    }
}
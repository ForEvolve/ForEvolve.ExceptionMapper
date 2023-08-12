using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace ForEvolve.ExceptionMapper;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void Should_register_all_dependencies()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var hostEnvironmentMock = new Mock<IHostEnvironment>();
        var services = new ServiceCollection();
        services.AddSingleton(hostEnvironmentMock.Object);
        services.AddExceptionMapper(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var manager = serviceProvider
            .GetRequiredService<IExceptionHandlingManager>();

        // Assert
        Assert.NotNull(manager);
    }
}

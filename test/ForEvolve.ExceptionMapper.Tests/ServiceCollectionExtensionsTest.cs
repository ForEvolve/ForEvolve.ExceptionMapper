using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionMapper
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void Should_register_all_dependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddExceptionMapper();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var manager = serviceProvider
                .GetRequiredService<IExceptionHandlingManager>();

            // Assert
            Assert.NotNull(manager);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionHandledResultTest
    {
        [Fact]
        public void Should_set_the_expected_values()
        {
            // Arrange
            var exception = new Exception();

            // Act
            var result = new ExceptionHandledResult(exception);

            // Assert
            Assert.Equal(exception, result.Error);
            Assert.True(result.ExceptionHandled);
            Assert.True(result.ExceptionHandlerFeatureSupported);
        }

        [Fact]
        public void Should_guard_against_null()
        {
            Assert.Throws<ArgumentNullException>(
                "error",
                () => new ExceptionHandledResult(default)
            );
        }
    }
}

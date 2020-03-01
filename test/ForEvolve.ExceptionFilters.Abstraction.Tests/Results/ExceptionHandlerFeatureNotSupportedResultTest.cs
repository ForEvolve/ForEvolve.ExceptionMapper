using Xunit;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionHandlerFeatureNotSupportedResultTest
    {
        [Fact]
        public void Should_set_the_expected_values()
        {
            // Act
            var result = new ExceptionHandlerFeatureNotSupportedResult();

            // Assert
            Assert.Null(result.Error);
            Assert.False(result.ExceptionHandled);
            Assert.False(result.ExceptionHandlerFeatureSupported);
        }
    }
}

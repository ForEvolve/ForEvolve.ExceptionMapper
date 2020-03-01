using Xunit;

namespace ForEvolve.ExceptionFilters
{
    public class NoExceptionResultTest
    {
        [Fact]
        public void Should_set_the_expected_values()
        {
            // Act
            var result = new NoExceptionResult();

            // Assert
            Assert.Null(result.Error);
            Assert.False(result.ExceptionHandled);
            Assert.True(result.ExceptionHandlerFeatureSupported);
        }
    }
}

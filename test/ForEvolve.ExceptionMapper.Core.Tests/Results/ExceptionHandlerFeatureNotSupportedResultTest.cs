using Xunit;

namespace ForEvolve.ExceptionMapper;

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
    }
}

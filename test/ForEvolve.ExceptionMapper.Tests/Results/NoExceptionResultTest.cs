using Xunit;

namespace ForEvolve.ExceptionMapper;

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
    }
}

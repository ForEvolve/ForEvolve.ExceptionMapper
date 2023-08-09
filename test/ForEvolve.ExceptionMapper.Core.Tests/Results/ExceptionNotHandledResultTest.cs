using System;
using Xunit;

namespace ForEvolve.ExceptionMapper;

public class ExceptionNotHandledResultTest
{
    [Fact]
    public void Should_set_the_expected_values()
    {
        // Arrange
        var exception = new Exception();

        // Act
        var result = new ExceptionNotHandledResult(exception);

        // Assert
        Assert.Equal(exception, result.Error);
        Assert.False(result.ExceptionHandled);
        Assert.True(result.ExceptionHandlerFeatureSupported);
    }

    [Fact]
    public void Should_guard_against_null()
    {
        Assert.Throws<ArgumentNullException>(
            "error",
            () => new ExceptionNotHandledResult(default)
        );
    }
}

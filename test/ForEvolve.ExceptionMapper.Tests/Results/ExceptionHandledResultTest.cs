using System;
using Xunit;

namespace ForEvolve.ExceptionMapper;

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

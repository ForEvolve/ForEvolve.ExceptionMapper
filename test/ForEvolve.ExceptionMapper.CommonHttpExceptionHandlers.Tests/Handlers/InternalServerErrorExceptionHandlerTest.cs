using Microsoft.AspNetCore.Http;
using Xunit;

namespace ForEvolve.ExceptionMapper.Handlers;

public class InternalServerErrorExceptionHandlerTest
{
    [Fact]
    public void StatusCode_should_equal_500()
    {
        var sut = new InternalServerErrorExceptionHandler();
        Assert.Equal(StatusCodes.Status500InternalServerError, sut.StatusCode);
    }
}

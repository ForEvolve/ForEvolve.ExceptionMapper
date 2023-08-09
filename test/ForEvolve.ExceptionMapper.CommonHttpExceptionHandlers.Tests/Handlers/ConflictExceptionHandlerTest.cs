using Microsoft.AspNetCore.Http;
using Xunit;

namespace ForEvolve.ExceptionMapper.Handlers;

public class ConflictExceptionHandlerTest
{
    [Fact]
    public void StatusCode_should_equal_409()
    {
        var sut = new ConflictExceptionHandler();
        Assert.Equal(StatusCodes.Status409Conflict, sut.StatusCode);
    }
}

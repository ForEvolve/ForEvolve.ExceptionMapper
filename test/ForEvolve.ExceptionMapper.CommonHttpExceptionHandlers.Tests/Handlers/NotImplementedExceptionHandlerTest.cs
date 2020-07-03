using Microsoft.AspNetCore.Http;
using Xunit;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class NotImplementedExceptionHandlerTest
    {
        [Fact]
        public void StatusCode_should_equal_501()
        {
            var sut = new NotImplementedExceptionHandler();
            Assert.Equal(StatusCodes.Status501NotImplemented, sut.StatusCode);
        }
    }
}

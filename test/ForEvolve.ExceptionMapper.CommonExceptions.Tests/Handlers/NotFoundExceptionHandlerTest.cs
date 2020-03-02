using Microsoft.AspNetCore.Http;
using Xunit;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class NotFoundExceptionHandlerTest
    {
        [Fact]
        public void StatusCode_should_equal_404()
        {
            var sut = new NotFoundExceptionHandler();
            Assert.Equal(StatusCodes.Status404NotFound, sut.StatusCode);
        }
    }
}

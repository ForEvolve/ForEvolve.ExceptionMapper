using ForEvolve.Testing.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionFilters
{
    public class HttpExceptionHandlingMiddlewareTest
    {
        private readonly HttpContextHelper _httpContextHelper = new HttpContextHelper();
        private HttpContext HttpContext => _httpContextHelper.HttpContextMock.Object;
        private bool _nextWasCalled = false;
        private readonly HttpExceptionHandlingMiddleware sut;
        private readonly Mock<IExceptionHandlingManager> _exceptionHandlingManagerMock;

        public HttpExceptionHandlingMiddlewareTest()
        {
            _exceptionHandlingManagerMock = new Mock<IExceptionHandlingManager>();
            sut = new HttpExceptionHandlingMiddleware(
                _exceptionHandlingManagerMock.Object,
                Next
            );
        }

        [Fact]
        public async Task Should_not_call_next_when_exception_is_handled()
        {
            // Arrange
            var resultMock = new Mock<IExceptionHandlingResult>();
            resultMock.Setup(x => x.ExceptionHandled).Returns(true);
            _exceptionHandlingManagerMock
                .Setup(x => x.HandleAsync(It.IsAny<HttpContext>()))
                .ReturnsAsync(resultMock.Object)
            ;

            // Act
            await sut.InvokeAsync(HttpContext);

            // Assert
            Assert.False(_nextWasCalled);
        }

        [Fact]
        public async Task Should_call_next_when_exception_is_not_handled()
        {
            // Arrange
            var resultMock = new Mock<IExceptionHandlingResult>();
            resultMock.Setup(x => x.ExceptionHandled).Returns(false);
            _exceptionHandlingManagerMock
                .Setup(x => x.HandleAsync(It.IsAny<HttpContext>()))
                .ReturnsAsync(resultMock.Object)
            ;

            // Act
            await sut.InvokeAsync(HttpContext);

            // Assert
            Assert.True(_nextWasCalled);
        }

        private Task Next(HttpContext context)
        {
            _nextWasCalled = true;
            return Task.CompletedTask;
        }
    }
}

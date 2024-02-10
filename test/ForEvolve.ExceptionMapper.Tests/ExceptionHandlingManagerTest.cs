using ForEvolve.Testing.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionMapper;

public class ExceptionHandlingManagerTest
{
    public class HandleAsync : ExceptionHandlingManagerTest
    {
        private readonly HttpContextHelper _httpContextHelper = new HttpContextHelper();
        private HttpContext HttpContext => _httpContextHelper.HttpContextMock.Object;
        private readonly List<IExceptionHandler> _handlers = new List<IExceptionHandler>();
        private readonly Mock<IExceptionSerializer> _serializer = new Mock<IExceptionSerializer>();
        private ExceptionMapperOptions Options => new(new ExceptionHandlerCollection(_handlers), _serializer.Object);

        public class When_IExceptionHandlerFeature_is_null : HandleAsync
        {
            [Fact]
            public async Task Should_return_ExceptionHandlerFeatureNotSupportedResult()
            {
                // Arrange
                var sut = new ExceptionHandlingManager(Options);
                _httpContextHelper.FeaturesMock
                    .Setup(x => x.Get<IExceptionHandlerFeature>())
                    .Returns(default(IExceptionHandlerFeature));

                // Act
                var result = await sut.HandleAsync(HttpContext);

                // Assert
                Assert.IsType<ExceptionHandlerFeatureNotSupportedResult>(result);
            }
        }

        public class When_IExceptionHandlerFeature_is_not_null : HandleAsync
        {
            private readonly Mock<IExceptionHandlerFeature> _exceptionHandlerFeatureMock;

            public When_IExceptionHandlerFeature_is_not_null()
            {
                _exceptionHandlerFeatureMock = new Mock<IExceptionHandlerFeature>();
                _httpContextHelper.FeaturesMock
                    .Setup(x => x.Get<IExceptionHandlerFeature>())
                    .Returns(_exceptionHandlerFeatureMock.Object);
            }

            public class And_has_no_Error : When_IExceptionHandlerFeature_is_not_null
            {
                [Fact]
                public async Task Should_return_NoExceptionResult()
                {
                    // Arrange
                    _exceptionHandlerFeatureMock
                        .Setup(x => x.Error)
                        .Returns(default(Exception));
                    var sut = new ExceptionHandlingManager(Options);

                    // Act
                    var result = await sut.HandleAsync(HttpContext);

                    // Assert
                    Assert.IsType<NoExceptionResult>(result);
                }
            }

            public class And_has_an_Error : When_IExceptionHandlerFeature_is_not_null
            {
                private readonly Exception _exception;
                public And_has_an_Error()
                {
                    _exception = new Exception();
                    _exceptionHandlerFeatureMock
                        .Setup(x => x.Error)
                        .Returns(_exception);
                }

                public class And_the_Exception_was_handled : And_has_an_Error
                {
                    [Fact]
                    public async Task Should_return_ExceptionHandlingContext_Result()
                    {
                        // Arrange
                        var handlerMock = new Mock<IExceptionHandler>();
                        handlerMock
                            .Setup(x => x.CanHandle(_exception))
                            .ReturnsAsync(true);
                        handlerMock
                            .Setup(x => x.ExecuteAsync(It.IsAny<ExceptionHandlingContext>()))
                            .Callback((ExceptionHandlingContext context) => context.Result = new TestResult())
                            .Returns(Task.CompletedTask);
                        _handlers.Add(handlerMock.Object);
                        var sut = new ExceptionHandlingManager(Options);

                        // Act
                        var result = await sut.HandleAsync(HttpContext);

                        // Assert
                        Assert.IsType<TestResult>(result);
                    }

                    private class TestResult : IExceptionHandlingResult
                    {
                        public bool ExceptionHandled => throw new NotImplementedException();

                        public Exception Error => throw new NotImplementedException();
                    }
                }

                public class And_the_Exception_was_not_handled_by_any_handler : And_has_an_Error
                {
                    [Fact]
                    public async Task Should_return_ExceptionNotHandledResult()
                    {
                        // Arrange
                        var sut = new ExceptionHandlingManager(Options);

                        // Act
                        var result = await sut.HandleAsync(HttpContext);

                        // Assert
                        Assert.IsType<ExceptionNotHandledResult>(result);
                    }
                }
            }
        }
    }
}

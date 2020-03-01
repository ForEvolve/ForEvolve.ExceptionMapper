using ForEvolve.Testing.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionHandlingManagerTest
    {
        public class HandleAsync : ExceptionHandlingManagerTest
        {
            private readonly HttpContextHelper _httpContextHelper = new HttpContextHelper();
            private HttpContext HttpContext => _httpContextHelper.HttpContextMock.Object;
            private readonly List<IExceptionHandler> _handlers = new List<IExceptionHandler>();

            public class When_IExceptionHandlerFeature_is_null : HandleAsync
            {
                [Fact]
                public async Task Should_return_ExceptionHandlerFeatureNotSupportedResult()
                {
                    // Arrange
                    var sut = new ExceptionHandlingManager(_handlers);
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
                        var sut = new ExceptionHandlingManager(_handlers);

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
                        public async Task Should_return_ExceptionHandledResult()
                        {
                            // Arrange
                            var handlerMock = new Mock<IExceptionHandler>();
                            handlerMock
                                .Setup(x => x.KnowHowToHandleAsync(_exception))
                                .ReturnsAsync(true);
                            handlerMock
                                .Setup(x => x.ExecuteAsync(HttpContext, _exception))
                                .Returns(Task.CompletedTask);
                            _handlers.Add(handlerMock.Object);
                            var sut = new ExceptionHandlingManager(_handlers);

                            // Act
                            var result = await sut.HandleAsync(HttpContext);

                            // Assert
                            Assert.IsType<ExceptionHandledResult>(result);
                        }
                    }

                    public class And_the_Exception_was_not_handled_by_any_handler : And_has_an_Error
                    {
                        [Fact]
                        public async Task Should_return_ExceptionNotHandledResult()
                        {
                            // Arrange
                            var sut = new ExceptionHandlingManager(_handlers);

                            // Act
                            var result = await sut.HandleAsync(HttpContext);

                            // Assert
                            Assert.IsType<ExceptionNotHandledResult>(result);
                        }
                    }
                }
            }
        }

        public class Handlers : ExceptionHandlingManagerTest
        {
            private static readonly OrderableTestExceptionHandler _handlerOrder1 = new OrderableTestExceptionHandler { Order = 1 };
            private static readonly OrderableTestExceptionHandler _handlerOrder2 = new OrderableTestExceptionHandler { Order = 2 };
            private static readonly OrderableTestExceptionHandler _handlerOrder3 = new OrderableTestExceptionHandler { Order = 3 };

            private static readonly OrderableTestExceptionHandler _handlerOrder1Version1 = new OrderableTestExceptionHandler { Order = 1 };
            private static readonly OrderableTestExceptionHandler _handlerOrder1Version2 = new OrderableTestExceptionHandler { Order = 1 };
            private static readonly OrderableTestExceptionHandler _handlerOrder1Version3 = new OrderableTestExceptionHandler { Order = 1 };

            public static TheoryData<string, IEnumerable<OrderableTestExceptionHandler>, Action<IReadOnlyCollection<IExceptionHandler>>> OrderTestsData = new TheoryData<string, IEnumerable<OrderableTestExceptionHandler>, Action<IReadOnlyCollection<IExceptionHandler>>>
            {
                {
                    "Should sort handlers using their Order property",
                    new[] {
                        _handlerOrder2,
                        _handlerOrder1,
                        _handlerOrder3,
                    },
                    orderedHandlers => Assert.Collection(orderedHandlers,
                        handler => Assert.Same(_handlerOrder1, handler),
                        handler => Assert.Same(_handlerOrder2, handler),
                        handler => Assert.Same(_handlerOrder3, handler)
                    )
                },
                {
                    "Should sort handlers 'first in last out'",
                    new[] {
                        _handlerOrder1Version1,
                        _handlerOrder2,
                        _handlerOrder1,
                        _handlerOrder3,
                        _handlerOrder1Version2,
                        _handlerOrder1Version3
                    },
                    orderedHandlers => Assert.Collection(orderedHandlers,
                        handler => Assert.Same(_handlerOrder1Version3, handler),
                        handler => Assert.Same(_handlerOrder1Version2, handler),
                        handler => Assert.Same(_handlerOrder1, handler),
                        handler => Assert.Same(_handlerOrder1Version1, handler),
                        handler => Assert.Same(_handlerOrder2, handler),
                        handler => Assert.Same(_handlerOrder3, handler)
                    )
                }
            };

            [Theory]
            [MemberData(nameof(OrderTestsData))]
            public void Should_order_handlers(
                string errorMessage,
                IEnumerable<OrderableTestExceptionHandler> input,
                Action<IReadOnlyCollection<IExceptionHandler>> assert
            )
            {
                var sut = new ExceptionHandlingManager(input);
                var orderedHandlers = sut.Handlers;
                try
                {
                    assert(orderedHandlers);
                }
                catch (XunitException ex)
                {
                    throw new DescriptiveException(errorMessage, ex);
                }
            }

            public class OrderableTestExceptionHandler : IExceptionHandler
            {
                public int Order { get; set; }

                public Task ExecuteAsync(HttpContext httpContext, Exception exception)
                {
                    throw new NotImplementedException();
                }

                public Task<bool> KnowHowToHandleAsync(Exception exception)
                {
                    throw new NotImplementedException();
                }
            }

            public class DescriptiveException : XunitException
            {
                public DescriptiveException(string userMessage, XunitException innerException)
                    : base(userMessage, innerException)
                {
                }
            }
        }
    }
}

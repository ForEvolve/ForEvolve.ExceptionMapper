using ForEvolve.Testing.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
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

            [Fact]
            public void Should_be_tested()
            {
                throw new NotImplementedException();
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

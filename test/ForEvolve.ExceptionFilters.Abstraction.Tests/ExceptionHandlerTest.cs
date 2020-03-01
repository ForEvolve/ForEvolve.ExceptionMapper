using ForEvolve.Testing.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionHandlerTest
    {
        public class KnowHowToHandleAsync : ExceptionHandlerTest
        {
            public static TheoryData<TestException> TrueResults = new TheoryData<TestException>
            {
                new TestException(),
                new TestSubException()
            };

            public static TheoryData<Exception> FalseResults = new TheoryData<Exception>
            {
                new TestWrongException(),
                new Exception()
            };

            [Theory]
            [MemberData(nameof(TrueResults))]
            public async Task Should_return_true_when_the_exception_is_a_TException(TestException exception)
            {
                var sut = new ExceptionTestHandler<TestException>();
                var result = await sut.KnowHowToHandleAsync(exception);
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(FalseResults))]
            public async Task Should_return_false_when_the_exception_type_is_not_a_TException(Exception exception)
            {
                var sut = new ExceptionTestHandler<TestException>();
                var result = await sut.KnowHowToHandleAsync(exception);
                Assert.False(result);
            }

        }

        public class ExecuteAsync : ExceptionHandlerTest
        {
            private HttpContextHelper _httpContextHelper = new HttpContextHelper();

            [Fact]
            public async Task Should_set_response_StatusCode_to_handler_StatusCode_value()
            {
                var sut = new ExceptionTestHandler<TestException>();
                var exception = new TestException();

                await sut.ExecuteAsync(new ExceptionHandlingContext(
                    _httpContextHelper.HttpContextMock.Object,
                    exception,
                    default
                ));

                Assert.Equal(
                    sut.StatusCode,
                    _httpContextHelper.HttpResponse.StatusCode
                );
            }

            [Fact]
            public async Task Should_call_ExecuteCoreAsync()
            {
                var sut = new ExceptionTestHandler<TestException>();
                var exception = new TestException();

                await sut.ExecuteAsync(new ExceptionHandlingContext(
                    _httpContextHelper.HttpContextMock.Object,
                    exception,
                    default
                ));

                Assert.True(sut.HandleCoreWasCalled);
                Assert.Same(exception, sut.Exception);
                Assert.Same(
                    _httpContextHelper.HttpContextMock.Object,
                    sut.HttpContext
                );
            }
        }

        private class ExceptionTestHandler<TException> : ExceptionHandler<TException>
            where TException : Exception
        {
            public override int StatusCode => 999;

            protected override Task ExecuteCoreAsync(ExceptionHandlingContext<TException> context)
            {
                HandleCoreWasCalled = true;
                HttpContext = context.HttpContext;
                Exception = context.Error;
                return base.ExecuteCoreAsync(context);
            }

            public bool HandleCoreWasCalled { get; private set; }
            public TException Exception { get; private set; }
            public HttpContext HttpContext { get; private set; }
        }

        public class TestException : Exception { }
        public class TestSubException : TestException { }
        public class TestWrongException : Exception { }
    }
}

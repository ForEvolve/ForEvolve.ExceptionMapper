using ForEvolve.Testing.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class FallbackExceptionHandlerTest
    {
        private readonly FallbackExceptionHandlerOptions _options;
        private readonly Mock<IOptionsMonitor<FallbackExceptionHandlerOptions>> _optionsMonitorMock;
        private readonly FallbackExceptionHandler sut;

        public FallbackExceptionHandlerTest()
        {
            _options = new FallbackExceptionHandlerOptions();
            _optionsMonitorMock = new Mock<IOptionsMonitor<FallbackExceptionHandlerOptions>>();
            _optionsMonitorMock.Setup(x => x.CurrentValue).Returns(_options);
            sut = new FallbackExceptionHandler(_optionsMonitorMock.Object);
        }

        public class Order : FallbackExceptionHandlerTest
        {
            [Fact]
            public void Should_be_equal_to_FallbackOrder()
            {
                Assert.Equal(HandlerOrder.FallbackOrder, sut.Order);
            }
        }

        public class KnowHowToHandleAsync : FallbackExceptionHandlerTest
        {
            [Fact]
            public async Task Should_return_true_when_FallbackStrategy_equals_Handle()
            {
                _options.Strategy = FallbackStrategy.Handle;
                var result = await sut.KnowHowToHandleAsync(new Exception());
                Assert.True(result);
            }

            [Fact]
            public async Task Should_return_false_when_FallbackStrategy_equals_Ignore()
            {
                _options.Strategy = FallbackStrategy.Ignore;
                var result = await sut.KnowHowToHandleAsync(new Exception());
                Assert.False(result);
            }
        }

        public class ExecuteAsync : FallbackExceptionHandlerTest
        {
            private readonly Exception _error;
            private readonly ExceptionHandlingContext _context;
            private readonly HttpContextHelper _httpContextHelper;
            private readonly ExceptionNotHandledResult _initialResult;

            public ExecuteAsync()
            {
                _error = new Exception();
                _initialResult = new ExceptionNotHandledResult(_error);
                _httpContextHelper = new HttpContextHelper();
                _context = new ExceptionHandlingContext(_httpContextHelper.HttpContext, _error, _initialResult);
            }

            [Fact]
            public async Task Should_handle_the_exception_when_FallbackStrategy_equals_Handle()
            {
                _options.Strategy = FallbackStrategy.Handle;
                await sut.ExecuteAsync(_context);
                Assert.IsType<ExceptionHandledResult>(_context.Result);
            }

            [Fact]
            public async Task Should_do_nothing_when_FallbackStrategy_equals_Ignore()
            {
                _options.Strategy = FallbackStrategy.Ignore;
                await sut.ExecuteAsync(_context);
                Assert.Same(_initialResult, _context.Result);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class ConflictExceptionHandlerTest
    {
        [Fact]
        public void StatusCode_should_equal_409()
        {
            var sut = new ConflictExceptionHandler();
            Assert.Equal(StatusCodes.Status409Conflict, sut.StatusCode);
        }
    }
}

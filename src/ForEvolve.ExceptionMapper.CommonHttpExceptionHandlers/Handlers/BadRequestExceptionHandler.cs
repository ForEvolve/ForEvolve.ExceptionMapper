using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class BadRequestExceptionHandler : ExceptionHandler<BadRequestException>
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}

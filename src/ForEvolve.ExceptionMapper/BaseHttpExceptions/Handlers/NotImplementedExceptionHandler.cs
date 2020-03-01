using Microsoft.AspNetCore.Http;
using System;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class NotImplementedExceptionHandler : ExceptionHandler<NotImplementedException>
    {
        public override int StatusCode => StatusCodes.Status501NotImplemented;
    }
}

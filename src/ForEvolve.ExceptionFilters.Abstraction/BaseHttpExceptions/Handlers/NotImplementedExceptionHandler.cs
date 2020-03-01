using Microsoft.AspNetCore.Http;
using System;

namespace ForEvolve.ExceptionFilters.Handlers
{
    public class NotImplementedExceptionHandler : ExceptionHandler<NotImplementedException>
    {
        public override int StatusCode => StatusCodes.Status501NotImplemented;
    }
}

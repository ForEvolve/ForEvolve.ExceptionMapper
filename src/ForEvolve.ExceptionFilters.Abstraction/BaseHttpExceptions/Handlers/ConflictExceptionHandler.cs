using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters.BaseHttpExceptions.Handlers
{
    public class ConflictExceptionHandler : ExceptionHandler<ConflictException>
    {
        public override int StatusCode => StatusCodes.Status409Conflict;
    }

    public class NotFoundExceptionHandler : ExceptionHandler<NotFoundException>
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
    }

    public class InternalServerErrorExceptionHandler : ExceptionHandler<InternalServerErrorException>
    {
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }

    public class NotImplementedExceptionHandler : ExceptionHandler<NotImplementedException>
    {
        public override int StatusCode => StatusCodes.Status501NotImplemented;
    }
}

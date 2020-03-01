using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionFilters.Handlers
{
    public class NotFoundExceptionHandler : ExceptionHandler<NotFoundException>
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}

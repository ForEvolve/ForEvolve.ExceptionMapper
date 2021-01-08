using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class ForbiddenExceptionHandler : ExceptionHandler<ForbiddenException>
    {
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}

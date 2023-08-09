using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionMapper.Handlers;

public class ConflictExceptionHandler : ExceptionHandler<ConflictException>
{
    public override int StatusCode => StatusCodes.Status409Conflict;
}

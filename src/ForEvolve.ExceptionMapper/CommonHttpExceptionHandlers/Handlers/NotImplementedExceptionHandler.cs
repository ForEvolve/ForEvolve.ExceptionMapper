using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionMapper.Handlers;

public class NotImplementedExceptionHandler : ExceptionHandler<NotImplementedException>
{
    public override int StatusCode => StatusCodes.Status501NotImplemented;
}

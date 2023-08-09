using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionMapper.Handlers;

public class UnauthorizedExceptionHandler : ExceptionHandler<UnauthorizedException>
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
}

using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionMapper.Handlers;

public class BadRequestExceptionHandler : ExceptionHandler<BadRequestException>
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
}

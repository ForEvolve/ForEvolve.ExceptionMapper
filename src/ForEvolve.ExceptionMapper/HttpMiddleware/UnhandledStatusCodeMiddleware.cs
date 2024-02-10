using Microsoft.AspNetCore.Http;
namespace ForEvolve.ExceptionMapper;

public class UnhandledStatusCodeMiddleware
{
    private readonly RequestDelegate _next;

    public UnhandledStatusCodeMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.HasStarted)
        {
            return;
        }

        // Client errors (400–499)
        // Server errors (500–599)
        if (context.Response.StatusCode >= 400)
        {
            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status400BadRequest:
                    throw new BadRequestException();
                case StatusCodes.Status401Unauthorized:
                    throw new UnauthorizedException();
                case StatusCodes.Status403Forbidden:
                    throw new ForbiddenException();
                case StatusCodes.Status404NotFound:
                    throw new ResourceNotFoundException(context);
                case StatusCodes.Status409Conflict:
                    throw new ConflictException();
                case StatusCodes.Status500InternalServerError:
                    throw new InternalServerErrorException(new UnhandledStatusCodeException());
                case StatusCodes.Status501NotImplemented:
                    throw new NotImplementedException();
            }
        }
    }
}
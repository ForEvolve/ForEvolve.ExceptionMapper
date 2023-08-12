using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.Handlers.Fallback;
using ForEvolve.ExceptionMapper.Serialization.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.AddExceptionMapper(builder =>
{
    builder.AddExceptionHandler<MyForbiddenExceptionHandler>();
    builder.Map<ImATeapotException>().ToStatusCode(StatusCodes.Status418ImATeapot);
});
builder.Services
    .Configure<ApiBehaviorOptions>(options =>
    {
        options.ClientErrorMapping.Add(StatusCodes.Status409Conflict, new ClientErrorData
        {
            Link = "https://localhost:8828/Status409Conflict", // This is taken into account because the middleware do not set any link by default.
            Title = "This will not be displayed." // Not taken into account because the middleware sets the title to the Exception.Message value.
        });
    })
;

var app = builder.Build();
app.UseExceptionMapper();
app.MapGet("/", () => new string[]
{
    "---[Client]---",
    "/BadRequestException",
    "/ConflictException",
    "/ForbiddenException",
    "/GoneException",
    "/NotFoundException",
    "/ResourceNotFoundException",
    "/UnauthorizedException",
    "---[Server]---",
    "/GatewayTimeoutException",
    "/InternalServerErrorException",
    "/ServiceUnavailableException",
    "---[Custom]---",
    "/ImATeapotException",
    "/MyForbiddenException",
    "/MyNotFoundException",
    "/MyUnauthorizedException",
    "---[Others]---",
    "/fallback",
    "/a-url-that-does-not-exist",
});
app.MapGet("/BadRequestException", context => throw new BadRequestException());
app.MapGet("/ConflictException", context => throw new ConflictException());
app.MapGet("/ForbiddenException", context => throw new ForbiddenException());
app.MapGet("/GoneException", context => throw new GoneException());
app.MapGet("/NotFoundException", context => throw new NotFoundException());
app.MapGet("/ResourceNotFoundException", context => throw new ResourceNotFoundException(context));
app.MapGet("/UnauthorizedException", context => throw new UnauthorizedException());

app.MapGet("/GatewayTimeoutException", context => throw new GatewayTimeoutException());
app.MapGet("/InternalServerErrorException", context => throw new InternalServerErrorException(new Exception("Some other error that occurred.")));
app.MapGet("/ServiceUnavailableException", context => throw new ServiceUnavailableException());

app.MapGet("/ImATeapotException", context => throw new ImATeapotException());
app.MapGet("/MyForbiddenException", context => throw new MyForbiddenException());
app.MapGet("/MyNotFoundException", context => throw new MyNotFoundException());
app.MapGet("/MyUnauthorizedException", context => throw new MyUnauthorizedException(Random.Shared.Next(100) % 2 == 0 ? "John" : "Jane"));

app.MapGet("/fallback", context => throw new Exception("An error that gets handled by the fallback handler."));

app.Run();

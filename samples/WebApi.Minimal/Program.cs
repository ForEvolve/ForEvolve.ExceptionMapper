using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.Handlers.Fallback;
using ForEvolve.ExceptionMapper.Serialization.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.AddExceptionMapper();
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
    "/BadRequestException",
    "/ConflictException",
    "/ForbiddenException",
    "/GoneException",
    "/NotFoundException",
    "/ResourceNotFoundException",
    "/UnauthorizedException",
    "/GatewayTimeoutException",
    "/InternalServerErrorException",
    "/ServiceUnavailableException",
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

app.Run();

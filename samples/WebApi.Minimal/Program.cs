using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.Handlers.Fallback;
using ForEvolve.ExceptionMapper.Serialization.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddExceptionMapper(builder => builder
        //.AddExceptionHandler<ImATeapotExceptionHandler>()
        //.AddExceptionHandler<MyForbiddenExceptionHandler>()
        .MapCommonHttpExceptions()
        .MapHttpFallback(options =>
        {
            options.Strategy = FallbackStrategy.Handle;
        })
        //.Map<MyUnauthorizedException>(map => map.ToStatusCode(401))
        //.Map<GoneException>(map => map.ToStatusCode(410))
        .Map<NotSupportedException>(map => map.To(context =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            context.Result = new ExceptionHandledResult(context.Error);
            //context.HttpContext.Response.WriteAsync("{\"title\":\"This operation is not supported at the moment!\"}");
            return Task.CompletedTask;
        }, ForEvolve.ExceptionMapper.FluentMapper.FluentHandlerStrategy.Append))
        .SerializeAsProblemDetails()
    )
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
    "/NotFound",
    "/Conflict",
    "/InternalServerError",
    "/NotImplemented",
    "/Fallback",
    "/BadRequestException",
    "/NotSupportedException"
});
app.MapGet("/NotFound", context => throw new NotFoundException());
app.MapGet("/Conflict", context => throw new ConflictException());
app.MapGet("/InternalServerError", context => throw new InternalServerErrorException(new Exception()));
app.MapGet("/NotImplemented", context => throw new NotImplementedException());
//app.MapGet("/MyNotFoundException", context => throw new MyNotFoundException());
app.MapGet("/Fallback", context => throw new Exception());
//app.MapGet("/MyUnauthorizedException", context => throw new MyUnauthorizedException());
//app.MapGet("/GoneException", context => throw new GoneException());
//app.MapGet("/ImATeapotException", context => throw new ImATeapotException());
//app.MapGet("/MyForbiddenException", context => throw new MyForbiddenException());
app.MapGet("/BadRequestException", context => throw new BadRequestException());
app.MapGet("/NotSupportedException", context => throw new NotSupportedException());

app.Run();

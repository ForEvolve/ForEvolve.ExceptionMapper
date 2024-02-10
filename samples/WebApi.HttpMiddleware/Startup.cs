using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebApi.Shared;

namespace WebApi.HttpMiddleware;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddExceptionMapper(Configuration, builder =>
            {
                builder.AddExceptionHandler<MyForbiddenExceptionHandler>();
                builder.Map<ImATeapotException>().ToStatusCode(StatusCodes.Status418ImATeapot);
            })
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.ClientErrorMapping[StatusCodes.Status409Conflict].Link = "https://localhost:8828/Status409Conflict";
            });
        ;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseExceptionMapper();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", () => new
            {
                routing = new string[]
                {
                    "---[Client]---",
                    "/Routing/BadRequestException",
                    "/Routing/ConflictException",
                    "/Routing/ForbiddenException",
                    "/Routing/GoneException",
                    "/Routing/NotFoundException",
                    "/Routing/ResourceNotFoundException",
                    "/Routing/UnauthorizedException",
                    "---[Server]---",
                    "/Routing/GatewayTimeoutException",
                    "/Routing/InternalServerErrorException",
                    "/Routing/ServiceUnavailableException",
                    "---[Custom]---",
                    "/Routing/ImATeapotException",
                    "/Routing/MyForbiddenException",
                    "/Routing/DroidNotFoundException",
                    "/Routing/MyUnauthorizedException",
                    "---[Others]---",
                    "/Routing/fallback",
                },
                mvc = new string[]
                {
                    "---[Client]---",
                    "/mvc/BadRequestException",
                    "/mvc/ConflictException",
                    "/mvc/ForbiddenException",
                    "/mvc/GoneException",
                    "/mvc/NotFoundException",
                    "/mvc/ResourceNotFoundException",
                    "/mvc/UnauthorizedException",
                    "---[Server]---",
                    "/mvc/GatewayTimeoutException",
                    "/mvc/InternalServerErrorException",
                    "/mvc/ServiceUnavailableException",
                    "---[Custom]---",
                    "/mvc/ImATeapotException",
                    "/mvc/MyForbiddenException",
                    "/mvc/DroidNotFoundException",
                    "/mvc/MyUnauthorizedException",
                    "---[Others]---",
                    "/mvc/fallback",
                    "/mvc/ValidationError"
                },
                other = new string[]
                {
                    "/a-url-that-does-not-exist"
                }
            });
           
            endpoints.MapGet("/Routing/BadRequestException", context => throw new BadRequestException());
            endpoints.MapGet("/Routing/ConflictException", context => throw new ConflictException());
            endpoints.MapGet("/Routing/ForbiddenException", context => throw new ForbiddenException());
            endpoints.MapGet("/Routing/GoneException", context => throw new GoneException());
            endpoints.MapGet("/Routing/NotFoundException", context => throw new NotFoundException());
            endpoints.MapGet("/Routing/ResourceNotFoundException", context => throw new ResourceNotFoundException(context));
            endpoints.MapGet("/Routing/UnauthorizedException", context => throw new UnauthorizedException());

            endpoints.MapGet("/Routing/GatewayTimeoutException", context => throw new GatewayTimeoutException());
            endpoints.MapGet("/Routing/InternalServerErrorException", context => throw new InternalServerErrorException(new Exception("Some other error that occurred.")));
            endpoints.MapGet("/Routing/ServiceUnavailableException", context => throw new ServiceUnavailableException());

            endpoints.MapGet("/Routing/ImATeapotException", context => throw new ImATeapotException());
            endpoints.MapGet("/Routing/MyForbiddenException", context => throw new MyForbiddenException());
            endpoints.MapGet("/Routing/DroidNotFoundException", context => throw new DroidNotFoundException());
            endpoints.MapGet("/Routing/MyUnauthorizedException", context => throw new MyUnauthorizedException(Random.Shared.Next(100) % 2 == 0 ? "John" : "Jane"));

            endpoints.MapGet("/Routing/Fallback", context => throw new Exception("An error that gets handled by the fallback handler."));
        });
    }
}

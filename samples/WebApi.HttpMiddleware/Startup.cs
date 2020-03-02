using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Shared;

namespace WebApi.HttpMiddleware
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddExceptionMapper(builder => builder
                    .AddExceptionHandler<ImATeapotExceptionHandler>()
                    .AddExceptionHandler<MyForbiddenExceptionHandler>()
                    .MapCommonHttpExceptions(options =>
                    {
                        options.Strategy = FallbackStrategy.Handle;
                    })
                    .Map<MyUnauthorizedException>(map => map.ToStatusCode(401))
                    .Map<GoneException>(map => map.ToStatusCode(410))
                    .Map<NotSupportedException>(map => map.To(context =>
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        context.Result = new ExceptionHandledResult(context.Error);
                        context.HttpContext.Response.WriteAsync("{\"title\":\"This operation is not supported at the moment!\"}");
                        return Task.CompletedTask;
                    }, ForEvolve.ExceptionMapper.FluentMapper.FluentHandlerStrategy.Append))
                )
                .AddControllers()
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
                endpoints.MapGet("/", async context =>
                {
                    var baseUri = $"{context.Request.Scheme}://{context.Request.Host}";
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{");
                    await context.Response.WriteAsync("\"name\":\"Exploration document\",");
                    await context.Response.WriteAsync("\"routing\":[");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/NotFound\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/Conflict\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/InternalServerError\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/NotImplemented\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/MyNotFoundException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/Exception\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/MyUnauthorizedException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/GoneException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/ImATeapotException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/MyForbiddenException\"");
                    await context.Response.WriteAsync("],");
                    await context.Response.WriteAsync("\"mvc\":[");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/NotFound\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/Conflict\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/InternalServerError\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/NotImplemented\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/MyNotFoundException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/Exception\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/MyUnauthorizedException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/GoneException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/ImATeapotException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/MyForbiddenException\"");
                    await context.Response.WriteAsync("]");
                    await context.Response.WriteAsync("}");
                });
                endpoints.MapGet("/Routing/NotFound", context => throw new NotFoundException());
                endpoints.MapGet("/Routing/Conflict", context => throw new ConflictException());
                endpoints.MapGet("/Routing/InternalServerError", context => throw new InternalServerErrorException(new Exception()));
                endpoints.MapGet("/Routing/NotImplemented", context => throw new NotImplementedException());
                endpoints.MapGet("/Routing/MyNotFoundException", context => throw new MyNotFoundException());
                endpoints.MapGet("/Routing/Exception", context => throw new Exception());
                endpoints.MapGet("/Routing/MyUnauthorizedException", context => throw new MyUnauthorizedException());
                endpoints.MapGet("/Routing/GoneException", context => throw new GoneException());
                endpoints.MapGet("/Routing/ImATeapotException", context => throw new ImATeapotException());
                endpoints.MapGet("/Routing/MyForbiddenException", context => throw new MyForbiddenException());
            });
        }
    }
}

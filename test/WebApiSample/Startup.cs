using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ForEvolve.ExceptionFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApiSample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
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
                    await context.Response.WriteAsync($"\"{baseUri}/Routing/Exception\"");
                    await context.Response.WriteAsync("],");
                    await context.Response.WriteAsync("\"mvc\":[");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/NotFound\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/Conflict\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/InternalServerError\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/NotImplemented\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/MyNotFoundException\",");
                    await context.Response.WriteAsync($"\"{baseUri}/mvc/Exception\"");
                    await context.Response.WriteAsync("]");
                    await context.Response.WriteAsync("}");
                });
                endpoints.MapGet("/Routing/NotFound", context => throw new NotFoundException());
                endpoints.MapGet("/Routing/Conflict", context => throw new ConflictException());
                endpoints.MapGet("/Routing/InternalServerError", context => throw new InternalServerErrorException(new Exception()));
                endpoints.MapGet("/Routing/NotImplemented", context => throw new NotImplementedException());
                endpoints.MapGet("/Routing/MyNotFoundException", context => throw new MyNotFoundException());
                endpoints.MapGet("/Routing/Exception", context => throw new Exception());
            });
        }
    }

    [ApiController]
    [Route("mvc")]
    public class ExceptionController
    {
#pragma warning disable IDE0022 // Use block body for methods
        [HttpGet("NotFound")]
        public IActionResult NotFound() => throw new NotFoundException();

        [HttpGet("Conflict")]
        public IActionResult Conflict() => throw new ConflictException();

        [HttpGet("InternalServerError")]
        public IActionResult InternalServerError() => throw new InternalServerErrorException(new Exception());

        [HttpGet("NotImplemented")]
        public IActionResult NotImplemented() => throw new NotImplementedException();

        [HttpGet("MyNotFoundException")]
        public IActionResult MyNotFoundException() => throw new MyNotFoundException();

        [HttpGet("Exception")]
        public IActionResult Exception() => throw new Exception();
#pragma warning restore IDE0022 // Use block body for methods
    }

    public class MyNotFoundException : NotFoundException
    {

    }
}

using Microsoft.AspNetCore.Builder;

namespace ForEvolve.ExceptionFilters
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpExceptionMapping(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(errorApp =>
            {
                errorApp.UseMiddleware<HttpExceptionHandlingMiddleware>();
            });
        }
    }
}

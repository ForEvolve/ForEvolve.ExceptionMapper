using ForEvolve.ExceptionMapper;
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionMapper(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(errorApp =>
            {
                errorApp.UseMiddleware<HttpExceptionHandlingMiddleware>();
            });
        }
    }
}
using ForEvolve.ExceptionMapper;
namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptionMapper(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.UseMiddleware<HttpExceptionHandlingMiddleware>();
        });
        app.UseMiddleware<UnhandledStatusCodeMiddleware>();
        return app;
    }
}
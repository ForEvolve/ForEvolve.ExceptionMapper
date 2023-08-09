using Microsoft.AspNetCore.Http;
namespace ForEvolve.ExceptionMapper
{
    public interface IExceptionHandlingManager
    {
        Task<IExceptionHandlingResult> HandleAsync(HttpContext context);
    }
}
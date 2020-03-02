using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace ForEvolve.ExceptionMapper
{
    public interface IExceptionHandlingManager
    {
        Task<IExceptionHandlingResult> HandleAsync(HttpContext context);
    }
}
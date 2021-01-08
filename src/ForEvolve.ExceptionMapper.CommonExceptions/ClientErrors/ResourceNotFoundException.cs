using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace ForEvolve.ExceptionMapper
{
    public class ResourceNotFoundException : NotFoundException
    {
        public ResourceNotFoundException(HttpContext context)
            : base($"No resource were found at '{context.Request.GetDisplayUrl()}'.")
        {

        }
    }
}

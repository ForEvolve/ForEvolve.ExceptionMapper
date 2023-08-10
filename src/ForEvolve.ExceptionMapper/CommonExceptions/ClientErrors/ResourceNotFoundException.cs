using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace ForEvolve.ExceptionMapper;

/// <summary>
/// The server cannot find the requested resource. In the browser, this means the URL is not recognized. In an API, this can also mean that the endpoint is valid but the resource itself does not exist.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404"/>
/// </summary>
/// <remarks>404 Not Found<br />A default error message is created using the requested URL.</remarks>
public class ResourceNotFoundException : NotFoundException
{
    public ResourceNotFoundException(HttpContext context)
        : base($"No resource were found at '{context.Request.GetDisplayUrl()}'.")
    {

    }
}

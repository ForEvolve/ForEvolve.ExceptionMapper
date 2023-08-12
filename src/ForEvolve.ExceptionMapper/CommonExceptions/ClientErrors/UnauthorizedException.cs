namespace ForEvolve.ExceptionMapper;

/// <summary>
/// Although the HTTP standard specifies "unauthorized", semantically this response means "unauthenticated". That is, the client must authenticate itself to get the requested response.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/401"/>
/// </summary>
/// <remarks>401 Unauthorized</remarks>
public class UnauthorizedException : ClientErrorException
{
    public UnauthorizedException()
        : base("You must be authenticated to access the requested resource.")
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

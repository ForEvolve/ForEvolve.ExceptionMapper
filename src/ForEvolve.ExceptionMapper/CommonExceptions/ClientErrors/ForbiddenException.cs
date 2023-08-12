namespace ForEvolve.ExceptionMapper;

/// <summary>
/// The client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401 Unauthorized, the client's identity is known to the server.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/403"/>
/// </summary>
/// <remarks>403 Forbidden</remarks>
public class ForbiddenException : ClientErrorException
{
    public ForbiddenException()
        : base("You do not have access rights to the content.")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

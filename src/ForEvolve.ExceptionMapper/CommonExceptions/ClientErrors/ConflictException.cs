namespace ForEvolve.ExceptionMapper;

/// <summary>
/// This response is sent when a request conflicts with the current state of the server.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/409"/>
/// </summary>
/// <remarks>409 Conflict</remarks>
public class ConflictException : ClientErrorException
{
    public ConflictException()
        : base("The request conflicts with the current state of the server.")
    {
    }

    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

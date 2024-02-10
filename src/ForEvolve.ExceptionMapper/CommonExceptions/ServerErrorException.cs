namespace ForEvolve.ExceptionMapper;

/// <summary>
/// Server error responses (500 – 599)
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status#server_error_responses"/>
/// </summary>
public abstract class ServerErrorException : ForEvolveException
{
    public ServerErrorException()
    {
    }

    public ServerErrorException(string message) : base(message)
    {
    }

    public ServerErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

namespace ForEvolve.ExceptionMapper;

/// <summary>
/// Client error responses (400 – 499)
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status#client_error_responses"/>
/// </summary>
public abstract class ClientErrorException : ForEvolveException
{
    public ClientErrorException()
    {
    }

    public ClientErrorException(string message) : base(message)
    {
    }

    public ClientErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

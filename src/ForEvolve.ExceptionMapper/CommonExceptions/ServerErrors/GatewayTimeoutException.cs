namespace ForEvolve.ExceptionMapper;

/// <summary>
/// This error response is given when the server is acting as a gateway and cannot get a response in time.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/504"/>
/// </summary>
/// <remarks>504 Gateway Timeout</remarks>
public class GatewayTimeoutException : ServerErrorException
{
    public GatewayTimeoutException()
    {
    }

    public GatewayTimeoutException(string message) : base(message)
    {
    }

    public GatewayTimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
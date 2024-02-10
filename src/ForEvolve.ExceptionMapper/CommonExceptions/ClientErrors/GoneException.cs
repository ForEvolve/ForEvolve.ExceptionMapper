namespace ForEvolve.ExceptionMapper;

/// <summary>
/// This response is sent when the requested content has been permanently deleted from server, with no forwarding address. Clients are expected to remove their caches and links to the resource. The HTTP specification intends this status code to be used for "limited-time, promotional services". APIs should not feel compelled to indicate resources that have been deleted with this status code.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/410"/>
/// </summary>
/// <remarks>410 Gone</remarks>
public class GoneException : ClientErrorException
{
    public GoneException()
        : base("The requested content has been permanently deleted from server.")
    {
    }

    public GoneException(string message) : base(message)
    {
    }

    public GoneException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

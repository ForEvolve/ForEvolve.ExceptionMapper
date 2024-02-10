namespace ForEvolve.ExceptionMapper;


/// <summary>
/// The server has encountered a situation it does not know how to handle.
/// <br /><br />See also <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"/>
/// </summary>
/// <remarks>500 Internal Server Error</remarks>
public class InternalServerErrorException : ServerErrorException
{
    public InternalServerErrorException(Exception innerException)
        : base(innerException.Message, innerException)
    {

    }
}

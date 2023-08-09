namespace ForEvolve.ExceptionMapper;

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

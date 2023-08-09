namespace ForEvolve.ExceptionMapper;

public class InternalServerErrorException : ServerErrorException
{
    public InternalServerErrorException(Exception innerException)
        : base(innerException.Message, innerException)
    {

    }
}

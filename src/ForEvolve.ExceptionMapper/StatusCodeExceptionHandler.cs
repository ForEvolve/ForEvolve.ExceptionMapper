namespace ForEvolve.ExceptionMapper;

public class StatusCodeExceptionHandler<TException> : ExceptionHandler<TException>
    where TException : Exception
{
    public StatusCodeExceptionHandler(int statusCode)
    {
        StatusCode = statusCode;
    }
    public override int StatusCode { get; }
}
namespace ForEvolve.ExceptionMapper;

public class NoExceptionResult : IExceptionHandlingResult
{
    public bool ExceptionHandled { get; }
    public Exception? Error { get; }
}
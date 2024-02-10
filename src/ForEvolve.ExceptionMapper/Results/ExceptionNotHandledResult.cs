namespace ForEvolve.ExceptionMapper;

public class ExceptionNotHandledResult : IExceptionHandlingResult
{
    public ExceptionNotHandledResult(Exception error)
    {
        Error = error ?? throw new ArgumentNullException(nameof(error));
    }

    public bool ExceptionHandled { get; }
    public Exception Error { get; }
}
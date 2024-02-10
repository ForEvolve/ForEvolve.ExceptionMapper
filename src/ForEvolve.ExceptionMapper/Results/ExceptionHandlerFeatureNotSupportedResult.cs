namespace ForEvolve.ExceptionMapper;

public sealed class ExceptionHandlerFeatureNotSupportedResult : IExceptionHandlingResult
{
    public bool ExceptionHandled { get; }
    public Exception? Error { get; }
}
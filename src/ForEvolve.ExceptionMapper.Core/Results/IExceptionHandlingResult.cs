namespace ForEvolve.ExceptionMapper;

public interface IExceptionHandlingResult
{
    bool ExceptionHandled { get; }
    Exception? Error { get; }
}
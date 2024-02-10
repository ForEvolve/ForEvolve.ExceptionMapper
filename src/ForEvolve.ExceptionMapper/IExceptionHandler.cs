namespace ForEvolve.ExceptionMapper;

public interface IExceptionHandler
{
    Task<bool> CanHandle(Exception exception);
    Task ExecuteAsync(ExceptionHandlingContext context);
}

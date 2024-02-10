namespace ForEvolve.ExceptionMapper;

public interface IExceptionSerializer
{
    Task ExecuteAsync(ExceptionHandlingContext context);
}
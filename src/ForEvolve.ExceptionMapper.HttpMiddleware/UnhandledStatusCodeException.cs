namespace ForEvolve.ExceptionMapper;

public class UnhandledStatusCodeException : Exception
{
    public UnhandledStatusCodeException()
        : base($"An unhandled error occured.") { }
}
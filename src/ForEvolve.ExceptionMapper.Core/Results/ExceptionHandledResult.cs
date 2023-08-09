namespace ForEvolve.ExceptionMapper
{
    public class ExceptionHandledResult : IExceptionHandlingResult
    {
        public ExceptionHandledResult(Exception error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
            ExceptionHandled = true;
            ExceptionHandlerFeatureSupported = true;
        }

        public bool ExceptionHandled { get; }
        public Exception Error { get; }
        public bool ExceptionHandlerFeatureSupported { get; }
    }
}
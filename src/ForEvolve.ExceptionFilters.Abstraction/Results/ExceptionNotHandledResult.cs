using System;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionNotHandledResult : IExceptionHandlingResult
    {
        public ExceptionNotHandledResult(Exception error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
            ExceptionHandlerFeatureSupported = true;
        }

        public bool ExceptionHandled { get; }
        public Exception Error { get; }
        public bool ExceptionHandlerFeatureSupported { get; }
    }
}
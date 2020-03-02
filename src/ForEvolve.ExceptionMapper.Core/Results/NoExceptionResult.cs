using System;

namespace ForEvolve.ExceptionMapper
{
    public class NoExceptionResult : IExceptionHandlingResult
    {
        public NoExceptionResult()
        {
            ExceptionHandlerFeatureSupported = true;
        }

        public bool ExceptionHandled { get; }
        public Exception Error { get; }
        public bool ExceptionHandlerFeatureSupported { get; }
    }
}
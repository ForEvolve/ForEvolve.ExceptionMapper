using System;

namespace ForEvolve.ExceptionFilters
{
    public sealed class ExceptionHandlerFeatureNotSupportedResult : IExceptionHandlingResult
    {
        public bool ExceptionHandled { get; }
        public Exception Error { get; }
        public bool ExceptionHandlerFeatureSupported => false;
    }
}
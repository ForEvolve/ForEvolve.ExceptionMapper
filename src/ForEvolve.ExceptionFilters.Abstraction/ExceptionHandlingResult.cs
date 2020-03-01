using System;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionHandlingResult
    {
        bool ExceptionHandled { get; }
        Exception Error { get; }
        bool ExceptionHandlerFeatureSupported { get; }
    }

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

    public sealed class ExceptionHandlerFeatureNotSupportedResult : IExceptionHandlingResult
    {
        public bool ExceptionHandled { get; }
        public Exception Error { get; }
        public bool ExceptionHandlerFeatureSupported => false;
    }
}
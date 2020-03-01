using System;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionHandlingResult
    {
        bool ExceptionHandled { get; }
        Exception Error { get; }
        bool ExceptionHandlerFeatureSupported { get; }
    }
}
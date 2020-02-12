using System;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionConverter<TResult>
        where TResult : class
    {
        TResult Convert(Exception exception);
    }
}

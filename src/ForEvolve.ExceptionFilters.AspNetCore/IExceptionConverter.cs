using System;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionConverter
    {
        object Convert(Exception exception);
    }
}

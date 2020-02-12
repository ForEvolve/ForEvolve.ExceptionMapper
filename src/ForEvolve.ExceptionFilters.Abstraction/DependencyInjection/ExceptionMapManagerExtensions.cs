using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public static class ExceptionMapManagerExtensions
    {
        public static Task<bool> HasMapForExceptionAsync<TException>(this IExceptionMapManager manager)
            where TException : Exception
        {
            return manager.HasMapForExceptionAsync(typeof(TException));
        }

        public static Task<ExceptionMap> GetMapForExceptionAsync<TException>(this IExceptionMapManager manager)
            where TException : Exception
        {
            return manager.GetMapForExceptionAsync(typeof(TException));
        }
    }
}

using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionMapManager
    {
        Task AddOrUpdateAsync(ExceptionMap exceptionMap);
        Task<bool> HasMapForExceptionAsync(Type exceptionType);
        Task<ExceptionMap> GetMapForExceptionAsync(Type exceptionType);
    }
}

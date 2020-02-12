using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public abstract class ExceptionMap
    {
        protected ExceptionMap(string exceptionName)
        {
            ExceptionName = string.IsNullOrWhiteSpace(exceptionName)
                ? throw new ArgumentNullException(nameof(exceptionName))
                : exceptionName;
        }


        public abstract Task ExecuteAsync(HttpContext httpContext);

        public string ExceptionName { get; }
        public bool IsUserGenerated { get; set; } = true;
    }
}

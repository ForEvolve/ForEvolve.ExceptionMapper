using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
namespace ForEvolve.ExceptionFilters
{
    public abstract class ExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        public const int DefaultOrder = 1;

        public abstract int StatusCode { get; }
        public virtual int Order => DefaultOrder;

        public Task<bool> KnowHowToHandleAsync(Exception exception)
        {
            return Task.FromResult(exception is TException);
        }

        public Task HandleAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = StatusCode;
            return HandleCoreAsync(httpContext, exception as TException);
        }

        protected virtual Task HandleCoreAsync(HttpContext httpContext, TException exception)
        {
            return Task.CompletedTask;
        }
    }
}
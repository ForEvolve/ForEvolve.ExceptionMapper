using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public class HttpExceptionMap : ExceptionMap
    {
        public HttpExceptionMap(string exceptionName, ExceptionMapHttpHandlerAsync handler)
            : base(exceptionName)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HttpExceptionMap(string exceptionName, ExceptionMapHttpHandlerAsync handler, bool isUserGenerated)
            : this(exceptionName, handler)
        {
            IsUserGenerated = isUserGenerated;
        }

        public override Task ExecuteAsync(HttpContext httpContext)
        {
            return Handler(httpContext);
        }

        public ExceptionMapHttpHandlerAsync Handler { get; }
    }

    public class HttpExceptionMap<TException> : HttpExceptionMap
        where TException : Exception
    {
        public HttpExceptionMap(ExceptionMapHttpHandlerAsync handler)
            : base(typeof(TException).FullName, handler)
        {
        }

        public HttpExceptionMap(ExceptionMapHttpHandlerAsync handler, bool isUserGenerated)
            : base(typeof(TException).FullName, handler, isUserGenerated)
        {
        }
    }
}

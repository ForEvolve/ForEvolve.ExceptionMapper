using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionToProblemDetailsConverter : IExceptionConverter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExceptionToProblemDetailsConverter(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public object Convert(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }
            var details = new ProblemDetails
            {
                Title = exception.GetType().FullName,
                Detail = exception.Message,
            };
            if (_hostingEnvironment.IsDevelopment())
            {
                details.Extensions.Add("source", exception.Source);
                details.Extensions.Add("hresult", exception.HResult);
                details.Extensions.Add("helpLink", exception.HelpLink);
                details.Extensions.Add("data", exception.Data);
                details.Extensions.Add("stackTrace", exception.StackTrace);
            }
            details.Extensions.Add("innerException", Convert(exception.InnerException));
            return details;
        }
    }
}

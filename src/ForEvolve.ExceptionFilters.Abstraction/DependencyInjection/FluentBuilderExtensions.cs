using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public static class FluentBuilderExtensions
    { 
        public static IExceptionMappingBuilder ToStatusCode<TException>(this FluentBuilder<TException> mapBuilder, int expectedStatusCode)
            where TException : Exception
        {
            return mapBuilder
                .To(context => StatusCodeHandler(context, expectedStatusCode));

            static Task StatusCodeHandler(HttpContext context, int statusCode)
            {
                context.Response.StatusCode = statusCode;
                return Task.CompletedTask;
            }
        }

        public static IExceptionMappingBuilder To<TException>(this FluentBuilder<TException> mapBuilder, ExceptionMapHttpHandlerAsync exceptionMapHandlerAsync)
            where TException : Exception
        {
            mapBuilder.Maps
                .AddOrUpdateAsync(new HttpExceptionMap<TException>(exceptionMapHandlerAsync));
            return mapBuilder;
        }
    }
}

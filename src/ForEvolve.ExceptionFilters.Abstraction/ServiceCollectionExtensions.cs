using ForEvolve.ExceptionFilters;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExceptionFiltersExtensions
    {
        private static IExceptionMappingBuilder _builder;

        public static IServiceCollection AddExceptionMapping(this IServiceCollection services, Action<IExceptionMappingBuilder> exceptionMappingBuilder = null)
        {
            services.AddLogging();
            if (_builder == null)
            {
                _builder = new ExceptionMappingBuilder(services);
            }
            services.TryAddSingleton(_builder);
            exceptionMappingBuilder?.Invoke(_builder);
            return services;
        }
    }

    public interface IExceptionMappingBuilder
    {
        IServiceCollection Services { get; }
    }

    public class ExceptionMappingBuilder : IExceptionMappingBuilder
    {
        public ExceptionMappingBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }

    public static class ExceptionMappingBuilderExtensions
    {
        public static IExceptionMappingBuilder AddExceptionHandler<THandler>(this IExceptionMappingBuilder builder)
            where THandler : class, IExceptionHandler
        {
            builder.Services.AddSingleton<IExceptionHandler, THandler>();
            return builder;
        }
        //public static FluentBuilder<TException> Map<TException>(this IExceptionMappingBuilder builder)
        //    where TException : Exception
        //{
        //    return new FluentBuilder<TException>(builder);
        //}
    }
}
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpExceptionMapping(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(errorApp =>
            {
                errorApp.UseMiddleware<HttpExceptionHandlingMiddleware>();
            });
        }
    }
}
namespace ForEvolve.ExceptionFilters
{
    public class HttpExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly IExceptionMapManager _exceptionMapManager;

        public HttpExceptionHandlingMiddleware(/*IExceptionMapManager exceptionMapManager, */RequestDelegate next)
        {
            //_exceptionMapManager = exceptionMapManager ?? throw new ArgumentNullException(nameof(exceptionMapManager));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                var exceptionType = exceptionHandlerPathFeature.Error.GetType();
                //if (await _exceptionMapManager.HasMapForExceptionAsync(exceptionType))
                //{
                //    var handler = await _exceptionMapManager.GetMapForExceptionAsync(exceptionType);
                //    await handler.ExecuteAsync(context);
                //}
                return;
            }
            await _next(context);
        }
    }

    public interface IExceptionHandler
    {
        int Order { get; }
        Task<bool> KnowHowToHandleAsync(Exception exception);
        Task HandleAsync(HttpContext httpContext, Exception exception);
    }

    public interface IExceptionHandlingManager
    {
        Task<bool> HasHandlerForExceptionAsync(Exception exception);
        Task<IExceptionHandler> GetHandlerForExceptionAsync(Exception exception);
    }

    //public abstract class ExceptionHandlerBase : IExceptionHandler
    //{
    //    public Task HandleAsync(HttpContext httpContext, Exception exception)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public abstract Task<bool> KnowHowToHandleAsync(Exception exception);
    //    protected abstract Task HandleCoreAsync(HttpContext httpContext, Exception exception);
    //}

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
            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            return HandleCoreAsync(httpContext, exception as TException);
        }

        protected virtual Task HandleCoreAsync(HttpContext httpContext, TException exception)
        {
            return Task.CompletedTask;
        }
    }


    //public abstract class ExceptionSerializerHandlerBase : IExceptionHandler
    //{
    //    public const int DefaultSerializerOrder = 100;
    //    public virtual int Order => DefaultSerializerOrder;

    //    public abstract Task HandleAsync(HttpContext httpContext, Exception exception);

    //    public virtual Task<bool> KnowHowToHandleAsync(Exception exception)
    //        => Task.FromResult(true);
    //}

    //public class JsonExceptionSerializerHandler : ExceptionSerializerHandlerBase
    //{
    //    public override Task HandleAsync(HttpContext httpContext, Exception exception)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
    //public class ExceptionHandlingOptions
    //{
    //    public bool SerializeExceptions { get; set; }
    //}
}
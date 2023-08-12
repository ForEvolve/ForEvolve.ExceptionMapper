using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
namespace ForEvolve.ExceptionMapper;

public class ExceptionHandlingManager : IExceptionHandlingManager
{
    private readonly ExceptionMapperOptions _options;

    public ExceptionHandlingManager(ExceptionMapperOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IReadOnlyCollection<IExceptionHandler> Handlers
        => new ReadOnlyCollection<IExceptionHandler>(_options.Handlers);

    public async Task<IExceptionHandlingResult> HandleAsync(HttpContext httpContext)
    {
        var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerPathFeature == null)
        {
            return new ExceptionHandlerFeatureNotSupportedResult();
        }

        var exception = exceptionHandlerPathFeature.Error;
        if (exception == null)
        {
            return new NoExceptionResult();
        }

        var context = new ExceptionHandlingContext(httpContext, exception, new ExceptionNotHandledResult(exception));
        foreach (var handler in _options.Handlers)
        {
            if (await handler.CanHandle(exception))
            {
                await handler.ExecuteAsync(context);
            }
        }

        await _options.Serializer.ExecuteAsync(context);

        return context.Result;
    }
}

public class ExceptionHandlingContext<TException>
    where TException : Exception
{
    public ExceptionHandlingContext(ExceptionHandlingContext previous)
        : this(previous.HttpContext, previous.Error as TException, previous.Result)
    {
    }

    public ExceptionHandlingContext(ExceptionHandlingContext<TException> previous)
        : this(previous.HttpContext, previous.Error, previous.Result)
    {
    }

    public ExceptionHandlingContext(HttpContext httpContext, TException? error, IExceptionHandlingResult initialResult)
    {
        HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        Error = error ?? throw new ArgumentNullException(nameof(error));
        Result = initialResult;
    }

    public HttpContext HttpContext { get; }
    public TException Error { get; }
    public IExceptionHandlingResult Result { get; set; }
}

public class ExceptionHandlingContext : ExceptionHandlingContext<Exception>
{
    public ExceptionHandlingContext(ExceptionHandlingContext previous)
        : base(previous)
    {
    }

    public ExceptionHandlingContext(HttpContext httpContext, Exception error, IExceptionHandlingResult initialResult)
        : base(httpContext, error, initialResult)
    {
    }
}
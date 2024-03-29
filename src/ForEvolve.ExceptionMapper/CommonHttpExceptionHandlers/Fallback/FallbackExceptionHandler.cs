﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ForEvolve.ExceptionMapper.Handlers.Fallback;

public class FallbackExceptionHandler : IExceptionHandler
{
    private readonly FallbackExceptionHandlerOptions _options;
    public FallbackExceptionHandler(IOptionsMonitor<FallbackExceptionHandlerOptions> options)
    {
        _options = options.CurrentValue;
    }

    public Task<bool> CanHandle(Exception exception)
    {
        if (_options.Strategy == FallbackStrategy.Handle)
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task ExecuteAsync(ExceptionHandlingContext context)
    {
        if (_options.Strategy == FallbackStrategy.Handle)
        {
            if (!context.Result.ExceptionHandled)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Result = new ExceptionHandledResult(context.Error);
            }
        }
        return Task.CompletedTask;
    }
}

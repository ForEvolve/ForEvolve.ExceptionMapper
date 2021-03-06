﻿using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionMapper
{
    public interface IExceptionHandler
    {
        int Order { get; }
        Task<bool> KnowHowToHandleAsync(Exception exception);
        Task ExecuteAsync(ExceptionHandlingContext context);
    }
}
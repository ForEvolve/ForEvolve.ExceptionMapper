﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace ForEvolve.ExceptionFilters
{
    public interface IExceptionHandlingManager
    {
        Task<IExceptionHandlingResult> HandleAsync(HttpContext context);
    }
}
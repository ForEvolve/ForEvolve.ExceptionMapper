﻿using Microsoft.AspNetCore.Http;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class InternalServerErrorExceptionHandler : ExceptionHandler<InternalServerErrorException>
    {
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}

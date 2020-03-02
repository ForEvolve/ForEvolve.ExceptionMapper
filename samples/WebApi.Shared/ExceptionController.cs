using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApi.Shared
{
    [ApiController]
    [Route("mvc")]
    public class ExceptionController
    {
#pragma warning disable IDE0022 // Use block body for methods
        [HttpGet("NotFound")]
        public IActionResult NotFound() => throw new NotFoundException();

        [HttpGet("Conflict")]
        public IActionResult Conflict() => throw new ConflictException();

        [HttpGet("InternalServerError")]
        public IActionResult InternalServerError() => throw new InternalServerErrorException(new Exception());

        [HttpGet("NotImplemented")]
        public IActionResult NotImplemented() => throw new NotImplementedException();

        [HttpGet("MyNotFoundException")]
        public IActionResult MyNotFoundException() => throw new MyNotFoundException();

        [HttpGet("Exception")]
        public IActionResult Exception() => throw new Exception();

        [HttpGet("MyUnauthorizedException")]
        public IActionResult MyUnauthorizedException() => throw new MyUnauthorizedException();

        [HttpGet("GoneException")]
        public IActionResult GoneException() => throw new GoneException();

        [HttpGet("ImATeapotException")]
        public IActionResult ImATeapotException() => throw new ImATeapotException();

        [HttpGet("MyForbiddenException")]
        public IActionResult MyForbiddenException() => throw new MyForbiddenException();
#pragma warning restore IDE0022 // Use block body for methods
    }
}

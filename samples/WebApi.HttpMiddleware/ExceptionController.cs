using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared
{
    [ApiController]
    [Route("mvc")]
    public class ExceptionController : ControllerBase
    {
#pragma warning disable IDE0022 // Use block body for methods
        [HttpGet("NotFound")]
        public IActionResult NotFoundException() => throw new NotFoundException();

        [HttpGet("Conflict")]
        public IActionResult ConflictException() => throw new ConflictException();

        [HttpGet("InternalServerError")]
        public IActionResult InternalServerError() => throw new InternalServerErrorException(new Exception());

        [HttpGet("NotImplemented")]
        public IActionResult NotImplemented() => throw new NotImplementedException();

        [HttpGet("MyNotFoundException")]
        public IActionResult MyNotFoundException() => throw new MyNotFoundException();

        [HttpGet("Fallback")]
        public IActionResult Fallback() => throw new Exception();

        [HttpGet("MyUnauthorizedException")]
        public IActionResult MyUnauthorizedException() => throw new MyUnauthorizedException();

        [HttpGet("GoneException")]
        public IActionResult GoneException() => throw new GoneException();

        [HttpGet("ImATeapotException")]
        public IActionResult ImATeapotException() => throw new ImATeapotException();

        [HttpGet("MyForbiddenException")]
        public IActionResult MyForbiddenException() => throw new MyForbiddenException();
#pragma warning restore IDE0022 // Use block body for methods

        [HttpGet("ValidationError")]
        public IActionResult ValidationError([FromQuery] MyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new
            {
                message = "This should not happen, please review the controller action."
            });
        }

        public class MyModel
        {
            [Required]
            public string Name { get; set; }

            [Range(18, 35)]
            public int Age { get; set; }
        }
    }
}

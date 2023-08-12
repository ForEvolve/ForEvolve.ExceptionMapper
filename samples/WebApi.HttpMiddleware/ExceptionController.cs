using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Shared;

[ApiController]
[Route("mvc")]
public class ExceptionController : ControllerBase
{
#pragma warning disable IDE0022 // Use block body for methods
    [HttpGet("BadRequestException")]
    public IActionResult BadRequestException() => throw new BadRequestException();

    [HttpGet("ConflictException")]
    public IActionResult ConflictException() => throw new ConflictException();

    [HttpGet("ForbiddenException")]
    public IActionResult ForbiddenException() => throw new ForbiddenException();

    [HttpGet("GoneException")]
    public IActionResult GoneException() => throw new GoneException();

    [HttpGet("NotFoundException")]
    public IActionResult NotFoundException() => throw new NotFoundException();

    [HttpGet("ResourceNotFoundException")]
    public IActionResult ResourceNotFoundException() => throw new ResourceNotFoundException(HttpContext);

    [HttpGet("UnauthorizedException")]
    public IActionResult UnauthorizedException() => throw new UnauthorizedException();



    [HttpGet("GatewayTimeoutException")]
    public IActionResult GatewayTimeoutException() => throw new GatewayTimeoutException();

    [HttpGet("InternalServerError")]
    public IActionResult InternalServerError() => throw new InternalServerErrorException(new Exception());

    [HttpGet("ServiceUnavailableException")]
    public IActionResult ServiceUnavailableException() => throw new ServiceUnavailableException();



    [HttpGet("ImATeapotException")]
    public IActionResult ImATeapotException() => throw new ImATeapotException();

    [HttpGet("MyForbiddenException")]
    public IActionResult MyForbiddenException() => throw new MyForbiddenException();

    [HttpGet("MyNotFoundException")]
    public IActionResult MyNotFoundException() => throw new MyNotFoundException();

    [HttpGet("MyUnauthorizedException")]
    public IActionResult MyUnauthorizedException() => throw new MyUnauthorizedException(Random.Shared.Next(100) % 2 == 0 ? "John" : "Jane");



    [HttpGet("Fallback")]
    public IActionResult Fallback() => throw new Exception("An error that gets handled by the fallback handler.");

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

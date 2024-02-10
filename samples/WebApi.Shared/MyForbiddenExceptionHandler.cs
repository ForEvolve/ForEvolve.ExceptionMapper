using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Http;

namespace WebApi.Shared;

public class MyForbiddenExceptionHandler : ExceptionHandler<MyForbiddenException>
{
    public override int StatusCode => StatusCodes.Status403Forbidden;
}

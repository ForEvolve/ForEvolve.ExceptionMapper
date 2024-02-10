using ForEvolve.ExceptionMapper;

namespace WebApi.Shared;

public class MyUnauthorizedException : UnauthorizedException
{
    public MyUnauthorizedException(string name)
        : base($"Sorry {name}, you can't access this page.")
    {
            
    }
}

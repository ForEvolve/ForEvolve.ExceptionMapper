using ForEvolve.ExceptionMapper;

namespace WebApi.Shared;

public class MyNotFoundException : NotFoundException
{
    public MyNotFoundException()
        : base("These aren't the droids we're looking for.")
    {
            
    }
}

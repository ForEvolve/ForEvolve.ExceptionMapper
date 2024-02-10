using ForEvolve.ExceptionMapper;

namespace WebApi.Shared;

public class DroidNotFoundException : NotFoundException
{
    public DroidNotFoundException()
        : base("These aren't the droids we're looking for.")
    {
            
    }
}

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionMapper.Handlers
{
    public class ConflictExceptionHandler : ExceptionHandler<ConflictException>
    {
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}

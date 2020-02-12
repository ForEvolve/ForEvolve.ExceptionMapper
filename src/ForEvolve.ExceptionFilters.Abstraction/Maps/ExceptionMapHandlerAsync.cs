using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public delegate Task ExceptionMapHttpHandlerAsync(HttpContext httpContext);
}

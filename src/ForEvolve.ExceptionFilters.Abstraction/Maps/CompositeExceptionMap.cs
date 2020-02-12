using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public class CompositeExceptionMap<TException> : ExceptionMap
    {
        private readonly IEnumerable<ExceptionMap> _maps;

        public CompositeExceptionMap(params ExceptionMap[] maps)
            : base(typeof(TException).FullName)
        {
            _maps = maps ?? throw new ArgumentNullException(nameof(maps));
        }

        public override async Task ExecuteAsync(HttpContext httpContext)
        {
            foreach (var map in _maps)
            {
                await map.ExecuteAsync(httpContext);
            }
        }
    }
}

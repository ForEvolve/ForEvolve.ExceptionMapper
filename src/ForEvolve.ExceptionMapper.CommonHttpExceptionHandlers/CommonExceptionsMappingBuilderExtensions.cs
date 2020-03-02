using ForEvolve.ExceptionMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommonHttpExceptionHandlersMappingBuilderExtensions
    {
        /// <summary>
        /// Registers all <see cref="IExceptionHandler"/> found in the assembly <see cref="ForEvolve.ExceptionMapper"/>
        /// with singleton lifetime.
        /// </summary>
        public static IExceptionMappingBuilder MapCommonHttpExceptionHandlers(this IExceptionMappingBuilder builder, Action<FallbackExceptionHandlerOptions> setup = null)
        {
            if (setup != null) { builder.Services.Configure(setup); }
            return builder.ScanHandlersFrom(
                s => s.FromAssembliesOf(typeof(CommonHttpExceptionHandlersMappingBuilderExtensions)),
                ServiceLifetime.Singleton
            );
        }
    }
}

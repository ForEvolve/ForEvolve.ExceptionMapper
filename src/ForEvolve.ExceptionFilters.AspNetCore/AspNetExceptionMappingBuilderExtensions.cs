using ForEvolve.ExceptionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AspNetExceptionMappingBuilderExtensions
    {
        /// <summary>
        /// Registers all <see cref="IExceptionHandler"/> found in the assembly <see cref="ForEvolve.ExceptionFilters.Abstraction"/>
        /// with singleton lifetime.
        /// </summary>
        public static IExceptionMappingBuilder AddDefaultHandlers(this IExceptionMappingBuilder builder)
        {
            builder.Services.Scan(s => s
                .FromAssemblyOf<IExceptionHandler>()
                .AddClasses(f => f.AssignableTo<IExceptionHandler>())
                .As<IExceptionHandler>()
                .WithSingletonLifetime()
            );
            return builder;
        }

        /// <typeparam name="T">The type in which assembly that should be scanned.</typeparam>
        public static IExceptionMappingBuilder ScanHandlersFromAssemblyOf<T>(this IExceptionMappingBuilder builder, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            builder.Services.Scan(s => s
                .FromAssemblyOf<T>()
                .AddClasses(f => f.AssignableTo<IExceptionHandler>())
                .As<IExceptionHandler>()
                .WithLifetime(lifetime)
            );
            return builder;
        }
    }
}

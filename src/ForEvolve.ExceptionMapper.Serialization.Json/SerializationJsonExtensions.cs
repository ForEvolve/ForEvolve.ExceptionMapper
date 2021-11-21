using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ForEvolve.ExceptionMapper.Serialization.Json
{
    public static class SerializationJsonExtensions
    {
        public static IExceptionMappingBuilder SerializeAsProblemDetails(this IExceptionMappingBuilder builder)
            => SerializeAsProblemDetails(builder, new ProblemDetailsSerializationOptions());

        public static IExceptionMappingBuilder SerializeAsProblemDetails(this IExceptionMappingBuilder builder, ProblemDetailsSerializationOptions options)
        {
            builder.Services.AddSingleton(options);
            return builder.AddExceptionHandler<ProblemDetailsSerializationHandler>();
        }

        public static IExceptionMappingBuilder SerializeAsProblemDetails(this IExceptionMappingBuilder builder, IConfiguration configuration)
        {
            builder.Services
                .Configure<ProblemDetailsSerializationOptions>(configuration)
                .AddSingleton(ctx => ctx.GetService<IOptionsMonitor<ProblemDetailsSerializationOptions>>().CurrentValue)
                .AddMvcCore() // Workaround
            ;
            return builder.AddExceptionHandler<ProblemDetailsSerializationHandler>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ForEvolve.ExceptionMapper.Serialization.Json;

public static class SerializationJsonExtensions
{
    public static IExceptionMappingBuilder SerializeAsProblemDetails(this IExceptionMappingBuilder builder)
        => SerializeAsProblemDetails(builder, new ProblemDetailsSerializationOptions());

    public static IExceptionMappingBuilder SerializeAsProblemDetails(this IExceptionMappingBuilder builder, ProblemDetailsSerializationOptions options)
    {
        builder.Services.AddSingleton(options);
        return builder.SerializeAsProblemDetailsCore();
    }

    public static IExceptionMappingBuilder SerializeAsProblemDetails(this IExceptionMappingBuilder builder, IConfiguration configuration)
    {
        builder.Services
            .Configure<ProblemDetailsSerializationOptions>(configuration)
            .AddSingleton(ctx => ctx.GetRequiredService<IOptionsMonitor<ProblemDetailsSerializationOptions>>().CurrentValue)
        ;
        return builder.SerializeAsProblemDetailsCore();
    }

    private static IExceptionMappingBuilder SerializeAsProblemDetailsCore(this IExceptionMappingBuilder builder)
    {
        builder.Services.AddMvcCore(); // Workaround
#if NET7_0_OR_GREATER
        builder.Services.AddProblemDetails();
#endif
        return builder.AddExceptionHandler<ProblemDetailsSerializationHandler>();
    }
}

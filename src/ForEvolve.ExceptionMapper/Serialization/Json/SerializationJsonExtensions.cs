using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
#if NET7_0_OR_GREATER
        builder.Services.AddProblemDetails();
#endif
        // Workaround: binding a local copy of the DefaultProblemDetailsFactory because the .NET class is internal.
        // Moreover, the only way to add the class is by calling the AddMvcCore method, which add way more services.
        // So until we can add the DefaultProblemDetailsFactory
        builder.Services.TryAddSingleton<ProblemDetailsFactory, DefaultProblemDetailsFactory>();

        return builder.AddExceptionHandler<ProblemDetailsSerializationHandler>();
    }
}

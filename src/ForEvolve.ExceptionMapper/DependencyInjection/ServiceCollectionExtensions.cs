using ForEvolve.ExceptionMapper;
using ForEvolve.ExceptionMapper.Handlers.Fallback;
using ForEvolve.ExceptionMapper.Serialization;
using ForEvolve.ExceptionMapper.Serialization.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExceptionFiltersExtensions
{
    public static IServiceCollection AddExceptionMapper(this IServiceCollection services, IConfiguration configuration, Action<IExceptionMappingBuilder>? exceptionMappingBuilder = null)
    {
        services.AddSingleton<ExceptionHandlerCollection>();
        services.AddSingleton<ExceptionMapperOptions>();
        services.TryAddSingleton<IExceptionHandlingManager, ExceptionHandlingManager>();
        services.AddSerializationHandler(configuration);

        var builder = new ExceptionMappingBuilder(services);
        builder
            // Common client exceptions
            .Map<BadRequestException>().ToStatusCode(StatusCodes.Status400BadRequest)
            .Map<ConflictException>().ToStatusCode(StatusCodes.Status409Conflict)
            .Map<ForbiddenException>().ToStatusCode(StatusCodes.Status403Forbidden)
            .Map<GoneException>().ToStatusCode(StatusCodes.Status410Gone)
            .Map<NotFoundException>().ToStatusCode(StatusCodes.Status404NotFound)
            .Map<ResourceNotFoundException>().ToStatusCode(StatusCodes.Status404NotFound)
            .Map<UnauthorizedException>().ToStatusCode(StatusCodes.Status401Unauthorized)

            // .NET exceptions
            .Map<BadHttpRequestException>().ToStatusCode(StatusCodes.Status400BadRequest)
            .Map<NotImplementedException>().ToStatusCode(StatusCodes.Status501NotImplemented)

            // Common server exceptions
            .Map<GatewayTimeoutException>().ToStatusCode(StatusCodes.Status504GatewayTimeout)
            .Map<InternalServerErrorException>().ToStatusCode(StatusCodes.Status500InternalServerError)
            .Map<ServiceUnavailableException>().ToStatusCode(StatusCodes.Status503ServiceUnavailable)
        ;
        exceptionMappingBuilder?.Invoke(builder);
        services.AddFallbackExceptionHandler(configuration, builder);
        return services;
    }

    public static WebApplicationBuilder AddExceptionMapper(this WebApplicationBuilder builder, Action<IExceptionMappingBuilder>? exceptionMappingBuilder = null)
    {
        AddExceptionMapper(builder.Services, builder.Configuration, exceptionMappingBuilder);
        return builder;
    }

    private static void AddSerializationHandler(this IServiceCollection services, IConfiguration configuration)
    {
#if NET7_0_OR_GREATER
        services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.DictionaryKeyPolicy = options.SerializerOptions.PropertyNamingPolicy;
        });
#endif
        services
            .AddOptions<ProblemDetailsSerializationOptions>()
            .Bind(configuration.GetSection("ExceptionMapper:ProblemDetailsSerialization"))
            .ValidateOnStart()
        ;
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<ProblemDetailsSerializationOptions>>().Value);
#if NET7_0_OR_GREATER
        services.AddProblemDetails();
#endif
        // Workaround: binding a local copy of the DefaultProblemDetailsFactory because the .NET class is internal.
        // Moreover, the only way to add the class is by calling the AddMvcCore method, which add way more services.
        // So until we can add the DefaultProblemDetailsFactory
        services.TryAddSingleton<ProblemDetailsFactory, DefaultProblemDetailsFactory>();
        services.TryAddSingleton<IExceptionSerializer, ProblemDetailsSerializationHandler>();
    }

    private static void AddFallbackExceptionHandler(this IServiceCollection services, IConfiguration configuration, IExceptionMappingBuilder builder)
    {
        services
            .AddOptions<FallbackExceptionHandlerOptions>()
            .Bind(configuration.GetSection("ExceptionMapper:FallbackExceptionHandler"))
            .ValidateOnStart()
        ;
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<FallbackExceptionHandlerOptions>>().Value);
        builder.AddExceptionHandler<FallbackExceptionHandler>();
    }

    public static Map<TException> Map<TException>(this IExceptionMappingBuilder builder)
        where TException : Exception
    {
        return new Map<TException>(builder);
    }

    public static IExceptionMappingBuilder ToStatusCode<TException>(this Map<TException> map, int expectedStatusCode)
        where TException : Exception
    {
        map.Builder.Services.AddSingleton<IExceptionHandler>(new StatusCodeExceptionHandler<TException>(expectedStatusCode));
        return map.Builder;
    }

    public static IExceptionMappingBuilder AddExceptionHandler<THandler>(this IExceptionMappingBuilder builder)
        where THandler : class, IExceptionHandler
    {
        builder.Services.AddSingleton<IExceptionHandler, THandler>();
        return builder;
    }

    public static IExceptionMappingBuilder AddExceptionHandler<THandler>(this IExceptionMappingBuilder builder, THandler handler)
        where THandler : class, IExceptionHandler
    {
        builder.Services.AddSingleton<IExceptionHandler>(handler);
        return builder;
    }
}

public class Map<TException>
    where TException : Exception
{
    public Map(IExceptionMappingBuilder builder)
    {
        Builder = builder;
    }

    public IExceptionMappingBuilder Builder { get; }
}

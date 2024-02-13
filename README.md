# ExceptionMapper

![Build, Test, and Deploy](https://github.com/ForEvolve/ForEvolve.ExceptionMapper/workflows/Build,%20Test,%20and%20Deploy/badge.svg)
[![NuGet.org](https://img.shields.io/nuget/vpre/ForEvolve.ExceptionMapper)](https://www.nuget.org/packages/ForEvolve.ExceptionMapper/)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fexception-mapper%2Fshield%2FForEvolve.ExceptionMapper%2Flatest)](https://f.feedz.io/forevolve/exception-mapper/packages/ForEvolve.ExceptionMapper/latest/download)

A simple ASP.NET Core middleware that intercepts and reacts to `Exception`.
You can map specific exception types to HTTP Status Codes, use predefined handlers, or create your own.

You can throw an exception from anywhere in your codebase and ExceptionMapper will handle it according to your specifications.
This makes it a breeze to uniformize exception handling in a REST API.

All the handlers are iterated through, in order, so you can build a pipeline to handle exceptions where multiple handlers have a single responsibility.
For example, you could have handlers that respond to certain exception types, then one or more fallback handlers that react only if no previous handler handled the exception.

Finally, there is a serializer that converts handled exceptions to JSON, in the format of your choice, making your API linear between endpoints and exception types without much effort. The default serializer converts the errors to [Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc7807).

## How to install

Add a reference to the `ForEvolve.ExceptionMapper` NuGet package:

```bash
dotnet add package ForEvolve.ExceptionMapper
```

_You can take a look at the `samples/WebApiSample` project for a working example._

## Getting started

You must register the services, optionally configure/register handlers, and use the middleware that catches exceptions (and that handles the logic).

**Program.cs**

```csharp
// Add the dependencies to the container
builder.AddExceptionMapper();

// Register the middleware
app.UseExceptionMapper();
```

**Startup.cs**

```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // ...
        services.AddExceptionMapper(Configuration);
        // ...
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //...
        app.UseExceptionMapper(); // Register the middleware
        //...
    }
}
```

## Extending the existing exception

An easy way to manage your custom exceptions is to inherit from the ones that are already mapped.
For example, you could create and throw the following `DroidNotFoundException` and ExceptionMapper will associate it with a 404 Not Found status code because it inherits from `NotFoundException`:

```csharp
public class DroidNotFoundException : NotFoundException
{
    public DroidNotFoundException()
        : base("These aren't the droids we're looking for.")
    {
    }
}
```

## Mapping Exception types to status code

If you do not want or can't inherit the provided exceptions, you can map any Exception types to a specific status code, like this:

```csharp
builder.AddExceptionMapper(builder =>
{
    builder.Map<ImATeapotException>().ToStatusCode(StatusCodes.Status418ImATeapot);
});

public class ImATeapotException : Exception
{
}
```

## Creating a custom exception handler

If the previous options are not enough to handle your use case, you can implement a custom exception handler.
To do so, you can inherit from the `ExceptionHandler<TException>` or implement the `IExceptionHandler` interface.
Then you must register it using the `AddExceptionHandler` extension method.

Let's start by creating an exception:

```csharp
public class MyForbiddenException : Exception { /* Omitted implementation */ }
```

Then create the handler:

```csharp
public class MyForbiddenExceptionHandler : ExceptionHandler<MyForbiddenException>
{
    public override int StatusCode => StatusCodes.Status403Forbidden;
}
```

Finally, you must register it:

```csharp
builder.AddExceptionMapper(builder =>
{
    builder.AddExceptionHandler<MyForbiddenExceptionHandler>();
});
```

## Updating the type URI

You can customize the `type` property of the problem details object by configuring the `ApiBehaviorOptions` object, like this:

```csharp
builder.Services
    .Configure<ApiBehaviorOptions>(options =>
    {
        options.ClientErrorMapping.Add(StatusCodes.Status409Conflict, new ClientErrorData
        {
            Link = "https://localhost:8828/Status409Conflict", // This is taken into account because the middleware do not set any link by default.
            Title = "This will not be displayed." // Not taken into account because the middleware sets the title to the Exception.Message value.
        });
    })
;
```

If using MVC the mappings should be registered so you may want to modify the data instead:

```csharp
builder.Services
    .Configure<ApiBehaviorOptions>(options =>
    {
        options.ClientErrorMapping[StatusCodes.Status409Conflict].Link = "https://localhost:8828/Status409Conflict";
    })
;
```

# Common Exceptions

ExceptionMapper implements different common exceptions and their handlers, like the following maps (namespace `ForEvolve.ExceptionMapper`):

```csharp
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
```

# Fallback handler

ExceptionMapper also comes with a fallback handler that converts unhandled exceptions to `500 InternalServerError`. This is an opt-out feature, configured by the `FallbackExceptionHandlerOptions`.

You can also configure the `FallbackExceptionHandlerOptions` like the following or under the `ExceptionMapper:FallbackExceptionHandler` key in your settings:

```csharp
services.Configure<FallbackExceptionHandlerOptions>(options =>
{
    options.Strategy = FallbackStrategy.Handle;
});
```

# Serialization (Json)

ExceptionMapper provides a default implementation of the `IExceptionSerializer` interface that serializes exceptions as `ProblemDetails`.
When targetting .NET 7+, ExceptionMapper uses the `IProblemDetailsService` interface from ASP.NET Core.

If you want to customize the default serializer, you can configure the `ProblemDetailsSerializationOptions` class, like this:

```csharp
builder.Services.Configure<ProblemDetailsSerializationOptions>(options =>
{
    options.SerializeExceptions = false;
    options.DisplayDebugInformation = (ExceptionHandlingContext ctx) =>
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    };
});
```

You can also customize the options from the `appsettings.json` file:

```json
{
    "ExceptionMapper": {
        "ProblemDetailsSerialization": {
            "SerializeExceptions": false
        }
    }
}
```

Note that the serializer displays the debug information when in development.
Use the `DisplayDebugInformation` function to display the debug info in other environments, like staging or production.

## Property names

To change the way the property names are serialized, you can configure the `JsonOptions` and change the `PropertyNamingPolicy` property.

**.NET 8+**

```csharp
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper;
});
```

**.NET 6 and .NET 7**

```csharp
builder.Services.Configure<JsonOptions>(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
```

### Dictionary keys

For .NET 7+ projects, ExceptionMapper sets the `DictionaryKeyPolicy` property to the `PropertyNamingPolicy` property value, so dictionaries are serialized the same way as the normal properties.

## Ensuring a property is not serialized

If your custom exception has properties that should not be serialized, you can mark them with the `[JsonIgnore]` attribute, like the following and the serializer will ignore them:

```csharp
public class MyForbiddenException : Exception
{
    public MyForbiddenException()
        : base("Accessing this resource is forbidden.")
    {

    }

    public string CustomProperty1 => "Lorem Ipsum 1";

    [JsonIgnore]
    public string CustomProperty2 => "Lorem Ipsum 2";
}
```

## Creating your own serializer

If you want to replace the default serializer, implement the `IExceptionSerializer` and register it with the container, before calling `AddExceptionMapper`:

```csharp
services.AddSingleton<IExceptionSerializer, MySerializationHandler>();
builder.AddExceptionMapper();
```

## Versioning

The package follows _semantic versioning_ and uses `Nerdbank.GitVersioning` to automatically version packages based on git commits.

### Pre-released

Prerelease packages are packaged code not yet merged to the `main` branch.
The prerelease CI builds are packaged and hosted at [feedz.io](feedz.io), thanks to their "Open Source" subscription.

# Release notes

## 3.0

The version 3 of ExceptionMapper is a major rewrite that simplifies the codebase and usage of the library. Here are a few important changes:

-   Add support to .NET 7 and .NET 8.
-   Remove transitive dependency on JSON.NET (`Newtonsoft.Json`).
-   Drop support for .NET Standard 2.0 because `ExceptionMapper` depends on the `HttpContext` class which requires a `<FrameworkReference Include="Microsoft.AspNetCore.App" />` which is not compatible with `netstandard2.0`.
-   Merge all assemblies in `ForEvolve.ExceptionMapper` but `ForEvolve.ExceptionMapper.Scrutor` and removed `ForEvolve.ExceptionMapper.Scrutor` altogether.
-   Replace the `AddMvcCore` call by registering a copy of the `DefaultProblemDetailsFactory` using a `TryAddSingleton` call, so you must register your custom `ProblemDetailsFactory` implementation before `AddExceptionMapper`. The good news is, if you are using a custom factory, the `ProblemDetailsSerializationHandler` will use it!
    > Removing the copy of the `DefaultProblemDetailsFactory` class could be resolved by https://github.com/dotnet/aspnetcore/issues/49982
-   Calling `AddExceptionMapper()` now registers the common exceptions and the serializer automatically.
-   The `Order` property was removed from the `IExceptionHandler` interface. The system uses the registration order instead.
-   The interface now leverages a serializer implementing the `IExceptionSerializer` interface. The serializer no longer implements the `IExceptionHandler` interface.
-   By default, `ProblemDetailsSerializationOptions` is bound to the section `ExceptionMapper:ProblemDetailsSerialization` and `FallbackExceptionHandlerOptions` is bound to the section `ExceptionMapper:FallbackExceptionHandler`.

### Breaking changes .NET 7+

-   Remove the `ContentType` and `JsonSerializerOptions` properties from the `ProblemDetailsSerializationOptions` class (`ForEvolve.ExceptionMapper.Serialization.Json`).
-   The `ProblemDetailsSerializationHandler` class now leverages the `IProblemDetailsService` interface to write the `ProblemDetails` object to the response stream instead of serializing it with the `JsonSerializer`, relinquishing the control of the process to .NET.
-   The `ProblemDetailsSerializationHandler` leverages the `JsonOptions` class to ensure the names are formatted according to the `PropertyNamingPolicy` object. The default is `camelCase`.
-   ExceptionMapper sets the `DictionaryKeyPolicy` property to the `PropertyNamingPolicy` property value so dictionaries are serialized the same way as the normal properties.

## 2.0 (deprecated)

-   Drop .NET Core 3.1 support
-   Add support for .NET 6.0

## 1.1 (deprecated)

-   Add a handler that serializes exceptions to `ProblemDetails` (JSON)
-   Add the `ForEvolve.ExceptionMapper.Serialization.Json` project

## 1.0 (deprecated)

-   Initial release (not yet released).

# Found a bug or have a feature request?

Please open an issue and be as clear as possible; see _How to contribute?_ for more information.

# How to contribute?

If you would like to contribute to the project, first, thank you for your interest, and please read [Contributing to ForEvolve open source projects](https://github.com/ForEvolve/Toc/blob/master/CONTRIBUTING.md) for more information.

## Contributor Covenant Code of Conduct

Also, please read the [Contributor Covenant Code of Conduct](https://github.com/ForEvolve/Toc/blob/master/CODE_OF_CONDUCT.md) that applies to all ForEvolve repositories.

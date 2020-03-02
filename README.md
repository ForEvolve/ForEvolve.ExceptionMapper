# ForEvolve.ExceptionMapper

![Build, Test, and Deploy](https://github.com/ForEvolve/ForEvolve.ExceptionMapper/workflows/Build,%20Test,%20and%20Deploy/badge.svg)

Asp.Net Core middleware that reacts to `Exception`.
You can map certain exception types to HTTP Status Code for example.
There are a few predefined exceptions and their respective handler, but you can do whatever you need with this.

All of the handlers are iterated through, in order, so you can build a pipeline to handle exceptions where multiple handlers have a single responsibility.
For example, you could have handlers that respond to certain exception types, then one or more fallback handlers that react only if no previous handler handled the exception. Finally, there could be a serializer that convert handled exceptions to JSON, in the format of your choice, making your API linear between endpoints and exception types without much effort.

> I plan on adding serialization handlers and Asp.Net Core MVC specific tweaks in a near future.

## Versioning

The packages follows _semantic versioning_. I use `Nerdbank.GitVersioning` to automatically version packages based on git commits/hashes.

### Pre-released

Prerelease packages are packaged code not yet merged to `master`.
The prerelease packages are hosted at [feedz.io](feedz.io), thanks to their "Open Source" subscription.

[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fexception-mapper%2Fshield%2FForEvolve.ExceptionMapper%2Flatest&label=ForEvolve.ExceptionMapper)](https://f.feedz.io/forevolve/exception-mapper/packages/ForEvolve.ExceptionMapper/latest/download)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fexception-mapper%2Fshield%2FForEvolve.ExceptionMapper.CommonExceptions%2Flatest&label=ForEvolve.ExceptionMapper.CommonExceptions)](https://f.feedz.io/forevolve/exception-mapper/packages/ForEvolve.ExceptionMapper.CommonExceptions/latest/download)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fexception-mapper%2Fshield%2FForEvolve.ExceptionMapper.FluentMapper%2Flatest&label=ForEvolve.ExceptionMapper.FluentMapper)](https://f.feedz.io/forevolve/exception-mapper/packages/ForEvolve.ExceptionMapper.FluentMapper/latest/download)

### Released

The packages are published on [NuGet.org](NuGet.org).

[![NuGet.org](https://img.shields.io/nuget/vpre/ForEvolve.ExceptionMapper)](https://www.nuget.org/packages/ForEvolve.ExceptionMapper/)
[![NuGet.org](https://img.shields.io/nuget/vpre/ForEvolve.ExceptionMapper.CommonExceptions)](https://www.nuget.org/packages/ForEvolve.ExceptionMapper.CommonExceptions/)
[![NuGet.org](https://img.shields.io/nuget/vpre/ForEvolve.ExceptionMapper.FluentMapper)](https://www.nuget.org/packages/ForEvolve.ExceptionMapper.FluentMapper/)

> Nothing released yet...

## How to install

Just load one or more of these NuGet packages:

```bash
dotnet add package ForEvolve.ExceptionMapper
dotnet add package ForEvolve.ExceptionMapper.CommonExceptions
dotnet add package ForEvolve.ExceptionMapper.FluentMapper
```

# ForEvolve.ExceptionMapper

This package contains the core components like the interfaces, the middleware, etc.
You can load only this package to create your own exceptions and handlers.

_You can take a look at the `samples/WebApiSample` project for a working example._

## Getting started

You must register the services, and optionally configure/register handlers, and use the middleware that catches exceptions (and that handles the logic).

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddExceptionMapper(builder => builder
        // Configure your pipeline here
    );
}
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //...
    app.UseExceptionMapper(); // Register the middleware
    //...
}
```

## Custom exception handler (simple)

If you want to create a custom exception handler that changes the status code and you don't want to use the `FluentMapper`. Maybe you simply like types or you want to scan one or more assembly to load mappers dynamically; no matter why, you can inherit from `ExceptionHandler<TException>`.

Let's start by creating an exception:

```csharp
public class MyForbiddenException : Exception { }
```

Then create the handler:

```csharp
public class MyForbiddenExceptionHandler : ExceptionHandler<MyForbiddenException>
{
    public override int StatusCode => StatusCodes.Status403Forbidden;
}
```

Finally, we can register it:

```csharp
services.AddExceptionMapper(builder => builder
    .AddExceptionHandler<MyForbiddenExceptionHandler>()
);
```

If you want a bit more control, you can override `Task ExecuteCoreAsync(ExceptionHandlingContext<TException> context)` method and add custom code.

## Custom exception handler (complex)

If you want to create a custom exception handler, implement `IExceptionHandler`, then register that handler using the `AddExceptionHandler<THandler>()` extension method.

In our case, let's create an exception:

```csharp
public class ImATeapotException : Exception { }
```

Then the handler:

```csharp
public class ImATeapotExceptionHandler : IExceptionHandler
{
    public int Order => HandlerOrder.DefaultOrder;

    public async Task ExecuteAsync(ExceptionHandlingContext context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = StatusCodes.Status418ImATeapot;
        response.ContentType = "text/html";
        context.Result = new ExceptionHandledResult(context.Error);
        await response.WriteAsync("<html><body><pre style=\"font-family: SFMono-Regular,Menlo,Monaco,Consolas,'Liberation Mono','Courier New',monospace;\">");
        await response.WriteAsync(@"             ;,'
    _o_    ;:;'
,-.'---`.__ ;
((j`=====',-'
`-\     /
`-=-'     hjw
Source: <a href=""https://www.asciiart.eu/food-and-drinks/coffee-and-tea"" target=""_blank"">https://www.asciiart.eu/food-and-drinks/coffee-and-tea</a>");
        await response.WriteAsync("</pre></body></html>");
    }

    public Task<bool> KnowHowToHandleAsync(Exception exception)
    {
        return Task.FromResult(exception is ImATeapotException);
    }
}
```

It is very important to set the `context.Result.ExceptionHandled` to `true`. The easiest way to achieve that is to create an instance of the `ExceptionHandledResult` class.

Once that's done, we can register it:

```csharp
services.AddExceptionMapper(builder => builder
    .AddExceptionHandler<ImATeapotExceptionHandler>()
);
```

## Assembly Scanning

You can also scan assemblies for `IExceptionHandler` using `IExceptionMappingBuilder` extensions.
For example, you could scan the `Startup` assembly for handlers:

```csharp
services.AddExceptionMapper(builder => builder
    .ScanHandlersFromAssemblyOf<Startup>()
);
```

This feature uses Scrutor under the hood and exposes part of it to simplify things, so you could use it and use the `ScanHandlersFrom()` extensions that exposes the Scrutor `ITypeSourceSelector`, like:

```csharp
services.AddExceptionMapper(builder => builder
    .ScanHandlersFrom(typeSourceSelector => typeSourceSelector.FromCallingAssembly())
);
```

# ForEvolve.ExceptionMapper.CommonExceptions

This package implements different common exceptions and their handlers, like:

-   `404 NotFound` (`ForEvolve.ExceptionMapper.NotFoundException`)
-   `409 Conflict` (`ForEvolve.ExceptionMapper.ConflictException`)
-   `500 InternalServerError` (`ForEvolve.ExceptionMapper.InternalServerErrorException`)
-   `501 NotImplemented` (`System.NotImplementedException`)

It also comes with a fallback handler that convert unhandled exceptions to `500 InternalServerError`. This is an opt-in feature, configured by the `FallbackExceptionHandlerOptions`. That handler can be useful, but be careful of what you share about your exceptions in production, this could help a malicious user acquire information about your server.

To use the prebuilt handlers, you have to:

```csharp
services.AddExceptionMapper(builder => builder
    .MapCommonExceptions()
);
```

You can also configure the `FallbackExceptionHandlerOptions` during the registration or like any other options:

```csharp
services.AddExceptionMapper(builder => builder
    .MapCommonExceptions(options =>
    {
        options.Strategy = FallbackStrategy.Handle;
    })
);
// OR using the Asp.Net Core options pattern like:
services.Configure<FallbackExceptionHandlerOptions>(options =>
{
    options.Strategy = FallbackStrategy.Handle;
});
```

# ForEvolve.ExceptionMapper.FluentMapper

This package contains utilities that can be used to program exception's mapping in the Startup class, without creating any new type.

For example, to map a `MyUnauthorizedException` to a status code 401, we could do the following:

```csharp
services.AddExceptionMapper(builder => builder
    .Map<MyUnauthorizedException>(map => map.ToStatusCode(401))
);
```

The `MyUnauthorizedException` looks as simple as this:

```csharp
public class MyUnauthorizedException : Exception { }
```

The `Map<TException>()` extension has been designed to be fluent, so it returns an `IExceptionMappingBuilder`, allowing us to chain multiple calls. We can also mix and match with any other configuration helpers as they all do that.

For example, we could do the following:

```csharp
services.AddExceptionMapper(builder => builder
    .MapCommonExceptions()
    .AddExceptionHandler<ImATeapotExceptionHandler>()
    .Map<MyUnauthorizedException>(map => map.ToStatusCode(401))
    .Map<GoneException>(map => map.ToStatusCode(410))
);
```

Under the hood, the `Map<TException>()` extension creates a `FluentExceptionHandler<TException>` that is configurable. You can append, prepend or replace handler actions.

# Release notes

## 1.0.0

-   Initial release

# Future/To do

Here is a list of what I want to do:

-   [ ] Create a MVC/Razor Pages filter that could replace the middleware or work in conjunction of it, adding more control over the process for MVC applications.
    -   [x] Create a `ForEvolve.ExceptionMapper.Mvc` project that contains MVC/Razor Pages filter/code.
-   [ ] Add one or more serialization handlers that at least support JSON serialization and that leverage `ProblemDetailsFactory` to create `ProblemDetails` objects.
-   [x] Samples
    -   [x] Rename `WebApiSample` to `WebApi.HttpMiddleware`.
    -   [x] Create a new project name `WebApi.Mvc` that uses the `ForEvolve.ExceptionMapper.Mvc` filter instead of `ForEvolve.ExceptionMapper.HttpMiddleware`.
-   [ ] Rearrange assemblies:
    -   [x] Move Http/Middleware out of `ForEvolve.ExceptionMapper`, into `ForEvolve.ExceptionMapper.HttpMiddleware` assembly.
    -   [x] Move the `Scrutor` dependency out of `ForEvolve.ExceptionMapper` to `ForEvolve.ExceptionMapper.FluentMapper` or create a new assembly for that (`ForEvolve.ExceptionMapper.Scrutor`).
    -   [x] Rename `ForEvolve.ExceptionMapper` to `ForEvolve.ExceptionMapper.Core`.
    -   [x] Create a new `ForEvolve.ExceptionMapper` that includes all packages.
    -   [ ] `ForEvolve.ExceptionMapper.CommonExceptions` could be renamed `ForEvolve.ExceptionMapper.CommonHttpExceptionHandlers` and the exceptions could be moved in another assembly, like in `ForEvolve.ExceptionMapper.Core` or a new `ForEvolve.ExceptionMapper.CommonExceptions` project. This would allow reusing the exceptions for both Http/Middleware and Mvc/Razor Page scenarios, without the need to load every handlers if not needed.
-   [ ] Write tests that covers `ForEvolve.ExceptionMapper.FluentMapper` and other missing pieces

# Found a bug or have a feature request?

Please open an issue and be as clear as possible; see _How to contribute?_ for more information.

# How to contribute?

If you would like to contribute to the project, first, thank you for your interest, and please read [Contributing to ForEvolve open source projects](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CONTRIBUTING.md) for more information.

## Contributor Covenant Code of Conduct

Also, please read the [Contributor Covenant Code of Conduct](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CODE_OF_CONDUCT.md) that applies to all ForEvolve repositories.


<!-- PROJECT LOGO -->
<br />
<br />
<div align="center">
  <a href="https://github.com/jomaxso/Segres/">
    <img src="#" alt="Segres" width="125">
  </a>

<h2 align="center">SEGRES</h2>

  <p align="center">
    The simple way to segerate your responsibilities
.
<br />
<br />
    <a href="#"><strong>Explore the docs »</strong></a>
<br />
<br />
  </p>

  <p align="center">
    <a href="#">View Demo</a>
    ·
    <a href="https://github.com/jomaxso/Segres/issues">Report Bug</a>
    ·
    <a href="https://github.com/jomaxso/Segres/issues">Request Feature</a>
  </p>

[![NuGet version](https://badgen.net/nuget/v/Segres)](https://www.nuget.org/packages/Segres/)
[![NuGet downloads](https://badgen.net/nuget/dt/Segres)](https://www.nuget.org/packages/Segres/)
[![Test status](https://badgen.net/github/checks/jomaxso/Segres/master/test)](https://www.nuget.org/packages/Segres/)




</div>

<!-- ABOUT THE PROJECT -->

A mediator library for .Net using strongly-typed handler implementations. It provides a synchronise and asynchronise api, which is optimized for speed and memory.


<!-- GETTING STARTED -->

## Getting Started

Segres can be installed using the Nuget package manager or the dotnet CLI.

```shell
dotnet add package Segres 
```

<a href="#">Explore the documentation</a> for instructions on how to use the package.



<!-- USAGE EXAMPLES -->

## Example


### Register all dependencies

```csharp
// Program.cs

using Segres

...

builder.Services.AddSegres(); 

// or

builder.Services.AddSegres(options =>
{
    options.UseReferencedAssemblies(typeof(Program));
    options.UseLifetime(ServiceLifetime.Scoped);
    ...
});
```

### Create a handler

```csharp
// CreateCustomerRequestHandler.cs

using Segres;

public record CreateCustomerRequest(string Firstname, string Lastname) : IRequest<Guid>;

public sealed class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerRequest, Guid>
{
    public async ValueTask<Guid> HandleAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        await ValueTask.CompletedTask;
        return Guid.NewGuid();
    }
} 
```

### Send a request

```csharp
// SomeService.cs

using Segres;

public class SomeService
{
    private readonly IMediator _mediator;

    public SomeService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    ...
    
    public async ValueTask<Guid> SomeService(CancellationToken cancellationToken)
    {
        var request = new CreateCustomerRequest("Peter", "Parker");
        Guid id = await _mediator.SendAsync(request, cancellationToken);
        return id;
    }
}

```


_For more examples, please refer to the [Documentation](#)_


<!-- LICENSE -->

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

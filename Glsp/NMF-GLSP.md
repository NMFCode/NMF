# NMF GLSP

NMF GLSP is an implementation of the GLSP protocol by tailoring NMFs synchronization technology to graphical editors using the Graphical Language Server Protocol (GLSP).

## When to use this package?

This package contains wrapper classes to expose the functionality of NMF GLSP through an ASP.NET Core Webserver project.

The minimal code to start a GLSP server for a given class `YourLanguage` that implements your graphical DSL is the following:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebSockets(opts => { });
builder.Services.AddGlspServer();
builder.Services.AddLanguage<YourLanguage>();

var app = builder.Build();
app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");

await app.RunAsync();
```

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
# NMF AnyText ASP.NET Core

This package contains the AnyText bindings for ASP.NET Core.

## Why should I use this package?

This package simplifies the development of LSP (or combined LSP/GLSP) servers using ASP.NET Core. The server will use web socket as transport for the LSP server,
which allows to run multiple servers (in particular: LSP+GLSP) in parallel on the same server.

For instance, the following snippet shows how to start a server with multiple languages, running LSP and GLSP in parallel:

````csharp
builder.Services.AddWebSockets(opts => { });
builder.Services.AddSynchronizingLspServer();
builder.Services.AddLanguage<StateMachineGrammar>();
builder.Services.AddLanguage<PetriNetGrammar>();

builder.Services.AddSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(".anyfsm", ".anypn", 
    isAutomatic: true);
builder.Services.AddSynchronization<INamespace>(".anymeta", ".nmeta");

builder.Services.AddGlspServer();
```

The C# compiler is able to run the correct extension method `AddLanguage`, depending on whether the target language is textual or graphical.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials.
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).

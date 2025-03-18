# NMF AnyText

This package contains the core AnyText library, an incremental packrat parser with support for left recursive grammars and optimized for the Language Server Protocol (LSP).

## Why you should use this package

This package contains helper classes to expose an AnyText parser as an LSP server.

The simplest form of an LSP server is to use the `Bootstrapper` class that provides an API to start and run an LSP server with a single line of code:

````csharp
await Bootstrapper.RunLspServerOnStandardInStandardOutAsync(yourGrammar);
```

In this listing, note that `yourGrammar` is a reference to an instance of the grammar class. You can create such a grammar class
and the metamodel underneath using the tool `NMF-AnyTextGen` from a **.anytext** file.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
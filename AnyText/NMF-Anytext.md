# NMF AnyText

This package contains the core AnyText library, an incremental packrat parser with support for left recursive grammars and optimized for the Language Server Protocol (LSP).

## Why should I use this package?

This package contains the AnyText metamodel and some helper classes used by the tooling to use the NMF modeling foundation with AnyText.
Hence, when generating your grammar from a .anytext file, you will need this package as a runtime dependency rather than `NMF-AnyText-Core`.

Use this package as a top-level reference in your project if you are consuming DSL instances in an application that needs to parse DSLs built using AnyText,
use the `NMF-AnyText-LSP` for the LSP integration.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials.
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).

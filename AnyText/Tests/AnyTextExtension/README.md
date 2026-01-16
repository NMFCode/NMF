# AnyText Extension

This extension includes editor services for AnyText and AnyMeta.

## AnyText

NMF AnyText is a domain-specific language (DSL) to specify extended parsing expression grammars (PEGs) and hence, to derive a parser, a pretty-printer and language services for a custom DSL.

As a starting point for your new DSL, we have a Yeoman Generator, available via `npm install --global generator-anytext`. We recommend a global installation of this tool, otherwise you will need a workspace to execute it. Once you installed this code generator, you can launch it via `yo anytext`. This code generator will create a template folder for a new DSL and a Visual Studio Code extension for it.

## AnyMeta

AnyMeta is a DSL to specify metamodels according to the NMeta meta-metamodel. From this metamodel, one can generate code that respects the rules defined by the metamodel, including bidirectional references, compositions, cross-references, refinements and decomposition.

## Why should I use this package?

Of course, you can edit your AnyText and AnyMeta documents as plain text, but this Visual Studio Code extension will make your life so much easier when working with these technologies.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials.
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).

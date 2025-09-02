# NMF Transformations

NMF Transformations (NTL) is a model transformation language implemented in an internal DSL.

## When to use this package?

Use this package if you need to generate code that matches one of the needs below:

- References to other types must be simplified (e.g., through using-statements) but naming conflicts must not occur. In case of naming conflicts, type references must be fully qualified.
- The concepts from which code is generated logically contains multiple inheritance which must be resolved to interface implementation. 

The idea of this package is to provide support for the above when generating code based on `System.CodeDOM` that can be reused independent from the context.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
# NMF Expressions

NMF Expressions is a library for building dynamic dependency graphs to support incremental change propagation.

## Why should I use this package?

This package contains a few utility classes that can be useful when working with NMF Expressions:

- `Binding` denotes a binding of a property to a given expression and provides a generic API for starting and stopping the binding.
- `ChangeAwareDictionary` is a hashtable implementation that notifies clients when the value for a given key changes
- `IncrementalFunc` is an alternative to `ObservingFunc` that reuses generated dependency graphs for the same inputs

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
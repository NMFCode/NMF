# NMF Expressions

NMF Expressions is a library for building dynamic dependency graphs to support incremental change propagation.

## Why should I use this package?

This package contains a library to execute queries and expressions in .NET with incremental change propagation by constructing a dynamic dependency graph (DDG).

For collections, we introduce two new monads:

- `INotifyEnumerable<T>` essentially combines `IEnumerable<T>` with `INotifyCollectionChanged`, allowing queries over collections that support change propagation
- `IEnumerableExpression<T>` behaves like `IEnumerable<T>` but has a dedicated method `AsNotifiable` to switch to incremental change propagation. This is meant for scenarios where the decision for or against incremental change propagation is delayed to runtime.

For both of these monads, many of the standard query operators are implemented, allowing you to obtain incremental change propagation e.g. for C# queries.

To support `ObservableCollection<T>`, there is also an extension method `WithUpdates` that tries to cast a collection to `INotifyCollectionChanged` in order to provide change notifications.

However, the library is not limited to queries, you can also obtain incremental change propagation for arbitrary functions 
using the classes `ObservingFunc`. These are a bit like the framework `Func` classes except that besides `Invoke`, 
they offer an `Observe` method that uses change notifications picked up using `INotifyPropertyChanged` and explicit 
implementations of dynamic algorithms to obtain efficient DDGs.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
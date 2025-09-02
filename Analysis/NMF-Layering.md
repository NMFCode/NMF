# NMF Analysis

NMF Analysis is a zoo of commonly used algorithms for analysis.

## Why should I use this package?

This package mainly contains an implementation of Tarjans algorithm to calculate strongly connected components in a graph.
The most common use case for this is to identify cycles (i.e., components with a size larger than 1).

The functionality is contained in the class `Layering` in the following two static methods:

- `public static IList<ICollection<T>> CreateLayers(IEnumerable<T> nodes, Func<T, IEnumerable<T>> edges)` calculates the components in topological order that are given by the given nodes and edges
- `public static IList<ICollection<T>> CreateLayers(T root, Func<T, IEnumerable<T>> edges)` does the same but for a single root node

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
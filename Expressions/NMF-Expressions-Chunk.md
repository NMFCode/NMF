# NMF Expressions

NMF Expressions is a library for building dynamic dependency graphs to support incremental change propagation.

## Why should I use this package?

This package contains a chunking operation and its incrementalization. In particular, it provides the operation

````csharp
public static IEnumerableExpression<TResult> Chunk<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector )
```

Variants using other monads (`IEnumerable<T>`, `INotifyEnumerable<T>`) or indexed chunks also exist. Optionally, a balancing strategy can be provided in
order to specify how the incremental version of the algorithm makes sure that the chunks are balanced.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
# NMF Expressions

NMF Expressions is a library for building dynamic dependency graphs to support incremental change propagation.

## Why should I use this package?

This package adds some debugging capabilities to NMF expressions, in particular to export a dynamic dependency graph to a .dgml-file that can
be viewed by Visual Studio in order to understand the topology of the generated dependency graph at a given point in time.

For this, there is an extension method `public static void Visualize(this INotifiable node)` that exports the given graph as DGML and runs the generated file,
e.g. opening the export in Visual Studio. A separate class `DgmlExporter` can be used to use the exporting functionality elsewhere.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
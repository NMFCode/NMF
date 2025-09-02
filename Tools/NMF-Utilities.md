# NMF

This package contains some utility functionalities that are needed in other parts of NMF.

## Why should I use this package?

The provided utility functions are:

- `ItemEqualityComparer` is a generic comparer that compares arrays based on the equality of their items
- `MemoizedFunc` is a class that executes functions but memoizes results such they can be reused independently
- `Extensions` contains a few helper methods:
  - `As` exposes the C# `as` operator as a method, useful since `System.CodeDOM` does not support this operator
  - `Closure` completes the closure of an operation
  - `DepthOfTree` calculates the depth of a tree
  - `ToPascalCase` and `ToCamelCase` calculate the PascalCase or camelCase version of a string
  - There are some further overloads to existing methods that make calling these more convenient

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
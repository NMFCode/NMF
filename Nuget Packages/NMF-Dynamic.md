# NMF Dynamic

This package contains two public classes to support dynamic models, i.e. loading models where the metamodel is not known at compile-time and no code can be generated for these models.
Nevertheless, the core class `DynamicModelElement` supports all the generic APIs to read and write attributes and references. 

## Why should I use this package?

Use this package if you need to work with models with metamodels not known at compile-time. Your entry-point will be the following two classes:

- `DynamicModelElement` is used to represent a model element with a class known only at compile-time in memory.
- `DynamicModelSerializer` is used to serialize dynamic models from and into XMI. This class uses a `INamespace` as a constructor parameter.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
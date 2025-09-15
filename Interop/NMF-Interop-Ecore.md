# NMF Interop

NMF Interop contains a range of packages that provide interoperability of NMF with other modeling foundations.

## Why should I use this package?

This package contains classes to load, save and transform Ecore metamodels to NMeta metamodels. The transformation from Ecore to NMeta is extensible
and can be customized for whatever purpose requires it.

The most commonly used functionality is bundled in the facade class `EcoreInterop`:

- `public static EPackage LoadPackageFromFile(string path)` loads an EPackage from a file
- `public static INamespace Transform2Meta(EPackage package, Action<IEPackage, INamespace> additionalPackageRegistry = null)` transforms a package into an NMeta metamodel

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
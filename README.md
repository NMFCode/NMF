# .NET Modeling Framework (NMF)

The .Net Modeling Framework (NMF) is a framework to support model-driven engineering using .NET technologies. It consists of several subprojects that ease various model-driven tasks, such as generating code for model representation, or languages to support model transformation, synchronization and optimization.

## NuGet Packages

There are a range of NuGet-packages available that bundle some of the functionality of NMF. These packages are potentially more stable than the source code, but may not contain the latest features of NMF.
The packages are:
* NMF-Expressions [![Downloads](https://img.shields.io/nuget/dt/NMF-Expressions.svg)](https://www.nuget.org/packages/NMF-Expressions/): Contains the NMF library for incremental computation. This package has no other dependencies and can even be used independently of the modeling framework.
* NMF-Repository [![Downloads](https://img.shields.io/nuget/dt/NMF-Repository.svg)](https://www.nuget.org/packages/NMF-Repository/): Conatins the repository management of NMF (depends on NMF-Expressions)
* NMF-Dynamics [![Downloads](https://img.shields.io/nuget/dt/NMF-Dynamic.svg)](https://www.nuget.org/packages/NMF-Dynamic/): Conatins classes to load models without code generation (depends on NMF-Repository)
* NMF-Basics [![Downloads](https://img.shields.io/nuget/dt/NMF-Basics.svg)](https://www.nuget.org/packages/NMF-Basics/): Contains the latter two plus a code generator to generate model representation code from any Ecore or NMeta metamodel. The code generator integrates with the Nuget Package Console.
* NMF-Expressions-Utilities [![Downloads](https://img.shields.io/nuget/dt/NMF-Expressions-Utilities.svg)](https://www.nuget.org/packages/NMF-Expressions-Utilities/): Contains some utility classes for NMF Expressions such as dictionary of incremental values
* NMF-Transformations [![Downloads](https://img.shields.io/nuget/dt/NMF-Transformations.svg)](https://www.nuget.org/packages/NMF-Transformations/): Contains the model transformation language NTL used in NMF
* NMF-Synchronizations [![Downloads](	https://img.shields.io/nuget/dt/NMF-Synchronizations.svg)](https://www.nuget.org/packages/NMF-Synchronizations/): Contains the incremental, uni- and bidirectional model transformation language NMF Synchronizations

## Tutorials

* [NMF Transformations](https://nmfcode.github.io/NMFdocs/transformations/TransformationTutorials.html)
* [NMF Synchronizations](https://nmfcode.github.io/NMFdocs/synchronizations/SynchronizationTutorials.html)

## Detailed Project information

Detailed project information to the sub-projects of NMF can be found on the wiki pages in the website of NMF: https://nmfcode.github.io/NMFdocs/

## Publications
The publications about NMF are available on https://sdqweb.ipd.kit.edu/publications/topics/nmf.html

## Contribute

Developers are dear welcome to contribute to NMF in various ways. Please just write an Email to [Georg Hinkel](mailto:georg.hinkel@gmail.com).
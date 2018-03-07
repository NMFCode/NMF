# .NET Modeling Framework (NMF) [![Build status](https://ci.appveyor.com/api/projects/status/jbma22noos4y712w?svg=true)](https://ci.appveyor.com/project/georghinkel/nmf) [![Version](https://img.shields.io/nuget/v/NMF-Basics.svg)]

This repository contains the entire code for the .NET Modeling Framework. The development of NMF started in about 2011 as a student project of Georg Hinkel. Since then, it has grown to a large development framework and multiple papers have been published on NMF. The source code used to be hosted under Codeplex but now has been migrated to Github.

## NuGet Packages
There are currently three NuGet-packages available that bundle some of the functionality of NMF. These packages are potentially more stable than the source code, but may not contain the latest features of NMF.
The packages are:
* NMF-Expressions [![Downloads](https://img.shields.io/nuget/dt/NMF-Expressions.svg)](https://www.nuget.org/packages/NMF-Expressions/): Contains the NMF library for incremental computation. This package has no other dependencies and can even be used independently of the modeling framework.
* NMF-Repository [![Downloads](https://img.shields.io/nuget/dt/NMF-Repository.svg)](https://www.nuget.org/packages/NMF-Repository/): Conatins the repository management of NMF (depends on NMF-Expressions)
* NMF-Basics [![Downloads](https://img.shields.io/nuget/dt/NMF-Basics.svg)](https://www.nuget.org/packages/NMF-Basics/): Contains the latter two plus a code generator to generate model representation code from any Ecore or NMeta metamodel. The code generator integrates with the Nuget Package Console.
* NMF-Expressions-Utilities [![Downloads](https://img.shields.io/nuget/dt/NMF-Expressions-Utilities.svg)](https://www.nuget.org/packages/NMF-Expressions-Utilities/): Contains some utility classes for NMF Expressions such as dictionary of incremental values
* NMF-Transformations [![Downloads](https://img.shields.io/nuget/dt/NMF-Transformations.svg)](https://www.nuget.org/packages/NMF-Transformations/): Contains the model transformation language NTL used in NMF
* NMF-Synchronizations [![Downloads](	https://img.shields.io/nuget/dt/NMF-Synchronizations.svg)](https://www.nuget.org/packages/NMF-Synchronizations/): Contains the incremental, uni- and bidirectional model transformation language NMF Synchronizations

## Publications
The publications about NMF are available on https://sdqweb.ipd.kit.edu/publications/topics/nmf.html
# .NET Modeling Framework (NMF) [![Build status](https://ci.appveyor.com/api/projects/status/jbma22noos4y712w?svg=true)](https://ci.appveyor.com/project/georghinkel/nmf)

This repository contains the entire code for the .NET Modeling Framework. The development of NMF started in about 2011 as a student project of Georg Hinkel. Since then, it has grown to a large development framework and multiple papers have been published on NMF. The source code used to be hosted under Codeplex but now has been migrated to Github.

## NuGet Packages
There are currently three NuGet-packages available that bundle some of the functionality of NMF. These packages are potentially more stable than the source code, but may not contain the latest features of NMF.
The packages are:
* NMFExpressions (https://www.nuget.org/packages/NMF-Expressions/): Contains the NMF library for incremental computation. This package has no other dependencies and can even be used independently of the modeling framework.
* NMFRepository (https://www.nuget.org/packages/NMF-Repository/): Conatins the repository management of NMF (depends on NMFExpressions)
* NMF-Basics (https://www.nuget.org/packages/NMF-Basics/): Contains the latter two plus a code generator to generate model representation code from any Ecore or NMeta metamodel. The code generator integrates with the Nuget Package Console.

## Publications
* Georg Hinkel and Thomas Goldschmidt. Tool Support for Model Transformations: On Solutions using Internal Languages. In Modellierung 2016, Karlsruhe, Germany, March 2-4, 2016.
* Georg Hinkel. NMF: A Modeling Framework for the .NET Platform. Technical report, Karlsruhe, 2016. (http://nbn-resolving.org/urn:nbn:de:swb:90-537082)
* Georg Hinkel and Lucia Happe. An NMF Solution to the TTC Train Benchmark Case. In Proceedings of the 8th Transformation Tool Contest, a part of the Software Technologies: Applications and Foundations (STAF 2015) federation of conferences, Louis Rose, Tassilo Horn, and Filip Krikava, editors, L'Aquila, Italy, July 24, 2015, volume 1524 of CEUR Workshop Proceedings, pages 142-146. CEUR-WS.org. July 2015. (http://ceur-ws.org/Vol-1524/paper8.pdf)
* Georg Hinkel. An NMF Solution to the Java Refactoring Case. In Proceedings of the 8th Transformation Tool Contest, a part of the Software Technologies: Applications and Foundations (STAF 2015) federation of conferences, Louis Rose, Tassilo Horn, and Filip Krikava, editors, L'Aquila, Italy, July 24, 2015, volume 1524 of CEUR Workshop Proceedings, pages 95-99. CEUR-WS.org. July 2015. (http://ceur-ws.org/Vol-1524/paper9.pdf)
* Georg Hinkel. Change Propagation in an Internal Model Transformation Language. In Theory and Practice of Model Transformations, pages 3-17. Springer, 2015. (http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2015a_slides.pdf)
* Georg Hinkel and Lucia Happe. Using component frameworks for model transformations by an internal DSL. In 1st International Workshop on Model-Driven Engineering for Component-Based Systems (ModComp 2014), 2014, CEUR.
* Georg Hinkel. An approach to maintainable model transformations using an internal DSL. Master's thesis, Karlsruhe Institute of Technology, October 2013.
* Georg Hinkel, Thomas Goldschmidt, and Lucia Happe. An NMF solution for the Petri Nets to State Charts case study at the TTC 2013. EPTCS, 135:95-100, 2013.
* Georg Hinkel, Thomas Goldschmidt, and Lucia Happe. An NMF solution for the Flowgraphs case at the TTC 2013. EPTCS, 135:37-42, 2013.

# .NET Modeling Framework (NMF) [![Build status](https://ci.appveyor.com/api/projects/status/jbma22noos4y712w?svg=true)](https://ci.appveyor.com/project/georghinkel/nmf)

This repository contains the entire code for the .NET Modeling Framework. The development of NMF started in about 2011 as a student project of Georg Hinkel. Since then, it has grown to a large development framework and multiple papers have been published on NMF. The source code used to be hosted under Codeplex but now has been migrated to Github.

## NuGet Packages
There are currently three NuGet-packages available that bundle some of the functionality of NMF. These packages are potentially more stable than the source code, but may not contain the latest features of NMF.
The packages are:
* NMFExpressions (https://www.nuget.org/packages/NMF-Expressions/): Contains the NMF library for incremental computation. This package has no other dependencies and can even be used independently of the modeling framework.
* NMFRepository (https://www.nuget.org/packages/NMF-Repository/): Conatins the repository management of NMF (depends on NMFExpressions)
* NMF-Basics (https://www.nuget.org/packages/NMF-Basics/): Contains the latter two plus a code generator to generate model representation code from any Ecore or NMeta metamodel. The code generator integrates with the Nuget Package Console.

## Publications


<table>

<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2016g">1</a>]
</td>
<td class="bibtexitem">
Georg Hinkel.
 An NMF solution to the Class Responsibility Assignment Case.
 In <em>Proceedings of the 9th Transformation Tool Contest, a part
  of the Software Technologies: Applications and Foundations (STAF 2015)
  federation of conferences</em>, Antonio Garcia-Dominguez, Filip Krikava, and
  Louis Rose, editors, Vienna, Austria, July 8, 2016, CEUR Workshop
  Proceedings. CEUR-WS.org.
 July 2016, accepted, to appear.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2016g">bib</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2016g.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
This paper presents a solution to the Class Responsibility Assignment (CRA) case at the Transformation Tool Contest (TTC) 2016 using the .NET Modeling Framework (NMF). The goal of this case was to find a class model with high cohesion but low coupling for a given set of attributes and methods with data dependencies and functional dependencies. The degree in which a given class model fulfills these properties is measured through the CRA-Index. We propose a generalpurpose code solution and discuss how this solution can benefit from incrementality. In particular, we show what steps are necessary to create an incremental solution using NMF Expressions and discuss its performance.
</blockquote></div>
</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2016b">2</a>]
</td>
<td class="bibtexitem">
Georg Hinkel and Thomas Goldschmidt.
 Tool Support for Model Transformations: On Solutions using Internal
  Languages.
 In <em>Modellierung 2016</em>, Karlsruhe, Germany, March 2-4, 2016.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2016b">bib</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2016b_slides.pdf">slides</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2016b.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
Model-driven engineering (MDE) has proven to be a useful approach to cope with todays ever growing complexity in the development of software systems, yet it is not widely applied in industry. As suggested by multiple studies, tool support is a major factor for this lack of adoption. Existing tools for MDE, in particular model transformation approaches, are often developed by small teams and cannot keep up with advanced tool support for mainstream languages such as provided by IntelliJ or Visual Studio. In this paper, we propose an approach to leverage existing tool support for model transformation using internal model transformation languages and investigate design decisions and their consequences for inherited tool support. The findings are used for the design of an internal model transformation language on the .NET platform.
</blockquote></div>
</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2016d">3</a>]
</td>
<td class="bibtexitem">
Georg Hinkel.
 NMF: A Modeling Framework for the .NET Platform.
 Technical report, Karlsruhe Institute of Technology, Karlsruhe, 2016.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2016d">bib</a>&nbsp;| 
<a href="http://nbn-resolving.org/urn:nbn:de:swb:90-537082">http</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2016d.pdf">.pdf</a>&nbsp;]

</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2016e">4</a>]
</td>
<td class="bibtexitem">
Georg Hinkel.
 Deep Modeling through Structural Decomposition.
 Technical report, Karlsruhe Institute of Technology, Karlsruhe, 2016.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2016e">bib</a>&nbsp;| 
<a href="http://nbn-resolving.org/urn:nbn:de:swb:90-576330">http</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2016e.pdf">.pdf</a>&nbsp;]

</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2015d">5</a>]
</td>
<td class="bibtexitem">
Georg Hinkel and Lucia Happe.
 An NMF Solution to the TTC Train Benchmark Case.
 In <em>Proceedings of the 8th Transformation Tool Contest, a part of
  the Software Technologies: Applications and Foundations (STAF 2015)
  federation of conferences</em>, Louis Rose, Tassilo Horn, and Filip Krikava,
  editors, L'Aquila, Italy, July 24, 2015, volume 1524 of <em>CEUR Workshop
  Proceedings</em>, pages 142-146. CEUR-WS.org.
 July 2015.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2015d">bib</a>&nbsp;| 
<a href="http://ceur-ws.org/Vol-1524/paper8.pdf">.pdf</a>&nbsp;]

</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2015e">6</a>]
</td>
<td class="bibtexitem">
Georg Hinkel.
 An NMF Solution to the Java Refactoring Case.
 In <em>Proceedings of the 8th Transformation Tool Contest, a part of
  the Software Technologies: Applications and Foundations (STAF 2015)
  federation of conferences</em>, Louis Rose, Tassilo Horn, and Filip Krikava,
  editors, July 24, 2015, volume 1524 of <em>CEUR Workshop Proceedings</em>, pages
  95-99. CEUR-WS.org.
 July 2015.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2015e">bib</a>&nbsp;| 
<a href="http://ceur-ws.org/Vol-1524/paper9.pdf">.pdf</a>&nbsp;]

</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2015a">7</a>]
</td>
<td class="bibtexitem">
Georg Hinkel.
 <em>Change Propagation in an Internal Model Transformation
  Language</em>, pages 3-17.
 Springer International Publishing, Cham, 2015.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2015a">bib</a>&nbsp;| 
<a href="http://dx.doi.org/10.1007/978-3-319-21155-8_1">DOI</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2015a_slides.pdf">slides</a>&nbsp;| 
<a href="http://dx.doi.org/10.1007/978-3-319-21155-8_1">http</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2015a.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
Despite good results, Model-Driven Engineering (MDE) has not been widely adopted in industry. According to studies by Staron and Mohaghegi, the lack of tool support is one of the major reasons for this. Although MDE has existed for more than a decade now, tool support is still insufficient. An approach to overcome this limitation for model transformations, which are a key part of MDE, is the usage of internal languages that reuse tool support for existing host languages. On the other hand, these internal languages typically do not provide key features like change propagation or bidirectional transformation. In this paper, we present an approach to use a single internal model transformation language to create unidirectional and bidirectional model transformations with optional change propagation. In total, we currently provide 18 operation modes based on a single specification. At the same time, the language may reuse tool support for C#. We validate the applicability of our language using a synthetic example with a transformation from finite state machines to Petri nets where we achieved speedups of up to 48 compared to classical batch transformations.
</blockquote></div>
</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2014">8</a>]
</td>
<td class="bibtexitem">
Georg Hinkel and Lucia Happe.
 Using component frameworks for model transformations by an internal
  DSL.
 In <em>Proceedings of the 1st International Workshop on
  Model-Driven Engineering for Component-Based Software Systems co-located with
  ACM/IEEE 17th International Conference on Model Driven Engineering Languages
  &amp; Systems (MoDELS 2014)</em>, 2014, volume 1281 of <em>CEUR Workshop
  Proceedings</em>, pages 6-15. CEUR-WS.org.
 2014.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2014">bib</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2014_slides.pdf">slides</a>&nbsp;| 
<a href="http://ceur-ws.org/Vol-1281/1.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
To increase the development productivity, possibilities for reuse, maintainability and quality of complex model transformations, modularization techniques are indispensable. Component-Based Software Engineering targets the challenge of modularity and is well-established in languages like Java or C# with component models like .NET, EJB or OSGi. There are still many challenging barriers to overcome in current model transformation languages to provide comparable support for component-based development of model transformations. Therefore, this paper provides a pragmatic solution based on NMF Transformations, a model transformation language realized as an internal DSL embedded in C#. An internal DSL can take advantage of the whole expressiveness and tooling build for the well established and known host language. In this work, we use the component model of the .NET platform to represent reusable components of model transformations to support internal and external model transformation composition. The transformation components are hidden behind transformation rule interfaces that can be exchanged dynamically through configuration. Using this approach we illustrate the possibilities to tackle typical issues of integrity and versioning, such as detecting versioning conflicts for model transformations.
</blockquote></div>
</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2013a">9</a>]
</td>
<td class="bibtexitem">
Georg Hinkel.
 An approach to maintainable model transformations using an internal
  DSL.
 Master's thesis, Karlsruhe Institute of Technology, 2013.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2013a">bib</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2013a.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
In recent years, model-driven software development (MDSD) has gained popularity among both industry and academia. MDSD aims to generate traditional software artifacts from models. This generation process is realized in multiple steps. Thus, before being transformed to software artifacts, models are transformed into models of other metamodels. Such model transformation is supported by dedicated model transformation languages. In many cases, these are entirely new languages (external domain-specific languages, DSLs) for a more clear and concise representation of abstractions. On the other hand, the tool support is rather poor and the transformation developers hardly know the transformation language. A possible solution for this problem is to extend the programming language typically used by developers (mostly Java or C#) with the required abstractions. This can be achieved with an internal DSL. Thus, concepts of the host language can easily be reused while still creating the necessary abstractions to ease development of model transformations. Furthermore, the tool support for the host language can be reused for the DSL. In this master thesis, NMF Transformations is presented, a framework and internal DSL for C#. It equips developers with the ability to specify model transformations in languages like C# without having to give up abstractions known from model transformation standards. Transformation developers get the full tool support provided for C#. The applicability of NMF Transformations as well as the impact of NMF Transformations to quality attributes of model transformations is evaluated in three case studies. Two of them come from the Transformation Tool Contests 2013 (TTC). With these case studies, NMF Transformations is compared with other approaches to model transformation. A further case study comes from ABB Corporate Research to demonstrate the advantages of NMF Transformations in an industrial scenario where aspects like testability gain special importance.
</blockquote></div>
</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2013b">10</a>]
</td>
<td class="bibtexitem">
Georg Hinkel, Thomas Goldschmidt, and Lucia Happe.
 An NMF solution for the Petri Nets to State Charts case study at the
  TTC 2013.
 <em>EPTCS</em>, 135:95-100, 2013, ArXiv.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2013b">bib</a>&nbsp;| 
<a href="http://dx.doi.org/10.4204/EPTCS.135.12">DOI</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2013b.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
Software systems are getting more and more complex. Model-driven engineering (MDE) offers ways to handle such increased complexity by lifting development to a higher level of abstraction. A key part in MDE are transformations that transform any given model into another. These transformations are used to generate all kinds of software artifacts from models. However, there is little consensus about the transformation tools. Thus, the Transformation Tool Contest (TTC) 2013 aims to compare different transformation engines. This is achieved through three different cases that have to be tackled. One of these cases is the Petri Net to State Chart case. A solution has to transform a Petri Net to a State Chart and has to derive a hierarchical structure within the State Chart. This paper presents the solution for this case using NMF Transformations as transformation engine. 
</blockquote></div>
</td>
</tr>


<tr valign="top">
<td align="right" class="bibtexnumber">
[<a name="hinkel2013c">11</a>]
</td>
<td class="bibtexitem">
Georg Hinkel, Thomas Goldschmidt, and Lucia Happe.
 An NMF solution for the Flowgraphs case at the TTC 2013.
 <em>EPTCS</em>, 135:37-42, 2013, ArXiv.
[&nbsp;<a href="http://sdqweb.ipd.kit.edu/publications/topics/nmf_bib.html#hinkel2013c">bib</a>&nbsp;| 
<a href="http://dx.doi.org/10.4204/EPTCS.135.5">DOI</a>&nbsp;| 
<a href="http://sdqweb.ipd.kit.edu/publications/pdfs/hinkel2013c.pdf">.pdf</a>&nbsp;|&nbsp;<a href="javascript:showAbstract(this);" onclick="showAbstract(this)">Abstract</a><noscript> (JavaScript required!)</noscript>&nbsp;]<div style="display:none;"><blockquote id="abstractBQ">
Software systems are getting more and more complex. Model-driven engineering (MDE) offers ways to handle such increased complexity by lifting development to a higher level of abstraction. A key part in MDE are transformations that transform any given model into another. These transformations are used to generate all kinds of software artifacts from models. However, there is little consensus about the transformation tools. Thus, the Transformation Tool Contest (TTC) 2013 aims to compare different transformation engines. This is achieved through three different cases that have to be tackled. One of these cases is the Flowgraphs case. A solution has to transform a Java code model into a simplified version and has to derive control and data flow. This paper presents the solution for this case using NMF Transformations as transformation engine. 
</blockquote></div>
</td>
</tr>
</table>

# AnyText Framework



## Usage



## The Language Server Protocol (LSP)
https://microsoft.github.io/language-server-protocol/specifications/lsp/3.17/specification/


### Features

* **Folding:** Enables certain parts of the document like comments, classes or other custom 
ranges to be folded away (hidden).
* **Selection:** Enables the selection (VSCode default: Alt + Shift + Right Arrow) of symbols 
and the surrounding scope.
* **Goto Definition:** Enables jumping to the definition of a symbol in the document.
* **Find References:** Enables viewing of all references of a symbol in a document.
* **Highlights:** Enables highlighting of matching symbols (see Find References) and textual 
occurrences in a document.
* **Rename:** Enables the renaming of symbols in a document (see Find References).
* **Document Symbols/Outline:** Enables the symbols of the document to be shown in the outline 
(VSCode default left-hand side and top of document). The outline at the top shows the current 
position in the document and both outlines can be used to jump to symbols in the document.

### Feature configuration

While the features were implemented generically, some of them can be configured or require 
manual configuration to function properly. Manual configurations can be added to the rules 
in the **\<GrammarName\>.manual.cs** file located under */AnyText/Grammars*.

**Folding:** 
Folding can be configured in multiple ways. A folding range can be a comment, a number of 
imports/includes statements, a region or a generic range. While comments don't need to be 
configured manually, the default implementations of the remaining kinds of folding ranges 
can and often should be overridden. Overrides need to be done for each rule individually.

* `public override bool IsRegionStartLiteral(string literal)`: 
True, if the given literal denotes the start of a foldable region.
* `public override bool IsRangeStartLiteral(string literal)`: 
True, if the given literal denotes the start of a generic foldable range.
* `public override bool IsMatchingEndLiteral(string literal, string startLiteral)`: 
True, if the given literal is a matching end literal for a given start literal.

Alternatively, it is also possible to directly mark specific rules as foldable ranges.

* `public override bool IsImports()` : Can be set to true to denote a foldable imports/includes region.
* `public override bool IsRegion()` : Can be set to true to denote a foldable region.
* `public override bool IsFoldable()`: Can be set to true to denote a generic foldable range.

**Selection:** No manual configuration required.

**GoTo Definition:** 
Definitions can be configured by defining which rules are considered definitions. 
While the default implementation for definitions imposed by the framework and code generator 
should generally suffice, it is possible to manually override which rules denote identifiers 
if it has not otherwise been defined in the metamodel (also see Find References).

* `public override bool IsIdentifier()`: Can be set to true to denote an identifier.

If necessary, the default implementation for definitions can also be overridden.

* `public override bool IsDefinition()`: Can be set to true to denote a definition.

**Find References:** 
References can be configured by defining which rules are considered references. 
While the default implementation for references imposed by the framework and code generator 
should generally suffice, it is possible to manually override which rules denote identifiers 
if it has not otherwise been defined in the metamodel (also see GoTo Definition).

* `public override bool IsIdentifier()`: Can be set to true to denote an identifier.

If necessary, the default implementation for references can also be overridden.

* `public override bool IsReference()`: Can be set to true to denote a reference.

**Highlights:** No manual configuration required, but is dependent on Find References feature.

**Rename:** No manual configuration required, but is dependent on Find References feature.

**Document Symbols/Outline:** 
The rules shown in the outline and symbols used for them can be configured. 

* `public override SymbolKind SymbolKind`: 
Can be set to any symbol from the **SymbolKind** enum located under */AnyText.Core*. 
By default, all rules are marked by the symbol `SymbolKind.Null`, resulting in neither 
them nor any child applications of the rule showing up in the outline. Consequently, any 
rule marked by a different symbol will show up in the outline.

* `public override bool PassAlongDocumentSymbols`: 
Can be set to true to denote that the rule should not show up in the outline, but any child 
applications of this rule should show up in the outline, provided that their SymbolKind has 
been set accordingly.

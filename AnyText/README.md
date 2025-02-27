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
* **Semantic Tokens:** Provides enhanced syntax highlighting by offering context-aware colorization based on the meaning of code elements.
* **Code Lens:** Displays inline actionable hints, such as references, tests, and run/debug options, directly within the document.
* **Code Actions:** Suggests and applies quick fixes or refactorings, such as converting a variable to a constant or optimizing imports.
* **Formatting:** Automatically formats the document based on predefined or user-configured style rules, ensuring consistent code style.
* **Diagnostic:** Provides real-time feedback on errors, warnings, and other issues within the document, helping to identify and resolve problems quickly.
* **SetTrace:** Allows debugging and logging levels to be adjusted dynamically, helping to track execution flow and diagnose issues.

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

**Semantic Tokens:**
Semantic tokens can be configured by defining `TokenType` and specifying `TokenModifiers` for specific rules.  

The following example illustrates how to define a token type named `"type"` with a modifier:  

```csharp
public partial class TypeRuleRule
{
    public override string TokenType => "type";
    public override string[] TokenModifiers => new[] { "tokenModifier" }; 
}
```

By default, only `LiteralRule` has a defined `TokenType`: it is set to `operator` if it does not start with a letter or digit; otherwise, it is set to `keyword`.

**Code Lens:**
To integrate CodeLens functionality, you need to define an actionable insight that provides additional context or interactivity within the editor, such as running a test or displaying the references.

The following example demonstrates how to define a CodeLens action that prints to error:


```csharp
public partial class ExampleRuleRule
{
    protected override IEnumerable<CodeLensInfo<ExampleRuleRule>> CodeLenses
    {
        get
        {
            yield return new()
            {
                Title = "Print Test",
                CommandIdentifier = "codelens.printTest",
                Action = (element, args) =>
                {
                    Console.Error.WriteLine("Test");
                }
            };
        }
    }
}
```

`Title` represents the text displayed for the CodeLens in the editor. To dynamically create a `Title` based on context information, the `TitleFunc` should be defined.

`CommandIdentifier` is the unique identifier for the action within the LSP server.

`Action` defines the action that is executed when the CodeLens is clicked.t
The `element` refers to the context element of the rule, while `args` are the arguments used to create the action. These arguments include the `RuleApplication`, `ParseContext` and methods such as `ShowRequest` and `ShowDocument`.

By default, only `ElementRules` with a defined `SymbolKind` have a CodeLens that displays the number of references.


**Code Actions:**
To integrate Code Actions provide automated suggestions, such as quick fixes, refactoring, or applying modifications to the workspace. A Code Action can either execute a command (like `CodeLens`) or apply a `WorkspaceEdit`. 
If it should execute a Command it should use `Action` just like `CodeLens` and set `CommandTitle` and `CommandIdentifier`.

The following example demonstrates how to define a `CodeAction` that creates a new file using `WorkspaceEdit`:
```csharp
public partial class ModelRuleRule
{
    protected override IEnumerable<CodeActionInfo<ModelRule>> CodeActions => new List<CodeActionInfo<ModelRule>>
    {
        new()
        {
            Title = "Create new File",
            Kind = "quickfix",
            WorkspaceEdit = (m, args) =>
            {
                return new WorkspaceEdit
                {
                    DocumentChanges = new List<DocumentChange>
                    {
                        new()
                        {
                            CreateFile = new CreateFile
                            {
                                Options = new FileOptions
                                {
                                    IgnoreIfExists = false,
                                    Overwrite = true
                                },
                                AnnotationId = "createFile",
                                Kind = "create",
                                Uri = "newDocument.anytext"
                            }
                        },
                    },
                   
                };
            },
            Diagnostics = new[] { "" },
        }
    };
}
```
`Kind` specifies the type of `CodeAction` like (`refactor` or `quickfix`):

The `Diagnostics` property defines a string array that restricts the action to diagnostics containing at least one of these strings in their message. 

With `CreateWorkspaceEdit`, it is possible to generate a `WorkspaceEdit` using context information.

There is no `CodeAction` defined by default.



**Formatting:** Uses formatting options, such as indent style and spacing, as configured in the client — no manual setup required.

**Diagnostic:** No manual configuration required.

**SetTrace:** Adjusts tracing by modifying the Trace setting in the client.
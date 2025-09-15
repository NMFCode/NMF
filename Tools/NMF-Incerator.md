# NMF Incerator

Incerator is a tool to optimize the creation of dynamic dependency graphs.

## When should I use this package?

Incerator supports the following strategies to build dynamic dependency graphs (DDGs):

- Instruction-level incrementalization: Every node in the AST is turned into a DDG node
- Batch: An entire expression is recalculated based on static analysis (e.g., if some property has changed that was used to calculate the result)
- Parameter-promotion: For a given method, the system calculates the minimal amount of model elements from which all changes to a given expression can be obtained. If this is not the case for the parameters of a method (due to cross-references), an optimized method is created that fulfills this condition.
- Tree-extension: Like parameter-promotion, but here the system uses parent model elements to conclude that some root elements are sufficient to capture all changes.

Parameter-promotion and tree extension only work if the models use the NMF modeling foundation.

You can change the strategy to create DDG nodes with a static property, demonstrated by the following line of code:

```csharp
NotifySystem.DefaultSystem = new PromotionNotifySystem();
```

Alternatively, Incerator also implements a notify system that records expressions where the decision is necessary and an implementation that decides
for each expression separately, based on a configuration. Finally, a tool delivered with this package and accessible using the PowerShell console
can optimize a given application for the best configuration using a genetic search algorithm.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials.
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).

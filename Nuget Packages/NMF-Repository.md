# NMF Models

The modeling foundation of NMF is a system that represents models as instances of classes generated for their meta-classes. 

## Why should I use this package?

This package contains the code for the NMF modeling foundation, in particular:

- `ModelElement` is used to represent a model element in memory
- `Model` is used to represent a model in memory, i.e. a coherent set of model elements
- `ModelRepository` is used to represent a repository of models, i.e. a collection of loaded models. A model repository is the unit to load models into memory. When models reference elements from other models, these other models are automatically loaded as required.
- `MetaRepository` is a special repository used to keep all the metamodels available in the process.
- `ModelChangeSet` represents a change sequence created over a model
- `ModelChangeRecorder` is used to record changes to models by collecting notifications
- `ModelHasher` is used to calculate cryptographic hash values for models, respecting that attributes and references can ignore order and ignoring referencing schemes

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
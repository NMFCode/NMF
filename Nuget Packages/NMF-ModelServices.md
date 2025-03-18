# NMF Model Services

NMF Model Services provides generic services connected to models when used in a server setting.

## Why should I use this package?

This package contains the following classes:

- `JsonModelSerializer` is a serializer implementation that serializes and deserializes models from and to JSON (rather than XMI which uses XML). The interface is the same.
- `IModelServer` and the implementation `ModelServer` allow a service to access sessions of models. These sessions control an undo/redo stack and an active session.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
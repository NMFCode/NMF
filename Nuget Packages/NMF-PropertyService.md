# NMF Model Services

NMF Model Services provides generic services connected to models when used in a server setting.

## Why should I use this package?

This package contains classes to add a generic property service to a model server. This service allows to

- Obtain the currently selected model element, either as a REST endpoint (limited to request/reply) or through a JSON-RPC based protocol over websockets
- Make changes to the currently selected model element

These services can be used to implement property views in web-based editing scenarios.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
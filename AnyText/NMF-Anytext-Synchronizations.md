# NMF AnyText

This package contains a library used to synchronize AnyText documents with other documents.

## Why should I use this package?

Use this package if you want to synchronize multiple DSLs with AnyText and NMF Synchronizations. If both languages are textual DSLs, this can be achieved using

```csharp
await SynchronizationBootstrapper.RunLspServerOnStandardInStandardOutAsync(
  [yourGrammar1, yourGrammar2],
  [new ModelSynchronization<Grammar1Type, Grammar2Type, SynchronizationType, SynchronizationStartRule>
   {
     IsAutomatic = true, // synchronization is started as soon as two files with the same name are opened
     LeftExtension = ".ext1", // file extension associated with grammar 1
     RightExtension = ".ext2" // file extension associated with grammar 2
   }]);
```

The server will then post workspace changes whenever a change in one file affects the other.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials.
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).

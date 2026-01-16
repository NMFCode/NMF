# NMF Collections

This package contains a few commonly used collection classes.

It defines the following interfaces:

- `IOrderedSet<T>` as an abstraction of a set that has an order.
- `INotifyCollectionChanging` as a counterpart for the base class library interface `INotifyCollectionChanged` in order to support listening for attempts to change the collection

It contains the following classes:

- `OrderedSet<T>` as a standard implementation of an ordered set, using both an array list and a hash set
- `ReadOnlyOrderedSet<T>` as a readonly view of an ordered set
- `ReadOnlyListSelection<T>` as a readonly view of a mapped list, implementing `IList<T>`
- Opposite collections for lists, sets and ordered sets where adding or removing elements has a side-effect to the affected element
- Observable collections for all of the above (and sets): classes that implement `INotifyCollectionChanged` and `INotifyCollectionChanging`

## When should I use this package?

Use this package if you need any collection implementation from the above. In particular, this package is interesting for ordered sets.

## Where can I get more information?

There is a [documentation website](https://nmfcode.github.io/) which we try to maintain that contains a few tutorials. 
There are [publications](https://nmfcode.github.io/publications/index.html) if you want to explore the technical details.
Also, please feel free to [ask a question or report a bug](https://github.com/NMFCode/NMF/issues).
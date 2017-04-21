using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using NMF.Collections.ObjectModel;
using NMF.Utilities;
using Type = System.Type;
using NMF.Models.Meta;
using System.Diagnostics;

namespace NMF.Models.Changes
{
    /// <summary>
    /// Represents a recorder for changes to a model.
    /// </summary>
    public partial class ModelChangeRecorder
    {
        private List<BubbledChangeEventArgs> recordedEvents = new List<BubbledChangeEventArgs>();
        private readonly Dictionary<IModelElement, Uri> uriMappings = new Dictionary<IModelElement, Uri>();
        private readonly Dictionary<IModelElement, IModelChange> elementSources = new Dictionary<IModelElement, IModelChange>();

        private bool _isInvertible;

        public ModelChangeRecorder(bool isInvertible)
        {
            _isInvertible = isInvertible;
        }

        public ModelChangeRecorder() : this(false) { }
        
        /// <summary>
        /// Checks whether the recorder is attached to a model element.
        /// </summary>
        public bool IsRecording { get { return AttachedElement != null; } }

        /// <summary>
        /// Gets the attached model element or null, if the recorder is not attached.
        /// </summary>
        public IModelElement AttachedElement { get; private set; }
        
        /// <summary>
        /// Attaches the recorder to the given model element. The recorder will track all
        /// changes made to the given element and every element further down in the
        /// containment hierarchy.
        /// </summary>
        /// <param name="element"></param>
        public void Start(IModelElement element)
        {
            if (IsRecording)
                throw new InvalidOperationException("The recorder is still attached.");

            AttachedElement = element;
            element.BubbledChange += OnBubbledChange;
            var elementMe = element as ModelElement;
            if (elementMe != null)
            {
                elementMe.RequestUris();
            }
        }

        /// <summary>
        /// Detaches the recorder, stopping the change tracking.
        /// </summary>
        public void Stop()
        {
            if (!IsRecording)
                throw new InvalidOperationException("The recorder is not attached.");

            var elementMe = AttachedElement as ModelElement;
            if (elementMe != null)
            {
                elementMe.UnregisterUriRequest();
            }
            AttachedElement.BubbledChange -= OnBubbledChange;
            AttachedElement = null;
        }

        private void OnBubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (e.ChangeType == ChangeType.UriChanged || e.ChangeType == ChangeType.ElementDeleted)
            {
                var eventArgs = (UriChangedEventArgs)e.OriginalEventArgs;
                RegisterAllChangedUris(e.Element, eventArgs.OldUri);
            }
            else
            {
                if (e.ChangeType != ChangeType.ElementCreated && e.Feature == null)
                    throw new InvalidOperationException($"The property {e.PropertyName} is not mapped to an attribute or reference. Therefore, changes of this property cannot be recorded. To fix this problem, try to regenerate the metamodel code or contact the NMF developers.");
                recordedEvents.Add(e);
            }
        }

        private void RegisterAllChangedUris(IModelElement origin, Uri originalUri)
        {
            if (originalUri == null) return;
            var stack = new Stack<IModelElement>();
            stack.Push(origin);
            while (stack.Count > 0)
            {
                var element = stack.Pop();
                if (!uriMappings.ContainsKey(element))
                {
                    var me = (ModelElement)element;
                    if (element == origin)
                    {
                        uriMappings.Add(element, originalUri);
                    }
                    else
                    {
                        var fragment = me.CreateUriWithFragment(null, false, origin);
                        var fragmentString = fragment != null ? "/" + fragment.OriginalString : null;
                        if (originalUri.IsAbsoluteUri)
                        {
                            uriMappings.Add(element, new Uri(originalUri, originalUri.Fragment + fragmentString));
                        }
                        else
                        {
                            uriMappings.Add(element, new Uri(originalUri.OriginalString + fragmentString, UriKind.Relative));
                        }
                    }
                    foreach (var child in element.Children)
                    {
                        stack.Push(child);
                    }
                }
            }
        }

        public Task<ModelChangeSet> GetModelChangesAsync()
        {
            return Task.Factory.StartNew(GetModelChanges);
        }

        /// <summary>
        /// Returns previously recorded changes in a tree hierarchy.
        /// </summary>
        /// <returns></returns>
        public ModelChangeSet GetModelChanges()
        {
            var changes = new ModelChangeSet();
            var changeModel = new ChangeModel(uriMappings, elementSources);
            changeModel.RootElements.Add(changes);
            int currentIndex = 0;
            var list = new List<IModelChange>();
            while (currentIndex < recordedEvents.Count)
            {
                ParseChange(list, ref currentIndex);
            }
            ConnectInsertionsAndDeletions(list);
#if DEBUG
            SanityCheckInsertionsAndDeletions(list);
#endif
            changes.Changes.AddRange(list);
            return changes;
        }

        private static void SanityCheckInsertionsAndDeletions(List<IModelChange> list)
        {
            var allInsertions = list.OfType<ICompositionInsertion>().Concat(list.SelectMany(c => c.Descendants().OfType<ICompositionInsertion>()));
            var allDeletions = list.OfType<ICompositionDeletion>()
                .Concat(list.SelectMany(c => c.Descendants().OfType<ICompositionDeletion>())
                .Where(c2 => c2.Parent != null &&
                       !(c2.Parent is CompositionMoveIntoProperty) &&
                       !(c2.Parent is CompositionMoveToCollection) &&
                       !(c2.Parent is CompositionMoveToList)));
            if (allInsertions.Select(c => c.AddedElement).IntersectsWith(allDeletions.Select(c => c.DeletedElement)))
            {
                throw new InvalidOperationException("There are element deletions and element insertions for the same element.");
            }
            if (allDeletions.Any(d => d.DeletedElement.Parent != null))
            {
                throw new InvalidOperationException("There are element deletions of elements that are not deleted.");
            }
        }

        private void ConnectInsertionsAndDeletions(List<IModelChange> list)
        {
            int currentIndex = 0;
            while (currentIndex < list.Count)
            {
                var insertion = list[currentIndex] as ICompositionInsertion;
                ICompositionDeletion deletion;
                if (insertion != null)
                {
                    currentIndex++;
                    if (currentIndex < list.Count)
                    {
                        deletion = list[currentIndex] as ICompositionDeletion;
                        ElementaryChangeTransaction t;
                        if (deletion != null && deletion.DeletedElement == insertion.AddedElement)
                        {
                            list.RemoveAt(currentIndex);
                            list[currentIndex - 1] = insertion.ConvertIntoMove(deletion);
                            elementSources.Remove(insertion.AddedElement);
                        }
                        else if ((t = list[currentIndex] as ElementaryChangeTransaction) != null && 
                                 t.NestedChanges.Count == 1 && 
                                 (deletion = t.NestedChanges[0] as ICompositionDeletion) != null &&
                                 deletion.DeletedElement == insertion.AddedElement)
                        {
                            list.RemoveAt(currentIndex);
                            list[currentIndex - 1] = insertion.ConvertIntoMove(deletion);
                            elementSources.Remove(insertion.AddedElement);
                        }
                    }
                }
                else if ((deletion = list[currentIndex] as ICompositionDeletion) != null)
                {
                    currentIndex++;
                    if (currentIndex < list.Count)
                    {
                        insertion = list[currentIndex] as ICompositionInsertion;
                        ElementaryChangeTransaction t;
                        if (insertion != null && insertion.AddedElement == deletion.DeletedElement)
                        {
                            list.RemoveAt(currentIndex);
                            list[currentIndex - 1] = insertion.ConvertIntoMove(deletion);
                            elementSources.Remove(insertion.AddedElement);
                        }
                        else if ((t = list[currentIndex] as ElementaryChangeTransaction) != null && 
                                 t.NestedChanges.Count == 1 &&
                                 (insertion = t.NestedChanges[0] as ICompositionInsertion) != null &&
                                 insertion.AddedElement == deletion.DeletedElement)
                        {
                            list.RemoveAt(currentIndex);
                            list[currentIndex - 1] = insertion.ConvertIntoMove(deletion);
                            elementSources.Remove(insertion.AddedElement);
                        }
                    }
                }
                else
                {
                    currentIndex++;
                }
            }
        }

        private void ParseChange(List<IModelChange> changes, ref int currentIndex)
        {
            BubbledChangeEventArgs currentEvent;
            IModelElement createdElement = null;

            do
            {
                currentEvent = recordedEvents[currentIndex++];
                if (currentEvent.ChangeType == ChangeType.ElementCreated)
                {
                    createdElement = currentEvent.Element;
                }
                if (currentIndex == recordedEvents.Count) return;
            } while (currentEvent.ChangeType == ChangeType.ElementCreated);

            var childChanges = new List<IModelChange>();

            if (currentEvent.ChangeType == ChangeType.PropertyChanged || currentEvent.ChangeType == ChangeType.CollectionChanged)
            {
                InterpretPastChanges(changes, currentEvent);
                return;
            }

            while (currentIndex < recordedEvents.Count)
            {
                var nextEvent = recordedEvents[currentIndex];

                if (nextEvent.ChangeType == ChangeType.ElementCreated)
                {
                    createdElement = nextEvent.Element;
                    currentIndex++;
                    continue;
                }
                if (MatchEvents(currentEvent, nextEvent))
                {
                    currentIndex++;
                    var currentChange = EventToChange(nextEvent);
                    if (childChanges.Count == 0)
                    {
                        if (createdElement != null && !elementSources.ContainsKey(createdElement))
                        {
                            var r = currentEvent.Feature as IReference;
                            if (r == null || !r.IsContainment)
                            {
                                throw new InvalidOperationException("The sequence of events could not be interpreted.");
                            }
                            elementSources.Add(createdElement, currentChange);
                        }
                        changes.Add(currentChange);
                        return;
                    }
                    if (createdElement != null)
                    {
                        var child = childChanges.OfType<ICompositionDeletion>().FirstOrDefault(c => c.DeletedElement == createdElement);
                        if (child != null)
                        {
                            childChanges.Remove(child);
                        }
                        var composition = currentChange as ICompositionInsertion;
                        if (composition == null)
                        {
                            composition = childChanges.OfType<ICompositionInsertion>().FirstOrDefault();
                        }
                        if (composition != null)
                        {
                            if (child != null)
                            {
                                if (childChanges.Count > 0)
                                {
                                    CreateTransaction(changes, childChanges, composition.ConvertIntoMove(child));
                                }
                                else
                                {
                                    changes.Add(composition.ConvertIntoMove(child));
                                }
                                return;
                            }
                            else
                            {
                                elementSources.Add(createdElement, composition);
                            }
                        }
                    }
                    CreateTransaction(changes, childChanges, currentChange);
                    return;
                }
                else if ((nextEvent.ChangeType == ChangeType.PropertyChanged || nextEvent.ChangeType == ChangeType.CollectionChanged) && nextEvent.Element == createdElement)
                {
                    // These events are opposites of creation messages and can be ignored
                    currentIndex++;
                    continue;
                }
                else
                {
                    ParseChange(childChanges, ref currentIndex);
                }
            }

            var element = currentEvent.Element;
            while (element != null)
            {
                if (elementSources.ContainsKey(element))
                {
                    changes.AddRange(childChanges);
                    return;
                }
                else
                {
                    element = element.Parent;
                }
            }

            throw new InvalidOperationException("No corresponding after-event found for " + currentEvent.ToString());
        }

        private static void CreateTransaction(List<IModelChange> changes, List<IModelChange> childChanges, IModelChange currentChange)
        {
            var transaction = new ElementaryChangeTransaction();
            transaction.SourceChange = currentChange;
            transaction.NestedChanges.AddRange(childChanges);
            changes.Add(transaction);
        }

        private void InterpretPastChanges(List<IModelChange> changes, BubbledChangeEventArgs currentEvent)
        {
            var r = currentEvent.Feature as IReference;
            if (r != null)
            {
                if (r.IsContainment)
                {
                    var change = EventToChange(currentEvent);
                    throw new NotImplementedException();
                }
                else if (r.Opposite != null && r.Opposite.IsContainment && r.UpperBound == 1)
                {
                    var change = EventToChange(currentEvent);
                    // last event created the item
                    var sourceChange = changes[changes.Count - 1];
                    var transaction = new ElementaryChangeTransaction
                    {
                        AffectedElement = null,
                        SourceChange = sourceChange
                    };
                    transaction.NestedChanges.Add(change);
                    changes[changes.Count - 1] = transaction;
                    return;
                }
            }
            throw new InvalidOperationException($"The {currentEvent.ChangeType} event of {currentEvent.Element} could not be interpreted because {currentEvent.Feature} is not a reference.");
        }

        private bool MatchEvents(BubbledChangeEventArgs beforeEvent, BubbledChangeEventArgs afterEvent)
        {
            if (beforeEvent.Element != afterEvent.Element)
                return false;

            switch (beforeEvent.ChangeType)
            {
                case ChangeType.CollectionChanging:
                    return afterEvent.ChangeType == ChangeType.CollectionChanged;
                case ChangeType.PropertyChanging:
                    return afterEvent.ChangeType == ChangeType.PropertyChanged;
                case ChangeType.ElementCreated:
                    return false;
                default:
                    throw new ArgumentException(nameof(beforeEvent.ChangeType));
            }
        }

        private IModelChange EventToChange(BubbledChangeEventArgs e)
        {
            if (e.Feature == null) throw new InvalidOperationException($"The property {e.PropertyName} of {e.Element} was not mapped to a feature");
            switch (e.ChangeType)
            {
                case ChangeType.PropertyChanged:
                    return CreatePropertyChange(e);
                case ChangeType.CollectionChanged:
                    var collectionChangeArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                    switch (collectionChangeArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            if (collectionChangeArgs.NewItems.Count > 1) throw new NotImplementedException();
                            return CreateCollectionInsertion(e, collectionChangeArgs);
                        case NotifyCollectionChangedAction.Remove:
                            if (collectionChangeArgs.OldItems.Count > 1) throw new NotImplementedException();
                            return CreateCollectionDeletion(e, collectionChangeArgs);
                        case NotifyCollectionChangedAction.Reset:
                            var reference = e.Feature as IReference;
                            if (reference != null)
                            {
                                if (reference.IsContainment)
                                {
                                    return new CompositionCollectionReset
                                    {
                                        AffectedElement = e.Element,
                                        Feature = e.Feature
                                    };
                                }
                                else
                                {
                                    return new AssociationCollectionReset
                                    {
                                        AffectedElement = e.Element,
                                        Feature = e.Feature
                                    };
                                }
                            }
                            else
                            {
                                return new AttributeCollectionReset
                                {
                                    AffectedElement = e.Element,
                                    Feature = e.Feature
                                };
                            }
                        default:
                            throw new NotSupportedException("The CollectionChanged action " + collectionChangeArgs.Action + " is not supported.");
                    }
                default:
                    throw new InvalidOperationException("The " + e.ChangeType + " event cannot be the base of a model change. Use after-events only.");
            }
        }

        private static IModelChange CreateCollectionInsertion(BubbledChangeEventArgs e, NotifyCollectionChangedEventArgs collectionChangeArgs)
        {
            if (e.Feature.IsOrdered)
            {
                var reference = e.Feature as IReference;
                if (reference != null)
                {
                    if (reference.IsContainment)
                    {
                        return new CompositionListInsertion
                        {
                            AddedElement = collectionChangeArgs.NewItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature,
                            Index = collectionChangeArgs.NewStartingIndex
                        };
                    }
                    else
                    {
                        return new AssociationListInsertion
                        {
                            AddedElement = collectionChangeArgs.NewItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature,
                            Index = collectionChangeArgs.NewStartingIndex
                        };
                    }
                }
                else
                {
                    return new AttributeListInsertion
                    {
                        AddedValue = e.Feature.Type.Serialize(collectionChangeArgs.NewItems[0]),
                        AffectedElement = e.Element,
                        Feature = e.Feature,
                        Index = collectionChangeArgs.NewStartingIndex
                    };
                }
            }
            else
            {
                var reference = e.Feature as IReference;
                if (reference != null)
                {
                    if (reference.IsContainment)
                    {
                        return new CompositionCollectionInsertion
                        {
                            AddedElement = collectionChangeArgs.NewItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature
                        };
                    }
                    else
                    {
                        return new AssociationCollectionInsertion
                        {
                            AddedElement = collectionChangeArgs.NewItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature
                        };
                    }
                }
                else
                {
                    return new AttributeCollectionInsertion
                    {
                        AddedValue = e.Feature.Type.Serialize(collectionChangeArgs.NewItems[0]),
                        AffectedElement = e.Element,
                        Feature = e.Feature
                    };
                }
            }
        }

        private static IModelChange CreateCollectionDeletion(BubbledChangeEventArgs e, NotifyCollectionChangedEventArgs collectionChangeArgs)
        {
            if (e.Feature.IsOrdered)
            {
                var reference = e.Feature as IReference;
                if (reference != null)
                {
                    if (reference.IsContainment)
                    {
                        return new CompositionListDeletion
                        {
                            DeletedElement = collectionChangeArgs.OldItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature,
                            Index = collectionChangeArgs.OldStartingIndex
                        };
                    }
                    else
                    {
                        return new AssociationListDeletion
                        {
                            DeletedElement = collectionChangeArgs.OldItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature,
                            Index = collectionChangeArgs.OldStartingIndex
                        };
                    }
                }
                else
                {
                    return new AttributeListDeletion
                    {
                        DeletedValue = e.Feature.Type.Serialize(collectionChangeArgs.OldItems[0]),
                        AffectedElement = e.Element,
                        Feature = e.Feature,
                        Index = collectionChangeArgs.OldStartingIndex
                    };
                }
            }
            else
            {
                var reference = e.Feature as IReference;
                if (reference != null)
                {
                    if (reference.IsContainment)
                    {
                        return new CompositionCollectionDeletion
                        {
                            DeletedElement = collectionChangeArgs.OldItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature
                        };
                    }
                    else
                    {
                        return new AssociationCollectionDeletion
                        {
                            DeletedElement = collectionChangeArgs.OldItems[0] as IModelElement,
                            AffectedElement = e.Element,
                            Feature = e.Feature
                        };
                    }
                }
                else
                {
                    return new AttributeCollectionDeletion
                    {
                        DeletedValue = e.Feature.Type.Serialize(collectionChangeArgs.OldItems[0]),
                        AffectedElement = e.Element,
                        Feature = e.Feature
                    };
                }
            }
        }

        private static IModelChange CreatePropertyChange(BubbledChangeEventArgs e)
        {
            var reference = e.Feature as IReference;
            var valChange = e.OriginalEventArgs as ValueChangedEventArgs;
            if (reference != null)
            {
                if (reference.IsContainment)
                {
                    var change = new CompositionChange
                    {
                        AffectedElement = e.Element,
                        Feature = e.Feature,
                        OldValue = valChange.OldValue as IModelElement,
                        NewValue = valChange.NewValue as IModelElement
                    };
                    if (change.DeletedElement == null || change.DeletedElement.Parent == null)
                    {
                        return change;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    return new AssociationChange
                    {
                        AffectedElement = e.Element,
                        Feature = e.Feature,
                        OldValue = valChange.OldValue as IModelElement,
                        NewValue = valChange.NewValue as IModelElement
                    };
                }
            }
            else
            {
                return new AttributeChange
                {
                    AffectedElement = e.Element,
                    Feature = e.Feature,
                    OldValue = valChange.OldValue != null ? e.Feature.Type.Serialize(valChange.OldValue) : null,
                    NewValue = valChange.NewValue != null ? e.Feature.Type.Serialize(valChange.NewValue) : null
                };
            }
        }
    }

    public class ChangeModel : Model
    {
        private Dictionary<IModelElement, Uri> uriMappings;
        private Dictionary<IModelElement, IModelChange> changeSources;

        public ChangeModel(Dictionary<IModelElement, Uri> uriMappings, Dictionary<IModelElement, IModelChange> changeSources)
        {
            this.uriMappings = uriMappings;
            this.changeSources = changeSources;
        }

        public override Uri CreateUriForElement(IModelElement element)
        {
            IModelChange elementSource;
            if (changeSources.TryGetValue(element, out elementSource))
            {
                var me = elementSource as ModelElement;
                return me.CreateUriWithFragment("addedElement", false);
            }
            Uri deletedUri;
            if (uriMappings.TryGetValue(element, out deletedUri))
            {
                return deletedUri;
            }
            return base.CreateUriForElement(element);
        }
    }
 }

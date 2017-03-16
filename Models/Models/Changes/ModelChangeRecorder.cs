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

namespace NMF.Models.Changes
{
    /// <summary>
    /// Represents a recorder for changes to a model.
    /// </summary>
    public partial class ModelChangeRecorder
    {
        private List<BubbledChangeEventArgs> recordedEvents = new List<BubbledChangeEventArgs>();
        private readonly Dictionary<BubbledChangeEventArgs, List<object>> preResetItems = new Dictionary<BubbledChangeEventArgs, List<object>>();
        private readonly Dictionary<BubbledChangeEventArgs, BubbledChangeEventArgs> changingFromChangedEvent = new Dictionary<BubbledChangeEventArgs, BubbledChangeEventArgs>();
        private readonly Dictionary<IModelElement, IModelElement> parentBeforeDeletion = new Dictionary<IModelElement, IModelElement>();
        private readonly Dictionary<IModelElement, List<IModelElement>> childrenOfDeletedElements = new Dictionary<IModelElement, List<IModelElement>>();

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
            if (_isInvertible)
            {
                if (e.ChangeType == ChangeType.ModelElementDeleting)
                {
                    var parent = e.Element.Parent;
                    if (childrenOfDeletedElements.ContainsKey(parent))
                    {
                        childrenOfDeletedElements[parent].Add(e.Element);
                    }
                    else
                    {
                        childrenOfDeletedElements[parent] = new List<IModelElement>() {e.Element};
                    }
                }
                if (e.ChangeType == ChangeType.CollectionChanging)
                {
                    var cArgs = e.OriginalEventArgs as NotifyCollectionChangingEventArgs;
                    if (cArgs.Action == NotifyCollectionChangedAction.Reset)
                    {
                        var property = e.Element.GetType().GetProperty(e.PropertyName);
                        var list = (property.GetValue(e.Element, null) as IEnumerable<object>).ToList();
                        preResetItems[e] = list;
                    }
                    if (cArgs.Action == NotifyCollectionChangedAction.Remove)
                    {
                        var parent = e.Element.Parent;
                        if (!parentBeforeDeletion.ContainsKey(e.Element))
                        {
                            parentBeforeDeletion[e.Element] = parent; //e.Element; // parent;
                        }

                        if (!childrenOfDeletedElements.ContainsKey(e.Element))
                        {
                            childrenOfDeletedElements[e.Element] = new List<IModelElement>();
                        }
                    }
                }
            }
            recordedEvents.Add(e);
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
            int currentIndex = 0;
            var list = changes.Changes;
            while (currentIndex < recordedEvents.Count)
                list.Add(ParseChange(ref currentIndex));
            return changes;
        }

        private IModelChange ParseChange(ref int currentIndex)
        {
            var currentEvent = recordedEvents[currentIndex++];
            if (currentEvent.ChangeType == ChangeType.ModelElementCreated)
                return EventToChange(currentEvent);

            var childChanges = new List<IModelChange>();

            while (currentIndex < recordedEvents.Count)
            {
                var nextEvent = recordedEvents[currentIndex];

                if (MatchEvents(currentEvent, nextEvent))
                {
                    currentIndex++;
                    var currentChange = EventToChange(nextEvent);
                    if (childChanges.Count == 0)
                    {
                        return currentChange;
                    }
                    else
                    {
                        var transaction = new ElementaryChangeTransaction();
                        transaction.SourceChange = currentChange;
                        transaction.NestedChanges.AddRange(childChanges);
                        return transaction;
                    }
                }
                else
                {
                    childChanges.Add(ParseChange(ref currentIndex));
                }
            }

            throw new InvalidOperationException("No corresponding after-event found for " + currentEvent.ToString());
        }

        private bool MatchEvents(BubbledChangeEventArgs beforeEvent, BubbledChangeEventArgs afterEvent)
        {
            if (beforeEvent.AbsoluteUri != afterEvent.AbsoluteUri)
                return false;

            switch (beforeEvent.ChangeType)
            {
                case ChangeType.CollectionChanging:
                    if (_isInvertible)
                    {
                        var cArgs = beforeEvent.OriginalEventArgs as NotifyCollectionChangingEventArgs;
                        if (cArgs.Action == NotifyCollectionChangedAction.Reset)
                        {
                            changingFromChangedEvent[afterEvent] = beforeEvent;
                        }
                    }
                    return afterEvent.ChangeType == ChangeType.CollectionChanged;
                case ChangeType.ModelElementDeleting:
                    return afterEvent.ChangeType == ChangeType.ModelElementDeleted;
                case ChangeType.PropertyChanging:
                    return afterEvent.ChangeType == ChangeType.PropertyChanged;
                case ChangeType.ModelElementCreated:
                    return false;
                default:
                    throw new ArgumentException(nameof(beforeEvent.ChangeType));
            }
        }

        private IModelChange EventToChange(BubbledChangeEventArgs e)
        {
            if (e.Feature == null) return null;
            switch (e.ChangeType)
            {
                case ChangeType.ModelElementCreated:
                case ChangeType.ModelElementDeleted:
                    return null;
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
                            DeletedElementUri = (collectionChangeArgs.OldItems[0] as IModelElement).RelativeUri,
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
                    return new CompositionChange
                    {
                        AffectedElement = e.Element,
                        Feature = e.Feature,
                        OldValue = valChange.OldValue as IModelElement,
                        NewValue = valChange.NewValue as IModelElement
                    };
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
}

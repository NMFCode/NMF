﻿using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using NMF.Utilities;
using NMF.Models.Meta;

namespace NMF.Models.Changes
{
    /// <summary>
    /// Represents a recorder for changes to a model.
    /// </summary>
    public partial class ModelChangeRecorder
    {
        private readonly List<BubbledChangeEventArgs> recordedEvents = new List<BubbledChangeEventArgs>();
        private readonly Dictionary<IModelElement, Uri> uriMappings = new Dictionary<IModelElement, Uri>();
        private Dictionary<IModelElement, ElementSourceInfo> elementSources = new Dictionary<IModelElement, ElementSourceInfo>();
        private bool isRecording;
        private readonly List<IModelElement> attachedElements = new List<IModelElement>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="isInvertible">ignored</param>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete("isInvertible is actually ignored")]
#pragma warning restore S1133 // Deprecated code should be removed
        public ModelChangeRecorder(bool isInvertible) : this()
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ModelChangeRecorder() { }
        
        /// <summary>
        /// Checks whether the recorder is attached to a model element.
        /// </summary>
        public bool IsRecording => isRecording;

        /// <summary>
        /// Gets the attached model element or null, if the recorder is not attached.
        /// </summary>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete("Use AttachedElements instead")]
#pragma warning restore S1133 // Deprecated code should be removed
        public IModelElement AttachedElement => attachedElements.FirstOrDefault();

        /// <summary>
        /// Gets the attached model elements
        /// </summary>
        public IEnumerable<IModelElement> AttachedElements => attachedElements.AsReadOnly();
        
        /// <summary>
        /// Attaches the recorder to the given model element. The recorder will track all
        /// changes made to the given element and every element further down in the
        /// containment hierarchy.
        /// </summary>
        /// <param name="element"></param>
        public void Start(IModelElement element)
        {
            Attach(element);
            Start();
        }

        /// <summary>
        /// Starts recording
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the recorder is already recording</exception>
        public void Start()
        {
            if (IsRecording)
                throw new InvalidOperationException("The recorder is already recording.");

            isRecording = true;
        }

        /// <summary>
        /// Attaches the recorder to the given model element. The recorder will track all
        /// changes made to the given element and every element further down in the
        /// containment hierarchy.
        /// </summary>
        /// <param name="element">The model element to attach to</param>
        public void Attach(IModelElement element)
        {
            Attach(element, true);
        }

        /// <summary>
        /// Attaches the recorder to the given model element. The recorder will track all
        /// changes made to the given element and every element further down in the
        /// containment hierarchy.
        /// </summary>
        /// <param name="element">The model element to attach to</param>
        /// <param name="serializable">True, if the recoirder should support serialization, otherwise false</param>
        public void Attach(IModelElement element, bool serializable)
        {
            if (element != null)
            {
                element.BubbledChange += OnBubbledChange;
                if (serializable && element is ModelElement elementMe)
                {
                    elementMe.RequestUris();
                }
                attachedElements.Add(element);
            }
        }

        /// <summary>
        /// Detaches from the given model element
        /// </summary>
        /// <param name="element">The element to detach from</param>
        public void Detach(IModelElement element)
        {
            Detach(element, true);
        }

        /// <summary>
        /// Detaches from the given model element
        /// </summary>
        /// <param name="element">The element to detach from</param>
        /// <param name="serializable">True, if the recoirder should support serialization, otherwise false</param>
        public void Detach(IModelElement element, bool serializable)
        {
            if (serializable && element is ModelElement elementMe)
            {
                elementMe.UnregisterUriRequest();
            }
            element.BubbledChange -= OnBubbledChange;
            attachedElements.Remove(element);
        }

        /// <summary>
        /// Detaches from all attached model elements
        /// </summary>
        public void DetachAll()
        {
            DetachAll(true);
        }

        /// <summary>
        /// Detaches from all attached model elements
        /// </summary>
        /// <param name="serializable">True, if the recoirder should support serialization, otherwise false</param>
        public void DetachAll(bool serializable)
        {
            foreach (var element in attachedElements)
            {
                if (serializable && element is ModelElement elementMe)
                {
                    elementMe.UnregisterUriRequest();
                }
                element.BubbledChange -= OnBubbledChange;
            }
            attachedElements.Clear();
        }

        /// <summary>
        /// Resets the model change recorder such that it can be started again
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the recorder is already recording</exception>
        public void Reset()
        {
            if (IsRecording)
                throw new InvalidOperationException("The recorder is already recording.");

            recordedEvents.Clear();
            elementSources = new Dictionary<IModelElement, ElementSourceInfo>();
        }

        /// <summary>
        /// Detaches the recorder, stopping the change tracking.
        /// </summary>
        public void Stop(bool detachAll = true)
        {
            if (!IsRecording)
                throw new InvalidOperationException("The recorder is not attached.");

            isRecording = false;

            if (detachAll)
            {
                DetachAll();
            }
        }

        private void OnBubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (!IsRecording) { return; }
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
                    RegisterChangedUris(origin, originalUri, element, me);
                    foreach (var child in element.Children)
                    {
                        stack.Push(child);
                    }
                }
            }
        }

        private void RegisterChangedUris(IModelElement origin, Uri originalUri, IModelElement element, ModelElement me)
        {
            if (element == origin)
            {
                uriMappings.Add(element, originalUri);
            }
            else
            {
                var fragment = me.CreateUriWithFragment(null, false, origin);
                var fragmentString = fragment != null ? fragment.OriginalString + "/" : null;
                if (originalUri.IsAbsoluteUri)
                {
#pragma warning disable S6610 // "StartsWith" and "EndsWith" overloads that take a "char" should be used instead of the ones that take a "string"
                    if (!originalUri.Fragment.EndsWith("/"))
                    {
                        fragmentString = "/" + fragmentString;
                    }
                    uriMappings.Add(element, new Uri(originalUri, originalUri.Fragment + fragmentString));
                }
                else
                {
                    if (!originalUri.OriginalString.EndsWith("/"))
#pragma warning restore S6610 // "StartsWith" and "EndsWith" overloads that take a "char" should be used instead of the ones that take a "string"
                    {
                        fragmentString = "/" + fragmentString;
                    }
                    uriMappings.Add(element, new Uri(originalUri.OriginalString + fragmentString, UriKind.Relative));
                }
            }
        }

        /// <summary>
        /// Returns previously recorded changes in a tree hierarchy.
        /// </summary>
        /// <returns></returns>
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
            var allDeletions = list.OfType<ICompositionDeletion>()
                .Concat(list.SelectMany(c => c.Descendants().OfType<ICompositionDeletion>())
                .Where(c2 => c2.Parent != null &&
                       !(c2.Parent is CompositionMoveIntoProperty) &&
                       !(c2.Parent is CompositionMoveToCollection) &&
                       !(c2.Parent is CompositionMoveToList)));
            if (allDeletions.Any(d => d.DeletedElement?.Parent != null))
            {
                throw new InvalidOperationException("There are element deletions of elements that are not deleted.");
            }
        }

        private void ConnectInsertionsAndDeletions(List<IModelChange> list)
        {
            RemoveChangesOfInsertedElements(list);
            int currentIndex = 0;
            while (currentIndex < list.Count)
            {
                if (list[currentIndex] is ICompositionDeletion deletion && deletion.DeletedElement?.Parent != null)
                {
                    if (elementSources.ContainsKey(deletion.DeletedElement))
                    {
                        elementSources.Remove(deletion.DeletedElement);
                        foreach (var item in deletion.DeletedElement.Descendants())
                        {
                            elementSources.Remove(item);
                        }
                        list.RemoveAt(currentIndex);
                        continue;
                    }
                    if (currentIndex < list.Count)
                    {
                        TryEliminateDeletion(list, ref currentIndex, deletion);
                    }
                }
                currentIndex++;
            }
        }

        private void RemoveChangesOfInsertedElements(List<IModelChange> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] is IElementaryChange elementary && elementary.AffectedElement != null && elementSources.ContainsKey(elementary.AffectedElement))
                {
                    list.RemoveAt(i);
                }
                else
                {
                    if (list[i] is IChangeTransaction t && t.NestedChanges.Count == 1)
                    {
                        // keep transactions, otherwise other changes are lost
                    }
                }
            }
        }

        private void TryEliminateDeletion(List<IModelChange> list, ref int currentIndex, ICompositionDeletion deletion)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var insertion = list[i] as ICompositionInsertion;
                if (insertion == null)
                {
                    if (list[i] is IChangeTransaction transaction && transaction.NestedChanges.Count == 1)
                    {
                        insertion = transaction.NestedChanges[0] as ICompositionInsertion;
                    }
                }
                if (insertion != null && insertion.AddedElement == deletion.DeletedElement)
                {
                    RemoveAllElementSources(insertion.AddedElement);
                    list[i] = insertion.ConvertIntoMove(deletion);
                    list.RemoveAt(currentIndex);
                    currentIndex--;
                    return;
                }
                else if (insertion != null && IsAncestor(insertion.AddedElement, deletion.DeletedElement))
                {
                    RemoveAllElementSources(deletion.DeletedElement);
                    list.RemoveAt(currentIndex);
                    list.Insert(currentIndex, CreateMove(deletion));
                    currentIndex--;
                    return;
                }
            }
        }

        private void RemoveAllElementSources(IModelElement element)
        {
            if (elementSources.Remove(element))
            {
                foreach (var item in element.Descendants())
                {
                    elementSources.Remove(item);
                }
            }
        }

        private IModelChange CreateMove(ICompositionDeletion deletion)
        {
            var parent = deletion.DeletedElement.Parent;
            int index;
            var reference = parent.GetContainerReference(deletion.DeletedElement, out index);
            if (reference == null || !reference.IsContainment) throw new InvalidOperationException("There is something wrong with your generated code. Please regenerate and try again. If the problem persists, please contact the NMF developers.");
            if (reference.UpperBound == 1)
            {
                return new CompositionMoveIntoProperty
                {
                    AffectedElement = parent,
                    Feature = reference,
                    NewValue = deletion.DeletedElement,
                    OldValue = null,
                    Origin = deletion
                };
            }
            else
            {
                if (reference.IsOrdered)
                {
                    return new CompositionMoveToList
                    {
                        AffectedElement = parent,
                        Feature = reference,
                        MovedElement = deletion.DeletedElement,
                        Index = index,
                        Origin = deletion
                    };
                }
                else
                {
                    return new CompositionMoveToCollection
                    {
                        AffectedElement = parent,
                        Feature = reference,
                        MovedElement = deletion.DeletedElement,
                        Origin = deletion
                    };
                }
            }
        }

        private static bool IsAncestor(IModelElement ancestor, IModelElement child)
        {
            if (child == null) return false;
            return child.Parent == ancestor || IsAncestor(ancestor, child.Parent);
        }

        private bool ParseChange(List<IModelChange> changes, ref int currentIndex)
        {
            int originalCurrentIndex = currentIndex;
            do
            {
                BubbledChangeEventArgs currentEvent;
                IModelElement createdElement = null;

                var shortCircuit = LookAtElementsCreated(changes, ref currentIndex, out currentEvent, ref createdElement);
                if (shortCircuit.HasValue) return shortCircuit.Value;

                var childChanges = new List<IModelChange>();

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
                            AddCurrentChange(changes, currentEvent, createdElement, currentChange);
                            return false;
                        }
                        if (createdElement != null)
                        {
                            if (currentChange is not ICompositionInsertion composition)
                            {
                                composition = childChanges.OfType<ICompositionInsertion>().FirstOrDefault();
                            }
                            if (composition != null)
                            {
                                var child = childChanges.OfType<ICompositionDeletion>().FirstOrDefault(c => c.DeletedElement == createdElement);
                                if (child != null)
                                {
                                    ConvertToMove(changes, childChanges, composition, child);
                                    return false;
                                }
                                else
                                {
                                    AddAllElementSources(createdElement, composition);
                                }
                            }
                            else
                            {
                                if (changes.Count == 0 && childChanges.Count == 1 && currentEvent.Feature is IReference r && !r.IsContainment && r.Opposite != null && r.Opposite.IsContainment)
                                {
                                    changes.AddRange(childChanges);
                                    return true;
                                }
                            }
                        }
                        CreateTransaction(changes, childChanges, currentChange);
                        return false;
                    }
                    else if ((nextEvent.ChangeType == ChangeType.PropertyChanged || nextEvent.ChangeType == ChangeType.CollectionChanged) && nextEvent.Element == createdElement)
                    {
                        // These events are opposites of creation messages and can be ignored
                        currentIndex++;
                        continue;
                    }
                    else
                    {
                        if (ParseChange(childChanges, ref currentIndex) && createdElement == null)
                        {
                            var deletion = childChanges.OfType<ICompositionDeletion>().FirstOrDefault();
                            if (deletion != null)
                            {
                                createdElement = deletion.DeletedElement;
                            }
                        }
                    }
                }

                var element = currentEvent.Element;
                while (element != null)
                {
                    if (elementSources.ContainsKey(element))
                    {
                        changes.AddRange(childChanges);
                        return true;
                    }
                    else
                    {
                        element = element.Parent;
                    }
                }
                originalCurrentIndex++;
                if (originalCurrentIndex < changes.Count)
                {
                    currentIndex = originalCurrentIndex;
                }
                else
                {
                    return false;
                }
            } while (true);
        }

        private static void ConvertToMove(List<IModelChange> changes, List<IModelChange> childChanges, ICompositionInsertion composition, ICompositionDeletion child)
        {
            childChanges.Remove(child);

            if (childChanges.Count > 0)
            {
                CreateTransaction(changes, childChanges, composition.ConvertIntoMove(child));
            }
            else
            {
                changes.Add(composition.ConvertIntoMove(child));
            }
        }

        private bool? LookAtElementsCreated(List<IModelChange> changes, ref int currentIndex, out BubbledChangeEventArgs currentEvent, ref IModelElement createdElement)
        {
            do
            {
                currentEvent = recordedEvents[currentIndex++];
                if (currentEvent.ChangeType == ChangeType.ElementCreated)
                {
                    createdElement = currentEvent.Element;
                }
                if (currentIndex == recordedEvents.Count)
                {
                    return createdElement != null;
                }
            } while (currentEvent.ChangeType == ChangeType.ElementCreated);

            if (currentEvent.ChangeType == ChangeType.PropertyChanged || currentEvent.ChangeType == ChangeType.CollectionChanged)
            {
                InterpretPastChanges(changes, currentEvent);
                return createdElement != null;
            }
            return null;
        }

        private void AddCurrentChange(List<IModelChange> changes, BubbledChangeEventArgs currentEvent, IModelElement createdElement, IModelChange currentChange)
        {
            if (createdElement != null && !elementSources.ContainsKey(createdElement))
            {
                if (currentEvent.Feature is not IReference r || !r.IsContainment)
                {
                    throw new InvalidOperationException("The sequence of events could not be interpreted.");
                }
                AddAllElementSources(createdElement, currentChange);
            }
            changes.Add(currentChange);
        }

        private void AddAllElementSources(IModelElement createdElement, IModelChange currentChange)
        {
            var changeMe = (ModelElement)currentChange;
            if (!elementSources.ContainsKey(createdElement))
            {
                elementSources.Add(createdElement, new ElementSourceInfo(changeMe, "addedElement"));
                foreach (var item in createdElement.Descendants().OfType<ModelElement>())
                {
                    var relative = item.CreateUriWithFragment(null, false, createdElement);
                    if (relative != null) elementSources.Add(item, new ElementSourceInfo(changeMe, "addedElement/" + relative.OriginalString));
                }
            }
        }

        private static void CreateTransaction(List<IModelChange> changes, List<IModelChange> childChanges, IModelChange currentChange)
        {
            var transaction = new ChangeTransaction();
            transaction.SourceChange = currentChange;
            transaction.NestedChanges.AddRange(childChanges);
            changes.Add(transaction);
        }

        private void InterpretPastChanges(List<IModelChange> changes, BubbledChangeEventArgs currentEvent)
        {
            if (currentEvent.Feature is IReference r)
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
                    var transaction = new ChangeTransaction
                    {
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
                case ChangeType.OperationCalling:
                    return afterEvent.ChangeType == ChangeType.OperationCalled;
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
                case ChangeType.OperationCalled:
                    return CreateOperationCall(e);
                case ChangeType.CollectionChanged:
                    return CreateCollectionChanged(e);
                default:
                    throw new InvalidOperationException("The " + e.ChangeType + " event cannot be the base of a model change. Use after-events only.");
            }
        }

        private static IModelChange CreateCollectionChanged(BubbledChangeEventArgs e)
        {
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
                    return CreateReset(e);
                default:
                    throw new NotSupportedException("The CollectionChanged action " + collectionChangeArgs.Action + " is not supported.");
            }
        }

        private static IModelChange CreateReset(BubbledChangeEventArgs e)
        {
            if (e.Feature is IReference reference)
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
        }

        private OperationCall CreateOperationCall(BubbledChangeEventArgs e)
        {
            var original = e.OriginalEventArgs as OperationCallEventArgs;
            var opCall = new OperationCall
            {
                Operation = original.Operation,
                TargetElement = original.Target
            };
            var index = 0;
            foreach (var par in original.Operation.Parameters)
            {
                if (par.Type is not IReferenceType rType)
                {
                    opCall.Arguments.Add(new ValueArgument
                    {
                        Name = par.Name,
                        Value = par.Type.Serialize(original.Arguments[index])
                    });
                }
                else
                {
                    opCall.Arguments.Add(new ReferenceArgument
                    {
                        Name = par.Name,
                        Value = (IModelElement)original.Arguments[index]
                    });
                }
                index++;
            }
            return opCall;
        }

        private static IModelChange CreateCollectionInsertion(BubbledChangeEventArgs e, NotifyCollectionChangedEventArgs collectionChangeArgs)
        {
            if (e.Feature.IsOrdered)
            {
                if (e.Feature is IReference reference)
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
                if (e.Feature is IReference reference)
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
                if (e.Feature is IReference reference)
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
                if (e.Feature is IReference reference)
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
            var valChange = e.OriginalEventArgs as ValueChangedEventArgs;
            if (e.Feature is IReference reference)
            {
                if (reference.IsContainment)
                {
                    var change = new CompositionPropertyChange
                    {
                        AffectedElement = e.Element,
                        Feature = e.Feature,
                        OldValue = valChange.OldValue as IModelElement,
                        NewValue = valChange.NewValue as IModelElement
                    };
                    if (change.OldValue == null || change.OldValue.Parent == null)
                    {
                        return change;
                    }
                    else
                    {
                        // throw new NotImplementedException();
                        return change;
                    }
                }
                else
                {
                    return new AssociationPropertyChange
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
                return new AttributePropertyChange
                {
                    AffectedElement = e.Element,
                    Feature = e.Feature,
                    OldValue = valChange.OldValue != null ? e.Feature.Type.Serialize(valChange.OldValue) : null,
                    NewValue = valChange.NewValue != null ? e.Feature.Type.Serialize(valChange.NewValue) : null
                };
            }
        }
    }

    internal struct ElementSourceInfo
    {
        public ModelElement Change { get; set; }
        public string Fragment { get; set; }

        public ElementSourceInfo(ModelElement change, string fragment) : this()
        {
            Change = change;
            Fragment = fragment;
        }
    }

    internal class ChangeModel : Model
    {
        private readonly Dictionary<IModelElement, Uri> uriMappings;
        private readonly Dictionary<IModelElement, ElementSourceInfo> changeSources;

        internal ChangeModel(Dictionary<IModelElement, Uri> uriMappings, Dictionary<IModelElement, ElementSourceInfo> changeSources)
        {
            this.uriMappings = uriMappings;
            this.changeSources = changeSources;
        }

        public override Uri CreateUriForElement(IModelElement element)
        {
            ElementSourceInfo elementSource;
            if (changeSources.TryGetValue(element, out elementSource))
            {
                var me = elementSource.Change;
                return SimplifyUri(me.CreateUriWithFragment(elementSource.Fragment, false));
            }
            Uri deletedUri;
            if (uriMappings.TryGetValue(element, out deletedUri))
            {
                return SimplifyUri(deletedUri);
            }
            return base.CreateUriForElement(element);
        }

        protected internal override bool SerializeAsReference(IModelElement element)
        {
            if (element == null) return false;
            if (changeSources.ContainsKey(element)) return false;
            if (uriMappings.ContainsKey(element)) return true;
            return base.SerializeAsReference(element);
        }

        protected internal override void EnsureAllElementsContained()
        {
            // not ensuring this for change models
        }
    }
 }

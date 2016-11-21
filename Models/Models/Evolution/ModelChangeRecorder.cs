using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NMF.Collections.ObjectModel;
using NMF.Models.Meta;
using NMF.Utilities;
using Type = System.Type;

namespace NMF.Models.Evolution
{
    /// <summary>
    /// Represents a recorder for changes to a model.
    /// </summary>
    public class ModelChangeRecorder
    {
        private List<BubbledChangeEventArgs> recordedEvents = new List<BubbledChangeEventArgs>();
        private readonly Dictionary<BubbledChangeEventArgs, List<object>> preResetItems = new Dictionary<BubbledChangeEventArgs, List<object>>();
        private readonly Dictionary<BubbledChangeEventArgs, BubbledChangeEventArgs> changingFromChangedEvent = new Dictionary<BubbledChangeEventArgs, BubbledChangeEventArgs>();

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
            //e.AbsoluteUri = e.Element.AbsoluteUri;
            if (_isInvertible && e.ChangeType == ChangeType.CollectionChanging)
            {
                var cArgs = e.OriginalEventArgs as NotifyCollectionChangingEventArgs;
                if (cArgs.Action == NotifyCollectionChangedAction.Reset)
                {
                    var property = e.Element.GetType().GetProperty(e.PropertyName);
                    var list = (property.GetValue(e.Element, null) as IEnumerable<object>).ToList();
                    preResetItems[e] = list;
                }
            }
            recordedEvents.Add(e);
        }

        public Task<ModelChangeCollection> GetModelChangesAsync()
        {
            return Task.Factory.StartNew(GetModelChanges);
        }

        /// <summary>
        /// Returns previously recorded changes in a tree hierarchy.
        /// </summary>
        /// <returns></returns>
        public ModelChangeCollection GetModelChanges()
        {
            return new ModelChangeCollection(ParseChangeList());
        }

        private List<IModelChange> ParseChangeList()
        {
            int currentIndex = 0;
            var list = new List<IModelChange>();
            while (currentIndex < recordedEvents.Count)
                list.Add(ParseChange(ref currentIndex));
            return list;
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
                        return currentChange;
                    else
                        return new ChangeTransaction(currentChange, childChanges);
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
            switch (e.ChangeType)
            {
                case ChangeType.ModelElementCreated:
                    return new ElementCreation(e.Element);
                case ChangeType.ModelElementDeleted:
                    return new ElementDeletion(e.AbsoluteUri);
                case ChangeType.PropertyChanged:
                    var valueChangeArgs = (ValueChangedEventArgs)e.OriginalEventArgs;
                    var oldUri = (valueChangeArgs.OldValue as IModelElement)?.AbsoluteUri;
                    return CreatePropertyChange(e.Element, e.AbsoluteUri, e.PropertyName, valueChangeArgs.OldValue, valueChangeArgs.NewValue, oldUri, e.ChildrenUris.First()); 
                case ChangeType.CollectionChanged:
                    var collectionChangeArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                    switch (collectionChangeArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            return CreateInsertion(e.Element, e.AbsoluteUri, e.PropertyName, collectionChangeArgs.NewStartingIndex, collectionChangeArgs.NewItems, e.ChildrenUris);
                        case NotifyCollectionChangedAction.Remove:
                            return CreateDeletion(e.Element, e.AbsoluteUri, e.PropertyName, collectionChangeArgs.OldStartingIndex, collectionChangeArgs.OldItems.Count, collectionChangeArgs.OldItems, e.ChildrenUris);
                        case NotifyCollectionChangedAction.Reset:
                            var property = e.Element.GetType().GetProperty(e.PropertyName);
                            var propertyValue = property.GetValue(e.Element, null) as IList;
                            var newCollectionState = new List<object>();
                            
                            foreach (var item in propertyValue)
                            {
                                newCollectionState.Add(item);
                            }
                            var newCollectionUris = newCollectionState.Select(i => ((IModelElement) i).AbsoluteUri).ToList();
                            BubbledChangeEventArgs resetEvent;
                            changingFromChangedEvent.TryGetValue(e, out resetEvent);
                            return CreateReset(e.Element, e.AbsoluteUri, e.PropertyName, resetEvent, newCollectionState, newCollectionUris); 
                        default:
                            throw new NotSupportedException("The CollectionChanged action " + collectionChangeArgs.Action + " is not supported.");
                    }
                default:
                    throw new InvalidOperationException("The " + e.ChangeType + " event cannot be the base of a model change. Use after-events only.");
            }
        }

        
        private IModelChange CreatePropertyChange(IModelElement element, Uri absoluteUri, string propertyName, object oldValue, object newValue, Uri oldValueUri, Uri newValueUri)
        {
            var propertyType = element.GetType().GetProperty(propertyName).PropertyType;
            if (!propertyType.GetInterfaces().Contains(typeof(IModelElement))) // only model elements can be references
                return CreatePropertyChangeAttribute(propertyType, absoluteUri, propertyName, newValue);
            else if (GetAllReferences(element.GetClass()).Any(a => a.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))) // a reference
                return CreatePropertyChangeReference(propertyType, absoluteUri, propertyName, oldValueUri, newValueUri);
            else // a recently created model element
                return CreatePropertyChangeAttribute(propertyType, absoluteUri, propertyName, newValue);
        }

        private IModelChange CreatePropertyChangeAttribute(Type propertyType, Uri absoluteUri, string propertyName, object newValue)
        {
            var genericType = typeof(PropertyChangeAttribute<>).MakeGenericType(propertyType);
            return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, newValue);
        }

        private IModelChange CreatePropertyChangeReference(Type propertyType, Uri absoluteUri, string propertyName, Uri oldValueUri, Uri newValueUri)
        {
            var genericType = typeof(PropertyChangeReference<>).MakeGenericType(propertyType);
            return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, oldValueUri, newValueUri);
        }

        private IModelChange CreateReset(IModelElement element, Uri absoluteUri, string propertyName, 
            BubbledChangeEventArgs resetEvent, ICollection newCollectionState, ICollection<Uri> newCollectionStateUris)
        {
            var reference = GetAllReferences(element.GetClass()).First(r => r.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (reference.IsContainment)
                return CreateResetContainment(element, absoluteUri, propertyName, resetEvent, newCollectionState);
            else
                return CreateResetReference(element, absoluteUri, propertyName, resetEvent, newCollectionStateUris);
        }

        private IModelChange CreateResetContainment(IModelElement element, Uri absoluteUri, string propertyName, BubbledChangeEventArgs resetEvent, ICollection newCollectionState)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);

            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = Activator.CreateInstance(listType) as IList;
            foreach (var item in newCollectionState)
                list.Add(item);

            var genericType = typeof(CollectionResetComposition<>).MakeGenericType(itemType);
            if (_isInvertible)
            {
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, this, resetEvent, list);
            }
            else
            {
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, list);
            }
        }

        private IModelChange CreateResetReference(IModelElement element, Uri absoluteUri, string propertyName, BubbledChangeEventArgs resetEvent, ICollection<Uri> newElementUris)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);

            var genericType = typeof(CollectionResetAssociation<>).MakeGenericType(itemType);
            if (_isInvertible)
            {
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, this, resetEvent, newElementUris);
            }
            else
            {
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, newElementUris);
            }
            
        }

        private IModelChange CreateInsertion(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, IList newItems, List<Uri> newItemsUris)
        {
            var reference = GetAllReferences(element.GetClass()).First(r => r.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            
            if (reference.IsContainment)
                return CreateInsertionContainment(element, absoluteUri, propertyName, startingIndex, newItems);
            else
                return CreateInsertionReference(element, absoluteUri, propertyName, startingIndex, newItemsUris);
        }

        private IModelChange CreateInsertionReference(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, List<Uri> newItemsUris)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);

            if (IsListType(collectionType))
            {
                var genericType = typeof(ListInsertionAssociation<>).MakeGenericType(itemType);
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, startingIndex, newItemsUris);
            }
            else
            {
                var genericType = typeof(CollectionInsertionAssociation<>).MakeGenericType(itemType);
                return (IModelChange) Activator.CreateInstance(genericType, absoluteUri, propertyName, newItemsUris);
            }
        }

        private IModelChange CreateInsertionContainment(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, IList newItems)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);

            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = Activator.CreateInstance(listType) as IList;
            foreach (var item in newItems)
                list.Add(item);

            if (IsListType(collectionType))
            {
                var genericType = typeof(ListInsertionComposition<>).MakeGenericType(itemType);
                return (IModelChange) Activator.CreateInstance(genericType, absoluteUri, propertyName, startingIndex, list);
            }
            else
            {
                var genericType = typeof(CollectionInsertionComposition<>).MakeGenericType(itemType);
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, list);
            }
        }

        private IModelChange CreateDeletion(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, int count, IList oldItems, List<Uri> oldItemsUris)
        {
            var reference = GetAllReferences(element.GetClass()).First(r => r.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (reference.IsContainment)
                return CreateDeletionContainment(element, absoluteUri, propertyName, startingIndex, count, oldItems);
            else
                return CreateDeletionReference(element, absoluteUri, propertyName, startingIndex, count, oldItemsUris);
        }

        private IModelChange CreateDeletionContainment(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, int count, IList oldItems)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);
            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = Activator.CreateInstance(listType) as IList;
            foreach (var item in oldItems)
                list.Add(item);

            if (IsListType(collectionType))
            {
                var genericType = typeof(ListDeletionComposition<>).MakeGenericType(itemType);
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, startingIndex, count, list);
            }
            else
            {
                var genericType = typeof(CollectionDeletionComposition<>).MakeGenericType(itemType);
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, list);
            }

        }

        private IModelChange CreateDeletionReference(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, int count, List<Uri> oldItemsUris)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);

            if (IsListType(collectionType))
            {
                var genericType = typeof(ListDeletionAssociation<>).MakeGenericType(itemType);
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, startingIndex, count, oldItemsUris);
            }
            else
            {
                var genericType = typeof(CollectionDeletionAssociation<>).MakeGenericType(itemType);
                return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, oldItemsUris);
            }
        }

        private bool IsListType(Type collectionType)
        {
            return collectionType.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
        }

        private static Type GetCollectionItemType(Type collectionType)
        {
            if (collectionType.IsInterface && collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return collectionType.GetGenericArguments()[0];
            }
            if (collectionType.IsArray)
                return collectionType.GetElementType();
            else
            {
                return collectionType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
                    ?.GetGenericArguments()[0] ?? typeof(object);
            }
        }

        private static IEnumerable<Meta.IReference> GetAllReferences(Meta.IClass c)
        {
            foreach (var r in c.References)
                yield return r;

            foreach (var t in c.BaseTypes)
                foreach (var r in GetAllReferences(t))
                    yield return r;
        }

        public List<object> GetItemsBeforeReset(BubbledChangeEventArgs resetEvent)
        {
            List<object> items;
            if (!preResetItems.TryGetValue(resetEvent, out items))
            {
                items = new List<object>();
            }
            return items;
        }
    }
}

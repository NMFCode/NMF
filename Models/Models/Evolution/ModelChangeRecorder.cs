using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Evolution
{
    /// <summary>
    /// Represents a recorder for changes to a model.
    /// </summary>
    public class ModelChangeRecorder
    {
        private List<BubbledChangeEventArgs> recordedEvents = new List<BubbledChangeEventArgs>();

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
        }

        /// <summary>
        /// Detaches the recorder, stopping the change tracking.
        /// </summary>
        public void Stop()
        {
            if (!IsRecording)
                throw new InvalidOperationException("The recorder is not attached.");

            AttachedElement.BubbledChange -= OnBubbledChange;
            AttachedElement = null;
        }

        private void OnBubbledChange(object sender, BubbledChangeEventArgs e)
        {
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
                    return CreatePropertyChange(e.Element, e.AbsoluteUri, e.PropertyName, valueChangeArgs.OldValue, valueChangeArgs.NewValue);
                case ChangeType.CollectionChanged:
                    var collectionChangeArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                    switch (collectionChangeArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            return CreateListInsertion(e.Element, e.AbsoluteUri, e.PropertyName, collectionChangeArgs.NewStartingIndex, collectionChangeArgs.NewItems);
                        case NotifyCollectionChangedAction.Remove:
                            return new ListDeletion(e.AbsoluteUri, e.PropertyName, collectionChangeArgs.OldStartingIndex, collectionChangeArgs.OldItems.Count);
                        case NotifyCollectionChangedAction.Reset:
                            //TODO reset == list.Clear()
                            throw new NotImplementedException();
                        default:
                            throw new NotSupportedException("The CollectionChanged action " + collectionChangeArgs.Action + " is not supported.");
                    }
                default:
                    throw new InvalidOperationException("The " + e.ChangeType + " event cannot be the base of a model change. Use after-events only.");
            }
        }

        private IModelChange CreatePropertyChange(IModelElement element, Uri absoluteUri, string propertyName, object oldValue, object newValue)
        {
            var propertyType = element.GetType().GetProperty(propertyName).PropertyType;
            var genericType = typeof(PropertyChange<>).MakeGenericType(propertyType);
            return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, oldValue, newValue);
        }

        private IModelChange CreateListInsertion(IModelElement element, Uri absoluteUri, string propertyName, int startingIndex, IList newItems)
        {
            var collectionType = element.GetType().GetProperty(propertyName).PropertyType;
            var itemType = GetCollectionItemType(collectionType);

            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = Activator.CreateInstance(listType) as IList;
            foreach (var item in newItems)
                list.Add(item);

            var genericType = typeof(ListInsertion<>).MakeGenericType(itemType);
            return (IModelChange)Activator.CreateInstance(genericType, absoluteUri, propertyName, startingIndex, list);
        }

        private static Type GetCollectionItemType(Type collectionType)
        {
            if (collectionType.IsArray)
                return collectionType.GetElementType();
            else
            {
                return collectionType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
                    ?.GetGenericArguments()[0] ?? typeof(object);
            }
        }
    }
}

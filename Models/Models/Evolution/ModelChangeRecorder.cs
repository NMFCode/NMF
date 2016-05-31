using NMF.Expressions;
using NMF.Models.Evolution.Minimizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Evolution
{
    public class ModelChangeRecorder
    {
        private List<BubbledChangeEventArgs> recordedEvents = new List<BubbledChangeEventArgs>();

        public bool IsRecording { get { return AttachedElement != null; } }

        public IModelElement AttachedElement { get; private set; }
        
        public void Start(IModelElement element)
        {
            AttachedElement = element;
            element.BubbledChange += OnBubbledChange;
        }

        public void Stop()
        {
            AttachedElement.BubbledChange -= OnBubbledChange;
            AttachedElement = null;
        }

        private void OnBubbledChange(object sender, BubbledChangeEventArgs e)
        {
            recordedEvents.Add(e);
        }

        public Task<List<IModelChange>> GetModelChangesAsync()
        {
            return Task.Factory.StartNew(GetModelChanges);
        }

        public List<IModelChange> GetModelChanges()
        {
            return Minimize(ParseChangeList());
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
                    return new ElementDeletion(e.Element);
                case ChangeType.PropertyChanged:
                    var valueChangeArgs = (ValueChangedEventArgs)e.OriginalEventArgs;
                    return new PropertyChange(e.AbsoluteUri, e.PropertyName, valueChangeArgs.NewValue, valueChangeArgs.OldValue);
                case ChangeType.CollectionChanged:
                    var collectionChangeArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                    switch (collectionChangeArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            return new CollectionInsertion(e.AbsoluteUri, e.PropertyName, collectionChangeArgs.NewItems, collectionChangeArgs.NewStartingIndex);
                        case NotifyCollectionChangedAction.Remove:
                            return new CollectionDeletion(e.AbsoluteUri, e.PropertyName, collectionChangeArgs.OldItems, collectionChangeArgs.OldStartingIndex);
                        case NotifyCollectionChangedAction.Reset:
                            //TODO
                            throw new NotImplementedException();
                        default:
                            throw new NotSupportedException("The CollectionChanged action " + collectionChangeArgs.Action + " is not supported.");
                    }
                default:
                    throw new InvalidOperationException("The " + e.ChangeType + " event cannot be the base of a model change. Use after-events only.");
            }
        }

        private List<IModelChange> Minimize(List<IModelChange> changeList)
        {
            var strategies = new[] { new MultiplePropertyChanges() };

            var localList = changeList;
            foreach (var strat in strategies)
                localList = strat.Execute(localList);

            return localList;
        }
    }
}

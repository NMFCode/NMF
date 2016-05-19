using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public class ModelChangeRecorder
    {
        public bool IsRecording { get { return AttachedElement != null; } }

        public IModelElement AttachedElement { get; private set; }

        public ModelChangeCollection RecordedChanges { get; private set; }

        public ModelChangeRecorder()
        {
            RecordedChanges = new ModelChangeCollection();
        }

        public void Start(IModelElement element)
        {
            AttachedElement = element;
            element.BubbledChange += AttachedElementBubbledChange;
        }

        public void Stop()
        {
            AttachedElement.BubbledChange -= AttachedElementBubbledChange;
            AttachedElement = null;
        }

        private void AttachedElementBubbledChange(object sender, BubbledChangeEventArgs e)
        {
            switch (e.ChangeType)
            {
                case ChangeType.CollectionChanged:
                    HandleCollectionChanged(e.Element, e.PropertyName, (NotifyCollectionChangedEventArgs)e.OriginalEventArgs);
                    break;
                case ChangeType.PropertyChanged:
                    HandlePropertyChanged(e.Element, e.PropertyName, (ValueChangedEventArgs)e.OriginalEventArgs);
                    break;
            }
        }

        private void HandlePropertyChanged(IModelElement parent, string propertyName, ValueChangedEventArgs args)
        {
            RecordedChanges.Add(new PropertyChange(parent.AbsoluteUri, propertyName, args.NewValue, args.OldValue));
        }

        private void HandleCollectionChanged(IModelElement parent, string propertyName, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    HandleCollectionAdd(parent, propertyName, args.NewItems, args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    HandleCollectionRemove(parent, propertyName, args.OldItems, args.OldStartingIndex);
                    break;
                default:
                    throw new NotImplementedException("The CollectionChanged action " + args.Action + " is not yet implemented.");
            }
        }

        private void HandleCollectionAdd(IModelElement parent, string propertyName, IList newItems, int startingIndex)
        {
            for (int i = 0; i < newItems.Count; i++)
            {
                RecordedChanges.Add(new CollectionInsertion(parent.AbsoluteUri, propertyName, newItems[i], startingIndex + i));
            }
        }

        private void HandleCollectionRemove(IModelElement parent, string propertyName, IList deletedItems, int startingIndex)
        {
            for (int i = deletedItems.Count - 1; i >= 0; i--)
            {
                RecordedChanges.Add(new CollectionDeletion(parent.AbsoluteUri, propertyName, deletedItems[i], startingIndex + i));
            }
        }
    }
}

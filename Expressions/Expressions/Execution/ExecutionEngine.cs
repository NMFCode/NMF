using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public abstract class ExecutionEngine : IExecutionContext
    {
        private readonly Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler> propertyChangedHandler = new Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler>();
        private readonly Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler> collectionChangedHandler = new Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler>();

        public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            PropertyChangedEventHandler handler = (obj, e) =>
            {
                if (e.PropertyName == propertyName)
                    SetInvalidNode(node);
            };

            element.PropertyChanged += handler;
            propertyChangedHandler[new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName)] = handler;
        }

        public void AddChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            NotifyCollectionChangedEventHandler handler = (obj, e) => SetInvalidNode(node);
            collection.CollectionChanged += handler;
            collectionChangedHandler[new Tuple<INotifiable, INotifyCollectionChanged>(node, collection)] = handler;
        }

        public void RemoveChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            var key = new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName);
            var handler = propertyChangedHandler[key];
            element.PropertyChanged -= handler;
            propertyChangedHandler.Remove(key);
        }

        public void RemoveChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            var key = new Tuple<INotifiable, INotifyCollectionChanged>(node, collection);
            var handler = collectionChangedHandler[key];
            collection.CollectionChanged -= handler;
            collectionChangedHandler.Remove(key);
        }

        protected abstract void SetInvalidNode(INotifiable node);
        
        public static ExecutionEngine Current { get; set; } = new ImmediateExecutionEngine();
    }
}

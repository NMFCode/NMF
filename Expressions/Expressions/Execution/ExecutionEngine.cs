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
        private Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler> propertyChangedHandler = new Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler>();

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e, INotifiable node, string propertyName)
        {
            if (e.PropertyName == propertyName)
                SetInvalidNode(node);
        }

        protected abstract void SetInvalidNode(INotifiable node);
        
        public static ExecutionEngine Current { get; set; } = new ImmediateExecutionEngine();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class PropertyChangeListener : IChangeListener
    {
        private INotifyPropertyChanged element;
        private string propertyName;
        private bool engineNotified;

        public INotifiable Node { get; private set; }

        public PropertyChangeListener(INotifiable node)
        {
            Node = node;
        }

        public void Subscribe(INotifyPropertyChanged element, string propertyName)
        {
            if (this.element != element || this.propertyName != propertyName)
            {
                Unsubscribe();
                this.element = element;
                this.propertyName = propertyName;
                this.element.PropertyChanged += OnPropertyChanged;
            }
        }

        public void Unsubscribe()
        {
            if (element != null)
            {
                element.PropertyChanged -= OnPropertyChanged;
                propertyName = null;
                element = null;
            }
            engineNotified = false;
        }

        public INotificationResult AggregateChanges()
        {
            engineNotified = false;
            return null;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName && !engineNotified)
            {
                engineNotified = true;
                ExecutionEngine.Current.InvalidateNode(this);
            }
        }
    }
}

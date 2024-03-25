using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a listener for property changes
    /// </summary>
    public class PropertyChangeListener : IChangeListener
    {
        private INotifyPropertyChanged element;
        private string propertyName;
        private bool engineNotified;

        /// <inheritdoc />
        public INotifiable Node { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="node">The target node</param>
        public PropertyChangeListener(INotifiable node)
        {
            Node = node;
        }

        /// <summary>
        /// Subscribe to the property change events
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="propertyName">The property name</param>
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

        /// <summary>
        /// Removes a subscription
        /// </summary>
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

        /// <inheritdoc />
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

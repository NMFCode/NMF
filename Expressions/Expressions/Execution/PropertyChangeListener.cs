using System.ComponentModel;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a listener for property changes
    /// </summary>
    public class PropertyChangeListener : IChangeListener
    {
        private INotifyPropertyChanged _element;
        private readonly string _propertyName;
        private bool _engineNotified;

        /// <inheritdoc />
        public INotifiable Node { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="node">The target node</param>
        /// <param name="propertyName">The property name</param>
        public PropertyChangeListener(INotifiable node, string propertyName)
        {
            Node = node;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Subscribe to the property change events
        /// </summary>
        /// <param name="element">The _element</param>
        public void Subscribe(INotifyPropertyChanged element)
        {
            if (_element != element)
            {
                Unsubscribe();
                _element = element;
                _element.PropertyChanged += OnPropertyChanged;
            }
        }

        /// <summary>
        /// Removes a subscription
        /// </summary>
        public void Unsubscribe()
        {
            if (_element != null)
            {
                _element.PropertyChanged -= OnPropertyChanged;
                _element = null;
            }
            _engineNotified = false;
        }

        /// <inheritdoc />
        public INotificationResult AggregateChanges()
        {
            _engineNotified = false;
            return null;
        }

        /// <summary>
        /// The name of the property to listen to
        /// </summary>
        public string MemberName => _propertyName;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _propertyName && !_engineNotified)
            {
                _engineNotified = true;
                ExecutionEngine.Current.InvalidateNode(this);
            }
        }
    }
}

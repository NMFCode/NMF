using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NMF.Collections.ObjectModel;
using NMF.Expressions;

namespace NMF.Analyses.Connectivity.Tests
{
    /// <summary>
    /// Represents an employee
    /// </summary>
    public class Node : INotifyPropertyChanged
    {
        private string name;
        private ObservableList<Node> edges = new ObservableList<Node>();

        /// <summary>
        /// The employees name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        /// <summary>
        /// Gets a collection of known employees
        /// </summary>
        public IListExpression<Node> Edges
        {
            get
            {
                return edges;
            }
        }

        /// <summary>
        /// Sets the given property
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="field">The backing field for the property</param>
        /// <param name="value">The new value</param>
        /// <param name="propertyName">The name of the property</param>
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the given property
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Is fired whenever a property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return name;
        }
    }
}
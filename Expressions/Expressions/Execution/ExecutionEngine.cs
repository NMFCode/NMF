using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public abstract class ExecutionEngine : IExecutionEngine
    {
        private readonly Dictionary<INotifyPropertyChanged, Dictionary<string, HashSet<INotifiable>>> propertyChangedListener = new Dictionary<INotifyPropertyChanged, Dictionary<string, HashSet<INotifiable>>>();

        public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            Dictionary<string, HashSet<INotifiable>> dict;
            if (!propertyChangedListener.TryGetValue(element, out dict))
            {
                dict = new Dictionary<string, HashSet<INotifiable>>();
                propertyChangedListener[element] = dict;
                element.PropertyChanged += OnPropertyChanged;
            }

            HashSet<INotifiable> set;
            if (!dict.TryGetValue(propertyName, out set))
            {
                set = new HashSet<INotifiable>();
                dict[propertyName] = set;
            }

            set.Add(node);
        }

        public void AddChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            throw new NotImplementedException();
        }

        public void RemoveChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            Dictionary<string, HashSet<INotifiable>> dict;
            if (propertyChangedListener.TryGetValue(element, out dict))
            {
                HashSet<INotifiable> set;
                if (dict.TryGetValue(propertyName, out set))
                {
                    set.Remove(node);
                    if (set.Count == 0)
                    {
                        dict.Remove(propertyName);
                        if (dict.Count == 0)
                        {
                            propertyChangedListener.Remove(element);
                            element.PropertyChanged -= OnPropertyChanged;
                        }
                    }
                }
            }
        }

        public void RemoveChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            throw new NotImplementedException();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var element = sender as INotifyPropertyChanged;
            if (element != null)
            {
                HashSet<INotifiable> handler;
                if (propertyChangedListener[element].TryGetValue(args.PropertyName, out handler))
                    OnPropertyChanged(handler);
            }
        }

        protected abstract void OnPropertyChanged(HashSet<INotifiable> handler);

        private static IExecutionEngine current = new ImmediateExecutionEngine();
        public static IExecutionEngine Current
        {
            get { return current; }
            set { current = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal interface IExecutionContext
    {
        void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName);

        void AddChangeListener<T>(INotifiable node, INotifyCollectionChanged collection);

        void RemoveChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName);

        void RemoveChangeListener(INotifiable node, INotifyCollectionChanged collection);
    }
}

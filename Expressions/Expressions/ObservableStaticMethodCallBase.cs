using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{

    internal abstract class ObservableStaticMethodBase<TDelegate, TResult> : NotifyExpression<TResult>
        where TDelegate : class
    {
        public ObservableStaticMethodBase(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");

            Function = ReflectionHelper.CreateDelegate(typeof(TDelegate), method) as TDelegate;
        }

        public ObservableStaticMethodBase(TDelegate function)
        {
            Function = function;
        }

        protected virtual void RegisterCollectionChanged<T>(INotifyValue<T> value)
        {
            if (value == null) return;
            var notifyCollection = value.Value as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += notifyCollection_CollectionChanged;
            }
            value.ValueChanged += value_ValueChanged;
        }

        private void value_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var oldNotifyCollection = e.OldValue as INotifyCollectionChanged;
            var newNotifyCollection = e.NewValue as INotifyCollectionChanged;
            if (oldNotifyCollection != null) oldNotifyCollection.CollectionChanged -= notifyCollection_CollectionChanged;
            if (newNotifyCollection != null) newNotifyCollection.CollectionChanged += notifyCollection_CollectionChanged;
        }

        private void notifyCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        public TDelegate Function { get; private set; }

        protected override void OnValueChanged(ValueChangedEventArgs e)
        {
            base.OnValueChanged(e);
            var disposable = e.OldValue as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        protected void ArgumentChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Invoke;
            }
        }
    }
}

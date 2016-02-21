using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;

namespace NMF.Models.Expressions
{
    public abstract class ModelPropertyChange<TClass, TProperty> : INotifyReversableExpression<TProperty>
    {
        public TClass ModelElement { get; private set; }
        private int attachedCount = 0;

        public ModelPropertyChange(TClass modelElement)
        {
            ModelElement = modelElement;
        }

        protected abstract void RegisterChangeEventHandler(EventHandler<ValueChangedEventArgs> handler);
        protected abstract void UnregisterChangeEventHandler(EventHandler<ValueChangedEventArgs> handler);

        public bool CanBeConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsAttached
        {
            get
            {
                return attachedCount > 0;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return true;
            }
        }

        public abstract TProperty Value
        {
            get;
            set;
        }

        public bool IsReversable
        {
            get
            {
                return true;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public INotifyExpression<TProperty> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }

        public void Attach()
        {
            attachedCount++;
            if (attachedCount == 1)
            {
                RegisterChangeEventHandler(ForwardValueChange);
            }
        }

        private void ForwardValueChange(object sender, ValueChangedEventArgs e)
        {
            var handler = ValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Detach()
        {
            attachedCount--;
            if (attachedCount == 0)
            {
                UnregisterChangeEventHandler(ForwardValueChange);
            }
        }

        public INotifyExpression<TProperty> Reduce()
        {
            return this;
        }

        public void Refresh()
        {
        }
    }
}

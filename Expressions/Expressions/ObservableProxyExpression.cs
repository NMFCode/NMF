using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    class ObservableProxyExpression<T> : Expression, INotifyExpression<T>
    {
        public int TotalVisits { get; set; }

        public int RemainingVisits { get; set; }

        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();
        protected INotifyValue<T> value;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public ObservableProxyExpression(INotifyValue<T> value)
        {
            if (value == null) throw new ArgumentNullException("value");

            this.value = value;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                {
                    foreach (var dep in Dependencies)
                        dep.Successors.Remove(this);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                {
                    foreach (var dep in Dependencies)
                        dep.Successors.Add(this);
                }
            };
        }

        public bool CanBeConstant
        {
            get { return IsConstant; }
        }

        public bool IsParameterFree
        {
            get { return true; }
        }

        public T Value
        {
            get { return value.Value; }
        }

        public object ValueObject
        {
            get { return Value; }
        }

        public override bool CanReduce
        {
            get { return false; }
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.Parameter; }
        }

        public override Type Type
        {
            get { return typeof(T); }
        }

        public bool IsConstant
        {
            get { return this is ConstantValue<T>; }
        }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                //TODO remove condition when circular reference in SelectMany is resolved
                if (!value.GetType().Name.Contains("SubSourcePair"))
                    yield return value;
            }
        }

        public IList<INotifiable> Successors
        {
            get { return successors; }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }
        
        public new INotifyExpression<T> Reduce()
        {
            return this;
        }

        public bool Notify(IEnumerable<INotifiable> sources)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(default(T), Value));
            return true;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters)
        {
            return ApplyParameters(parameters);
        }
    }

    class ObservableProxyReversableExpression<T> : ObservableProxyExpression<T>, INotifyReversableExpression<T>
    {
        public ObservableProxyReversableExpression(INotifyReversableValue<T> value)
            : base(value) { }

        public new T Value
        {
            get
            {
                return this.value.Value;
            }
            set
            {
                var reversable = (INotifyReversableValue<T>)this.value;
                reversable.Value = value;
            }
        }

        public bool IsReversable
        {
            get
            {
                var reversable = (INotifyReversableValue<T>)this.value;
                return reversable.IsReversable;
            }
        }
    }

}

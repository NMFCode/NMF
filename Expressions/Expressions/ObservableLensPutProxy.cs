using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal sealed class ObservableLensPutProxy<TBase, T> : INotifyReversableExpression<T>
    {
        private readonly INotifyExpression<T> inner;
        private readonly LensPut<TBase, T> lens;

        public ObservableLensPutProxy(INotifyReversableValue<TBase> target, INotifyExpression<T> inner, LensPutAttribute lensAttribute, MethodInfo method)
        {
            this.inner = inner;
            this.lens = LensPut<TBase, T>.FromLensPutAttribute(lensAttribute, method, target);
        }

        public bool CanBeConstant
        {
            get
            {
                return inner.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return inner.IsConstant;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return inner.IsParameterFree;
            }
        }

        public object ValueObject
        {
            get
            {
                return inner.ValueObject;
            }
        }

        public T Value
        {
            get
            {
                return inner.Value;
            }
            set
            {
                lens.SetValue(default(TBase), value);
            }
        }

        public bool IsReversable
        {
            get
            {
                return lens != null && lens.CanApply;
            }
        }

        public ISuccessorList Successors
        {
            get
            {
                return inner.Successors;
            }
        }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                return inner.Dependencies;
            }
        }

        public ExecutionMetaData ExecutionMetaData
        {
            get
            {
                return inner.ExecutionMetaData;
            }
        }

        T INotifyValue<T>.Value
        {
            get
            {
                return inner.Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                inner.ValueChanged += value;
            }
            remove
            {
                inner.ValueChanged -= value;
            }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            inner.Dispose();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return inner.Notify(sources);
        }

        public INotifyExpression<T> Reduce()
        {
            throw new NotImplementedException();
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }
    }
}

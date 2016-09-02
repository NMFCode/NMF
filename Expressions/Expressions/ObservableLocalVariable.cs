using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal sealed class ObservableLocalVariable<T, TVar> : INotifyExpression<T>
    {
        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public INotifyExpression<T> Inner { get; private set; }
        public INotifyExpression<TVar> Variable { get; private set; }
        public string ParameterName { get; private set; }

        public ObservableLocalVariable(INotifyExpression<T> inner, INotifyExpression<TVar> variable, string parameterName)
        {
            if (inner == null) throw new ArgumentNullException("inner");
            if (variable == null) throw new ArgumentNullException("variable");
            if (parameterName == null) throw new ArgumentNullException("parameter");

            Inner = inner;
            ParameterName = parameterName;
            Variable = variable;

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
            get { return Inner.CanBeConstant; }
        }

        public bool IsConstant
        {
            get { return Inner.IsConstant; }
        }

        public bool IsParameterFree
        {
            get { return Inner.IsParameterFree; }
        }

        public T Value
        {
            get { return Inner.Value; }
        }

        public object ValueObject
        {
            get { return Value; }
        }
        
        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Inner;
                yield return Variable;
            }
        }

        public IList<INotifiable> Successors { get { return successors; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            var applied = Variable.ApplyParameters(parameters);
            if (applied.IsParameterFree)
            {
                parameters.Add(ParameterName, applied);
                return Inner.ApplyParameters(parameters);
            }
            return new ObservableLocalVariable<T, TVar>(Inner.ApplyParameters(parameters), applied, ParameterName);
        }

        public void Dispose()
        {
            Successors.Clear();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var innerChange = sources[0] as ValueChangedNotificationResult<T>;
                if (ValueChanged != null)
                    ValueChanged(this, new ValueChangedEventArgs(innerChange.OldValue, Value));
                return new ValueChangedNotificationResult<T>(this, innerChange.OldValue, innerChange.NewValue);
            }
            return new ValueChangedNotificationResult<T>(this, Inner.Value, Inner.Value);
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters)
        {
            return ApplyParameters(parameters);
        }
    }
}

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

            Successors.Attached += (obj, e) =>
            {
                foreach (var dep in Dependencies)
                    dep.Successors.Set(this);
            };

            Successors.Detached += (obj, e) =>
            {
                foreach (var dep in Dependencies)
                    dep.Successors.Unset(this);
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

        public MultiSuccessorList Successors { get; } = new MultiSuccessorList();

        ISuccessorList INotifiable.Successors => Successors;

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var applied = Variable.ApplyParameters(parameters, trace);
            if (applied.IsParameterFree)
            {
                parameters.Add(ParameterName, applied);
                return Inner.ApplyParameters(parameters, trace);
            }
            return new ObservableLocalVariable<T, TVar>(Inner.ApplyParameters(parameters, trace), applied, ParameterName);
        }

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var innerChange = sources[0] as IValueChangedNotificationResult<T>;
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(innerChange.OldValue, Value));
                return new ValueChangedNotificationResult<T>(this, innerChange.OldValue, innerChange.NewValue);
            }
            return new ValueChangedNotificationResult<T>(this, Inner.Value, Inner.Value);
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal sealed class ObservableLocalVariable<T, TVar> : INotifyExpression<T>
    {
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

        public void Refresh()
        {
            Inner.Refresh();
        }

        public T Value
        {
            get { return Inner.Value; }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                Inner.ValueChanged += value;
            }
            remove
            {
                Inner.ValueChanged -= value;
            }
        }

        public void Detach()
        {
            Inner.Detach();
        }

        public void Attach()
        {
            Inner.Attach();
        }

        public bool IsAttached
        {
            get { return Inner.IsAttached; }
        }


        public INotifyExpression<T> Reduce()
        {
            return this;
        }
    }
}

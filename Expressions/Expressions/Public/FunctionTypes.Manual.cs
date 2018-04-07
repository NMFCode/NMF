﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents an observable expression with one input parameter
    /// </summary>
    /// <typeparam name="T">The type of the input parameter</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ObservingFunc<T1, TResult>
    {
        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(T1 input, TTag tag = default(TTag))
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
                parameters.Add(parameter1Name, input);
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()), tag);
        }

        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(INotifyValue<T1> input, TTag tag = default(TTag))
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
                parameters.Add(parameter1Name, input);
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()), tag);
        }

        public INotifiable Expression { get { return expression; } }
    }

    /// <summary>
    /// Represents an observable expression with two input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the first input parameter</typeparam>
    /// <typeparam name="T2">The type of the second input parameter</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ObservingFunc<T1, T2, TResult>
    {
        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(T1 input1, T2 input2, TTag tag)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, input1);
                parameters.Add(parameter2Name, input2);
            }
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()), tag);
        }

        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(INotifyValue<T1> input1, T2 input2, TTag tag)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, input1);
                parameters.Add(parameter2Name, input2);
            }
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()), tag);
        }
    }

    internal class TaggedObservableValue<T, TTag> : INotifyReversableExpression<T>, IValueChangedNotificationResult<T>
    {
        private T oldValue;
        public INotifyExpression<T> Expression { get; set; }
        public TTag Tag { get; set; }
        
        public bool CanBeConstant { get { return Expression.CanBeConstant; } }

        public bool IsConstant { get { return Expression.IsConstant; } }

        public bool IsParameterFree { get { return Expression.IsParameterFree; } }

        public object ValueObject { get { return Expression.ValueObject; } }

        public T Value
        {
            get
            {
                return Expression.Value;
            }

            set
            {
                var expression = Expression as INotifyReversableExpression<T>;
                if (expression != null)
                {
                    expression.Value = value;
                }
                else
                {
                    throw new InvalidOperationException("The expression is read-only.");
                }
            }
        }

        public bool IsReversable
        {
            get
            {
                var expression = Expression as INotifyReversableExpression<T>;
                return expression != null && expression.IsReversable;
            }
        }

        
        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Expression;
            }
        }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public TaggedObservableValue(INotifyExpression<T> expression, TTag tag)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression = expression;
            Tag = tag;

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

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new TaggedObservableValue<T, TTag>(Expression.ApplyParameters(parameters, trace), Tag);
        }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            var valueChange = (IValueChangedNotificationResult<T>)sources[0];
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(valueChange.OldValue, Value));
            if (valueChange.Changed)
            {
                oldValue = valueChange.OldValue;
                return this;
            }
            else
            {
                return UnchangedNotificationResult.Instance;
            }
        }

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        #region embedded change notification

        T IValueChangedNotificationResult<T>.OldValue { get { return oldValue; } }

        T IValueChangedNotificationResult<T>.NewValue { get { return Value; } }

        object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return Value; } }

        INotifiable INotificationResult.Source { get { return this; } }

        bool INotificationResult.Changed { get { return true; } }

        void INotificationResult.IncreaseReferences(int references) { }

        void INotificationResult.FreeReference() { }
        #endregion
    }
}

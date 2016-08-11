using System;
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
            if (isParameterFree) return new TaggedObservableValue<TResult, TTag>(expression, tag);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, input);
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters), tag);
        }

        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(INotifyValue<T1> input, TTag tag = default(TTag))
        {
            if (isParameterFree) return new TaggedObservableValue<TResult, TTag>(expression, tag);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, input);
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters), tag);
        }
    }

    internal class TaggedObservableValue<T, TTag> : INotifyReversableExpression<T>
    {
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

        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();
        public IList<INotifiable> Successors { get { return successors; } }

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

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new TaggedObservableValue<T, TTag>(Expression.ApplyParameters(parameters), Tag);
        }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }

        public void Dispose()
        {
            Successors.Clear();
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters)
        {
            return ApplyParameters(parameters);
        }
    }

    /// <summary>
    /// Represents an observable expression with one input parameter
    /// </summary>
    /// <typeparam name="T1">The type of the first input parameter</typeparam>
    /// <typeparam name="T2">The type of the second input parameter</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ObservingFunc<T1, T2, TResult>
    {

        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(T1 input1, T2 input2, TTag tag)
        {
            if (isParameterFree) return new TaggedObservableValue<TResult, TTag>(expression, tag);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, input1);
            parameters.Add(parameter2Name, input2);
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters), tag);
        }

        internal TaggedObservableValue<TResult, TTag> InvokeTagged<TTag>(INotifyValue<T1> input1, T2 input2, TTag tag)
        {
            if (isParameterFree) return new TaggedObservableValue<TResult, TTag>(expression, tag);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, input1);
            parameters.Add(parameter2Name, input2);
            return new TaggedObservableValue<TResult, TTag>(expression.ApplyParameters(parameters), tag);
        }
    }
}

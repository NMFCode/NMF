using System;
using System.Collections.Generic;

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

    internal class TaggedObservableValue<T, TTag> : NotifyValue<T>, INotifyReversableValue<T>
    {
        public TTag Tag { get; set; }

        public TaggedObservableValue(INotifyExpression<T> expression, TTag tag)
            : base(expression)
        {
            Tag = tag;
        }

        public new T Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                var reversable = Expression as INotifyReversableValue<T>;
                if (reversable != null)
                {
                    reversable.Value = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool IsReversable
        {
            get {
                var reversable = Expression as INotifyReversableValue<T>;
                return reversable != null && reversable.IsReversable;
            }
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

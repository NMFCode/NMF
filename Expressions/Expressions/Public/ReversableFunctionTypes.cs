using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents an observable expression with 1 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, TResult> : ObservingFunc<T1, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1,  TResult>> expression, Action<T1, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 2 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, TResult> : ObservingFunc<T1, T2, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2,  TResult>> expression, Action<T1, T2, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 3 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, TResult> : ObservingFunc<T1, T2, T3, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3,  TResult>> expression, Action<T1, T2, T3, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 4 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, TResult> : ObservingFunc<T1, T2, T3, T4, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4,  TResult>> expression, Action<T1, T2, T3, T4, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 5 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, TResult> : ObservingFunc<T1, T2, T3, T4, T5, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5,  TResult>> expression, Action<T1, T2, T3, T4, T5, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 6 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 7 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 8 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 9 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 10 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="T10">The type of the input parameter 10</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 11 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="T10">The type of the input parameter 10</typeparam>
    /// <typeparam name="T11">The type of the input parameter 11</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 12 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="T10">The type of the input parameter 10</typeparam>
    /// <typeparam name="T11">The type of the input parameter 11</typeparam>
    /// <typeparam name="T12">The type of the input parameter 12</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 13 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="T10">The type of the input parameter 10</typeparam>
    /// <typeparam name="T11">The type of the input parameter 11</typeparam>
    /// <typeparam name="T12">The type of the input parameter 12</typeparam>
    /// <typeparam name="T13">The type of the input parameter 13</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <param name="in13">The input parameter 13</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            parameters.Add(parameter13Name, in13);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <param name="in13">The input parameter 13</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, in13.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            parameters.Add(parameter13Name, in13);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, in13.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 14 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="T10">The type of the input parameter 10</typeparam>
    /// <typeparam name="T11">The type of the input parameter 11</typeparam>
    /// <typeparam name="T12">The type of the input parameter 12</typeparam>
    /// <typeparam name="T13">The type of the input parameter 13</typeparam>
    /// <typeparam name="T14">The type of the input parameter 14</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <param name="in13">The input parameter 13</param>
        /// <param name="in14">The input parameter 14</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            parameters.Add(parameter13Name, in13);
            parameters.Add(parameter14Name, in14);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <param name="in13">The input parameter 13</param>
        /// <param name="in14">The input parameter 14</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, in13.Value, in14.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            parameters.Add(parameter13Name, in13);
            parameters.Add(parameter14Name, in14);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, in13.Value, in14.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 15 input parameters and a custom update handler
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    /// <typeparam name="T9">The type of the input parameter 9</typeparam>
    /// <typeparam name="T10">The type of the input parameter 10</typeparam>
    /// <typeparam name="T11">The type of the input parameter 11</typeparam>
    /// <typeparam name="T12">The type of the input parameter 12</typeparam>
    /// <typeparam name="T13">The type of the input parameter 13</typeparam>
    /// <typeparam name="T14">The type of the input parameter 14</typeparam>
    /// <typeparam name="T15">The type of the input parameter 15</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public partial class ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
    {
        /// <summary>
        /// The method that handles value updates for this func
        /// </summary>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> UpdateHandler { get; private set; }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <param name="updateHandler">A function that is executed when the result is changed</param>
        public ReversableObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> updateHandler)
            : base(expression)
        {
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            UpdateHandler = updateHandler;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <param name="in13">The input parameter 13</param>
        /// <param name="in14">The input parameter 14</param>
        /// <param name="in15">The input parameter 15</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14, T15 in15)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            parameters.Add(parameter13Name, in13);
            parameters.Add(parameter14Name, in14);
            parameters.Add(parameter15Name, in15);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15, newValue));
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <param name="in8">The input parameter 8</param>
        /// <param name="in9">The input parameter 9</param>
        /// <param name="in10">The input parameter 10</param>
        /// <param name="in11">The input parameter 11</param>
        /// <param name="in12">The input parameter 12</param>
        /// <param name="in13">The input parameter 13</param>
        /// <param name="in14">The input parameter 14</param>
        /// <param name="in15">The input parameter 15</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public override INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14, INotifyValue<T15> in15)
        {
            if (isParameterFree) return new ReversableProxyExpression<TResult>(expression, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, in13.Value, in14.Value, in15.Value, newValue));
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            parameters.Add(parameter8Name, in8);
            parameters.Add(parameter9Name, in9);
            parameters.Add(parameter10Name, in10);
            parameters.Add(parameter11Name, in11);
            parameters.Add(parameter12Name, in12);
            parameters.Add(parameter13Name, in13);
            parameters.Add(parameter14Name, in14);
            parameters.Add(parameter15Name, in15);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
            return new ReversableProxyExpression<TResult>(result, newValue => UpdateHandler(in1.Value, in2.Value, in3.Value, in4.Value, in5.Value, in6.Value, in7.Value, in8.Value, in9.Value, in10.Value, in11.Value, in12.Value, in13.Value, in14.Value, in15.Value, newValue));
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public override bool IsReversable
        {
            get
            {
                return true;
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents an observable expression with 1 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    public partial class ObservingFunc<T1, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1,  TResult> compiled;
        internal string parameter1Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,>), "Observe")]
        public TResult Evaluate(T1 in1)
        {
            return compiled.Invoke(in1);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(T1 in1)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }


        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1,  TResult>(Expression<Func<T1,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1,  TResult> FromExpression(Expression<Func<T1,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 2 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    public partial class ObservingFunc<T1, T2, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2)
        {
            return compiled.Invoke(in1, in2);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(T1 in1, T2 in2)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,>), "ObservePartial")]
        public Func<T2, TResult> EvaluatePartial(T1 in1)
        {
            return (in2) => Evaluate(in1, in2);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, TResult>(result, result.IsParameterFree);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2,  TResult>(Expression<Func<T1, T2,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2,  TResult> FromExpression(Expression<Func<T1, T2,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 3 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    public partial class ObservingFunc<T1, T2, T3, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3)
        {
            return compiled.Invoke(in1, in2, in3);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,>), "ObservePartial")]
        public Func<T2, T3, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3) => Evaluate(in1, in2, in3);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,>), "ObservePartial")]
        public Func<T3, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3) => Evaluate(in1, in2, in3);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, TResult>(result, result.IsParameterFree);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3,  TResult>(Expression<Func<T1, T2, T3,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3,  TResult> FromExpression(Expression<Func<T1, T2, T3,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 4 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    public partial class ObservingFunc<T1, T2, T3, T4, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return compiled.Invoke(in1, in2, in3, in4);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4) => Evaluate(in1, in2, in3, in4);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,>), "ObservePartial")]
        public Func<T3, T4, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4) => Evaluate(in1, in2, in3, in4);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,>), "ObservePartial")]
        public Func<T4, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4) => Evaluate(in1, in2, in3, in4);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, TResult>(result, result.IsParameterFree);
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }

        /// <summary>
        /// Invokes the expression
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4,  TResult>(Expression<Func<T1, T2, T3, T4,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 5 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    public partial class ObservingFunc<T1, T2, T3, T4, T5, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5) => Evaluate(in1, in2, in3, in4, in5);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5) => Evaluate(in1, in2, in3, in4, in5);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,>), "ObservePartial")]
        public Func<T4, T5, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5) => Evaluate(in1, in2, in3, in4, in5);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,>), "ObservePartial")]
        public Func<T5, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5) => Evaluate(in1, in2, in3, in4, in5);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5,  TResult>(Expression<Func<T1, T2, T3, T4, T5,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 6 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6) => Evaluate(in1, in2, in3, in4, in5, in6);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6) => Evaluate(in1, in2, in3, in4, in5, in6);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6) => Evaluate(in1, in2, in3, in4, in5, in6);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,>), "ObservePartial")]
        public Func<T5, T6, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6) => Evaluate(in1, in2, in3, in4, in5, in6);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,>), "ObservePartial")]
        public Func<T6, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6) => Evaluate(in1, in2, in3, in4, in5, in6);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 7 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7) => Evaluate(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7) => Evaluate(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7) => Evaluate(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7) => Evaluate(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7) => Evaluate(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,>), "ObservePartial")]
        public Func<T7, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7) => Evaluate(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 8 input parameters
    /// </summary>
    /// <typeparam name="T1">The type of the input parameter 1</typeparam>
    /// <typeparam name="T2">The type of the input parameter 2</typeparam>
    /// <typeparam name="T3">The type of the input parameter 3</typeparam>
    /// <typeparam name="T4">The type of the input parameter 4</typeparam>
    /// <typeparam name="T5">The type of the input parameter 5</typeparam>
    /// <typeparam name="T6">The type of the input parameter 6</typeparam>
    /// <typeparam name="T7">The type of the input parameter 7</typeparam>
    /// <typeparam name="T8">The type of the input parameter 8</typeparam>
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,>), "ObservePartial")]
        public Func<T8, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 9 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
                parameters.Add(parameter9Name, in9);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
                parameters.Add(parameter9Name, in9);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,>), "ObservePartial")]
        public Func<T9, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, TResult>(expression, true);
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
            return new ObservingFunc<T9, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
                parameters.Add(parameter9Name, in9);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
                parameters.Add(parameter1Name, in1);
                parameters.Add(parameter2Name, in2);
                parameters.Add(parameter3Name, in3);
                parameters.Add(parameter4Name, in4);
                parameters.Add(parameter5Name, in5);
                parameters.Add(parameter6Name, in6);
                parameters.Add(parameter7Name, in7);
                parameters.Add(parameter8Name, in8);
                parameters.Add(parameter9Name, in9);
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 10 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal string parameter10Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.parameter10Name = expression.Parameters[9].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, T10, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T9, T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9, in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, T10, TResult>(expression, true);
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
            return new ObservingFunc<T9, T10, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,>), "ObservePartial")]
        public Func<T10, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return (in10) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T10, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ObservingFunc<T10, TResult>(expression, true);
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
            return new ObservingFunc<T10, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 11 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal string parameter10Name;
        internal string parameter11Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.parameter10Name = expression.Parameters[9].Name;
            this.parameter11Name = expression.Parameters[10].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, T10, T11, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T9, T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9, in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, T10, T11, TResult>(expression, true);
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
            return new ObservingFunc<T9, T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T10, T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return (in10, in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T10, T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ObservingFunc<T10, T11, TResult>(expression, true);
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
            return new ObservingFunc<T10, T11, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,>), "ObservePartial")]
        public Func<T11, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            return (in11) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T11, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (isParameterFree) return new ObservingFunc<T11, TResult>(expression, true);
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
            return new ObservingFunc<T11, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 12 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal string parameter10Name;
        internal string parameter11Name;
        internal string parameter12Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.parameter10Name = expression.Parameters[9].Name;
            this.parameter11Name = expression.Parameters[10].Name;
            this.parameter12Name = expression.Parameters[11].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, T10, T11, T12, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T9, T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9, in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, T10, T11, T12, TResult>(expression, true);
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
            return new ObservingFunc<T9, T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T10, T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return (in10, in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T10, T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ObservingFunc<T10, T11, T12, TResult>(expression, true);
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
            return new ObservingFunc<T10, T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T11, T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            return (in11, in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T11, T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (isParameterFree) return new ObservingFunc<T11, T12, TResult>(expression, true);
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
            return new ObservingFunc<T11, T12, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T12, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            return (in12) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T12, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            if (isParameterFree) return new ObservingFunc<T12, TResult>(expression, true);
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
            return new ObservingFunc<T12, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 13 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal string parameter10Name;
        internal string parameter11Name;
        internal string parameter12Name;
        internal string parameter13Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.parameter10Name = expression.Parameters[9].Name;
            this.parameter11Name = expression.Parameters[10].Name;
            this.parameter12Name = expression.Parameters[11].Name;
            this.parameter13Name = expression.Parameters[12].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, T10, T11, T12, T13, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T9, T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9, in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, T10, T11, T12, T13, TResult>(expression, true);
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
            return new ObservingFunc<T9, T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T10, T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return (in10, in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T10, T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ObservingFunc<T10, T11, T12, T13, TResult>(expression, true);
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
            return new ObservingFunc<T10, T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T11, T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            return (in11, in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T11, T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (isParameterFree) return new ObservingFunc<T11, T12, T13, TResult>(expression, true);
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
            return new ObservingFunc<T11, T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T12, T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            return (in12, in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T12, T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            if (isParameterFree) return new ObservingFunc<T12, T13, TResult>(expression, true);
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
            return new ObservingFunc<T12, T13, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T13, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            return (in13) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T13, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12)
        {
            if (isParameterFree) return new ObservingFunc<T13, TResult>(expression, true);
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
            return new ObservingFunc<T13, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 14 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal string parameter10Name;
        internal string parameter11Name;
        internal string parameter12Name;
        internal string parameter13Name;
        internal string parameter14Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.parameter10Name = expression.Parameters[9].Name;
            this.parameter11Name = expression.Parameters[10].Name;
            this.parameter12Name = expression.Parameters[11].Name;
            this.parameter13Name = expression.Parameters[12].Name;
            this.parameter14Name = expression.Parameters[13].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, T10, T11, T12, T13, T14, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T9, T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9, in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, T10, T11, T12, T13, T14, TResult>(expression, true);
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
            return new ObservingFunc<T9, T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T10, T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return (in10, in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T10, T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ObservingFunc<T10, T11, T12, T13, T14, TResult>(expression, true);
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
            return new ObservingFunc<T10, T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T11, T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            return (in11, in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T11, T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (isParameterFree) return new ObservingFunc<T11, T12, T13, T14, TResult>(expression, true);
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
            return new ObservingFunc<T11, T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T12, T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            return (in12, in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T12, T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            if (isParameterFree) return new ObservingFunc<T12, T13, T14, TResult>(expression, true);
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
            return new ObservingFunc<T12, T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T13, T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            return (in13, in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T13, T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12)
        {
            if (isParameterFree) return new ObservingFunc<T13, T14, TResult>(expression, true);
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
            return new ObservingFunc<T13, T14, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T14, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13)
        {
            return (in14) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T14, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13)
        {
            if (isParameterFree) return new ObservingFunc<T14, TResult>(expression, true);
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
            return new ObservingFunc<T14, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
    /// <summary>
    /// Represents an observable expression with 15 input parameters
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
    public partial class ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
    {
        internal INotifyExpression<TResult> expression;
        internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult> compiled;
        internal string parameter1Name;
        internal string parameter2Name;
        internal string parameter3Name;
        internal string parameter4Name;
        internal string parameter5Name;
        internal string parameter6Name;
        internal string parameter7Name;
        internal string parameter8Name;
        internal string parameter9Name;
        internal string parameter10Name;
        internal string parameter11Name;
        internal string parameter12Name;
        internal string parameter13Name;
        internal string parameter14Name;
        internal string parameter15Name;
        internal bool isParameterFree;

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        public ObservingFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            compiled = expression.Compile();
            
            this.parameter1Name = expression.Parameters[0].Name;
            this.parameter2Name = expression.Parameters[1].Name;
            this.parameter3Name = expression.Parameters[2].Name;
            this.parameter4Name = expression.Parameters[3].Name;
            this.parameter5Name = expression.Parameters[4].Name;
            this.parameter6Name = expression.Parameters[5].Name;
            this.parameter7Name = expression.Parameters[6].Name;
            this.parameter8Name = expression.Parameters[7].Name;
            this.parameter9Name = expression.Parameters[8].Name;
            this.parameter10Name = expression.Parameters[9].Name;
            this.parameter11Name = expression.Parameters[10].Name;
            this.parameter12Name = expression.Parameters[11].Name;
            this.parameter13Name = expression.Parameters[12].Name;
            this.parameter14Name = expression.Parameters[13].Name;
            this.parameter15Name = expression.Parameters[14].Name;
            this.expression = NotifySystem.CreateExpression<TResult>(expression.Body, expression.Parameters);
            this.isParameterFree = this.expression.IsParameterFree;
        }

        /// <summary>
        /// Creates a new observable expression for the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        internal ObservingFunc(INotifyExpression<TResult> expression, bool isParameterFree)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            this.expression = expression;
            this.isParameterFree = isParameterFree;
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "Observe")]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14, T15 in15)
        {
            return compiled.Invoke(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
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
        public INotifyValue<TResult> Observe(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14, T15 in15)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
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
        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14, INotifyValue<T15> in15)
        {
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1)
        {
            return (in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1)
        {
            if (isParameterFree) return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2)
        {
            return (in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            if (isParameterFree) return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3)
        {
            return (in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            if (isParameterFree) return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return (in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            if (isParameterFree) return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return (in6, in7, in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            if (isParameterFree) return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return (in7, in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            if (isParameterFree) return new ObservingFunc<T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T8, T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return (in8, in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
        /// </summary>
        /// <param name="in1">The input parameter 1</param>
        /// <param name="in2">The input parameter 2</param>
        /// <param name="in3">The input parameter 3</param>
        /// <param name="in4">The input parameter 4</param>
        /// <param name="in5">The input parameter 5</param>
        /// <param name="in6">The input parameter 6</param>
        /// <param name="in7">The input parameter 7</param>
        /// <returns>An observable value that keeps track of any changes</returns>
        public ObservingFunc<T8, T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            if (isParameterFree) return new ObservingFunc<T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
            var parameters = new Dictionary<string, object>();
            parameters.Add(parameter1Name, in1);
            parameters.Add(parameter2Name, in2);
            parameters.Add(parameter3Name, in3);
            parameters.Add(parameter4Name, in4);
            parameters.Add(parameter5Name, in5);
            parameters.Add(parameter6Name, in6);
            parameters.Add(parameter7Name, in7);
            var result = expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>());
            return new ObservingFunc<T8, T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T9, T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return (in9, in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T9, T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            if (isParameterFree) return new ObservingFunc<T9, T10, T11, T12, T13, T14, T15, TResult>(expression, true);
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
            return new ObservingFunc<T9, T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T10, T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return (in10, in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T10, T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            if (isParameterFree) return new ObservingFunc<T10, T11, T12, T13, T14, T15, TResult>(expression, true);
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
            return new ObservingFunc<T10, T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T11, T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10)
        {
            return (in11, in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T11, T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10)
        {
            if (isParameterFree) return new ObservingFunc<T11, T12, T13, T14, T15, TResult>(expression, true);
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
            return new ObservingFunc<T11, T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T12, T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11)
        {
            return (in12, in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T12, T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11)
        {
            if (isParameterFree) return new ObservingFunc<T12, T13, T14, T15, TResult>(expression, true);
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
            return new ObservingFunc<T12, T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T13, T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12)
        {
            return (in13, in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T13, T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12)
        {
            if (isParameterFree) return new ObservingFunc<T13, T14, T15, TResult>(expression, true);
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
            return new ObservingFunc<T13, T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T14, T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13)
        {
            return (in14, in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T14, T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13)
        {
            if (isParameterFree) return new ObservingFunc<T14, T15, TResult>(expression, true);
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
            return new ObservingFunc<T14, T15, TResult>(result, result.IsParameterFree);
        }
        /// <summary>
        /// Invokes the expression partially
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
        [ObservableProxy(typeof(ObservingFunc<,,,,,,,,,,,,,,,>), "ObservePartial")]
        public Func<T15, TResult> EvaluatePartial(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14)
        {
            return (in15) => Evaluate(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14, in15);
        }

        /// <summary>
        /// Invokes the expression partially
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
        public ObservingFunc<T15, TResult> ObservePartial(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14)
        {
            if (isParameterFree) return new ObservingFunc<T15, TResult>(expression, true);
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
            return new ObservingFunc<T15, TResult>(result, result.IsParameterFree);
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9, T10 in10, T11 in11, T12 in12, T13 in13, T14 in14, T15 in15)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
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
        public virtual INotifyReversableValue<TResult> InvokeReversable(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9, INotifyValue<T10> in10, INotifyValue<T11> in11, INotifyValue<T12> in12, INotifyValue<T13> in13, INotifyValue<T14> in14, INotifyValue<T15> in15)
        {
            if (!IsReversable) throw new InvalidOperationException("Expression is not reversable");
            var parameters = new Dictionary<string, object>();
            if (!isParameterFree)
            {
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
            }
            return expression.ApplyParameters(parameters, new Dictionary<INotifiable, INotifiable>()) as INotifyReversableExpression<TResult>;
        }


        /// <summary>
        /// Creates a new observable expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static implicit operator ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult>> expression)
        {
            return FromExpression(expression);
        }


        /// <summary>
        /// Creates a new observable expression from the given expression
        /// </summary>
        /// <param name="expression">The expression that is to be observed</param>
        /// <returns>An observable function</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult> FromExpression(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult>> expression)
        {
            if (expression == null) return null;
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,  TResult>(expression);
        }

        /// <summary>
        /// Gets a value indicating whether this function can be reversed
        /// </summary>
        public virtual bool IsReversable
        {
            get
            {
                var reversable = expression as INotifyReversableExpression<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }
}
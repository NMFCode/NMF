using System;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// This is a facade class that exposes the functionality of NMF Expressions compactly
    /// </summary>
    public static partial class Observable
    {
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, TResult> Func<T1, TResult>(Expression<Func<T1, TResult>> expression)
        {
            return new ObservingFunc<T1, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, TResult> Func<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, TResult> Func<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, TResult> Func<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, TResult> Func<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult> Func<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="T13">The type of the argument 13</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="T13">The type of the argument 13</typeparam>
        /// <typeparam name="T14">The type of the argument 14</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="T13">The type of the argument 13</typeparam>
        /// <typeparam name="T14">The type of the argument 14</typeparam>
        /// <typeparam name="T15">The type of the argument 15</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression)
        {
            return new ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression);
        }

        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, TResult> Func<T1, TResult>(Expression<Func<T1, TResult>> expression, Action<T1, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, TResult> Func<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression, Action<T1, T2, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expression, Action<T1, T2, T3, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, TResult> Func<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, Action<T1, T2, T3, T4, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, TResult> Func<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> expression, Action<T1, T2, T3, T4, T5, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, TResult> Func<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult> Func<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="T13">The type of the argument 13</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="T13">The type of the argument 13</typeparam>
        /// <typeparam name="T14">The type of the argument 14</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(expression, updateHandler);
        }
        /// <summary>
        /// Creates an observable function from the given LINQ Expression of a lambda expression with the given update handler
        /// </summary>
        /// <typeparam name="T1">The type of the argument 1</typeparam>
        /// <typeparam name="T2">The type of the argument 2</typeparam>
        /// <typeparam name="T3">The type of the argument 3</typeparam>
        /// <typeparam name="T4">The type of the argument 4</typeparam>
        /// <typeparam name="T5">The type of the argument 5</typeparam>
        /// <typeparam name="T6">The type of the argument 6</typeparam>
        /// <typeparam name="T7">The type of the argument 7</typeparam>
        /// <typeparam name="T8">The type of the argument 8</typeparam>
        /// <typeparam name="T9">The type of the argument 9</typeparam>
        /// <typeparam name="T10">The type of the argument 10</typeparam>
        /// <typeparam name="T11">The type of the argument 11</typeparam>
        /// <typeparam name="T12">The type of the argument 12</typeparam>
        /// <typeparam name="T13">The type of the argument 13</typeparam>
        /// <typeparam name="T14">The type of the argument 14</typeparam>
        /// <typeparam name="T15">The type of the argument 15</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The LINQ Expression tree</param>
		/// <param name="updateHandler">The update handler</param>
        /// <returns>An observable function. If this function is invoked with a set of arguments, the resulting notify value will update on underlying model updates</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> updateHandler)
        {
            return new ReversableObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(expression, updateHandler);
        }

        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, TResult> Recurse<T1, TResult>(Expression<Func<Func<T1, TResult>, T1, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, TResult> Recurse<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>, T1, T2, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, TResult> Recurse<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>, T1, T2, T3, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="T4">The type of function argument 4</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, T4, TResult> Recurse<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>, T1, T2, T3, T4, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, T4, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="T4">The type of function argument 4</typeparam>
        /// <typeparam name="T5">The type of function argument 5</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, TResult> Recurse<T1, T2, T3, T4, T5, TResult>(Expression<Func<Func<T1, T2, T3, T4, T5, TResult>, T1, T2, T3, T4, T5, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, T4, T5, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="T4">The type of function argument 4</typeparam>
        /// <typeparam name="T5">The type of function argument 5</typeparam>
        /// <typeparam name="T6">The type of function argument 6</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, TResult> Recurse<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<Func<T1, T2, T3, T4, T5, T6, TResult>, T1, T2, T3, T4, T5, T6, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, T4, T5, T6, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="T4">The type of function argument 4</typeparam>
        /// <typeparam name="T5">The type of function argument 5</typeparam>
        /// <typeparam name="T6">The type of function argument 6</typeparam>
        /// <typeparam name="T7">The type of function argument 7</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult> Recurse<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<Func<T1, T2, T3, T4, T5, T6, T7, TResult>, T1, T2, T3, T4, T5, T6, T7, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="T4">The type of function argument 4</typeparam>
        /// <typeparam name="T5">The type of function argument 5</typeparam>
        /// <typeparam name="T6">The type of function argument 6</typeparam>
        /// <typeparam name="T7">The type of function argument 7</typeparam>
        /// <typeparam name="T8">The type of function argument 8</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Recurse<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
        /// <summary>
        /// Recurses the given function
        /// </summary>
        /// <typeparam name="T1">The type of function argument 1</typeparam>
        /// <typeparam name="T2">The type of function argument 2</typeparam>
        /// <typeparam name="T3">The type of function argument 3</typeparam>
        /// <typeparam name="T4">The type of function argument 4</typeparam>
        /// <typeparam name="T5">The type of function argument 5</typeparam>
        /// <typeparam name="T6">The type of function argument 6</typeparam>
        /// <typeparam name="T7">The type of function argument 7</typeparam>
        /// <typeparam name="T8">The type of function argument 8</typeparam>
        /// <typeparam name="T9">The type of function argument 9</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="innerFunc">The inner function</param>
        /// <returns>An observing function that runs the given function recursively</returns>
        public static ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Recurse<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> innerFunc)
        {
            var func = Func(innerFunc);
            var recurse = new RecurseInfo<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(func);
            return func.ObservePartial(recurse.Func);
        }
    }
}

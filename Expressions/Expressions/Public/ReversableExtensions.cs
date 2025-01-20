using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Describes extensions to support reversable operations
    /// </summary>
    public static class ReversableExtensions
    {
        /// <summary>
        /// Turns the given value into a reversable by providing an explicit put operation
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value">The incremental value</param>
        /// <param name="reversableHandler">The put operation</param>
        /// <returns>A reversable incremental value</returns>
        /// <exception cref="ArgumentNullException">Thrown if reversableHandler is null</exception>
        public static INotifyValue<T> AsReversable<T>(this INotifyValue<T> value, Action<T> reversableHandler)
        {
            if (reversableHandler == null) throw new ArgumentNullException(nameof(reversableHandler));
            if(value is INotifyExpression<T> expression)
            {
                return new ReversableProxyExpression<T>( expression, reversableHandler );
            }
            else
            {
                return new ReversableProxyValue<T, INotifyValue<T>>( value, reversableHandler );
            }
        }
    }
}

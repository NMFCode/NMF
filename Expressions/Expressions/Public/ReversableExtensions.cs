using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Describes extensions to support reversable operations
    /// </summary>
    public static class ReversableExtensions
    {
        public static INotifyValue<T> AsReversable<T>(this INotifyValue<T> value, Action<T> reversableHandler)
        {
            if (reversableHandler == null) throw new ArgumentNullException("reversableHandler");
            if(value is INotifyExpression<T> expression)
            {
                return new ReversableProxyExpression<T>( expression, reversableHandler );
            }
            else
            {
                return new ReversableProxyValue<T, INotifyValue<T>>( value, reversableHandler );
            }
        }

        private static class ProxyMethods
        {
            public static INotifyValue<T> AsReversableObserver<T>(INotifyValue<T> value, INotifyValue<Action<T>> reversableHandler)
            {
                if (reversableHandler == null) throw new ArgumentNullException("reversableHandler");
                return value.AsReversable(reversableHandler.Value);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents an incremental computation system
    /// </summary>
    public interface INotifySystem
    {
        /// <summary>
        /// Creates a local variable expression for the given expression and the given local variable
        /// </summary>
        /// <typeparam name="T">The type of the expression whose scope should be used to create the local variable</typeparam>
        /// <typeparam name="TVar">The type of the variable</typeparam>
        /// <param name="inner">The expression for which the local variable should be created</param>
        /// <param name="localVariable">The local variable</param>
        /// <param name="paramName">Returns a parameter name under which the local variable can be referenced</param>
        /// <returns>The inner expression withthe local variable attached to it</returns>
        INotifyExpression<T> CreateLocal<T, TVar>(INotifyExpression<T> inner, INotifyExpression<TVar> localVariable, out string paramName);

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        INotifyExpression<T> CreateExpression<T>(Expression expression, IDictionary<string, object> parameterMappings);

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IDictionary<string, object> parameterMappings);
    }
}

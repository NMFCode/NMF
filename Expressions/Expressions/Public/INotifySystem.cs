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
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings);

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings);

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <returns>An incremental expression object</returns>
        INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings);
    }
}

using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// This class encapsulates a default incremental system
    /// </summary>
    public static class NotifySystem
    {
        static NotifySystem()
        {
            defaultSystem = new InstructionLevelNotifySystem();
        }

        private static INotifySystem defaultSystem;

        /// <summary>
        /// Gets or sets the incremental computation system to be used by default
        /// </summary>
        /// <remarks>This property can never be set to a null value</remarks>
        public static INotifySystem DefaultSystem
        {
            get
            {
                return defaultSystem;
            }
            set
            {
                defaultSystem = value ?? defaultSystem;
            }
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        public static INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings = null)
        {
            return DefaultSystem.CreateExpression<T>(expression, parameters, parameterMappings);
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <returns>An incremental expression object</returns>
        public static INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings = null)
        {
            return DefaultSystem.CreateReversableExpression<T>(expression, parameters, parameterMappings);
        }
    }
}

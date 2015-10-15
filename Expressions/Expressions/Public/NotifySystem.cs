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
        /// Creates a local variable expression for the given expression and the given local variable
        /// </summary>
        /// <typeparam name="T">The type of the expression whose scope should be used to create the local variable</typeparam>
        /// <typeparam name="TVar">The type of the variable</typeparam>
        /// <param name="inner">The expression for which the local variable should be created</param>
        /// <param name="localVariable">The local variable</param>
        /// <param name="paramName">Returns a parameter name under which the local variable can be referenced</param>
        /// <returns>The inner expression withthe local variable attached to it</returns>
        public static INotifyExpression<T> CreateLocal<T, TVar>(INotifyExpression<T> inner, INotifyExpression<TVar> localVariable, out string paramName)
        {
            return DefaultSystem.CreateLocal<T, TVar>(inner, localVariable, out paramName);
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        public static INotifyExpression<T> CreateExpression<T>(Expression expression, IDictionary<string, object> parameterMappings = null)
        {
            return DefaultSystem.CreateExpression<T>(expression, parameterMappings);
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <returns>An incremental expression object</returns>
        public static INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IDictionary<string, object> parameterMappings = null)
        {
            return DefaultSystem.CreateReversableExpression<T>(expression, parameterMappings);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents an implementation of an incremental computation system on instruction level
    /// </summary>
    public class InstructionLevelNotifySystem : INotifySystem
    {
        private int counter = 1;
        private static ObservableExpressionBinder binder = new ObservableExpressionBinder();
        private static InstructionLevelNotifySystem defaultSystem = new InstructionLevelNotifySystem();

        /// <summary>
        /// Gets the default instruction-level-incremental system
        /// </summary>
        public static InstructionLevelNotifySystem Instance
        {
            get
            {
                return defaultSystem;
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
        public INotifyExpression<T> CreateLocal<T, TVar>(INotifyExpression<T> inner, INotifyExpression<TVar> localVariable, out string paramName)
        {
            var baseName = "_<>nmf_transparentIdentifier";
            paramName = baseName + counter.ToString();
            counter++;
            return new ObservableLocalVariable<T, TVar>(inner, localVariable, paramName);
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <returns>An incremental expression object</returns>
        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            if (parameterMappings == null)
            {
                binder.Compress = parameters == null;
                return binder.VisitObservable<T>(expression, false);
            }
            else
            {
                var newBinder = new ObservableExpressionBinder(parameters == null, parameterMappings);
                return newBinder.VisitObservable<T>(expression, false);
            }
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <returns>An incremental expression object</returns>
        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            INotifyReversableExpression<T> exp;
            if (parameterMappings == null)
            {
                binder.Compress = parameters == null;
                exp = binder.VisitObservable<T>(expression, false) as INotifyReversableExpression<T>;
            }
            else
            {
                var newBinder = new ObservableExpressionBinder(parameters == null, parameterMappings);
                exp = newBinder.VisitObservable<T>(expression, false) as INotifyReversableExpression<T>;
            }
            if (exp == null) throw new InvalidOperationException("The given expression could not be reversed!");
            return exp;
        }

        /// <summary>
        /// Creates an incremental expression for the given code expression
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The expression from which to create an incremental expression</param>
        /// <param name="parameterMappings">A given mapping of parameters</param>
        /// <param name="parameters">The parameters of the expression</param>
        /// <returns>An incremental expression object</returns>
        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            if (parameterMappings == null)
            {
                return binder.VisitObservable(expression, false);
            }
            else
            {
                var newBinder = new ObservableExpressionBinder(false, parameterMappings);
                return newBinder.VisitObservable(expression, false);
            }
        }
    }
}

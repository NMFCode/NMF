using System.Collections.Generic;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents an expression with additional information on the program flow
    /// </summary>
    public interface INotifyExpression : INotifiable
    {
        /// <summary>
        /// Determines whether the expression can be replaced by a constant expression
        /// </summary>
        bool CanBeConstant { get; }

        /// <summary>
        /// Determines whether the current expression is a constant
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        /// Determines whether the current expression contains parameters
        /// </summary>
        bool IsParameterFree { get; }

        /// <summary>
        /// Gets the current value as object
        /// </summary>
        object ValueObject { get; }

        /// <summary>
        /// Applies the given set of parameters to the expression
        /// </summary>
        /// <param name="parameters">A set of parameter values</param>
        /// <param name="trace">A trace to make sure parameters are only applied once for every DDG node</param>
        /// <returns>A new expression with all parameter placeholders replaced with the parameter values</returns>
        /// <remarks>In case that the current expression is parameter free, it simply returns itself</remarks>
        INotifyExpression ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace);
    }

    /// <summary>
    /// Represents a typed expression with additional information on the program flow
    /// </summary>
    /// <typeparam name="T">The type of the expression</typeparam>
    public interface INotifyExpression<out T> : INotifyValue<T>, INotifyExpression
    {
        /// <summary>
        /// Applies the given set of parameters to the expression
        /// </summary>
        /// <param name="parameters">A set of parameter values</param>
        /// <param name="trace">A trace to make sure parameters are only applied once for every DDG node</param>
        /// <returns>A new expression with all parameter placeholders replaced with the parameter values</returns>
        /// <remarks>In case that the current expression is parameter free, it simply returns itself</remarks>
        new INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace);

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression repüresenting the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
        INotifyExpression<T> Reduce();
    }
}

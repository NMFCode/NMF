using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Base class for incremental expressions
    /// </summary>
    /// <remarks>The purpose of this class is only to be able to both shadow and override the inherited Reduce method</remarks>
    public abstract class NotifyExpressionBase : Expression
    {
        /// <summary>
        /// Expression reduce method
        /// </summary>
        /// <returns></returns>
        public sealed override Expression Reduce()
        {
            return BaseReduce();
        }

        /// <summary>
        /// Rerouted inherited reduce method
        /// </summary>
        /// <returns></returns>
        protected virtual Expression BaseReduce()
        {
            return base.Reduce();
        }
    }
}

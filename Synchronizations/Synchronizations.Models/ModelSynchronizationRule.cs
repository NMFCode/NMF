using NMF.Models;

namespace NMF.Synchronizations.Models
{
    /// <summary>
    /// Denotes a synchronization rule of model elements
    /// </summary>
    /// <typeparam name="TLeft">The LHS model type</typeparam>
    /// <typeparam name="TRight">The RHS model type</typeparam>
    public abstract class ModelSynchronizationRule<TLeft, TRight> : SynchronizationRule<TLeft, TRight>
        where TLeft : class, IModelElement
        where TRight : class, IModelElement
    {
        /// <inheritdoc />
        public override bool ShouldCorrespond(TLeft left, TRight right, ISynchronizationContext context)
        {
            if (left == null || right == null)
            {
                return left == right;
            }
            if (left.IsIdentified && right.IsIdentified)
            {
                return left.ToIdentifierString() == right.ToIdentifierString();
            }
            return false;
        }
    }
}

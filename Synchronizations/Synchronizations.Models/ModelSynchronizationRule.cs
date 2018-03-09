using NMF.Models;

namespace NMF.Synchronizations.Models
{

    public abstract class ModelSynchronizationRule<TLeft, TRight> : SynchronizationRule<TLeft, TRight>
        where TLeft : class, IModelElement
        where TRight : class, IModelElement
    {
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

using System;
using NMF.Models;
using NMF.Synchronizations;
using NMF.Transformations;

namespace NMF.AnyText
{
    public abstract class ModelSynchronization
    {
        public abstract void Initialize();
        public abstract void Synchronize(IModelElement left, IModelElement right);
    }
    public class ModelSynchronization<TLeft, TRight, TSynchronization, TStartRule> : ModelSynchronization
        where TLeft : class, IModelElement
        where TRight : class, IModelElement
        where TSynchronization : ReflectiveSynchronization, new()
        where TStartRule: SynchronizationRule<TLeft,TRight>
    {
        private readonly TSynchronization _sync = new();
        private readonly SynchronizationDirection _direction;
        private readonly ChangePropagationMode _propagation;

        public ModelSynchronization(
            SynchronizationDirection direction = SynchronizationDirection.LeftWins,
            ChangePropagationMode propagation = ChangePropagationMode.TwoWay)
        {
            _direction = direction;
            _propagation = propagation;
        }

        public override void Initialize() => _sync.Initialize();

        public override void Synchronize(IModelElement left, IModelElement right)
        {
            var leftModelElement = (TLeft)left;
            var rightModelElement = (TRight)right;
            _sync.Synchronize(_sync.SynchronizationRule<TStartRule>(), ref leftModelElement, ref rightModelElement, _direction, _propagation);
        }

    }
}

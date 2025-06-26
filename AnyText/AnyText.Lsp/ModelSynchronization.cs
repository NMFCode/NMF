using System;
using NMF.Models;
using NMF.Synchronizations;
using NMF.Transformations;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents the abstract base class for model synchronization logic.
    /// </summary>
    public abstract class ModelSynchronization
    {
        /// <summary>
        /// Gets the language identifier for the left model.
        /// </summary>
        public readonly string LeftLanguageId;
        /// <summary>
        /// Gets the language identifier for the right model.
        /// </summary>
        public readonly string RightLanguageId;

        /// <summary>
        /// Gets the synchronization direction.
        /// </summary>
        protected readonly SynchronizationDirection Direction;

        /// <summary>
        /// Gets the change propagation mode.
        /// </summary>
        protected readonly ChangePropagationMode Propagation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSynchronization"/> class with specified language identifiers and synchronization settings.
        /// </summary>
        /// <param name="leftLanguageId">The language identifier for the left model.</param>
        /// <param name="rightLanguageId">The language identifier for the right model.</param>
        /// <param name="direction">The synchronization direction.</param>
        /// <param name="propagation">The change propagation mode.</param>
        protected ModelSynchronization(
            string leftLanguageId,
            string rightLanguageId,
            SynchronizationDirection direction = SynchronizationDirection.LeftWins,
            ChangePropagationMode propagation = ChangePropagationMode.TwoWay)
        {
            LeftLanguageId = leftLanguageId;
            RightLanguageId = rightLanguageId;
            Direction = direction;
            Propagation = propagation;
        }

        /// <summary>
        /// Initializes the synchronization engine.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Synchronizes the specified left and right model elements.
        /// </summary>
        /// <param name="left">The left model element.</param>
        /// <param name="right">The right model element.</param>
        public abstract void Synchronize(IModelElement left, IModelElement right);
    }

    /// <summary>
    /// Represents a generic model synchronization class for specific model element types.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left model element.</typeparam>
    /// <typeparam name="TRight">The type of the right model element.</typeparam>
    /// <typeparam name="TSynchronization">The type of the synchronization engine used.</typeparam>
    /// <typeparam name="TStartRule">The type of the starting synchronization rule.</typeparam>
    public class ModelSynchronization<TLeft, TRight, TSynchronization, TStartRule> : ModelSynchronization
        where TLeft : class, IModelElement
        where TRight : class, IModelElement
        where TSynchronization : ReflectiveSynchronization, new()
        where TStartRule: SynchronizationRule<TLeft,TRight>
    {
        private readonly TSynchronization _sync = new();


        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSynchronization{TLeft, TRight, TSynchronization, TStartRule}"/> class.
        /// </summary>
        public ModelSynchronization(
            string leftLanguageId,
            string rightLanguageId,
            SynchronizationDirection direction = SynchronizationDirection.LeftWins,
            ChangePropagationMode propagation = ChangePropagationMode.TwoWay)
            : base(leftLanguageId, rightLanguageId, direction, propagation)
        {
        }

        /// <summary>
        /// Initializes the synchronization engine.
        /// </summary>
        public override void Initialize() => _sync.Initialize();


        /// <summary>
        /// Synchronizes the specified left and right model elements using the configured synchronization engine.
        /// </summary>
        /// <param name="left">The left model element.</param>
        /// <param name="right">The right model element.</param>
        public override void Synchronize(IModelElement left, IModelElement right)
        {
            var leftModelElement = (TLeft)left;
            var rightModelElement = (TRight)right;
            _sync.Synchronize(_sync.SynchronizationRule<TStartRule>(), ref leftModelElement, ref rightModelElement, Direction, Propagation);
        }

    }
}

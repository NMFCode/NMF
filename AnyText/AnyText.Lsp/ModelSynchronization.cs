using System;
using NMF.AnyText.Grammars;
using NMF.Models;
using NMF.Synchronizations;
using NMF.Synchronizations.Models;
using NMF.Transformations;

namespace NMF.AnyText
{
    /// <summary>
    ///     Represents the abstract base class for model synchronization logic.
    /// </summary>
    public abstract class ModelSynchronization
    {
        /// <summary>
        ///     Gets the grammar for the left model.
        /// </summary>
        public readonly Grammar LeftLanguage;

        /// <summary>
        ///     Gets the grammar for the right model.
        /// </summary>
        public readonly Grammar RightLanguage;

        /// <summary>
        ///     Gets the synchronization direction.
        /// </summary>
        protected readonly SynchronizationDirection Direction;

        /// <summary>
        ///     Gets the change propagation mode.
        /// </summary>
        protected readonly ChangePropagationMode Propagation;

        /// <summary>
        ///     Gets the predicate to filter model elements during synchronization.
        /// </summary>
        private readonly Func<ParseContext, ParseContext, bool> _synchronizationPredicate = (left, right) => true;

        /// <summary>
        ///     Gets a value indicating whether synchronization should be performed automatically.
        /// </summary>
        public readonly bool IsAutomatic;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelSynchronization" /> class with specified language identifiers and
        ///     synchronization settings.
        /// </summary>
        /// <param name="leftLanguage">The grammar for the left model.</param>
        /// <param name="rightLanguage">The grammar for the right model.</param>
        /// <param name="direction">The synchronization direction.</param>
        /// <param name="propagation">The change propagation mode.</param>
        /// <param name="predicate">The predicate to filter model elements. Defaults to a predicate that always returns true.</param>
        /// <param name="isAutomatic">Indicates whether synchronization is automatic. Defaults to true.</param>
        protected ModelSynchronization(
            Grammar leftLanguage,
            Grammar rightLanguage,
            SynchronizationDirection direction = SynchronizationDirection.LeftWins,
            ChangePropagationMode propagation = ChangePropagationMode.TwoWay,
            Func<ParseContext, ParseContext, bool> predicate = null,
            bool isAutomatic = true)
        {
            LeftLanguage = leftLanguage;
            RightLanguage = rightLanguage;
            Direction = direction;
            Propagation = propagation;
            _synchronizationPredicate = predicate ?? _synchronizationPredicate;
            IsAutomatic = isAutomatic;
        }

        /// <summary>
        ///     Initializes the synchronization engine.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        ///     Synchronizes the specified left and right model elements.
        /// </summary>
        /// <param name="left">The left model element.</param>
        /// <param name="right">The right model element.</param>
        protected abstract void Synchronize(IModelElement left, IModelElement right);


        /// <summary>
        ///     Attempts to synchronize two models (and their parsers, if available) based on the rules
        ///     defined in this <see cref="ModelSynchronization" /> instance.
        /// </summary>
        /// <param name="leftModel">
        ///     The first model element to be synchronized, or <c>null</c> to extract it from <paramref name="leftParser" />.
        /// </param>
        /// <param name="rightModel">
        ///     The second model element to be synchronized, or <c>null</c> to extract it from <paramref name="rightParser" />.
        /// </param>
        /// <param name="leftParser">
        ///     The parser associated with the first model element, or <c>null</c> if unavailable.
        /// </param>
        /// <param name="rightParser">
        ///     The parser associated with the second model element, or <c>null</c> if unavailable.
        /// </param>
        /// <param name="service">
        ///     The <see cref="SynchronizationService" /> instance responsible for subscribing to model changes
        ///     and coordinating the synchronization process.
        /// </param>
        public void TrySynchronize(
            IModelElement leftModel,
            IModelElement rightModel,
            Parser leftParser,
            Parser rightParser,
            SynchronizationService service)
        {
            if (leftModel == null && leftParser != null)
                leftModel = (IModelElement)leftParser.Context.Root;
            if (rightModel == null && rightParser != null)
                rightModel = (IModelElement)rightParser.Context.Root;

            if (leftParser != null && rightParser != null &&
                !_synchronizationPredicate(leftParser.Context, rightParser.Context))
                return;

            Initialize();

            if (leftParser != null)
                service.SubscribeToModelChanges(leftModel, leftParser);
            if (rightParser != null)
                service.SubscribeToModelChanges(rightModel, rightParser);

            Synchronize(leftModel, rightModel);
        }

        /// <summary>
        ///     Attempts to synchronize two models extracted from the provided parsers,
        ///     based on the rules defined in this <see cref="ModelSynchronization" /> instance.
        /// </summary>
        /// <param name="leftParser">The parser for the first model to synchronize.</param>
        /// <param name="rightParser">The parser for the second model to synchronize.</param>
        /// <param name="service">
        ///     The <see cref="SynchronizationService" /> responsible for subscribing to model changes
        ///     and coordinating the synchronization process.
        /// </param>
        public void TrySynchronize(Parser leftParser, Parser rightParser, SynchronizationService service)
        {
            TrySynchronize(null, null, leftParser, rightParser, service);
        }
    }


    /// <summary>
    ///     Represents a synchronization class for homogeneous model elements.
    /// </summary>
    /// <typeparam name="T">The type of model element being synchronized.</typeparam>
    public class HomogenModelSync<T> : ModelSynchronization where T : class, IModelElement
    {
        private readonly ReflectiveSynchronization _sync = new HomogeneousSynchronization<T>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomogenModelSync{T}" /> class.
        /// </summary>
        /// <param name="leftLanguage">The grammar for the left model.</param>
        /// <param name="rightLanguage">The grammar for the right model.</param>
        /// <param name="direction">The synchronization direction.</param>
        /// <param name="propagation">The change propagation mode.</param>
        /// <param name="predicate">Optional predicate to filter model elements.</param>
        /// <param name="isAutomatic">Indicates whether synchronization is automatic.</param>
        public HomogenModelSync(Grammar leftLanguage, Grammar rightLanguage,
            SynchronizationDirection direction = SynchronizationDirection.RightToLeftForced,
            ChangePropagationMode propagation = ChangePropagationMode.TwoWay,
            Func<ParseContext, ParseContext, bool> predicate = null, bool isAutomatic = true) : base(leftLanguage,
            rightLanguage, direction, propagation, predicate, isAutomatic)
        {
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            _sync.Initialize();
        }

        /// <inheritdoc />
        protected override void Synchronize(IModelElement left, IModelElement right)
        {
            var leftModelElement = (T)left;
            var rightModelElement = (T)right;
            _sync.Synchronize(ref leftModelElement, ref rightModelElement, Direction, Propagation);
        }
    }

    /// <summary>
    ///     Represents a generic model synchronization class for specific model element types.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left model element.</typeparam>
    /// <typeparam name="TRight">The type of the right model element.</typeparam>
    /// <typeparam name="TSynchronization">The type of the synchronization engine used.</typeparam>
    /// <typeparam name="TStartRule">The type of the starting synchronization rule.</typeparam>
    public class ModelSynchronization<TLeft, TRight, TSynchronization, TStartRule> : ModelSynchronization
        where TLeft : class, IModelElement
        where TRight : class, IModelElement
        where TSynchronization : ReflectiveSynchronization, new()
        where TStartRule : SynchronizationRule<TLeft, TRight>
    {
        private readonly TSynchronization _sync = new();


        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelSynchronization{TLeft, TRight, TSynchronization, TStartRule}" />
        ///     class.
        /// </summary>
        public ModelSynchronization(
            Grammar leftLanguage,
            Grammar rightLanguage,
            SynchronizationDirection direction = SynchronizationDirection.LeftWins,
            ChangePropagationMode propagation = ChangePropagationMode.TwoWay,
            Func<ParseContext, ParseContext, bool> predicate = null,
            bool isAutomatic = true)
            : base(leftLanguage, rightLanguage, direction, propagation, predicate, isAutomatic)
        {
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            _sync.Initialize();
        }


        /// <inheritdoc />
        protected override void Synchronize(IModelElement left, IModelElement right)
        {
            var leftModelElement = (TLeft)left;
            var rightModelElement = (TRight)right;
            _sync.Synchronize(_sync.SynchronizationRule<TStartRule>(), ref leftModelElement, ref rightModelElement,
                Direction, Propagation);
        }
    }
}
using System;
using NMF.AnyText.Grammars;
using NMF.Models;
using NMF.Synchronizations;
using NMF.Synchronizations.Models;
using NMF.Transformations;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents the abstract base class for model synchronization logic.
    /// </summary>
    public abstract class ModelSynchronization
    {
        /// <summary>
        /// Gets the grammar for the left model.
        /// </summary>
        public readonly Grammar LeftLanguage;
        
        /// <summary>
        /// Gets the grammar for the right model.
        /// </summary>
        public readonly Grammar RightLanguage;

        /// <summary>
        /// Gets the synchronization direction.
        /// </summary>
        protected readonly SynchronizationDirection Direction;

        /// <summary>
        /// Gets the change propagation mode.
        /// </summary>
        protected readonly ChangePropagationMode Propagation;
        
        /// <summary>
        /// Gets the predicate to filter model elements during synchronization.
        /// </summary>
        private readonly Func<ParseContext, ParseContext, bool> _synchronizationPredicate = (left, right) => true;
        
        /// <summary>
        /// Gets a value indicating whether synchronization should be performed automatically.
        /// </summary>
        public readonly bool IsAutomatic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSynchronization"/> class with specified language identifiers and synchronization settings.
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
        /// Initializes the synchronization engine.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Synchronizes the specified left and right model elements.
        /// </summary>
        /// <param name="left">The left model element.</param>
        /// <param name="right">The right model element.</param>
        protected abstract void Synchronize(IModelElement left, IModelElement right);
        
         /// <summary>
        /// Attempts to synchronize two parsers based on the rules defined in this instance.
        /// </summary>
        /// <param name="leftParser">The first parser to consider for synchronization.</param>
        /// <param name="rightParser">The second parser to consider for synchronization.</param>
        /// <param name="server">The LSP server instance for subscribing to changes.</param>
        public void TrySynchronize(Parser leftParser, Parser rightParser, ILspServer server)
        {
           
            if (!_synchronizationPredicate(leftParser.Context, rightParser.Context))
            {
                return;
            }
            Initialize();
            var leftModel = (IModelElement)leftParser.Context.Root;
            var rightModel = (IModelElement)rightParser.Context.Root;
            ModelChangeHandler.SubscribeToModelChanges(leftModel, leftParser, server);
            ModelChangeHandler.SubscribeToModelChanges(rightModel, rightParser, server);

            Synchronize(leftModel, rightModel);
        }
    }

    
    /// <summary>
    /// Represents a synchronization class for homogeneous model elements.
    /// </summary>
    /// <typeparam name="T">The type of model element being synchronized.</typeparam>
    public class HomogenModelSync<T> : ModelSynchronization where T : class, IModelElement
    {
        private readonly ReflectiveSynchronization _sync = new HomogeneousSynchronization<T>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HomogenModelSync{T}"/> class.
        /// </summary>
        /// <param name="leftLanguage">The grammar for the left model.</param>
        /// <param name="rightLanguage">The grammar for the right model.</param>
        /// <param name="direction">The synchronization direction.</param>
        /// <param name="propagation">The change propagation mode.</param>
        /// <param name="predicate">Optional predicate to filter model elements.</param>
        /// <param name="isAutomatic">Indicates whether synchronization is automatic.</param>
        public HomogenModelSync(Grammar leftLanguage, Grammar rightLanguage, SynchronizationDirection direction = SynchronizationDirection.RightToLeftForced, ChangePropagationMode propagation = ChangePropagationMode.TwoWay, Func<ParseContext, ParseContext, bool> predicate = null, bool isAutomatic = true) : base(leftLanguage, rightLanguage, direction, propagation, predicate, isAutomatic)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
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
        public override void Initialize() => _sync.Initialize();


        /// <inheritdoc />
        protected override void Synchronize(IModelElement left, IModelElement right)
        {
            var leftModelElement = (TLeft)left;
            var rightModelElement = (TRight)right;
            _sync.Synchronize(_sync.SynchronizationRule<TStartRule>(), ref leftModelElement, ref rightModelElement, Direction, Propagation);
        }

    }
}

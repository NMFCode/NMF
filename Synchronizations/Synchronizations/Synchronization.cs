using NMF.Transformations.Core;
using NMF.Transformations;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes an abstract synchronization
    /// </summary>
    public abstract class Synchronization : Transformation
    {
        /// <summary>
        /// Gets a collection of synchronization rules
        /// </summary>
        public abstract IEnumerable<SynchronizationRuleBase> SynchronizationRules { get; }

        /// <inheritdoc />
        public override void RegisterRules()
        {
            if(!IsRulesRegistered)
            {
                base.RegisterRules();

                if(SynchronizationRules != null)
                {
                    foreach(var rule in SynchronizationRules)
                    {
                        // Check whether the synchronization has already been registered.
                        // This happens when a rule is overridden
                        if(rule.Synchronization == null)
                        {
                            rule.Synchronization = this;
                            rule.DeclareSynchronization();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the synchronization rule for the given synchronization rule type
        /// </summary>
        /// <param name="type">The type of synchronization rule</param>
        /// <returns>The synchronization rule object</returns>
        public virtual SynchronizationRuleBase GetSynchronizationRuleForType( Type type )
        {
            if(type == null) throw new ArgumentNullException( nameof(type));
            return SynchronizationRules.Where( type.IsInstanceOfType ).FirstOrDefault();
        }

        /// <summary>
        /// Gets all synchronization rules of the given type
        /// </summary>
        /// <param name="type">The type of synchronization rule</param>
        /// <returns>All synchronization rules of the given type</returns>
        public virtual IEnumerable<SynchronizationRuleBase> GetSynchronizationRulesForType( Type type )
        {
            if(type == null) throw new ArgumentNullException( nameof(type));
            return SynchronizationRules.Where( type.IsInstanceOfType );
        }

        /// <summary>
        /// Gets the synchronization rule for the given LHS and RHS type combination
        /// </summary>
        /// <param name="left">The LHS type</param>
        /// <param name="right">The RHS type</param>
        /// <returns>The synchronization rule or null, if no synchronization rule could be found</returns>
        public virtual SynchronizationRuleBase GetSynchronizationRuleForSignature( Type left, Type right )
        {
            var exactMatch = SynchronizationRules.Where( s => s.LeftType == left && s.RightType == right ).FirstOrDefault();
            if(exactMatch != null) return exactMatch;
            return SynchronizationRules.Where( s => left.IsAssignableFrom( s.LeftType ) && right.IsAssignableFrom( s.RightType ) ).FirstOrDefault();
        }

        /// <summary>
        /// Gets all synchronization rules for the given LHS and RHS type combination
        /// </summary>
        /// <param name="left">The LHS type</param>
        /// <param name="right">The RHS type</param>
        /// <returns></returns>
        public virtual IEnumerable<SynchronizationRuleBase> GetSynchronizationRulesForSignature( Type left, Type right )
        {
            return SynchronizationRules.Where( s => left.IsAssignableFrom( s.LeftType ) && right.IsAssignableFrom( s.RightType ) );
        }

        /// <summary>
        /// Synchronizes the given LHS element with the provided RHS element or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="left">The LHS element</param>
        /// <param name="right">The RHS element</param>
        /// <param name="direction">The direction of the model synchronization</param>
        /// <param name="changePropagation">The change propagation mode</param>
        /// <returns>The synchronization context in which the synchronization takes place</returns>
        public ISynchronizationContext Synchronize<TLeft, TRight>( ref TLeft left, ref TRight right, SynchronizationDirection direction, ChangePropagationMode changePropagation )
            where TLeft : class
            where TRight : class
        {
            return Synchronize( GetSynchronizationRuleForSignature( typeof( TLeft ), typeof( TRight ) ) as SynchronizationRule<TLeft, TRight>, ref left, ref right, direction, changePropagation );
        }

        /// <summary>
        /// Synchronizes the given LHS element with the provided RHS element or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="startRule">The rule that should be started with</param>
        /// <param name="left">The LHS element</param>
        /// <param name="right">The RHS element</param>
        /// <param name="direction">The direction of the model synchronization</param>
        /// <param name="changePropagation">The change propagation mode</param>
        /// <returns>The synchronization context in which the synchronization takes place</returns>
        public ISynchronizationContext Synchronize<TLeft, TRight>( SynchronizationRule<TLeft, TRight> startRule, ref TLeft left, ref TRight right, SynchronizationDirection direction, ChangePropagationMode changePropagation )
            where TLeft : class
            where TRight : class
        {
            if(startRule == null) throw new ArgumentNullException( nameof(startRule));

            var context = new SynchronizationContext( this, direction, changePropagation );
            Synchronize( startRule, ref left, ref right, context );
            return context;
        }

        /// <summary>
        /// Synchronizes the given LHS element with the provided RHS element or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="left">The LHS element</param>
        /// <param name="right">The RHS element</param>
        /// <param name="synchronizationContext">The context in which the synchronization shall be executed</param>
        public void Synchronize<TLeft, TRight>( ref TLeft left, ref TRight right, ISynchronizationContext synchronizationContext )
            where TLeft : class
            where TRight : class
        {
            Synchronize( GetSynchronizationRuleForSignature( typeof( TLeft ), typeof( TRight ) ) as SynchronizationRule<TLeft, TRight>, ref left, ref right, synchronizationContext );
        }

        /// <summary>
        /// Synchronizes the given LHS element with the provided RHS element or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="startRule">The rule that should be started with</param>
        /// <param name="left">The LHS element</param>
        /// <param name="right">The RHS element</param>
        /// <param name="synchronizationContext">The context in which the synchronization shall be executed</param>
        public void Synchronize<TLeft, TRight>( SynchronizationRule<TLeft, TRight> startRule, ref TLeft left, ref TRight right, ISynchronizationContext synchronizationContext )
            where TLeft : class
            where TRight : class
        {
            if(startRule == null) throw new ArgumentNullException( nameof(startRule));
            if(synchronizationContext == null) throw new ArgumentNullException( nameof( synchronizationContext ) );

            Initialize();

            switch(synchronizationContext.Direction)
            {
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                case SynchronizationDirection.LeftWins:
                    var c1 = TransformationRunner.Transform( new object[] { left }, right != null ? new Axiom( right ) : null, startRule.LeftToRight, synchronizationContext );
                    right = c1.Output as TRight;
                    break;
                case SynchronizationDirection.RightToLeft:
                case SynchronizationDirection.RightToLeftForced:
                case SynchronizationDirection.RightWins:
                    var c2 = TransformationRunner.Transform( new object[] { right }, left != null ? new Axiom( left ) : null, startRule.RightToLeft, synchronizationContext );
                    left = c2.Output as TLeft;
                    break;
                case SynchronizationDirection.CheckOnly:
                    if(left == null) throw new ArgumentException( "Passed model must not be null when running in check-only mode", nameof( left ) );
                    if(right == null) throw new ArgumentException( "Passed model must not be null when running in check-only mode", nameof( right ) );
                    TransformationRunner.Transform( new object[] { left }, new Axiom( right ), startRule.LeftToRight, synchronizationContext );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( "direction" );
            }
        }

        /// <summary>
        /// Synchronizes the given LHS elements with the provided RHS elements or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="lefts">The LHS elements</param>
        /// <param name="rights">The RHS elements</param>
        /// <param name="direction">The direction of the model synchronization</param>
        /// <param name="changePropagation">The change propagation mode</param>
        /// <returns>The synchronization context in which the synchronization takes place</returns>
        public ISynchronizationContext SynchronizeMany<TLeft, TRight>( ICollection<TLeft> lefts, ICollection<TRight> rights, SynchronizationDirection direction, ChangePropagationMode changePropagation )
            where TLeft : class
            where TRight : class
        {
            return SynchronizeMany( GetSynchronizationRuleForSignature( typeof( TLeft ), typeof( TRight ) ) as SynchronizationRule<TLeft, TRight>, lefts, rights, direction, changePropagation );
        }

        /// <summary>
        /// Synchronizes the given LHS elements with the provided RHS elements or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="startRule">The synchronization rule to start with</param>
        /// <param name="lefts">The LHS elements</param>
        /// <param name="rights">The RHS elements</param>
        /// <param name="direction">The direction of the model synchronization</param>
        /// <param name="changePropagation">The change propagation mode</param>
        /// <returns>The synchronization context in which the synchronization takes place</returns>
        public ISynchronizationContext SynchronizeMany<TLeft, TRight>( SynchronizationRule<TLeft, TRight> startRule, ICollection<TLeft> lefts, ICollection<TRight> rights, SynchronizationDirection direction, ChangePropagationMode changePropagation )
            where TLeft : class
            where TRight : class
        {
            if(startRule == null) throw new ArgumentNullException( nameof(startRule));

            var context = new SynchronizationContext( this, direction, changePropagation );
            SynchronizeMany( startRule, lefts, rights, context );
            return context;
        }

        /// <summary>
        /// Synchronizes the given LHS elements with the provided RHS elements or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="lefts">The LHS elements</param>
        /// <param name="rights">The RHS elements</param>
        /// <param name="synchronizationContext">The context in which the synchronization shall be executed</param>
        /// <returns>The synchronization context in which the synchronization takes place</returns>
        public void SynchronizeMany<TLeft, TRight>( ICollection<TLeft> lefts, ICollection<TRight> rights, ISynchronizationContext synchronizationContext )
            where TLeft : class
            where TRight : class
        {
            SynchronizeMany( GetSynchronizationRuleForSignature( typeof( TLeft ), typeof( TRight ) ) as SynchronizationRule<TLeft, TRight>, lefts, rights, synchronizationContext );
        }

        /// <summary>
        /// Synchronizes the given LHS elements with the provided RHS elements or creates them if necessary
        /// </summary>
        /// <typeparam name="TLeft">The LHS type</typeparam>
        /// <typeparam name="TRight">The RHS type</typeparam>
        /// <param name="startRule">The synchronization rule to start with</param>
        /// <param name="lefts">The LHS elements</param>
        /// <param name="rights">The RHS elements</param>
        /// <param name="synchronizationContext">The context in which the synchronization shall be executed</param>
        /// <returns>The synchronization context in which the synchronization takes place</returns>
        public void SynchronizeMany<TLeft, TRight>( SynchronizationRule<TLeft, TRight> startRule, ICollection<TLeft> lefts, ICollection<TRight> rights, ISynchronizationContext synchronizationContext )
            where TLeft : class
            where TRight : class
        {
            if(startRule == null) throw new ArgumentNullException( nameof(startRule));
            if(synchronizationContext == null) throw new ArgumentNullException( nameof( synchronizationContext ) );

            switch(synchronizationContext.Direction)
            {
                case SynchronizationDirection.LeftToRight:
                    var c1 = TransformationRunner.TransformMany( lefts.Select( l => new object[] { l } ), rights, startRule.LeftToRight, synchronizationContext );
                    rights.Clear();
                    rights.AddRange( c1.Select( c => c.Output as TRight ) );
                    break;
                case SynchronizationDirection.RightToLeft:
                    var c2 = TransformationRunner.TransformMany( rights.Select( r => new object[] { r } ), lefts, startRule.RightToLeft, synchronizationContext );
                    lefts.Clear();
                    lefts.AddRange( c2.Select( c => c.Output as TLeft ) );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( "direction" );
            }
        }
    }
}

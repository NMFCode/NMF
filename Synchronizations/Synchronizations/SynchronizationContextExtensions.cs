using NMF.Expressions;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes extension methods for the transformation context to easily access the trace links in expressions
    /// </summary>
    public static class SynchronizationContextExtensions
    {
        /// <summary>
        /// Maps the given LHS element using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TLeft">The LHS element type</typeparam>
        /// <typeparam name="TRight">The RHS element type</typeparam>
        /// <param name="context">The transformation context</param>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="left">The LHS element</param>
        /// <returns>The RHS element corresponding to the provided LHS element</returns>
        [ObservableProxy(typeof(IncrementalHelpers), nameof(IncrementalHelpers.MapLeft))]
        public static TRight MapLeft<TLeft, TRight>( this ITransformationContext context, SynchronizationRule<TLeft, TRight> rule, TLeft left )
        {
            var computation = context.CallTransformation( rule.LeftToRight, new object[] { left }, null );
            return (TRight)computation.Output;
        }

        /// <summary>
        /// Maps the given LHS elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TLeft">The LHS element type</typeparam>
        /// <typeparam name="TRight">The RHS element type</typeparam>
        /// <param name="context">The transformation context</param>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="lefts">The LHS elements</param>
        /// <returns>The RHS elements corresponding to the provided LHS elements</returns>
        public static IEnumerableExpression<TRight> MapLefts<TLeft, TRight>(this ITransformationContext context, SynchronizationRule<TLeft, TRight> rule, IEnumerableExpression<TLeft> lefts)
        {
            return new MappingCollection<TLeft, TRight>( lefts, rule.LeftToRight, context );
        }


        /// <summary>
        /// Maps the given RHS element using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TLeft">The LHS element type</typeparam>
        /// <typeparam name="TRight">The RHS element type</typeparam>
        /// <param name="context">The transformation context</param>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="right">The RHS element</param>
        /// <returns>The LHS element corresponding to the provided RHS element</returns>
        [ObservableProxy( typeof( IncrementalHelpers ), nameof( IncrementalHelpers.MapRight ) )]
        public static TLeft MapRight<TLeft, TRight>( this ITransformationContext context, SynchronizationRule<TLeft, TRight> rule, TRight right )
        {
            var computation = context.CallTransformation( rule.RightToLeft, new object[] { right }, null );
            return (TLeft)computation.Output;
        }

        /// <summary>
        /// Maps the given RHS elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TLeft">The LHS element type</typeparam>
        /// <typeparam name="TRight">The RHS element type</typeparam>
        /// <param name="context">The transformation context</param>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="rights">The RHS elements</param>
        /// <returns>The LHS elements corresponding to the provided RHS elements</returns>
        public static IEnumerableExpression<TLeft> MapRights<TLeft, TRight>( this ITransformationContext context, SynchronizationRule<TLeft, TRight> rule, IEnumerableExpression<TRight> rights )
        {
            return new MappingCollection<TRight, TLeft>( rights, rule.RightToLeft, context );
        }

        private class IncrementalHelpers
        {
            public static INotifyValue<TRight> MapLeft<TLeft, TRight>( ITransformationContext context, SynchronizationRule<TLeft, TRight> rule, TLeft left )
            {
                var computation = context.CallTransformation( rule.LeftToRight, new object[] { left }, null );
                return (SynchronizationComputation<TLeft, TRight>)computation;
            }
            public static INotifyValue<TLeft> MapRight<TLeft, TRight>( ITransformationContext context, SynchronizationRule<TLeft, TRight> rule, TRight right )
            {
                var computation = context.CallTransformation( rule.RightToLeft, new object[] { right }, null );
                return (SynchronizationComputation<TRight, TLeft>)computation;
            }
        }
    }
}

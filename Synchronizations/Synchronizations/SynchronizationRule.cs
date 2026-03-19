using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes an abstract synchronization rule
    /// </summary>
    /// <typeparam name="TLeft">The LHS type of the synchronization rule</typeparam>
    /// <typeparam name="TRight">The RHS type of the synchronization rule</typeparam>
    public abstract class SynchronizationRule<TLeft, TRight> : SynchronizationRuleBase
    {
        /// <summary>
        /// Gets the jobs performed by this synchronization rule
        /// </summary>
        public ICollection<ISynchronizationJob<TLeft, TRight>> SynchronizationJobs { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected SynchronizationRule()
        {
            leftToRight = new LeftToRightRule<TLeft, TRight>( this );
            rightToLeft = new RightToLeftRule<TLeft, TRight>( this );
            SynchronizationJobs = new List<ISynchronizationJob<TLeft, TRight>>();
        }

        /// <summary>
        /// Gets the transformation rule responsible of transforming elements from left to right
        /// </summary>
        public TransformationRuleBase<TLeft, TRight> LeftToRight { get { return leftToRight; } }

        /// <summary>
        /// Gets the transformation rule responsible of transforming elements from right to left
        /// </summary>
        public TransformationRuleBase<TRight, TLeft> RightToLeft { get { return rightToLeft; } }

        private readonly LeftToRightRule<TLeft, TRight> leftToRight;
        private readonly RightToLeftRule<TLeft, TRight> rightToLeft;

        private static readonly Type leftImplementationType = GetImplementationType(typeof(TLeft));
        private static readonly Type rightImplementationType = GetImplementationType(typeof(TRight));

        private static Type GetImplementationType( Type type )
        {
            if(!type.IsAbstract && !type.IsInterface) return type;

            var customs = type.GetCustomAttributes( typeof( DefaultImplementationTypeAttribute ), false );
            if(customs != null && customs.Length > 0)
            {
                var defaultImplAtt = customs[0] as DefaultImplementationTypeAttribute;
                return defaultImplAtt.DefaultImplementationType;
            }
            return null;
        }

        internal override GeneralTransformationRule LTR
        {
            get { return LeftToRight; }
        }

        internal override GeneralTransformationRule RTL
        {
            get { return RightToLeft; }
        }

        /// <summary>
        /// Gets or sets the transformation delay level
        /// </summary>
        public byte TransformationDelayLevel
        {
            get
            {
                return Math.Max( LTR.TransformationDelayLevel, RTL.TransformationDelayLevel );
            }
            set
            {
                leftToRight.SetTransformationDelay( value );
                rightToLeft.SetTransformationDelay( value );
            }
        }

        /// <summary>
        /// Gets or sets the output delay level of the transformations
        /// </summary>
        public byte OutputDelayLevel
        {
            get
            {
                return Math.Max( LTR.OutputDelayLevel, RTL.OutputDelayLevel );
            }
            set
            {
                leftToRight.SetOutputDelay( value );
                rightToLeft.SetOutputDelay( value );
            }
        }

        internal void InitializeOutput( SynchronizationComputation<TLeft, TRight> computation )
        {
            var context = computation.SynchronizationContext;
            var dependencies = computation.Dependencies;
            foreach(var job in SynchronizationJobs.Where( j => j.IsEarly ))
            {
                var dependency = job.Perform( computation, context.Direction, context );
                if(dependency != null)
                {
                    dependencies.Add( dependency );
                }
            }
        }

        internal void Synchronize( SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context )
        {
            var dependencies = computation.Dependencies;
            foreach(var job in SynchronizationJobs.Where( j => !j.IsEarly ))
            {
                var dependency = job.Perform( computation, direction, context );
                if(dependency != null)
                {
                    dependencies.Add( dependency );
                }
            }
        }

        /// <summary>
        /// Determines whether a correspondence shoulf be established between the given LHS and RHS elements
        /// </summary>
        /// <param name="left">The LHS element</param>
        /// <param name="right">The RHS element</param>
        /// <param name="context">The context in which the synchronization is run</param>
        /// <returns>True, if the elements should be regarded as corresponding to each other, otherwise false</returns>
        public virtual bool ShouldCorrespond( TLeft left, TRight right, ISynchronizationContext context )
        {
            foreach (var instantiation in _instantiations)
            {
                var innerResult = instantiation.rule.ShouldCorrespondInternal(left, right, instantiation.leftPredicate, instantiation.rightPredicate, context);
                if (innerResult.HasValue)
                {
                    return innerResult.Value;
                }
            }
            return false;
        }

        internal override bool? ShouldCorrespondInternal(object left, object right, object leftPredicate, object rightPredicate, ISynchronizationContext context)
        {
            if (left is TLeft leftElement && right is TRight rightElement)
            {
                if (leftPredicate is ObservingFunc<TLeft, bool> leftFunc && !leftFunc.Evaluate(leftElement))
                {
                    return null;
                }
                if (rightPredicate is ObservingFunc<TRight, bool> rightFunc && !rightFunc.Evaluate(rightElement))
                {
                    return null; 
                }
                return ShouldCorrespond(leftElement, rightElement, context);
            }
            return null;
        }

        /// <summary>
        /// Gets the LHS type of this rule
        /// </summary>
        public sealed override Type LeftType
        {
            get { return typeof( TLeft ); }
        }

        /// <summary>
        /// Gets the RHS type of this rule
        /// </summary>
        public sealed override Type RightType
        {
            get { return typeof( TRight ); }
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            if(rule == null) throw new ArgumentNullException( nameof(rule));
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector, false, false);
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression( guard );
            LeftToRight.Dependencies.Add( dependency.CreateLeftToRightDependency( guardFunc ) );
            RightToLeft.Dependencies.Add( dependency.CreateRightToLeftDependency( guardFunc ) );
        }


        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="leftSetter">An alternative LHS lens put</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Action<TLeft, TDepLeft> leftSetter, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ( left, ctx, leftValue ) => leftSetter( left, leftValue ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="leftSetter">An alternative LHS lens put</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            if(rule == null) throw new ArgumentNullException( nameof(rule));
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector, leftSetter, null, false, false);
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression( guard );
            LeftToRight.Dependencies.Add( dependency.CreateLeftToRightDependency( guardFunc ) );
            RightToLeft.Dependencies.Add( dependency.CreateRightToLeftDependency( guardFunc ) );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="rightSetter">An alternative RHS lens put</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Action<TRight, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            return Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), ( r, ctx, value ) => rightSetter( r, value ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="rightSetter">An alternative RHS lens put</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Action<TRight, ITransformationContext, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector, null, rightSetter, false, false);
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression(guard);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency(guardFunc));
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency(guardFunc));
            return dependency;
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="leftSetter">An alternative LHS lens put</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="rightSetter">An alternative RHS lens put</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Action<TLeft, TDepLeft> leftSetter, Expression<Func<TRight, TDepRight>> rightSelector, Action<TRight, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            return Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ( left, ctx, leftValue ) => leftSetter( left, leftValue ), ExpressionHelper.AddContextParameter( rightSelector ), ( r, ctx, value ) => rightSetter( r, value ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="leftSetter">An alternative LHS lens put</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="rightSetter">An alternative RHS lens put</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Action<TRight, ITransformationContext, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector, leftSetter, rightSetter, false, false);
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression(guard);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency(guardFunc));
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency(guardFunc));
            return dependency;
        }


        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Left to Right
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TDepLeft, TDepRight> SynchronizeLeftToRightOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TLeft, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            return SynchronizeLeftToRightOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Left to Right
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TDepLeft, TDepRight> SynchronizeLeftToRightOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TLeft, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector, true, false);
            var guardFunc = ObservingFunc<TLeft, bool>.FromExpression(guard);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightOnlyDependency(guardFunc));
            return dependency;
        }


        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Right to Left
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TDepLeft, TDepRight> SynchronizeRightToLeftOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            return SynchronizeRightToLeftOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Right to Left
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TDepLeft, TDepRight> SynchronizeRightToLeftOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TRight, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector, false, true);
            var guardFunc = ObservingFunc<TRight, bool>.FromExpression(guard);
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftOnlyDependency(guardFunc));
            return dependency;
        }


        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> SynchronizeMany<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ICollectionExpression<TDepRight>>> rightSelector )
            where TDepLeft : class
            where TDepRight : class
        {
            return SynchronizeMany( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> SynchronizeMany<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, ICollectionExpression<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>(rule, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
            return dependency;
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Left to Right
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TDepLeft, TDepRight> SynchronizeManyLeftToRightOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, IEnumerableExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ICollection<TDepRight>>> rightSelector )
            where TDepLeft : class
            where TDepRight : class
        {
            return SynchronizeManyLeftToRightOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Left to Right
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TDepLeft, TDepRight> SynchronizeManyLeftToRightOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, IEnumerableExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, ICollection<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new OneWaySynchronizationMultipleDependencyLTR<TLeft, TRight, TDepLeft, TDepRight>(rule.LeftToRight, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency);
            return dependency;
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Right to Left
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TDepLeft, TDepRight> SynchronizeManyRightToLeftOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ICollection<TDepLeft>>> leftSelector, Expression<Func<TRight, IEnumerableExpression<TDepRight>>> rightSelector )
            where TDepLeft : class
            where TDepRight : class
        {
            return SynchronizeManyRightToLeftOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        /// <summary>
        /// Synchronizes the dependent elements using the provided synchronization rule but only Right to Left
        /// </summary>
        /// <typeparam name="TDepLeft">The dependent LHS type</typeparam>
        /// <typeparam name="TDepRight">The dependent RHS type</typeparam>
        /// <param name="rule">The rule that should be used as isomorphism</param>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TDepLeft, TDepRight> SynchronizeManyRightToLeftOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, ICollection<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, IEnumerableExpression<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var dependency = new OneWaySynchronizationMultipleDependencyRTL<TRight, TLeft, TDepRight, TDepLeft>(rule.RightToLeft, rightSelector, leftSelector);
            RightToLeft.Dependencies.Add(dependency);
            return dependency;
        }

        /// <summary>
        /// Synchronizes the dependent values late
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> SynchronizeLate<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            var job = new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> Synchronize<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            var job = new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> Synchronize<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard)
        {
            var job = new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, false);
            if (guard == null)
            {
                SynchronizationJobs.Add(job);
            }
            else
            {
                SynchronizationJobs.Add(new BothGuardedSynchronizationJob<TLeft, TRight>(job, guard));
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> SynchronizeMany<TValue>(Func<TLeft, ICollectionExpression<TValue>> leftSelector, Func<TRight, ICollectionExpression<TValue>> rightSelector)
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new CollectionSynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> SynchronizeMany<TValue>( Func<TLeft, ICollectionExpression<TValue>> leftSelector, Func<TRight, ICollectionExpression<TValue>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new CollectionSynchronizationJob<TLeft, TRight, TValue>( leftSelector, rightSelector, true );
            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new BothGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> SynchronizeManyLate<TValue>( Func<TLeft, ICollectionExpression<TValue>> leftSelector, Func<TRight, ICollectionExpression<TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new CollectionSynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> SynchronizeManyLate<TValue>( Func<TLeft, ICollectionExpression<TValue>> leftSelector, Func<TRight, ICollectionExpression<TValue>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new CollectionSynchronizationJob<TLeft, TRight, TValue>( leftSelector, rightSelector, false );
            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new BothGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeManyLeftToRightOnly<TValue>( Func<TLeft, IEnumerableExpression<TValue>> leftSelector, Func<TRight, ICollection<TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new LeftToRightCollectionSynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftSelector), ExpressionHelper.AddContextParameter(rightSelector), true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeManyLeftToRightOnly<TValue>( Func<TLeft, ITransformationContext, IEnumerableExpression<TValue>> leftSelector, Func<TRight, ITransformationContext, ICollection<TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new LeftToRightCollectionSynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeManyLeftToRightOnly<TValue>( Func<TLeft, IEnumerableExpression<TValue>> leftSelector, Func<TRight, ICollection<TValue>> rightSelector, Expression<Func<TLeft, bool>> guard )
        {
            return SynchronizeManyLeftToRightOnly( ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeManyLeftToRightOnly<TValue>( Func<TLeft, ITransformationContext, IEnumerableExpression<TValue>> leftSelector, Func<TRight, ITransformationContext, ICollection<TValue>> rightSelector, Expression<Func<TLeft, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            guard = SimplifyPredicate( guard );

            var job = new LeftToRightCollectionSynchronizationJob<TLeft, TRight, TValue>( leftSelector, rightSelector, true );

            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new LeftGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeManyLeftToRightOnlyLate<TValue>( Func<TLeft, IEnumerableExpression<TValue>> leftSelector, Func<TRight, ICollection<TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new LeftToRightCollectionSynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftSelector), ExpressionHelper.AddContextParameter(rightSelector), false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeManyLeftToRightOnlyLate<TValue>( Func<TLeft, IEnumerableExpression<TValue>> leftSelector, Func<TRight, ICollection<TValue>> rightSelector, Expression<Func<TLeft, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            guard = SimplifyPredicate( guard );

            var job = new LeftToRightCollectionSynchronizationJob<TLeft, TRight, TValue>( ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), false );

            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new LeftGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeManyRightToLeftOnly<TValue>( Func<TLeft, ICollection<TValue>> leftSelector, Func<TRight, IEnumerableExpression<TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new RightToLeftCollectionSynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(rightSelector), ExpressionHelper.AddContextParameter(leftSelector), true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeManyRightToLeftOnly<TValue>( Func<TLeft, ICollection<TValue>> leftSelector, Func<TRight, IEnumerableExpression<TValue>> rightSelector, Expression<Func<TRight, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            guard = SimplifyPredicate( guard );

            var job = new RightToLeftCollectionSynchronizationJob<TLeft, TRight, TValue>( ExpressionHelper.AddContextParameter( rightSelector ), ExpressionHelper.AddContextParameter( leftSelector ), true );

            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new RightGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeManyRightToLeftOnlyLate<TValue>( Func<TLeft, ICollection<TValue>> leftSelector, Func<TRight, IEnumerableExpression<TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var job = new RightToLeftCollectionSynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(rightSelector), ExpressionHelper.AddContextParameter(leftSelector), false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeManyRightToLeftOnlyLate<TValue>( Func<TLeft, ICollection<TValue>> leftSelector, Func<TRight, IEnumerableExpression<TValue>> rightSelector, Expression<Func<TRight, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            guard = SimplifyPredicate( guard );

            var job = new RightToLeftCollectionSynchronizationJob<TLeft, TRight, TValue>( ExpressionHelper.AddContextParameter( rightSelector ), ExpressionHelper.AddContextParameter( leftSelector ), false );

            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new RightGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var rightSetter = SetExpressionRewriter.CreateSetter(rightSelector);

            if (rightSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), nameof(rightSelector));

            return SynchronizeLeftToRightOnly( ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>( Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector )
        {
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var rightSetter = SetExpressionRewriter.CreateSetter( rightSelector );

            if(rightSetter == null) throw new ArgumentException( string.Format( "The expression {0} cannot be inverted.", rightSelector ), nameof(rightSelector));

            return SynchronizeLeftToRightOnly( leftSelector, rightSelector.Compile(), rightSetter.Compile() );
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Action<TRight, TValue> rightSetter)
        {
            return SynchronizeLeftToRightOnly(leftSelector, null, rightSetter);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightGetter">A RHS getter</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Func<TRight, TValue> rightGetter, Action<TRight, TValue> rightSetter)
        {
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSetter == null) throw new ArgumentNullException(nameof(rightSetter));

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftSelector), ExpressionHelper.AddContextParameter(rightGetter), (r, c, v) => rightSetter(r, v), true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Action<TRight, ITransformationContext, TValue> rightSetter)
        {
            return SynchronizeLeftToRightOnly(leftSelector, null, rightSetter);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightGetter">A RHS getter</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>( Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Func<TRight, ITransformationContext, TValue> rightGetter, Action<TRight, ITransformationContext, TValue> rightSetter )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSetter == null) throw new ArgumentNullException( nameof(rightSetter));

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightGetter, rightSetter, true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLateLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var rightSetter = SetExpressionRewriter.CreateSetter(rightSelector);

            if (rightSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), nameof(rightSelector));

            return SynchronizeLateLeftToRightOnly(leftSelector, rightSelector.Compile(), rightSetter.Compile());
        }


        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLateLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Action<TRight, TValue> rightSetter)
        {
            return SynchronizeLateLeftToRightOnly(leftSelector, null, rightSetter);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightGetter">A RHS getter</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLateLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Func<TRight, TValue> rightGetter, Action<TRight, TValue> rightSetter)
        {
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSetter == null) throw new ArgumentNullException(nameof(rightSetter));

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftSelector), ExpressionHelper.AddContextParameter(rightGetter), (r, c, v) => rightSetter(r, v), false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLateLeftToRightOnly<TValue>(Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Action<TRight, ITransformationContext, TValue> rightSetter)
        {
            return SynchronizeLateLeftToRightOnly(leftSelector, null, rightSetter);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightGetter">A RHS getter</param>
        /// <param name="rightSetter">A RHS setter</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLateLeftToRightOnly<TValue>( Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Func<TRight, ITransformationContext, TValue> rightGetter, Action<TRight, ITransformationContext, TValue> rightSetter )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSetter == null) throw new ArgumentNullException( nameof(rightSetter));

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightGetter, rightSetter, false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TLeft, bool>> guard)
        {
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            var rightSetter = SetExpressionRewriter.CreateSetter(rightSelector);

            if (rightSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), nameof(rightSelector));

            return SynchronizeLeftToRightOnly(leftSelector, rightSelector.Compile(), rightSetter.Compile(), guard);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSetter">A RHS setter</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Action<TRight, TValue> rightSetter, Expression<Func<TLeft, bool>> guard)
        {
            return SynchronizeLeftToRightOnly(leftSelector, null, rightSetter, guard);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightGetter">A RHS getter</param>
        /// <param name="rightSetter">A RHS setter</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>( Expression<Func<TLeft, TValue>> leftSelector, Func<TRight, TValue> rightGetter, Action<TRight, TValue> rightSetter, Expression<Func<TLeft, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSetter == null) throw new ArgumentNullException( nameof(rightSetter));

            guard = SimplifyPredicate( guard );

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>( ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter(rightGetter), ( r, c, v ) => rightSetter( r, v ), false );

            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new LeftGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSetter">A RHS setter</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Action<TRight, ITransformationContext, TValue> rightSetter, Expression<Func<TLeft, bool>> guard)
        {
            return SynchronizeLeftToRightOnly(leftSelector, null, rightSetter, guard);
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightGetter">A RHS getter</param>
        /// <param name="rightSetter">A RHS setter</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Func<TRight, ITransformationContext, TValue> rightGetter, Action<TRight, ITransformationContext, TValue> rightSetter, Expression<Func<TLeft, bool>> guard)
        {
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSetter == null) throw new ArgumentNullException(nameof(rightSetter));

            guard = SimplifyPredicate(guard);

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightGetter, rightSetter, false);

            if (guard == null)
            {
                SynchronizationJobs.Add(job);
            }
            else
            {
                SynchronizationJobs.Add(new LeftGuardedSynchronizationJob<TLeft, TRight>(job, guard));
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only left to right
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> SynchronizeLeftToRightOnly<TValue>( Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector, Expression<Func<TLeft, bool>> guard )
        {
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

            var rightSetter = SetExpressionRewriter.CreateSetter( rightSelector );

            if(rightSetter == null) throw new ArgumentException( string.Format( "The expression {0} cannot be inverted.", rightSelector ), nameof(rightSelector));

            return SynchronizeLeftToRightOnly( leftSelector, rightSelector.Compile(), rightSetter.Compile(), guard );
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));

            var leftSetter = SetExpressionRewriter.CreateSetter(leftSelector);

            if (leftSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), nameof(leftSelector));

            return SynchronizeRightToLeftOnly(leftSelector.Compile(), leftSetter.Compile(), rightSelector);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector)
        {
            return SynchronizeRightToLeftOnly(null, leftSetter, rightSelector);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftGetter">A LHS getter</param>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Func<TLeft, TValue> leftGetter, Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));
            if (leftSetter == null) throw new ArgumentNullException(nameof(leftSetter));

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftGetter), (l, c, v) => leftSetter(l, v), ExpressionHelper.AddContextParameter(rightSelector), true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector)
        {
            return SynchronizeRightToLeftOnly(null, leftSetter, rightSelector);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftGetter">A LHS getter</param>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>( Func<TLeft, ITransformationContext, TValue> leftGetter, Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector )
        {
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));
            if(leftSetter == null) throw new ArgumentNullException( nameof(leftSetter));

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(leftGetter, leftSetter, rightSelector, true);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeLateRightToLeftOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));

            var leftSetter = SetExpressionRewriter.CreateSetter(leftSelector);

            if (leftSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), nameof(rightSelector));

            return SynchronizeLateRightToLeftOnly(leftSelector.Compile(), leftSetter.Compile(), rightSelector);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeLateRightToLeftOnly<TValue>( Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));

            var leftSetter = SetExpressionRewriter.CreateSetter( leftSelector );

            if(leftSetter == null) throw new ArgumentException( string.Format( "The expression {0} cannot be inverted.", rightSelector ), nameof(rightSelector));

            return SynchronizeLateRightToLeftOnly( leftSelector.Compile(), leftSetter.Compile(), rightSelector );
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeLateRightToLeftOnly<TValue>(Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector)
        {
            return SynchronizeLateRightToLeftOnly(null, leftSetter, rightSelector);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftGetter">A LHS getter</param>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeLateRightToLeftOnly<TValue>(Func<TLeft, TValue> leftGetter, Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));
            if (leftSetter == null) throw new ArgumentNullException(nameof(leftSetter));

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftGetter), (l, c, v) => leftSetter(l, v), ExpressionHelper.AddContextParameter(rightSelector), false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeLateRightToLeftOnly<TValue>(Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector)
        {
            return SynchronizeLateRightToLeftOnly(null, leftSetter, rightSelector);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftGetter">A LHS getter</param>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeLateRightToLeftOnly<TValue>(Func<TLeft, ITransformationContext, TValue> leftGetter, Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector )
        {
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));
            if(leftSetter == null) throw new ArgumentNullException( nameof(leftSetter));

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(leftGetter, leftSetter, rightSelector, false);
            SynchronizationJobs.Add(job);
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TRight, bool>> guard)
        {
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));

            var leftSetter = SetExpressionRewriter.CreateSetter(leftSelector);

            if (leftSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), nameof(rightSelector));

            return SynchronizeRightToLeftOnly(leftSelector.Compile(), leftSetter.Compile(), rightSelector, guard);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TRight, bool>> guard)
        {
            return SynchronizeRightToLeftOnly(null, leftSetter, rightSelector, guard);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftGetter">A LHS getter</param>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Func<TLeft, TValue> leftGetter, Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TRight, bool>> guard)
        {
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));
            if (leftSetter == null) throw new ArgumentNullException(nameof(leftSetter));

            guard = SimplifyPredicate(guard);

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(ExpressionHelper.AddContextParameter(leftGetter), ( l, c, v ) => leftSetter( l, v ), ExpressionHelper.AddContextParameter( rightSelector ), false);

            if (guard == null)
            {
                SynchronizationJobs.Add(job);
            }
            else
            {
                SynchronizationJobs.Add(new RightGuardedSynchronizationJob<TLeft, TRight>(job, guard));
            }
            return job;
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSelector">The LHS in-model lens</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>( Expression<Func<TLeft, ITransformationContext, TValue>> leftSelector, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector, Expression<Func<TRight, bool>> guard )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));

            var leftSetter = SetExpressionRewriter.CreateSetter( leftSelector );

            if(leftSetter == null) throw new ArgumentException( string.Format( "The expression {0} cannot be inverted.", rightSelector ), nameof(rightSelector));

            return SynchronizeRightToLeftOnly( leftSelector.Compile(), leftSetter.Compile(), rightSelector, guard );
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector, Expression<Func<TRight, bool>> guard)
        {
            return SynchronizeRightToLeftOnly(null, leftSetter, rightSelector, guard);
        }

        /// <summary>
        /// Synchronizes the dependent values but only right to left
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftGetter">A LHS getter</param>
        /// <param name="leftSetter">A LHS setter</param>
        /// <param name="rightSelector">The RHS in-model lens</param>
        /// <param name="guard">A guard condition or null</param>
        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> SynchronizeRightToLeftOnly<TValue>(Func<TLeft, ITransformationContext, TValue> leftGetter, Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightSelector, Expression<Func<TRight, bool>> guard )
        {
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));
            if(leftSetter == null) throw new ArgumentNullException( nameof(leftSetter));

            guard = SimplifyPredicate( guard );

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(leftGetter, leftSetter, rightSelector, false );

            if(guard == null)
            {
                SynchronizationJobs.Add( job );
            }
            else
            {
                SynchronizationJobs.Add( new RightGuardedSynchronizationJob<TLeft, TRight>( job, guard ) );
            }
            return job;
        }

        /// <summary>
        /// Executes the given action when a correspondence between LHS and RHS elements is established
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <param name="isEarly">True, if the synchronization job should be performed before dependencies, otherwise False</param>
        public void SynchronizeOpaque(Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> action, bool isEarly = false)
        {
            SynchronizationJobs.Add(new OpaqueSynchronizationJob<TLeft, TRight>(action, isEarly));
        }

        /// <summary>
        /// Executes the given action when a correspondence between LHS and RHS elements is established
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <param name="isEarly">True, if the synchronization job should be performed before dependencies, otherwise False</param>
        public void SynchronizeOpaque( Action<TLeft, TRight, SynchronizationDirection, ISynchronizationContext> action, bool isEarly = false )
        {
            SynchronizationJobs.Add( new OpaqueSynchronizationJob<TLeft, TRight>( (left, right, direction, context) =>
            {
                action?.Invoke( left, right, direction, context );
                return null;
            }, isEarly ) );
        }

        internal object CreateRightOutputInternal(TLeft input, IEnumerable candidates, ISynchronizationContext context, out bool existing)
        {
            if (candidates is Axiom ax)
            {
                existing = true;
                return ax.Object;
            }
            return CreateRightOutput(input, candidates != null ? candidates.OfType<TRight>() : null, context, out existing);
        }

        internal object CreateLeftOutputInternal(TRight input, IEnumerable candidates, ISynchronizationContext context, out bool existing)
        {
            if (candidates is Axiom ax)
            {
                existing = true;
                return ax.Object;
            }
            return CreateLeftOutput(input, candidates != null ? candidates.OfType<TLeft>() : null, context, out existing);
        }

        /// <summary>
        /// Creates the RHS output
        /// </summary>
        /// <param name="input">The corresponding LHS element</param>
        /// <param name="candidates">Candidates for the RHS element or null</param>
        /// <param name="context">The synchronization context</param>
        /// <param name="existing">True, if an existing element is returned, otherwise False</param>
        /// <returns>The RHS element</returns>
        protected virtual TRight CreateRightOutput(TLeft input, IEnumerable<TRight> candidates, ISynchronizationContext context, out bool existing)
        {
            existing = false;
            if (candidates != null)
            {
                var inputArray = new object[1];
                foreach (var item in candidates)
                {
                    inputArray[0] = item;
                    if (item != null && !context.Trace.TraceIn(rightToLeft, inputArray).Any() && ShouldCorrespond(input, item, context))
                    {
                        existing = true;
                        return item;
                    }
                }
            }
            return CreateRightOutputInstance();
        }

        private static TRight CreateRightOutputInstance()
        {
            TRight output;
            if (rightImplementationType != null)
            {
                output = (TRight)Activator.CreateInstance(rightImplementationType);
            }
            else
            {
                throw new NotImplementedException();
            }

            return output;
        }

        /// <summary>
        /// Creates the LHS output
        /// </summary>
        /// <param name="input">The corresponding RHS element</param>
        /// <param name="candidates">Candidates for the LHS element or null</param>
        /// <param name="context">The synchronization context</param>
        /// <param name="existing">True, if an existing element is returned, otherwise False</param>
        /// <returns>The LHS element</returns>
        protected virtual TLeft CreateLeftOutput(TRight input, IEnumerable<TLeft> candidates, ISynchronizationContext context, out bool existing)
        {
            existing = false;
            if (candidates != null)
            {
                var inputArray = new object[1];
                foreach (var item in candidates)
                {
                    inputArray[0] = item;
                    if (item != null && !context.Trace.TraceIn(rightToLeft, inputArray).Any() && ShouldCorrespond(item, input, context))
                    {
                        existing = true;
                        return item;
                    }
                }
            }
            return CreateLeftOutputInstance();
        }
        
        internal void SynchronizeCollectionsRightToLeft(object leftElement, object rightElement, ICollection<TLeft> lefts, ICollection<TRight> rights, ISynchronizationContext context, bool ignoreCandidates, IInconsistencyDescriptor<object, object, object, object> descriptor)
        {
            if (context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections(leftElement, rightElement, lefts, rights, context, descriptor);
                return;
            }
            if (lefts.IsReadOnly) throw new InvalidOperationException("Collection is read-only!");
            IEnumerable<TLeft> leftsSaved;
            HashSet<TLeft> doubles;
            if (context.Direction == SynchronizationDirection.RightToLeft)
            {
                leftsSaved = null;
                doubles = null;
            }
            else
            {
                leftsSaved = lefts.ToArray();
                doubles = new HashSet<TLeft>();
            }
            IEnumerable leftContext = ignoreCandidates ? null : lefts;
            foreach (var item in rights)
            {
                MatchRightItem(lefts, context, doubles, leftContext, item);
            }
            if (context.Direction == SynchronizationDirection.RightWins)
            {
                AddMissingItemsToRights(rights, context, leftsSaved, doubles);
            }
            else if (context.Direction == SynchronizationDirection.RightToLeftForced)
            {
                RemoveUnmatchedItemsFromLefts(lefts, leftsSaved, doubles);
            }
        }

        private void MatchRightItem(ICollection<TLeft> lefts, ISynchronizationContext context, HashSet<TLeft> doubles, IEnumerable leftContext, TRight item)
        {
            var comp = context.CallTransformation(RightToLeft, new object[] { item }, leftContext) as SynchronizationComputation<TRight, TLeft>;
            comp.DoWhenOutputIsAvailable((inp, outp) =>
            {
                if (!lefts.Contains(outp))
                {
                    lefts.Add(outp);
                }
                else if (context.Direction != SynchronizationDirection.RightToLeft)
                {
                    doubles.Add(outp);
                }
            });
        }

        private static void RemoveUnmatchedItemsFromLefts(ICollection<TLeft> lefts, IEnumerable<TLeft> leftsSaved, HashSet<TLeft> doubles)
        {
            foreach (var item in leftsSaved.Except(doubles))
            {
                lefts.Remove(item);
            }
        }

        private void AddMissingItemsToRights(ICollection<TRight> rights, ISynchronizationContext context, IEnumerable<TLeft> leftsSaved, HashSet<TLeft> doubles)
        {
            foreach (var item in leftsSaved.Except(doubles))
            {
                var comp = context.CallTransformation(LeftToRight, new object[] { item }, null) as SynchronizationComputation<TLeft, TRight>;
                comp.DoWhenOutputIsAvailable((inp, outp) =>
                {
                    rights.Add(outp);
                });
            }
        }

        internal void SynchronizeCollectionsLeftToRight(object leftElement, object rightElement, ICollection<TRight> rights, ICollection<TLeft> lefts, ISynchronizationContext context, bool ignoreCandidates, IInconsistencyDescriptor<object, object, object, object> descriptor)
        {
            if (context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections(leftElement, rightElement, lefts, rights, context, descriptor);
                return;
            }
            if (rights.IsReadOnly) throw new InvalidOperationException("Collection is read-only!");
            IEnumerable<TRight> rightsSaved;
            HashSet<TRight> doubles;
            if (context.Direction == SynchronizationDirection.LeftToRight)
            {
                rightsSaved = null;
                doubles = null;
            }
            else
            {
                rightsSaved = rights.ToArray();
                doubles = new HashSet<TRight>();
            }
            IEnumerable rightContext = ignoreCandidates ? null : rights;
            foreach (var item in lefts)
            {
                MatchLeftItem(rights, context, doubles, rightContext, item);
            }
            if (context.Direction == SynchronizationDirection.LeftWins)
            {
                AddMissingItemsToLefts(lefts, context, rightsSaved, doubles);
            }
            else if (context.Direction == SynchronizationDirection.LeftToRightForced)
            {
                RemoveUnmatchedItemsFromRight(rights, rightsSaved, doubles);
            }
        }

        private void MatchLeftItem(ICollection<TRight> rights, ISynchronizationContext context, HashSet<TRight> doubles, IEnumerable rightContext, TLeft item)
        {
            var comp = context.CallTransformation(LeftToRight, new object[] { item }, rightContext) as SynchronizationComputation<TLeft, TRight>;
            comp.DoWhenOutputIsAvailable((inp, outp) =>
            {
                if (!rights.Contains(outp))
                {
                    rights.Add(outp);
                }
                else if (context.Direction != SynchronizationDirection.LeftToRight)
                {
                    doubles.Add(outp);
                }
            });
        }

        private static void RemoveUnmatchedItemsFromRight(ICollection<TRight> rights, IEnumerable<TRight> rightsSaved, HashSet<TRight> doubles)
        {
            foreach (var item in rightsSaved.Except(doubles))
            {
                rights.Remove(item);
            }
        }

        private void AddMissingItemsToLefts(ICollection<TLeft> lefts, ISynchronizationContext context, IEnumerable<TRight> rightsSaved, HashSet<TRight> doubles)
        {
            foreach (var item in rightsSaved.Except(doubles))
            {
                var comp = context.CallTransformation(RightToLeft, new object[] { item }, null) as SynchronizationComputation<TRight, TLeft>;
                comp.DoWhenOutputIsAvailable((inp, outp) =>
                {
                    lefts.Add(outp);
                });
            }
        }

        private void MatchCollections(object leftElement, object rightElement, ICollection<TLeft> lefts, ICollection<TRight> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, object, object> descriptor)
        {
            var rightsRemaining = new HashSet<TRight>(rights);
            foreach (var left in lefts)
            {
                var right = default(TRight);
                var found = false;
                foreach (var item in rightsRemaining)
                {
                    if (ShouldCorrespond(left, item, context))
                    {
                        right = item;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    // call rule in order to establish trace entry and determine inner differences
                    context.CallTransformation(LeftToRight, new object[] { left }, new Axiom(right));
                    rightsRemaining.Remove(right);
                }
                else
                {
                    context.Inconsistencies.Add(new MissingItemInconsistency<TLeft, TRight>(leftElement, rightElement, descriptor, context, LeftToRight, lefts, rights, left, false));
                }
            }
            foreach (var item in rightsRemaining)
            {
                context.Inconsistencies.Add(new MissingItemInconsistency<TRight, TLeft>(rightElement, leftElement, descriptor, context, RightToLeft, rights, lefts, item, true));
            }
        }

        private static TLeft CreateLeftOutputInstance()
        {
            if (leftImplementationType != null)
            {
                return (TLeft)Activator.CreateInstance(leftImplementationType);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Marks this synchronization rule instantiating for the given other synchronization rule
        /// </summary>
        /// <param name="synchronizationRule">The other synchronization rule with more abstract LHS and RHS types</param>
        /// <param name="leftPredicate">A filter function on the LHS when this instantiation applies or null</param>
        /// <param name="rightPredicate">A filter function on the RHS when this instantiation applies or null</param>
        public void MarkInstantiatingFor(SynchronizationRuleBase synchronizationRule, Expression<Func<TLeft, bool>> leftPredicate = null, Expression<Func<TRight, bool>> rightPredicate = null)
        {
            if (synchronizationRule == null) throw new ArgumentNullException(nameof(synchronizationRule));

            if (!synchronizationRule.LeftType.IsAssignableFrom(typeof(TLeft)))
            {
                throw new ArgumentException("The left types do not conform. The left type of the current rule must be an assignable of the given synchronization rules left type.", nameof(synchronizationRule));
            }
            if (!synchronizationRule.RightType.IsAssignableFrom(typeof(TRight)))
            {
                throw new ArgumentException("The right types do not conform. The right type of the current rule must be an assignable of the given synchronization rules right type.", nameof(synchronizationRule));
            }
            leftPredicate = SimplifyPredicate(leftPredicate);
            rightPredicate = SimplifyPredicate(rightPredicate);

            ObservingFunc<TLeft, bool> leftFunc = ObservingFunc<TLeft, bool>.FromExpression(leftPredicate);
            ObservingFunc<TRight, bool> rightFunc = ObservingFunc<TRight, bool>.FromExpression(rightPredicate);

            if (leftFunc != null)
            {
                LeftToRight.MarkInstantiatingFor(synchronizationRule.LTR, leftFunc.Evaluate);
                SynchronizationJobs.Add(new LeftInstantiationMonitorJob<TLeft, TRight>(leftFunc));
            }
            else
            {
                LeftToRight.MarkInstantiatingFor(synchronizationRule.LTR);
            }
            if (rightFunc != null)
            {
                RightToLeft.MarkInstantiatingFor(synchronizationRule.RTL, rightFunc.Evaluate);
                SynchronizationJobs.Add(new RightInstantiationMonitorJob<TLeft, TRight>(rightFunc));
            }
            else
            {
                RightToLeft.MarkInstantiatingFor(synchronizationRule.RTL);
            }
            synchronizationRule._instantiations.Add((this, leftFunc, rightFunc));
        }

        private static Expression<Func<T, bool>> SimplifyPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                var body = predicate.Body;
                while (body.CanReduce)
                {
                    body = body.Reduce();
                }
                predicate.Update(body, predicate.Parameters);
                if (predicate.Body.NodeType == ExpressionType.Constant)
                {
                    var constant = (ConstantExpression)predicate.Body;
                    if ((bool)constant.Value)
                    {
                        return null;
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("The predicate {0} is always false", predicate));
                    }
                }
            }

            return predicate;
        }

    }
}

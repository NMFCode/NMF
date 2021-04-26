using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public abstract class SynchronizationRule<TLeft, TRight> : SynchronizationRuleBase
        where TLeft : class
        where TRight : class
    {
        public ICollection<ISynchronizationJob<TLeft, TRight>> SynchronizationJobs { get; private set; }

        public SynchronizationRule()
        {
            leftToRight = new LeftToRightRule<TLeft, TRight>( this );
            rightToLeft = new RightToLeftRule<TLeft, TRight>( this );
            SynchronizationJobs = new List<ISynchronizationJob<TLeft, TRight>>();
        }

        public TransformationRuleBase<TLeft, TRight> LeftToRight { get { return leftToRight; } }

        public TransformationRuleBase<TRight, TLeft> RightToLeft { get { return rightToLeft; } }

        private LeftToRightRule<TLeft, TRight> leftToRight;
        private RightToLeftRule<TLeft, TRight> rightToLeft;

        private static Type leftImplementationType = GetImplementationType( typeof( TLeft ) );
        private static Type rightImplementationType = GetImplementationType( typeof( TRight ) );

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

        public virtual bool ShouldCorrespond( TLeft left, TRight right, ISynchronizationContext context )
        {
            return false;
        }

        public sealed override Type LeftType
        {
            get { return typeof( TLeft ); }
        }

        public sealed override Type RightType
        {
            get { return typeof( TRight ); }
        }

        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            if(rule == null) throw new ArgumentNullException( "rule" );
            if(leftSelector == null) throw new ArgumentNullException( "leftSelector" );
            if(rightSelector == null) throw new ArgumentNullException( "rightSelector" );

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>( this, rule, leftSelector, rightSelector );
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression( guard );
            LeftToRight.Dependencies.Add( dependency.CreateLeftToRightDependency( guardFunc ) );
            RightToLeft.Dependencies.Add( dependency.CreateRightToLeftDependency( guardFunc ) );
        }


        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Action<TLeft, TDepLeft> leftSetter, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ( left, ctx, leftValue ) => leftSetter( left, leftValue ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            if(rule == null) throw new ArgumentNullException( "rule" );
            if(leftSelector == null) throw new ArgumentNullException( "leftSelector" );
            if(rightSelector == null) throw new ArgumentNullException( "rightSelector" );

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>( this, rule, leftSelector, rightSelector, leftSetter, null );
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression( guard );
            LeftToRight.Dependencies.Add( dependency.CreateLeftToRightDependency( guardFunc ) );
            RightToLeft.Dependencies.Add( dependency.CreateRightToLeftDependency( guardFunc ) );
        }

        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Action<TRight, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), ( r, ctx, value ) => rightSetter( r, value ), guard );
        }

        public void Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Action<TRight, ITransformationContext, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector, null, rightSetter);
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression(guard);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency(guardFunc));
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency(guardFunc));
        }

        public void Synchronize<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Action<TLeft, TDepLeft> leftSetter, Expression<Func<TRight, TDepRight>> rightSelector, Action<TRight, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            Synchronize( rule, ExpressionHelper.AddContextParameter( leftSelector ), ( left, ctx, leftValue ) => leftSetter( left, leftValue ), ExpressionHelper.AddContextParameter( rightSelector ), ( r, ctx, value ) => rightSetter( r, value ), guard );
        }

        public void Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Action<TRight, ITransformationContext, TDepRight> rightSetter, Expression<Func<TLeft, TRight, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector, leftSetter, rightSetter);
            var guardFunc = ObservingFunc<TLeft, TRight, bool>.FromExpression(guard);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency(guardFunc));
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency(guardFunc));
        }


        public void SynchronizeLeftToRightOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TLeft, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            SynchronizeLeftToRightOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        public void SynchronizeLeftToRightOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TLeft, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            var guardFunc = ObservingFunc<TLeft, bool>.FromExpression(guard);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightOnlyDependency(guardFunc));
        }


        public void SynchronizeRightToLeftOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Expression<Func<TRight, bool>> guard = null )
            where TDepLeft : class
            where TDepRight : class
        {
            SynchronizeRightToLeftOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ), guard );
        }

        public void SynchronizeRightToLeftOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Expression<Func<TRight, bool>> guard = null)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            var guardFunc = ObservingFunc<TRight, bool>.FromExpression(guard);
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftOnlyDependency(guardFunc));
        }


        public void SynchronizeMany<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ICollectionExpression<TDepRight>>> rightSelector )
            where TDepLeft : class
            where TDepRight : class
        {
            SynchronizeMany( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        public void SynchronizeMany<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, ICollectionExpression<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
        }

        public void SynchronizeManyLeftToRightOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, IEnumerableExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ICollection<TDepRight>>> rightSelector )
            where TDepLeft : class
            where TDepRight : class
        {
            SynchronizeManyLeftToRightOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        public void SynchronizeManyLeftToRightOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, IEnumerableExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, ICollection<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new OneWaySynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>(LeftToRight, rule.LeftToRight, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency);
        }

        public void SynchronizeManyRightToLeftOnly<TDepLeft, TDepRight>( SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ICollection<TDepLeft>>> leftSelector, Expression<Func<TRight, IEnumerableExpression<TDepRight>>> rightSelector )
            where TDepLeft : class
            where TDepRight : class
        {
            SynchronizeManyRightToLeftOnly( rule, ExpressionHelper.AddContextParameter( leftSelector ), ExpressionHelper.AddContextParameter( rightSelector ) );
        }

        public void SynchronizeManyRightToLeftOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ITransformationContext, ICollection<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, IEnumerableExpression<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var dependency = new OneWaySynchronizationMultipleDependency<TRight, TLeft, TDepRight, TDepLeft>(RightToLeft, rule.RightToLeft, rightSelector, leftSelector);
            RightToLeft.Dependencies.Add(dependency);
        }

        public void SynchronizeLate<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            SynchronizationJobs.Add(new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, false));
        }

        public void Synchronize<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TLeft, TRight, bool>> guard)
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
        }

        public void Synchronize<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            SynchronizationJobs.Add(new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, true));
        }

        public void SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var rightSetter = SetExpressionRewriter.CreateSetter(rightSelector);

            if (rightSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), "rightSelector");

            SynchronizeLeftToRightOnly(leftSelector, rightSetter.Compile());
        }

        public void SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Action<TRight, TValue> rightSetter)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSetter == null) throw new ArgumentNullException("rightSetter");

            SynchronizationJobs.Add(new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSetter, true));
        }

        public void SynchronizeLateLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var rightSetter = SetExpressionRewriter.CreateSetter(rightSelector);

            if (rightSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), "rightSelector");

            SynchronizeLateLeftToRightOnly(leftSelector, rightSetter.Compile());
        }

        public void SynchronizeLateLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Action<TRight, TValue> rightSetter)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSetter == null) throw new ArgumentNullException("rightSetter");

            SynchronizationJobs.Add(new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSetter, false));
        }

        public void SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TLeft, bool>> guard)
        {
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            var rightSetter = SetExpressionRewriter.CreateSetter(rightSelector);

            if (rightSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), "rightSelector");

            SynchronizeLeftToRightOnly(leftSelector, rightSetter.Compile(), guard);
        }

        public void SynchronizeLeftToRightOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Action<TRight, TValue> rightSetter, Expression<Func<TLeft, bool>> guard)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSetter == null) throw new ArgumentNullException("rightSetter");

            guard = SimplifyPredicate(guard);

            var job = new LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSetter, false);

            if (guard == null)
            {
                SynchronizationJobs.Add(job);
            }
            else
            {
                SynchronizationJobs.Add(new LeftGuardedSynchronizationJob<TLeft, TRight>(job, guard));
            }
        }

        public void SynchronizeRightToLeftOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");

            var leftSetter = SetExpressionRewriter.CreateSetter(leftSelector);

            if (leftSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), "leftSelector");

            SynchronizeRightToLeftOnly(leftSetter.Compile(), rightSelector);
        }

        public void SynchronizeRightToLeftOnly<TValue>(Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");
            if (leftSetter == null) throw new ArgumentNullException("leftSetter");

            SynchronizationJobs.Add(new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(leftSetter, rightSelector, true));
        }

        public void SynchronizeLateRightToLeftOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");

            var leftSetter = SetExpressionRewriter.CreateSetter(leftSelector);

            if (leftSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), "rightSelector");

            SynchronizeLateRightToLeftOnly(leftSetter.Compile(), rightSelector);
        }

        public void SynchronizeLateRightToLeftOnly<TValue>(Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector)
        {
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");
            if (leftSetter == null) throw new ArgumentNullException("leftSetter");

            SynchronizationJobs.Add(new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(leftSetter, rightSelector, false));
        }

        public void SynchronizeRightToLeftOnly<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TRight, bool>> guard)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");

            var leftSetter = SetExpressionRewriter.CreateSetter(leftSelector);

            if (leftSetter == null) throw new ArgumentException(string.Format("The expression {0} cannot be inverted.", rightSelector), "rightSelector");

            SynchronizeRightToLeftOnly(leftSetter.Compile(), rightSelector, guard);
        }

        public void SynchronizeRightToLeftOnly<TValue>(Action<TLeft, TValue> leftSetter, Expression<Func<TRight, TValue>> rightSelector, Expression<Func<TRight, bool>> guard)
        {
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");
            if (leftSetter == null) throw new ArgumentNullException("leftSetter");

            guard = SimplifyPredicate(guard);

            var job = new RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue>(leftSetter, rightSelector, false);

            if (guard == null)
            {
                SynchronizationJobs.Add(job);
            }
            else
            {
                SynchronizationJobs.Add(new RightGuardedSynchronizationJob<TLeft, TRight>(job, guard));
            }
        }

        public void SynchronizeOpaque(Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> action)
        {
            SynchronizationJobs.Add(new OpaqueSynchronizationJob<TLeft, TRight>(action));
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

        protected virtual TRight CreateRightOutput(TLeft input, IEnumerable<TRight> candidates, ISynchronizationContext context, out bool existing)
        {
            TRight output = null;
            existing = false;
            if (candidates != null)
            {
                var inputArray = new object[1];
                foreach (var item in candidates)
                {
                    inputArray[0] = item;
                    if (item != null && !context.Trace.TraceIn(rightToLeft, inputArray).Any() && ShouldCorrespond(input, item, context))
                    {
                        output = item;
                        existing = true;
                        return output;
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
                output = Activator.CreateInstance(rightImplementationType) as TRight;
            }
            else
            {
                throw new NotImplementedException();
            }

            return output;
        }

        protected virtual TLeft CreateLeftOutput(TRight input, IEnumerable<TLeft> candidates, ISynchronizationContext context, out bool existing)
        {
            TLeft output = null;
            existing = false;
            if (candidates != null)
            {
                var inputArray = new object[1];
                foreach (var item in candidates)
                {
                    inputArray[0] = item;
                    if (item != null && !context.Trace.TraceIn(rightToLeft, inputArray).Any() && ShouldCorrespond(item, input, context))
                    {
                        output = item;
                        existing = true;
                        return output;
                    }
                }
            }
            return CreateLeftOutputInstance(out output);
        }
        

        protected internal virtual void SynchronizeCollectionsRightToLeft(ICollection<TLeft> lefts, ICollection<TRight> rights, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections(lefts, rights, context);
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
            if (context.Direction == SynchronizationDirection.RightWins)
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
            else if (context.Direction == SynchronizationDirection.RightToLeftForced)
            {
                foreach (var item in leftsSaved.Except(doubles))
                {
                    lefts.Remove(item);
                }
            }
        }

        protected internal virtual void SynchronizeCollectionsLeftToRight(ICollection<TRight> rights, ICollection<TLeft> lefts, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections(lefts, rights, context);
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
            if (context.Direction == SynchronizationDirection.LeftWins)
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
            else if (context.Direction == SynchronizationDirection.LeftToRightForced)
            {
                foreach (var item in rightsSaved.Except(doubles))
                {
                    rights.Remove(item);
                }
            }
        }

        private void MatchCollections(ICollection<TLeft> lefts, ICollection<TRight> rights, ISynchronizationContext context)
        {
            var leftsRemaining = lefts.ToList();
            var rightsRemaining = new HashSet<TRight>(rights);
            foreach (var left in lefts)
            {
                var right = null as TRight;
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
                    context.Inconsistencies.Add(new MissingItemInconsistency<TLeft, TRight>(context, LeftToRight, lefts, rights, left, false));
                }
            }
            foreach (var item in rightsRemaining)
            {
                context.Inconsistencies.Add(new MissingItemInconsistency<TRight, TLeft>(context, RightToLeft, rights, lefts, item, true));
            }
        }

        private static TLeft CreateLeftOutputInstance(out TLeft output)
        {
            if (leftImplementationType != null)
            {
                output = Activator.CreateInstance(leftImplementationType) as TLeft;
            }
            else
            {
                throw new NotImplementedException();
            }
            return output;
        }

        public void MarkInstantiatingFor(SynchronizationRuleBase synchronizationRule, Expression<Func<TLeft, bool>> leftPredicate = null, Expression<Func<TRight, bool>> rightPredicate = null)
        {
            if (synchronizationRule == null) throw new ArgumentNullException("synchronizationRule");

            if (!synchronizationRule.LeftType.IsAssignableFrom(typeof(TLeft)))
            {
                throw new ArgumentException("The left types do not conform. The left type of the current rule must be an assignable of the given synchronization rules left type.", "synchronizationRule");
            }
            if (!synchronizationRule.RightType.IsAssignableFrom(typeof(TRight)))
            {
                throw new ArgumentException("The right types do not conform. The right type of the current rule must be an assignable of the given synchronization rules right type.", "synchronizationRule");
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

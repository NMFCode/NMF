using NMF.Expressions;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
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
            leftToRight = new LeftToRightRule<TLeft, TRight>(this);
            rightToLeft = new RightToLeftRule<TLeft, TRight>(this);
            SynchronizationJobs = new List<ISynchronizationJob<TLeft, TRight>>();
        }

        public TransformationRuleBase<TLeft, TRight> LeftToRight { get { return leftToRight; } }

        public TransformationRuleBase<TRight, TLeft> RightToLeft { get { return rightToLeft; } }

        private LeftToRightRule<TLeft, TRight> leftToRight;
        private RightToLeftRule<TLeft, TRight> rightToLeft;

        private static Type leftImplementationType = GetImplementationType(typeof(TLeft));
        private static Type rightImplementationType = GetImplementationType(typeof(TRight));

        private static Type GetImplementationType(Type type)
        {
            if (!type.IsAbstract && !type.IsInterface) return type;

            var customs = type.GetCustomAttributes(typeof(DefaultImplementationTypeAttribute), false);
            if (customs != null && customs.Length > 0)
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
                return Math.Max(LTR.TransformationDelayLevel, RTL.TransformationDelayLevel);
            }
            set
            {
                leftToRight.SetTransformationDelay(value);
                rightToLeft.SetTransformationDelay(value);
            }
        }

        public byte OutputDelayLevel
        {
            get
            {
                return Math.Max(LTR.OutputDelayLevel, RTL.OutputDelayLevel);
            }
            set
            {
                leftToRight.SetOutputDelay(value);
                rightToLeft.SetOutputDelay(value);
            }
        }

        internal void InitializeOutput(SynchronizationComputation<TLeft, TRight> computation)
        {
            var context = computation.SynchronizationContext;
            foreach (var job in SynchronizationJobs.Where(j => j.IsEarly))
            {
                job.Perform(computation, context.Direction, context);
            }
        }

        internal void Synchronize(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            foreach (var job in SynchronizationJobs.Where(j => !j.IsEarly))
            {
                job.Perform(computation, direction, context);
            }
        }

        public virtual bool ShouldCorrespond(TLeft left, TRight right, ISynchronizationContext context)
        {
            return false;
        }

        public sealed override Type LeftType
        {
            get { return typeof(TLeft); }
        }

        public sealed override Type RightType
        {
            get { return typeof(TRight); }
        }

        public void Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
        }

        public void Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Action<TLeft, TDepLeft> leftSetter, Expression<Func<TRight, TDepRight>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector, leftSetter, null);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
        }

        public void Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Action<TRight, TDepRight> rightSetter)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector, null, rightSetter);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
        }

        public void Synchronize<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Action<TLeft, TDepLeft> leftSetter, Expression<Func<TRight, TDepRight>> rightSelector, Action<TRight, TDepRight> rightSetter)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector, leftSetter, rightSetter);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
        }

        public void SynchronizeLeftToRightOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightOnlyDependency());
        }

        public void SynchronizeRightToLeftOnly<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftOnlyDependency());
        }

        public void SynchronizeMany<TDepLeft, TDepRight>(SynchronizationRule<TDepLeft, TDepRight> rule, Expression<Func<TLeft, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ICollectionExpression<TDepRight>>> rightSelector)
            where TDepLeft : class
            where TDepRight : class
        {
            var dependency = new SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>(this, rule, leftSelector, rightSelector);
            LeftToRight.Dependencies.Add(dependency.CreateLeftToRightDependency());
            RightToLeft.Dependencies.Add(dependency.CreateRightToLeftDependency());
        }

        public void SynchronizeLate<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            SynchronizationJobs.Add(new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, true));
        }

        public void Synchronize<TValue>(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector)
        {
            SynchronizationJobs.Add(new PropertySynchronizationJob<TLeft, TRight, TValue>(leftSelector, rightSelector, false));
        }

        public void SynchronizeOpaque(Action<TLeft, TRight, SynchronizationDirection, ISynchronizationContext> action)
        {
            SynchronizationJobs.Add(new OpaqueSynchronizationJob<TLeft, TRight>(action));
        }

        internal object CreateRightOutputInternal(TLeft input, IEnumerable candidates, ISynchronizationContext context, out bool existing)
        {
            return CreateRightOutput(input, candidates != null ? candidates.OfType<TRight>() : null, context, out existing);
        }

        internal object CreateLeftOutputInternal(TRight input, IEnumerable candidates, ISynchronizationContext context, out bool existing)
        {
            return CreateLeftOutput(input, candidates != null ? candidates.OfType<TLeft>() : null, context, out existing);
        }

        protected virtual TRight CreateRightOutput(TLeft input, IEnumerable<TRight> candidates, ISynchronizationContext context, out bool existing)
        {
            TRight output = null;
            existing = false;
            if (candidates != null)
            {
                foreach (var item in candidates)
                {
                    if (item != null && ShouldCorrespond(input, item, context))
                    {
                        output = item;
                        existing = true;
                        return output;
                    }
                }
            }
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
                foreach (var item in candidates)
                {
                    if (item != null && ShouldCorrespond(item, input, context))
                    {
                        output = item;
                        existing = true;
                        return output;
                    }
                }
            }
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
                        throw new ArgumentException("The left predicate is always false", "leftPredicate");
                    }
                }
            }

            return predicate;
        }
    }
}

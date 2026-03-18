using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a job of a property that is only synchronized in one direction
    /// </summary>
    internal class OneWayPropertySynchronizationJob<TSource, TTarget, TValue>
    {
        private readonly ObservingFunc<TSource, ITransformationContext, TValue> sourceFunc;

        private readonly Func<TSource, ITransformationContext, TValue> sourceGetter;
        internal readonly Action<TTarget, ITransformationContext, TValue> targetSetter;
        private readonly Func<TTarget, ITransformationContext, TValue> targetGetter;

        private readonly bool isEarly;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sourceGetter">A function to get the value at the source side</param>
        /// <param name="targetGetter">A function to get the value at the target side</param>
        /// <param name="targetSetter">A setter function to store the value at the target side</param>
        /// <param name="isEarly">A flag indicating whether the job should be processed early</param>
        public OneWayPropertySynchronizationJob(Expression<Func<TSource, ITransformationContext, TValue>> sourceGetter, Func<TTarget, ITransformationContext, TValue> targetGetter, Action<TTarget, ITransformationContext, TValue> targetSetter, bool isEarly)
        {
            if (sourceGetter == null) throw new ArgumentNullException(nameof(sourceGetter));
            if (targetSetter == null) throw new ArgumentNullException(nameof(targetSetter));

            sourceFunc = new ObservingFunc<TSource, ITransformationContext, TValue>(sourceGetter);

            this.sourceGetter = sourceGetter.Compile();
            this.targetSetter = targetSetter;
            this.targetGetter = targetGetter;

            this.isEarly = isEarly;
        }

        /// <inheritdoc />
        public bool IsEarly
        {
            get
            {
                return isEarly;
            }
        }

        /// <inheritdoc />
        protected IDisposable Perform(TSource source, TTarget target, ISynchronizationContext context)
        {
            switch (context.ChangePropagation)
            {
                case Transformations.ChangePropagationMode.None:
                    targetSetter(target, context, sourceGetter(source, context));
                    return null;
                case Transformations.ChangePropagationMode.OneWay:
                case Transformations.ChangePropagationMode.TwoWay:
                    var incVal = sourceFunc.Observe(source, context);
                    incVal.Successors.SetDummy();
                    targetSetter(target, context, incVal.Value);
                    return new PropertySynchronization<TValue>(incVal, (o,val) => targetSetter(target, context, val));
                default:
                    throw new InvalidOperationException("Change propagation mode is not supported");
            }
        }

        protected IDisposable RegisterChangePropagationOnly(TSource source, TTarget target, ISynchronizationContext context)
        {
            var incVal = sourceFunc.Observe( source, context );
            incVal.Successors.SetDummy();
            return new PropertySynchronization<TValue>( incVal, (old,val) => targetSetter( target, context, val ) );
        }

        protected IDisposable PerformCheckOnly(TSource source, TTarget target, ISynchronizationContext context, Func<TSource, TTarget, TValue, TValue, ISynchronizationContext, IInconsistency> inconsistencyCreator)
        {
            if (targetGetter == null)
            {
                throw new NotSupportedException("Check-only mode requires a getter for the target");
            }
            if (context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay)
            {
                var incVal = sourceFunc.Observe(source, context);
                incVal.Successors.SetDummy();
                var targetValue = targetGetter(target, context);
                if (!EqualityComparer<TValue>.Default.Equals(incVal.Value, targetValue))
                {
                    context.Inconsistencies.Add(inconsistencyCreator(source, target, incVal.Value, targetValue, context));
                }
                return new PropertySynchronization<TValue>(incVal, (old,val) => targetSetter(target, context, val));
            }
            else
            {
                var sourceValue = sourceGetter(source, context);
                var targetValue = targetGetter(target, context);
                if (!EqualityComparer<TValue>.Default.Equals(sourceValue, targetValue))
                {
                    context.Inconsistencies.Add(inconsistencyCreator(source, target, sourceValue, targetValue, context));
                }
                return null;
            }
        }

        private void UpdateInconsistency(TSource source, TTarget target, object old, TValue newValue, TValue targetValue, ISynchronizationContext context, Func<TSource, TTarget, TValue, TValue, ISynchronizationContext, IInconsistency> inconsistencyCreator)
        {
            if (old is TValue oldValue && !EqualityComparer<TValue>.Default.Equals(oldValue, targetValue))
            {
                context.Inconsistencies.Remove(inconsistencyCreator(source, target, oldValue, targetValue, context));
            }
            if (!EqualityComparer<TValue>.Default.Equals(newValue, targetValue))
            {
                context.Inconsistencies.Add(inconsistencyCreator(source, target, newValue, targetValue, context));
            }
        }
    }
}

using NMF.Expressions;
using NMF.Transformations.Core;
using System;
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
        private readonly Action<TTarget, ITransformationContext, TValue> targetSetter;

        private readonly bool isEarly;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sourceGetter">A function to get the value at the source side</param>
        /// <param name="targetSetter">A setter function to store the value at the target side</param>
        /// <param name="isEarly">A flag indicating whether the job should be processed early</param>
        public OneWayPropertySynchronizationJob(Expression<Func<TSource, ITransformationContext, TValue>> sourceGetter, Action<TTarget, ITransformationContext, TValue> targetSetter, bool isEarly)
        {
            if (sourceGetter == null) throw new ArgumentNullException("leftSelector");
            if (targetSetter == null) throw new ArgumentNullException("rightSelector");

            sourceFunc = new ObservingFunc<TSource, ITransformationContext, TValue>(sourceGetter);

            this.sourceGetter = sourceGetter.Compile();
            this.targetSetter = targetSetter;

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
                    return new PropertySynchronization<TValue>(incVal, val => targetSetter(target, context, val));
                default:
                    throw new InvalidOperationException("Change propagation mode is not supported");
            }
        }

        protected IDisposable RegisterChangePropagationOnly(TSource source, TTarget target, ISynchronizationContext context)
        {
            var incVal = sourceFunc.Observe( source, context );
            incVal.Successors.SetDummy();
            return new PropertySynchronization<TValue>( incVal, val => targetSetter( target, context, val ) );
        }
    }
}

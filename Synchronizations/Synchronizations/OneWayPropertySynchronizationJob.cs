using NMF.Expressions;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a job of a property that is only synchronized in one direction
    /// </summary>
    /// <typeparam name="TSource">The source type of elements</typeparam>
    /// <typeparam name="TTarget">The target type of elements</typeparam>
    /// <typeparam name="TValue">The type of the values that are transmitted</typeparam>
    internal class OneWayPropertySynchronizationJob<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private readonly ObservingFunc<TSource, TValue> sourceFunc;

        private readonly Func<TSource, TValue> sourceGetter;
        private readonly Action<TTarget, TValue> targetSetter;

        private readonly bool isEarly;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sourceGetter">A function to get the value at the source side</param>
        /// <param name="targetSetter">A setter function to store the value at the target side</param>
        /// <param name="isEarly">A flag indicating whether the job should be processed early</param>
        public OneWayPropertySynchronizationJob(Expression<Func<TSource, TValue>> sourceGetter, Action<TTarget, TValue> targetSetter, bool isEarly)
        {
            if (sourceGetter == null) throw new ArgumentNullException("leftSelector");
            if (targetSetter == null) throw new ArgumentNullException("rightSelector");

            sourceFunc = new ObservingFunc<TSource, TValue>(sourceGetter);

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
                    targetSetter(target, sourceGetter(source));
                    return null;
                case Transformations.ChangePropagationMode.OneWay:
                case Transformations.ChangePropagationMode.TwoWay:
                    var incVal = sourceFunc.Observe(source);
                    incVal.Successors.SetDummy();
                    targetSetter(target, incVal.Value);
                    return new PropertySynchronization<TValue>(incVal, val => targetSetter(target, val));
                default:
                    throw new InvalidOperationException("Change propagation mode is not supported");
            }
        }
    }
}

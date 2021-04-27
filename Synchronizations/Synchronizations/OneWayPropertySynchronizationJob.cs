using NMF.Expressions;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class OneWayPropertySynchronizationJob<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private ObservingFunc<TSource, TValue> sourceFunc;

        private Func<TSource, TValue> sourceGetter;
        private Action<TTarget, TValue> targetSetter;

        private bool isEarly;

        public OneWayPropertySynchronizationJob(Expression<Func<TSource, TValue>> sourceGetter, Action<TTarget, TValue> targetSetter, bool isEarly)
        {
            if (sourceGetter == null) throw new ArgumentNullException("leftSelector");
            if (targetSetter == null) throw new ArgumentNullException("rightSelector");

            sourceFunc = new ObservingFunc<TSource, TValue>(sourceGetter);

            this.sourceGetter = sourceGetter.Compile();
            this.targetSetter = targetSetter;

            this.isEarly = isEarly;
        }

        public bool IsEarly
        {
            get
            {
                return isEarly;
            }
        }

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

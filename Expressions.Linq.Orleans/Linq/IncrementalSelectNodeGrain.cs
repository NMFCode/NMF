using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class IncrementalSelectNodeGrain<TSource, TResult> : IncrementalNodeGrainBase<TSource, TResult>,
        IIncrementalSelectNodeGrain<TSource, TResult>
    {

        public Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc)
        {
            ResultEnumerable = InputList.Select(observingFunc.Value);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
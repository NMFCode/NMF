using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class IncrementalSimpleSelectManyNodeNodeGrain<TSource, TResult> : IncrementalNodeGrainBase<TSource, TResult>,
        IIncrementalSimpleSelectManyNodeGrain<TSource, TResult>
    {

        public Task SetSelector(SerializableFunc<TSource, IEnumerable<TResult>> selector)
        {
            ResultEnumerable = InputList.SelectMany(selector.Value);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
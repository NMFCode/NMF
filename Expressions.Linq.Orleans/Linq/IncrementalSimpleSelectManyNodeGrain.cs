using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class IncrementalSimpleSelectManyNodeGrain<TSource, TResult, TModel> : IncrementalNodeGrainBase<TSource, TResult, TModel>,
        IIncrementalSimpleSelectManyNodeGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {

        public Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TResult>> selector)
        {
            ResultEnumerable = InputList.SelectMany(selector.Value);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
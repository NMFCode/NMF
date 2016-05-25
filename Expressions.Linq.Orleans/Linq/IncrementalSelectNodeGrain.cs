using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class IncrementalSelectNodeGrain<TSource, TResult, TModel> : IncrementalNodeGrainBase<TSource, TResult, TModel>,
        IIncrementalSelectNodeGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {

        public Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc)
        {
            ResultEnumerable = InputList.Select(observingFunc.Value);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
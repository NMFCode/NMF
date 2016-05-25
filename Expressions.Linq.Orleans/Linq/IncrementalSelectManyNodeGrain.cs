using System;
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
    internal sealed class IncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel> : IncrementalNodeGrainBase<TSource, TResult, TModel>,
        IIncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel> where TModel : IResolvableModel
    {
        public Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TIntermediate>> collectionSelectorFunc, SerializableFunc<TSource, TIntermediate, TResult> resultSelectorFunc)
        {
            ResultEnumerable = InputList.SelectMany(collectionSelectorFunc.Value, resultSelectorFunc.Value);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
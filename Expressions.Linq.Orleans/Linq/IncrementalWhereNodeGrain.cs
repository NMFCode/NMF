using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    internal sealed class IncrementalWhereNodeGrain<TSource, TModel> : IncrementalNodeGrainBase<TSource, TSource, TModel>,
        IIncrementalWhereNodeGrain<TSource, TModel> where TModel : IResolvableModel
    {

        public Task SetObservingFunc(SerializableFunc<TSource, bool> observingFunc)
        {
            ResultEnumerable = InputList.Where(observingFunc.Value);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
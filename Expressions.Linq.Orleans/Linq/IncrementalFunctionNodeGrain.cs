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
    internal sealed class IncrementalFunctionNodeGrain<TSource, TResult, TModel> : IncrementalNodeGrainBase<TSource, TResult, TModel>,
        IIncrementalFunctionNodeGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        public Task SetIncrementalFunc(Func<INotifyEnumerable<TSource>, INotifyEnumerable<TResult>> incrementalFunc)
        {
            ResultEnumerable = incrementalFunc(InputList);
            AttachToResult();
            return TaskDone.Done;
        }
    }
}
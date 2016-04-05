using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans
{
    public interface IObservingFuncProcessor<TSource, TResult>
    {
        Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc);
    }
}
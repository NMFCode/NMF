using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NMF.Expressions.Linq.Orleans
{
    public interface IObservingFuncProcessor<TSource, TResult>
    {
        Task SetObservingFunc(Func<TSource, TResult> observingFunc);
    }
}
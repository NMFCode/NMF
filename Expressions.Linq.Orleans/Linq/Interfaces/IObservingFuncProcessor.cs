using System.Threading.Tasks;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans
{
    /// <summary>
    /// Uses the following function to observe its results.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IObservingFuncProcessor<T1, TResult>
    {
        Task SetObservingFunc(SerializableFunc<T1, TResult> observingFunc);
    }

    /// <summary>
    /// Uses the following function to observe its results.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IObservingFuncProcessor<T1, T2, TResult>
    {
        Task SetObservingFunc(SerializableFunc<T1, T2, TResult> observingFunc);
    }

}
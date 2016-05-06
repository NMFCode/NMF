using System.Threading.Tasks;
using NMF.Models;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelProcessingAggregateGrain<TSource, TResult, TModel> : IStreamProcessorAggregate<TSource, TResult>, IModelConsumer<TModel> where TModel : IResolvableModel
    {
        Task Setup(IModelContainerGrain<TModel> modelContainer, int outputMultiplexFactor = 1);
    }
}
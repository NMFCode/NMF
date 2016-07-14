using System.Threading.Tasks;
using NMF.Models;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    /// Transforms data that can be based on a model from TIn to TOut using multiple IStreamProcessorNode.
    /// </summary>
    /// <typeparam name="TSource">Data input type.</typeparam>
    /// <typeparam name="TResult">Data output type.</typeparam>
    /// <typeparam name="TModel">Data type of the model.</typeparam>
    public interface IModelProcessingAggregateGrain<TSource, TResult, TModel> : IStreamProcessorAggregate<TSource, TResult> where TModel : IResolvableModel
    {
        /// <summary>
        /// Setup the aggregate.
        /// </summary>
        /// <param name="modelContainer">Model container to use.</param>
        /// <param name="outputMultiplexFactor">Multiplexing from input to output streams. Values in range [1, int32.max] are allowed.</param>
        /// <returns></returns>
        Task Setup(IModelContainerGrain<TModel> modelContainer, int outputMultiplexFactor = 1); // TODO might move parts to Orleans.Streams.Stateful?
    }
}
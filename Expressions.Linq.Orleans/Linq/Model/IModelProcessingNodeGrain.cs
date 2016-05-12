using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    /// Executes operations on streaming data based on a model and forwards it to its output stream.
    /// </summary>
    public interface IModelProcessingNodeGrain<TIn, TOut, TModel> : IStreamProcessorNodeGrain<TIn, TOut>, IContainsModel<TModel>
        where TModel : IResolvableModel
    {
        /// <summary>
        /// Setup the node.
        /// </summary>
        /// <param name="modelContainer">Model container to use.</param>
        /// <param name="inputStreams">Input streams to subscribe to.</param>
        /// <param name="outputMultiplexFactor">Multiplexing from input to output streams. Values in range [1, int32.max] are allowed.</param>
        /// <returns></returns>
        Task Setup(IModelContainerGrain<TModel> modelContainer, IEnumerable<StreamIdentity> inputStreams = null, int outputMultiplexFactor = 1);
    }
}
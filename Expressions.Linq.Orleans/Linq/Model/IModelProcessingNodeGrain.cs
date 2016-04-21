using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelProcessingNodeGrain<TIn, TOut, TModel> : IStreamProcessorNodeGrain<TIn, TOut>, IModelLoader<TModel> where TModel : Models.Model
    {
        Task SetModelContainer(IModelContainerGrain<TModel> modelContainer);
    }
}
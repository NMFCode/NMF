using System.Threading.Tasks;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelConsumer<TModel> where TModel : IResolvableModel
    {
        Task SetModelContainer(IModelContainerGrain<TModel> modelContainer);

        Task<IModelContainerGrain<TModel>> GetModelContainer();
    }
}
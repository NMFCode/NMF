using System.Threading.Tasks;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelConsumer<TModel> where TModel : IResolvableModel
    {
        Task<IModelContainerGrain<TModel>> GetModelContainer();
    }
}
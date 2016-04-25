using System.Threading.Tasks;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelConsumer<TModel> where TModel : Models.Model
    {
        Task SetModelContainer(IModelContainerGrain<TModel> modelContainer);
    }
}
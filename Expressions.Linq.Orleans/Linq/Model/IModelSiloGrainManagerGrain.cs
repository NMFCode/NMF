using System.Threading.Tasks;
using NMF.Models;
using Orleans;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelSiloGrainManagerGrain<T> : IGrainWithGuidKey where T : IResolvableModel
    {
        Task<IModelSiloGrain<T>> GetModelSiloGrainForSilo(string siloIdentity);
    }
}
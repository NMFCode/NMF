using System;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelLoader<T>
    {
        Task LoadModel(Func<T> modelLoadingFunc);

        Task<string> ModelToString(Func<T, IModelElement> elementSelectorFunc);
    }
}
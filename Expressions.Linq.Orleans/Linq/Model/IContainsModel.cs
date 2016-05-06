using System;
using System.Threading.Tasks;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IContainsModel<T>
    {
        Task<string> ModelToString(Func<T, IModelElement> elementSelectorFunc);
    }
}
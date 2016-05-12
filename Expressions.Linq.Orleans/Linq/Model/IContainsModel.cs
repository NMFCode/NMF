using System;
using System.Threading.Tasks;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    /// Actions to be executed on a model (copy).
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    public interface IContainsModel<T>
    {
        /// <summary>
        /// Transform the model into an xml string.
        /// </summary>
        /// <param name="elementSelectorFunc">Element to start serializing from.</param>
        /// <returns>XML serialization of the model.</returns>
        Task<string> ModelToString(Func<T, IModelElement> elementSelectorFunc);
    }
}
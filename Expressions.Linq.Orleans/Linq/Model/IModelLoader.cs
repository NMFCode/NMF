using System;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Models.Repository;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelLoader<T>
    {
        Task LoadModelFromPath(string modelPath);
    }
}
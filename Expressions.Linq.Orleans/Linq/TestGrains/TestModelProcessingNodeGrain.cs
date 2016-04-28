using System;
using System.IO;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.Expressions.Linq.Orleans.TestGrains
{
    public class TestModelProcessingNodeGrain<TIn, TOut, TModel> : ModelProcessingNodeGrain<TIn, TOut, TModel>, ITestModelProcessingNodeGrain<TIn, TOut, TModel> where TModel : IResolvableModel
    {

    }

    public interface ITestModelProcessingNodeGrain<TIn, TOut, TModel> : IModelProcessingNodeGrain<TIn, TOut, TModel> where TModel : IResolvableModel
    {
         
    }
}
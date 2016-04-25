using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSimpleSelectManyNodeGrain<TSource, TResult> : IModelProcessingNodeGrain<TSource, TResult, NMF.Models.Model>, IElementEnumeratorNode<TResult>
    {
        Task SetSelector(SerializableFunc<TSource, IEnumerable<TResult>> selector);
    }
}
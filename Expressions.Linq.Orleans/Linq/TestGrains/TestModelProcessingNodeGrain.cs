using System;
using System.IO;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;

namespace NMF.Expressions.Linq.Orleans.TestGrains
{
    public class TestModelProcessingNodeGrain<TIn, TOut> : ModelProcessingNodeGrain<TIn, TOut, NMF.Models.Model>, ITestModelProcessingNodeGrain<TIn, TOut>
    {
        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            StreamConsumer = new TransactionalStreamModelConsumer<TIn, Models.Model>(GetStreamProvider(StreamProviderNamespace));
        }
    }

    public interface ITestModelProcessingNodeGrain<TIn, TOut> : IModelProcessingNodeGrain<TIn, TOut, NMF.Models.Model>
    {
         
    }
}
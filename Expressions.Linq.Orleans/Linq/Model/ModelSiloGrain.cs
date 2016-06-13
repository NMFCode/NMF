using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using Orleans;
using Orleans.Concurrency;
using Orleans.Placement;

namespace NMF.Expressions.Linq.Orleans.Model
{
    [PreferLocalPlacement]
    public class ModelSiloGrain<T> : Grain, IModelSiloGrain<T> where T : IModelElement, IResolvableModel
    {
        private TransactionalStreamModelConsumer<object, T> _streamConsumer;
        protected const string StreamProviderNamespace = "CollectionStreamProvider";

        public Task<Immutable<T>> GetModel()
        {
            return Task.FromResult(new Immutable<T>(_streamConsumer.Model));
        }

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            _streamConsumer = new TransactionalStreamModelConsumer<object, T>(GetStreamProvider(StreamProviderNamespace));
            return base.OnActivateAsync();
        }

        public async Task SetModelContainer(IModelContainerGrain<T> modelContainer, string modelPath = "")
        {
            await _streamConsumer.SetModelContainer(modelContainer, modelPath);
            _streamConsumer.MessageDispatcher.Register<PosLengthFixMessage>(message =>
            {
                message.Execute(_streamConsumer.Model as Models.Model);
                return TaskDone.Done;
            });
        }

        public Task<string> GetIdentity()
        {
            return Task.FromResult(RuntimeIdentity);
        }

        public async Task TearDown()
        {
            await _streamConsumer.TearDown();
            DeactivateOnIdle();
        }

        public Task<bool> IsTearedDown()
        {
            return Task.FromResult(false);
        }
    }
}
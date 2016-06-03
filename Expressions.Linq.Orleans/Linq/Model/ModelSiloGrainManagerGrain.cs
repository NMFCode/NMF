using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams.Partitioning;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelSiloGrainManagerGrain<T> : Grain, IModelSiloGrainManagerGrain<T> where T : IResolvableModel
    {
        private Dictionary<string, IModelSiloGrain<T>> _slaveModels;
        private IModelContainerGrain<T> _modelContainerGrain;

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            _slaveModels = new Dictionary<string, IModelSiloGrain<T>>();
            _modelContainerGrain = GrainFactory.GetGrain<IModelContainerGrain<T>>(this.GetPrimaryKey());
            return base.OnActivateAsync();
        }


        public async Task<IModelSiloGrain<T>> GetModelSiloGrainForSilo(string siloIdentity)
        {
            if (_slaveModels.ContainsKey(siloIdentity))
                return _slaveModels[siloIdentity];

            // Create new model grain
            var executionGrain = await PartitionGrainUtil.GetExecutionGrain(siloIdentity, GrainFactory);

            var modelGrain = (IModelSiloGrain<T>) await executionGrain.ExecuteFunc(async (factory, state) =>
            {
                var grain = factory.GetGrain<IModelSiloGrain<T>>(Guid.NewGuid().ToString());
                await grain.SetModelContainer((IModelContainerGrain<T>) state);
                return grain;
            }, _modelContainerGrain);

            _slaveModels[siloIdentity] = modelGrain;

            return modelGrain;
        }

        public async Task TearDownModelSiloGrain(string siloIdentity)
        {
            if (_slaveModels.ContainsKey(siloIdentity))
            {
                await _slaveModels[siloIdentity].TearDown();
                _slaveModels.Remove(siloIdentity);
            }
        }
    }
}
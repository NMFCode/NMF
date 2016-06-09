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
        private string _masterModelSilo;

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override async Task OnActivateAsync()
        {
            _slaveModels = new Dictionary<string, IModelSiloGrain<T>>();
            _modelContainerGrain = GrainFactory.GetGrain<IModelContainerGrain<T>>(this.GetPrimaryKey());
            _masterModelSilo = await _modelContainerGrain.GetIdentity();
            await base.OnActivateAsync();
        }

        public async Task<IModelContainerGrain<T>> SetupModelAcrossCluster(string path)
        {
            var siloNames = await PartitionGrainUtil.GetAllSiloNames(GrainFactory);
            _modelContainerGrain = GrainFactory.GetGrain<IModelContainerGrain<T>>(this.GetPrimaryKey());
            var containerTask = _modelContainerGrain.LoadModelFromPath(path);

            var siloTasks = new List<Task<IModelSiloGrain<T>>>();
            foreach (var silo in siloNames)
            {
               siloTasks.Add(GetModelSiloGrainForSilo(silo, path));
            }

            await containerTask;
            await Task.WhenAll(siloTasks);

            return _modelContainerGrain;
        }


        public async Task<IModelSiloGrain<T>> GetModelSiloGrainForSilo(string siloIdentity, string modelPath = "")
        {
            if (siloIdentity == _masterModelSilo)
                return _modelContainerGrain;

            if (_slaveModels.ContainsKey(siloIdentity))
                return _slaveModels[siloIdentity];

            // Create new model grain
            var executionGrain = await PartitionGrainUtil.GetExecutionGrain(siloIdentity, GrainFactory);

            var modelGrain = (IModelSiloGrain<T>) await executionGrain.ExecuteFunc(async (factory, state) =>
            {
                object[] stateArray = (object[]) state;
                var grain = factory.GetGrain<IModelSiloGrain<T>>(Guid.NewGuid());
                await grain.SetModelContainer((IModelContainerGrain<T>)stateArray[0], (string) stateArray[1]);
                return grain;
            }, new object[] { _modelContainerGrain, modelPath });

            _slaveModels[siloIdentity] = modelGrain;

            return modelGrain;
        }

        public async Task TearDownModelSiloGrain(string siloIdentity)
        {
            if (_slaveModels.ContainsKey(siloIdentity) && siloIdentity != _masterModelSilo)
            {
                await _slaveModels[siloIdentity].TearDown();
                _slaveModels.Remove(siloIdentity);
            }
        }
    }
}
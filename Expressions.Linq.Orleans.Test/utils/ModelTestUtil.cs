using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using TTC2015.TrainBenchmark.Railway;

namespace Expressions.Linq.Orleans.Test.utils
{
    public static class ModelTestUtil
    {

        public static readonly Func<Model, IModelElement> DefaultModelSelectorFunc = model =>
        {
            var railwayContainer = model.RootElements.Single() as RailwayContainer;
            return railwayContainer;
        };

        public static readonly Func<Model> ModelLoadingFunc = () =>
        {
            var repository = new ModelRepository();
            var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
            var railwayContainer = train.Model.RootElements.Single() as RailwayContainer;

            return train.Model;
        };

        public static async Task<IModelContainerGrain<Model>> LoadModelContainer(IGrainFactory factory)
        {
            var modelContainerGrain = factory.GetGrain<IModelContainerGrain<Model>>(Guid.NewGuid());
            await modelContainerGrain.LoadModel(ModelLoadingFunc);

            return modelContainerGrain;
        }

        public static async Task<bool> CurrentModelsMatch<T>(IModelLoader<T> loader1,
IModelLoader<T> loader2) where T : Model
        {
            return await CurrentModelsMatch(loader1, loader2, DefaultModelSelectorFunc);
        }

        public static async Task<bool> CurrentModelsMatch<T>(IModelLoader<T> loader1,
    IModelLoader<T> loader2, Func<Model, IModelElement> elementSelectorFunc) where T : Model
        {
            var s1 = await loader1.ModelToString(elementSelectorFunc);
            var s2 = await loader2.ModelToString(elementSelectorFunc);

            return s1.Equals(s2);
        }

    }
}
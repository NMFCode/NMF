using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using Orleans;

namespace Expressions.Linq.Orleans.Test.utils
{
    public static class ModelTestUtil
    {

        public static readonly Func<Model, IModelElement> DefaultModelSelectorFunc = model =>
        {
            var railwayContainer = model.RootElements.Single() as RailwayContainer;
            return railwayContainer;
        };

        public static readonly string ModelPath = "models/railway-1.xmi";

        public static readonly Func<string, Model> ModelLoadingFunc = (path) =>
        {
            var repository = new ModelRepository();
            var train = repository.Resolve(new Uri(new FileInfo(path).FullName));
            var railwayContainer = train.Model.RootElements.Single() as RailwayContainer;

            return train.Model;
        };

        public static async Task<IModelContainerGrain<Model>> LoadModelContainer(IGrainFactory factory)
        {
            var modelContainerGrain = factory.GetGrain<IModelContainerGrain<Model>>(Guid.NewGuid());
            await modelContainerGrain.LoadModelFromPath(ModelLoadingFunc, ModelPath);

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

        public static void AssertXmlEquals(IEnumerable<IModelElement> elements1, IEnumerable<IModelElement> elements2)
        {
            var localXmlString = elements1.Select(r => r.ToXmlString()).OrderBy(s => s).ToList();
            var processedXmlstring = elements2.Select(r => r.ToXmlString()).OrderBy(s => s).ToList();

            CollectionAssert.AreEqual(localXmlString, processedXmlstring);
        }

    }
}
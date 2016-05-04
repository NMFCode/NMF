using System;
using System.IO;
using System.Linq;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public static class ModelUtil
    {
        public static T LoadModelFromPath<T>(string modelPath) where T : IResolvableModel
        {
            ModelElement.EnforceModels = false; // TODO remove once bug is fixed
            var repository = new ModelRepository();
            var rootModelElement = repository.Resolve(new Uri(new FileInfo(modelPath).FullName));
            ModelElement.EnforceModels = true;

            IResolvableModel model = rootModelElement.Model;
            return (T) model;
        }
    }
}
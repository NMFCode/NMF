using System;
using System.IO;
using System.Linq;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelLoader
    {
        private static IModelLoader _instance;

        public static IModelLoader Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new FullPathModelLoader();

                return _instance;
            }
            set { _instance = value; }
        }

    }

    public interface IModelLoader
    {
        T LoadModel<T>(string modelPath) where T : IResolvableModel;
    }

    public class FullPathModelLoader : IModelLoader
    {
        public FullPathModelLoader()
        {
        }

        public T LoadModel<T>(string modelPath) where T : IResolvableModel
        {
            ModelElement.EnforceModels = true;
            Models.Model.PromoteSingleRootElement = true;
            var repository = new ModelRepository();
            var rootModelElement = repository.Resolve(new Uri(new FileInfo(modelPath).FullName));

            IResolvableModel model = rootModelElement.Model;
            return (T)model;
        }
    }

    public static class ModelUtil
    {
        /// <summary>
        /// Loads a model from a path.
        /// </summary>
        /// <typeparam name="T">Type of the model.</typeparam>
        /// <param name="modelPath">Path to the model.</param>
        /// <returns></returns>
        public static T LoadModelFromPath<T>(string modelPath) where T : IResolvableModel
        {
            ModelElement.EnforceModels = true;
            Models.Model.PromoteSingleRootElement = false;

            var repository = new ModelRepository();
            var rootModelElement = repository.Resolve(new Uri(new FileInfo(modelPath).FullName));

            IResolvableModel model = rootModelElement.Model;
            return (T) model;
        }
    }
}
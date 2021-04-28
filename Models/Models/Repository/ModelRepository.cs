using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Represents a standard model repository
    /// </summary>
    public class ModelRepository : IModelRepository
    {
        private readonly ModelRepositoryModelCollection models;
        private EventHandler<BubbledChangeEventArgs> bubbledChange;

        /// <summary>
        /// Gets the parent model repository.
        /// </summary>
        public IModelRepository Parent { get; private set; }

        /// <summary>
        /// Gets or sets the serializer that is used for the deserialization of the models
        /// </summary>
        public IModelSerializer Serializer { get; set; }

        /// <summary>
        /// Gets a collection of model locators
        /// </summary>
        public ICollection<IModelLocator> Locators { get; private set; }

        /// <summary>
        /// Creates a new model repository with the meta repository as parent
        /// </summary>
        public ModelRepository() : this(null) { }

        /// <summary>
        /// Creates a new model repository with a given parent
        /// </summary>
        /// <param name="parent">The parent repository</param>
        /// <remarks>If no parent repository is provided, the meta repository is used as parent repository</remarks>
        public ModelRepository(IModelRepository parent) : this(parent, null, FileLocator.Instance) { }

        /// <summary>
        /// Creates a new model repository with a given parent
        /// </summary>
        /// <param name="parent">The parent repository</param>
        /// <param name="serializer">A serializer object or null to use the default</param>
        /// <param name="locators">A set of model locators</param>
        /// <remarks>If no parent repository is provided, the meta repository is used as parent repository</remarks>
        public ModelRepository(IModelRepository parent, IModelSerializer serializer, params IModelLocator[] locators)
        {
            models = new ModelRepositoryModelCollection(this);
            Locators = new List<IModelLocator>(locators);
            Parent = parent ?? MetaRepository.Instance;
            Serializer = serializer ?? MetaRepository.Instance.Serializer;
            Parent.BubbledChange += Parent_BubbledChange;
        }

        private void Parent_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            OnBubbledChange(e);
        }

        /// <summary>
        /// Resolves the given Uri and returns the model element
        /// </summary>
        /// <param name="uri">The Uri where to look for the model element</param>
        /// <param name="loadOnDemand">A boolean flag indicating whether the uri should be attempted
        /// to load, if the model is not already registered with the repository</param>
        /// <returns>A model element at the given Uri or null if none can be found</returns>
        public IModelElement Resolve(Uri uri, bool loadOnDemand = true)
        {
            return Resolve(uri, null, loadOnDemand);
        }

        /// <summary>
        /// Resolves the given Uri and returns the model element
        /// </summary>
        /// <param name="uri">The Uri where to look for the model element</param>
        /// <param name="hintPath">The path where the model can be found</param>
        /// <param name="loadOnDemand">A boolean flag indicating whether the uri should be attempted
        /// to load, if the model is not already registered with the repository</param>
        /// <returns>A model element at the given Uri or null if none can be found</returns>
        public IModelElement Resolve(Uri uri, string hintPath, bool loadOnDemand = true)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            Model model;
            Uri modelUri;
            Func<Stream> streamCreator;
            if (!models.TryGetValue(uri, out model))
            {
                var parentResolved = Parent.Resolve(uri, false);
                if (parentResolved != null) return parentResolved;

                if (loadOnDemand)
                {
                    if (hintPath == null)
                    {
                        var locator = Locators.Where(l => l.CanLocate(uri)).FirstOrDefault();

                        if (locator == null)
                        {
                            if (parentResolved != null) return parentResolved;

                            var e = new UnresolvedModelElementEventArgs(uri);
                            OnUnresolvedModelElement(e);
                            return e.ModelElement;
                        }
                        modelUri = locator.GetRepositoryUri(uri);
                        streamCreator = () => locator.Open(modelUri);
                    }
                    else
                    {
                        modelUri = uri;
                        streamCreator = () => File.OpenRead(hintPath);
                    }
                    if (!models.TryGetValue(modelUri, out model))
                    {
                        using (var stream = streamCreator())
                        {
                            model = Serializer.Deserialize(stream, modelUri, this, true);
                            if (model.RootElements.Count == 1)
                            {
                                if (model.RootElements[0] is INamespace ns)
                                {
                                    model.ModelUri = ns.Uri;
                                    if (!models.ContainsKey(ns.Uri))
                                    {
                                        models.Add(ns.Uri, model);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            if (uri.IsAbsoluteUri)
            {
                var element = model.Resolve(uri.Fragment);

                if (element == null)
                {
                    var e = new UnresolvedModelElementEventArgs(uri);
                    OnUnresolvedModelElement(e);
                    element = e.ModelElement;
                }

                return element;
            }
            else
            {
                return model;
            }
        }

        /// <summary>
        /// Resolves the given file path for a model element
        /// </summary>
        /// <param name="path">The file path where to look for models</param>
        /// <returns>The model at this file path or null if the file cannot be found</returns>
        public Model Resolve(string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var element = Resolve(new Uri(file.FullName));
                if (element == null) return null;
                return element.Model;
            }
            else
            {
                return null;
            }
        }

        private void EnsureModelIsKnown(Model model)
        {
            Model existingModel;
            if (models.TryGetValue(model.ModelUri, out existingModel))
            {
                if (model != existingModel)
                {
                    throw new InvalidOperationException(string.Format("This repository already contains a different model with the Uri {0}", model.ModelUri));
                }
            }
            else
            {
                models.Add(model.ModelUri, model);
            }
        }

        /// <summary>
        /// Saves the given model element to the specified stream
        /// </summary>
        /// <param name="element">The model element</param>
        /// <param name="path">The path where to save the model element</param>
        public void Save(IModelElement element, string path)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var model = element.Model;
            if (model != null)
            {
                model.EnsureAllElementsContained();
                Serializer.Serialize(model, path);
                EnsureModelIsKnown(model);
            }
            else
            {
                Serializer.Serialize(element, path);
            }
        }

        /// <summary>
        /// Saves the given model element to the specified stream
        /// </summary>
        /// <param name="element">The model element</param>
        /// <param name="path">The path where to save the model element</param>
        /// <param name="uri">The uri under which the model element can be retrieved</param>
        public void Save(IModelElement element, string path, Uri uri)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var model = element.Model;
            if (model != null)
            {
                model.EnsureAllElementsContained();
                Serializer.Serialize(model, path, uri);
                EnsureModelIsKnown(model);
            }
            else
            {
                Serializer.Serialize(element, path, uri);
            }
        }

        /// <summary>
        /// Saves the given model element to the specified stream
        /// </summary>
        /// <param name="element">The model element</param>
        /// <param name="stream">The stream to save the model element to</param>
        /// <param name="uri">The uri under which the model element shall be retrievable</param>
        public void Save(IModelElement element, Stream stream, Uri uri)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var model = element.Model;
            if (model != null)
            {
                model.EnsureAllElementsContained();
                Serializer.Serialize(model, stream, uri);
                EnsureModelIsKnown(model);
            }
            else
            {
                Serializer.Serialize(element, stream, uri);
            }
        }

        /// <summary>
        /// Gets called when a Uri cannot be resolved
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnUnresolvedModelElement(UnresolvedModelElementEventArgs e)
        {
            UnresolvedModelElement?.Invoke(this, e);
        }

        /// <summary>
        /// Gets fired when a Uri cannot be resolved
        /// </summary>
        public event EventHandler<UnresolvedModelElementEventArgs> UnresolvedModelElement;

        protected virtual void OnBubbledChange(BubbledChangeEventArgs e)
        {
            bubbledChange?.Invoke(this, e);
        }

        public event EventHandler<BubbledChangeEventArgs> BubbledChange
        {
            add
            {
                var isAttached = bubbledChange != null;
                bubbledChange += value;
                if (!isAttached && (bubbledChange != null))
                {
                    models.RegisterChangeHandlers();
                }
            }
            remove
            {
                var isAttached = bubbledChange != null;
                bubbledChange -= value;
                if (isAttached && (bubbledChange == null))
                {
                    models.UnregisterChangeHandlers();
                }
            }
        }

        /// <summary>
        /// Gets a dictionary of the models loaded to this repository
        /// </summary>
        public ModelCollection Models
        {
            get { return models; }
        }

        protected class ModelRepositoryModelCollection : ModelCollection
        {
            public ModelRepositoryModelCollection(ModelRepository repo) : base(repo) { }

            public override void Add(Uri key, Model value)
            {
                base.Add(key, value);
                var repository = (ModelRepository)Repository;
                if (repository.bubbledChange != null)
                {
                    value.BubbledChange += ModelBubbledChange;
                }
            }

            public void RegisterChangeHandlers()
            {
                foreach (var model in Values)
                {
                    model.BubbledChange += ModelBubbledChange;
                }
            }

            public void UnregisterChangeHandlers()
            {
                foreach (var model in Values)
                {
                    model.BubbledChange -= ModelBubbledChange;
                }
            }

            private void ModelBubbledChange(object sender, BubbledChangeEventArgs e)
            {
                ((ModelRepository)Repository).OnBubbledChange(e);
            }
        }
    }
}

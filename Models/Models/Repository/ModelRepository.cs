using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Represents a standard model repository
    /// </summary>
    public class ModelRepository : IModelRepository
    {
        private Dictionary<Uri, Model> models = new Dictionary<Uri, Model>();

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
        public ModelRepository(IModelRepository parent)
        {
            Locators = new List<IModelLocator>();
            Locators.Add(FileLocator.Instance);
            Parent = parent ?? MetaRepository.Instance;
            Serializer = MetaRepository.Instance.Serializer;
        }

        /// <summary>
        /// Resolves the given Uri and returns the model element
        /// </summary>
        /// <param name="uri">The Uri where to look for the model element</param>
        /// <returns>A model element at the given Uri or null if none can be found</returns>
        public IModelElement Resolve(Uri uri)
        {
            return Resolve(uri, null);
        }

        /// <summary>
        /// Resolves the given Uri and returns the model element
        /// </summary>
        /// <param name="uri">The Uri where to look for the model element</param>
        /// <param name="hintPath">The path where the model can be found</param>
        /// <returns>A model element at the given Uri or null if none can be found</returns>
        public IModelElement Resolve(Uri uri, string hintPath)
        {
            var parentResolved = Parent.Resolve(uri);
            if (parentResolved != null && !models.ContainsKey(uri)) return parentResolved;

            if (uri == null) throw new ArgumentNullException("uri");
            Model model;
            Uri modelUri;
            Func<Stream> streamCreator;
            if (!models.TryGetValue(uri, out model))
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
                            var ns = model.RootElements[0] as INamespace;
                            if (ns != null)
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

            var element = model.Resolve(uri.Fragment);

            if (element == null)
            {
                if (parentResolved != null) return parentResolved;

                var e = new UnresolvedModelElementEventArgs(uri);
                OnUnresolvedModelElement(e);
                element = e.ModelElement;
            }

            return element;
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

        private void EnsureModelIsKnown(IModelElement element)
        {
            var model = element.Model;
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
            Serializer.Serialize(element, path);
            EnsureModelIsKnown(element);
        }

        /// <summary>
        /// Saves the given model element to the specified stream
        /// </summary>
        /// <param name="element">The model element</param>
        /// <param name="path">The path where to save the model element</param>
        /// <param name="uri">The uri under which the model element can be retrieved</param>
        public void Save(IModelElement element, string path, Uri uri)
        {
            Serializer.Serialize(element, path, uri);
            EnsureModelIsKnown(element);
        }

        /// <summary>
        /// Saves the given model element to the specified stream
        /// </summary>
        /// <param name="element">The model element</param>
        /// <param name="stream">The stream to save the model element to</param>
        /// <param name="uri">The uri under which the model element shall be retrievable</param>
        public void Save(IModelElement element, Stream stream, Uri uri)
        {
            Serializer.Serialize(element, stream, uri);
            EnsureModelIsKnown(element);
        }

        /// <summary>
        /// Gets called when a Uri cannot be resolved
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnUnresolvedModelElement(UnresolvedModelElementEventArgs e)
        {
            if (UnresolvedModelElement != null) UnresolvedModelElement(this, e);
        }

        /// <summary>
        /// Gets fired when a Uri cannot be resolved
        /// </summary>
        public event EventHandler<UnresolvedModelElementEventArgs> UnresolvedModelElement;

        /// <summary>
        /// Gets a dictionary of the models loaded to this repository
        /// </summary>
        public IDictionary<Uri, Model> Models
        {
            get { return models; }
        }
    }
}

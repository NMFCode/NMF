using NMF.Expressions;
using NMF.Models.Collections;
using System;
using NMF.Models.Repository;
using System.IO;

namespace NMF.Models
{
    /// <summary>
    /// Denotes common extensions to model elements
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Gets an incrementalizable collection with all descendent elements
        /// </summary>
        /// <param name="element">The root model element</param>
        /// <returns>An incrementalizable collection with all descendent elements</returns>
        public static IEnumerableExpression<IModelElement> Descendants(this IModelElement element)
        {
            return new DescendantsCollection(element);
        }

        /// <summary>
        /// Gets an incrementalizable collection with all ancestor elements
        /// </summary>
        /// <param name="element">The leaf element</param>
        /// <returns>A collection including the element and all its ancestors</returns>
        public static IEnumerableExpression<IModelElement> Ancestors(this IModelElement element)
        {
            return new AncestorCollection(element);
        }

        /// <summary>
        /// Gets an incrementalizable collection with all ancestor elements
        /// </summary>
        /// <param name="element">The leaf element</param>
        /// <returns>A collection including the element and all its ancestors</returns>
        public static IEnumerableExpression<ModelTreeItem> AncestorTree(this IModelElement element)
        {
            return new AncestorTreeCollection(element);
        }

        /// <summary>
        /// Serializes the given model element to the given path
        /// </summary>
        /// <param name="serializer">The serializer</param>
        /// <param name="element">The model element</param>
        /// <param name="path">The path</param>
        public static void Serialize(this IModelSerializer serializer, IModelElement element, string path)
        {
            Uri uri;
            Model model = element.Model;
            if (model == null || model.ModelUri == null)
            {
                if (!Uri.TryCreate(path, UriKind.Absolute, out uri))
                {
                    uri = new Uri(Path.GetFullPath(path));
                }
            }
            else
            {
                uri = model.ModelUri;
            }
            serializer.Serialize(element, path, uri);
        }

        /// <summary>
        /// Serializes the given model element to the given path
        /// </summary>
        /// <param name="serializer">The serializer</param>
        /// <param name="element">The model element</param>
        /// <param name="path">The path</param>
        /// <param name="uri">The uri under which the element should be serialized</param>
        public static void Serialize(this IModelSerializer serializer, IModelElement element, string path, Uri uri)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(element, fs, uri);
            }
        }

        /// <summary>
        /// Serializes the given model element to the given path
        /// </summary>
        /// <param name="serializer">The serializer</param>
        /// <param name="element">The model element</param>
        /// <param name="target">The target to which the model element should be serialized</param>
        /// <param name="uri">The uri under which the element should be serialized</param>
        public static void Serialize(this IModelSerializer serializer, IModelElement element, Stream target, Uri uri)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var model = element.Model;
            if (model == null)
            {
                model = new Model();
                model.RootElements.Add(element);
            }
            model.ModelUri = uri;

            serializer.Serialize(model, target);
        }
    }
}

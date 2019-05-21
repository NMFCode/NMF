using NMF.Expressions;
using NMF.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using System.IO;

namespace NMF.Models
{
    public static class ModelExtensions
    {
        public static IEnumerableExpression<IModelElement> Descendants(this IModelElement element)
        {
            return new DescendantsCollection(element);
        }

        public static IEnumerableExpression<IModelElement> Ancestors(this IModelElement element)
        {
            return new AncestorCollection(element);
        }

        public static IEnumerableExpression<ModelTreeItem> AncestorTree(this IModelElement element)
        {
            return new AncestorTreeCollection(element);
        }

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

        public static void Serialize(this IModelSerializer serializer, IModelElement element, string path, Uri uri)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(element, fs, uri);
            }
        }

        public static void Serialize(this IModelSerializer serializer, IModelElement element, Stream target, Uri uri)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (uri == null) throw new ArgumentNullException("uri");

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

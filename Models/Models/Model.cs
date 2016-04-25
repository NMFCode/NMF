using NMF.Collections.Generic;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Collections;
using NMF.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [DebuggerDisplayAttribute("Model {ModelUri}")]
    public class Model : ModelElement
    {
        public Model()
        {
            RootElements = new ObservableCompositionOrderedSet<IModelElement>(this);
        }

        public Uri ModelUri
        {
            get;
            set;
        }

        public IOrderedSetExpression<IModelElement> RootElements { get; private set; }

        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return RootElements.Concat(Extensions);
            }
        }

        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if (reference == "#" && index < RootElements.Count)
            {
                return RootElements[index];
            }
            else
            {
                int num;
                if (int.TryParse(reference, out num) && num > 0 && num < RootElements.Count)
                {
                    return RootElements[num];
                }
                return null;
            }
        }

        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat<IModelElement>(RootElements);
            }
        }

        protected override string GetRelativePathForNonIdentifiedChild(IModelElement child)
        {
            if (RootElements.Count == 1)
            {
                return null;
            }
            else
            {
                var index = RootElements.IndexOf(child);
                if (index != -1)
                {
                    return "/" + index.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        protected internal override Uri CreateUriWithFragment(string fragment, bool absolute)
        {
            if (fragment != null)
            {
                var leftPad = fragment == string.Empty ?
                    "/" : string.Empty;
                if (ModelUri == null || !absolute || !ModelUri.IsAbsoluteUri)
                {
                    return new Uri("/" + leftPad + fragment, UriKind.Relative);
                }
                else
                {
                    return new Uri(ModelUri, "#/" + leftPad + fragment);
                }
            }
            else
            {
                if (absolute)
                {
                    return ModelUri;
                }
                else
                {
                    return null;
                }
            }
        }

        public override IModelElement Resolve(string path)
        {
            if (string.IsNullOrEmpty(path)) return this;
            if (path.StartsWith("//")) path = path.Substring(2);
            if (path.StartsWith("#//")) path = path.Substring(3);
            if (RootElements.Count == 1)
            {
                var root = RootElements[0] as ModelElement;
                if (root != null)
                {
                    var resolved = root.Resolve(path);
                    if (resolved != null) return resolved;
                }
            }
            return base.Resolve(path);
        }

        protected override string GetRelativePathForChild(IModelElement child)
        {
            if (RootElements.Count == 1 && child == RootElements[0])
            {
                return string.Empty;
            }
            return base.GetRelativePathForChild(child);
        }

        public override Meta.IClass GetClass()
        {
            return NMF.Models.Repository.MetaRepository.Instance.ResolveClass("http://nmf.codeplex.com/nmeta/#//NMeta/Model/");
        }
    }
}

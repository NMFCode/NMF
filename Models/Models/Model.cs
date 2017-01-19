using NMF.Collections.Generic;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Collections;
using NMF.Models.Repository;
using NMF.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Models
{
    [XmlElementName("XMI")]
    [XmlNamespaceAttribute("http://www.omg.org/XMI")]
    [XmlNamespacePrefixAttribute("xmi")]
    [DebuggerDisplayAttribute("Model {ModelUri}")]
    [ModelRepresentationClass("http://nmf.codeplex.com/nmeta/#//Model/")]
    public class Model : ModelElement
    {
        private Dictionary<string, ModelElement> IdStore;

        static Model()
        {
            PromoteSingleRootElement = true;
        }

        public static bool PromoteSingleRootElement { get; set; }

        public Model()
        {
            RootElements = new ObservableCompositionOrderedSet<IModelElement>(this);
        }

        public Uri ModelUri
        {
            get;
            set;
        }

        public IModelRepository Repository { get; internal set; }

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
            if (RootElements.Count == 1 && PromoteSingleRootElement)
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
                if (fragment == string.Empty) fragment = "/";
                if (!absolute)
                {
                    return new Uri("#/" + fragment, UriKind.Relative);
                }
                else if (ModelUri != null && ModelUri.IsAbsoluteUri)
                {
                    return new Uri(ModelUri.AbsoluteUri + "#/" + fragment);
                }
                else
                {
                    return null;
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

        public bool RegisterId(string id, ModelElement element)
        {
            var idStore = IdStore;
            if (idStore == null)
            {
                idStore = Interlocked.CompareExchange(ref IdStore, new Dictionary<string, ModelElement>(), null) ?? IdStore;
            }
            if (idStore.ContainsKey(id))
            {
                return false;
            }
            else
            {
                idStore.Add(id, element);
                return true;
            }
        }

        public bool UnregisterId(string id)
        {
            if (IdStore == null)
            {
                return false;
            }
            else
            {
                return IdStore.Remove(id);
            }
        }

        public override IModelElement Resolve(string path)
        {
            if (path == null) return this;
            if (path.StartsWith("#"))
            {
                path = path.Substring(1);
            }
            if (path == string.Empty) return this;
            if (!path.StartsWith("//"))
            {
                var index = path.IndexOf('/');
                if (index == 0)
                {
                    return ResolveNonIdentified(path.Substring(1));
                }
                if (IdStore != null)
                {
                    ModelElement element;
                    if (IdStore.TryGetValue(index == -1 ? path : path.Substring(0, index), out element))
                    {
                        if (index == -1)
                        {
                            return element;
                        }
                        else
                        {
                            return element.Resolve(path.Substring(index + 1));
                        }
                    }
                }
                return null;
            }
            else
            {
                return ResolveNonIdentified(path.Substring(2));
            }
        }

        private IModelElement ResolveNonIdentified(string path)
        {
            if (RootElements.Count == 1 && PromoteSingleRootElement)
            {
                var root = RootElements[0] as ModelElement;
                if (root != null)
                {
                    var resolved = root.Resolve(path);
                    if (resolved != null) return resolved;
                }
            }
            var baseResolve = base.Resolve(path);
            if (baseResolve != null || PromoteSingleRootElement || RootElements.Count != 1) return baseResolve;
            var rootCasted = RootElements[0] as ModelElement;
            if (rootCasted != null)
            {
                return rootCasted.Resolve(path);
            }
            else
            {
                return null;
            }
        }

        protected override string GetRelativePathForChild(IModelElement child)
        {
            if (PromoteSingleRootElement && RootElements.Count == 1 && child == RootElements[0])
            {
                return string.Empty;
            }
            return base.GetRelativePathForChild(child);
        }

        public override Meta.IClass GetClass()
        {
            return (Meta.IClass)NMF.Models.Repository.MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//NMeta/Model/");
        }
    }
}

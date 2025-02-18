using NMF.AnyText.Grammars;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.AnyMeta
{
    internal class AnyMetaParseContext : ModelParseContext
    {
        private Dictionary<string, INamespace> _namespaces;

        private Dictionary<string, INamespace> GetOrCreateNamespaceDictionary()
        {
            if (_namespaces == null)
            {
                _namespaces = new Dictionary<string, INamespace>();

                RegisterNamespace(MetaElement.ClassInstance.Namespace);

                if (Imports.Count > 0)
                {
                    var repository = new ModelRepository();

                    foreach (var import in Imports)
                    {
                        var imported = repository.Resolve(import);
                        foreach (var ns in imported.RootElements.OfType<INamespace>())
                        {
                            RegisterNamespace(ns);
                        }
                    }
                }
            }
            return _namespaces;
        }

        private void RegisterNamespace(INamespace ns)
        {
            _namespaces[ns.Prefix] = ns;
            foreach (var child in ns.ChildNamespaces)
            {
                RegisterNamespace(child);
            }
        }

        public AnyMetaParseContext(Grammar grammar, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) : base(grammar, stringComparison)
        {
        }

        public override bool TryResolveReference<T>(object contextElement, string input, out T resolved)
        {
            if (input != null)
            {
                if (typeof(T) == typeof(IType) && TryResolveType(input, contextElement, out var type))
                {
                    resolved = (T)type;
                    return true;
                }
                if (typeof(T) == typeof(IClass) && TryResolveClass(input, contextElement, out var cl))
                {
                    resolved = (T)cl;
                    return true;
                }
                if (contextElement is IClass declaringClass)
                {
                    if (typeof(T) == typeof(IAttribute))
                    {
#pragma warning disable S2955 // Generic parameters not constrained to reference types should not be compared to "null"
                        resolved = (T)(declaringClass.LookupAttribute(input));
                        return resolved != null;
                    }
                    if (typeof(T) == typeof(IReference))
                    {
                        resolved = (T)(declaringClass.LookupReference(input));
                        return resolved != null;
                    }
                    if (typeof(T) == typeof(IOperation))
                    {
                        resolved = (T)(declaringClass.LookupOperation(input));
                        return resolved != null;
#pragma warning restore S2955 // Generic parameters not constrained to reference types should not be compared to "null"
                    }
                }
            }
            return base.TryResolveReference(contextElement, input, out resolved);
        }

        private bool TryResolveType(string input, object contextElement, out IType resolved)
        {
            var dotIndex = input.IndexOf('.');
            if (dotIndex == -1)
            {
                var modelElement = contextElement as IModelElement;
                while (modelElement != null)
                {
                    var ns = modelElement.Ancestors().OfType<INamespace>().FirstOrDefault();
                    if (ns == null)
                    {
                        break;
                    }
                    resolved = ns.Types.FirstOrDefault(t => t.Name == input);
                    if (resolved != null)
                    {
                        return true;
                    }
                    modelElement = modelElement.Parent;
                }
            }
            else if (dotIndex < input.Length - 1)
            {
                var prefix = input.Substring(0, dotIndex);
                var localName = input.Substring(dotIndex + 1);

                var prefixes = GetOrCreateNamespaceDictionary();
                if (prefixes.TryGetValue(prefix, out var ns))
                {
                    resolved = ns.Types.FirstOrDefault(t => t.Name == localName);
                    return resolved != null;
                }
            }
            resolved = null;
            return false;
        }

        private bool TryResolveClass(string input, object contextElement, out IClass resolved)
        {
            if (TryResolveType(input, contextElement, out var type) && type is IClass cl)
            {
                resolved = cl;
                return true;
            }
            resolved = null;
            return false;
        }
    }
}

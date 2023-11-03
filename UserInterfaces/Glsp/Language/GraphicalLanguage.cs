using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes a graphical language
    /// </summary>
    public abstract class GraphicalLanguage
    {
        /// <summary>
        /// Gets the diagram type supported by this graphical language
        /// </summary>
        public virtual string DiagramType => GetType().Name;

        /// <summary>
        /// Gets the semantic type of the diagram root model element
        /// </summary>
        public abstract Type SemanticRootType { get; }

        private readonly Dictionary<Type, DescriptorBase> _rules = new Dictionary<Type, DescriptorBase>();
        private bool _isInitialized;

        /// <summary>
        /// Initializes the graphical language
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized) { return; }
            _isInitialized = true;
            foreach (var ruleType in GetType().GetNestedTypes())
            {
                if (!ruleType.IsAbstract && !ruleType.IsInterface && typeof(DescriptorBase).IsAssignableFrom(ruleType))
                {
                    var ruleInstance = Activator.CreateInstance(ruleType) as DescriptorBase;
                    ruleInstance.Language = this;
                    _rules.Add(ruleType, ruleInstance);
                }
            }
            foreach (var rule in _rules.Values)
            {
                rule.DefineLayout();
            }
        }

        /// <summary>
        /// Resolves the given descriptor type to a descriptor
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <returns>The descriptor instance, if any</returns>
        public T Descriptor<T>() where T : DescriptorBase
        {
            if (_rules.TryGetValue(typeof(T), out var ruleInstance))
            {
                return ruleInstance as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Calculates shape hints for this language
        /// </summary>
        /// <returns>A collection of shape hints</returns>
        public IEnumerable<ShapeTypeHint> CalculateShapeHints()
        {
            return _rules.Values.SelectMany(r => r.CalculateShapeHints());
        }

        /// <summary>
        /// Calculates edge hints for this language
        /// </summary>
        /// <returns>A collection of edge hints</returns>
        public IEnumerable<EdgeTypeHint> CalculateEdgeHints()
        {
            return _rules.Values.SelectMany(r => r.CalculateEdgeHints());
        }

        /// <summary>
        /// Creates a graph element for the given semantic root element
        /// </summary>
        /// <param name="semanticRoot">The semantic root element</param>
        /// <param name="diagram">The notational instance</param>
        /// <param name="trace">The trace object</param>
        /// <returns>A graph notation</returns>
        public GGraph Create(object semanticRoot, IDiagram diagram, ISkeletonTrace trace)
        {
            if (!SemanticRootType.IsInstanceOfType(semanticRoot))
            {
                throw new InvalidOperationException($"Model is not an element of expected type {SemanticRootType.Name}");
            }

            throw new NotImplementedException();
        }
    }
}

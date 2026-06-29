using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Graph
{
    /// <summary>
    /// Represents a graph as denoted by GLSP
    /// </summary>
    public class GGraph : GElement
    {
        private readonly Dictionary<string, GElement> _elementsById = new Dictionary<string, GElement>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public GGraph(string id) : base(id)
        {
            _elementsById.Add(Id, this);
        }

        /// <summary>
        /// Resolves the given element id
        /// </summary>
        /// <param name="id">The id of the element</param>
        /// <returns>The resolved graph element or null, if not found</returns>
        public GElement Resolve(string id)
        {
            if (_elementsById.TryGetValue(id, out var element))
            {
                return element;
            }
            return null;
        }

        /// <summary>
        /// Registers the given graph element with the provided id
        /// </summary>
        /// <param name="id">The id of the element</param>
        /// <param name="element">The actual element</param>
        internal void RegisterId(string id, GElement element)
        {
            lock (_elementsById)
            {
                _elementsById[id] = element;
            }
        }

        /// <inheritdoc />
        [JsonIgnore]
        public override GGraph Graph => this;

        public event EventHandler<IEnumerable<Marker>> MarkersChanged;
        
        internal void OnMarkersChanged(IEnumerable<Marker> markers)
        {
            MarkersChanged?.Invoke(this, markers);
        }

    }
}

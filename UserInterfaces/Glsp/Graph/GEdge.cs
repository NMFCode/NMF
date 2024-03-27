using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Glsp.Protocol.Types;
using System;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Graph
{
    /// <summary>
    /// Denotes an edge in the GLSP graph structure
    /// </summary>
    public class GEdge : GElement
    {
        private string _sourceId;
        private string _targetId;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public GEdge() : base() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="id">The id of the edge</param>
        public GEdge(string id) : base(id) { }

        /// <summary>
        /// Gets or sets the source id of the edge
        /// </summary>
        public string SourceId
        {
            get => _sourceId;
            set
            {
                if (Set(ref _sourceId, value)) SourceIdChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets or sets the target id of the edge
        /// </summary>
        public string TargetId
        {
            get => _targetId;
            set
            {
                if (Set(ref _targetId, value)) TargetIdChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Event that is raised if the source id is changed
        /// </summary>
        public event Action<GEdge> SourceIdChanged;

        /// <summary>
        /// Event that is raised if the target id is changed
        /// </summary>
        public event Action<GEdge> TargetIdChanged;

        /// <summary>
        /// True, if an event listener is registered to handle changes of the source id
        /// </summary>
        [JsonIgnore]
        public bool SupportsChangingSourceId => SourceIdChanged != null;

        /// <summary>
        /// True, if an event listener is registered to handle changes of the target id
        /// </summary>
        [JsonIgnore]
        public bool SupportsChangingTargetId => TargetIdChanged != null;

        /// <summary>
        /// Gets a collection of routing points
        /// </summary>
        public IListExpression<Point> RoutingPoints { get; } = new ObservableList<Point>();

        internal void SilentSetSource(string sourceId)
        {
            _sourceId = sourceId;
        }

        internal void SilentSetTarget(string targetId)
        {
            _targetId = targetId;
        }
    }
}

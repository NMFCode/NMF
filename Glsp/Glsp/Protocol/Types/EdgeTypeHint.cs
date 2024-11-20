namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Type hints for edges
    /// </summary>
    public class EdgeTypeHint : TypeHint
    {

        /// <summary>
        ///  Specifies whether the routing points of the edge can be changed
        ///  i.e. edited by the user.
        /// </summary>
        public bool Routable { get; init; }

        /// <summary>
        ///  Allowed source element types for this edge type
        ///  If not defined unknown element can be used as source element for this edge.
        /// </summary>
        public string[] SourceElementTypeIds { get; init; }

        /// <summary>
        ///  Allowed targe element types for this edge type
        ///  If not defined unknown element can be used as target element for this edge.
        /// </summary>
        public string[] TargetElementTypeIds { get; init; }

        /// <summary>
        ///  Indicates whether this type hint is dynamic or not. Dynamic edge type hints
        ///  require an additional runtime check before creating an edge, when checking
        ///  source and target element types is not sufficient.
        /// </summary>
        public bool Dynamic { get; init; }
    }
}

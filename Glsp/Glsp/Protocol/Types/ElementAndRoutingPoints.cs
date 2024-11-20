namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// The ElementAndRoutingPoints type is used to associate an edge with specific routing points.
    /// </summary>
    public class ElementAndRoutingPoints
    {

        /// <summary>
        ///  The identifier of an element.
        /// </summary>
        public string ElementId { get; init; }

        /// <summary>
        ///  The new list of routing points.
        /// </summary>
        public Point[] NewRoutingPoints { get; init; }
    }
}

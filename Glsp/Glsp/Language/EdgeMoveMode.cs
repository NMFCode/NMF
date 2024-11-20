namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes where a label of an edge can be moved
    /// </summary>
    public enum EdgeMoveMode
    {
        /// <summary>
        /// Denotes that the edge cannot be moved at all
        /// </summary>
        None,

        /// <summary>
        /// Denotes that the label can be moved along the edge
        /// </summary>
        Edge,

        /// <summary>
        /// Denotes that the label can be moved freely
        /// </summary>
        Free
    }
}

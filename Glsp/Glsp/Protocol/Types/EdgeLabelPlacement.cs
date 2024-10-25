namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Denotes the placement of a label relative to an edge
    /// </summary>
    /// <param name="Position">The position of the edge</param>
    /// <param name="Rotate">A boolean value indicating whether the label should be rotated</param>
    /// <param name="Side">The side of the edge</param>
    /// <param name="MoveMode">The way the label can be moved</param>
    /// <param name="Offset">An offset to the next node or edge</param>
    public record EdgeLabelPlacement(double Position, bool Rotate, string Side, string MoveMode, double? Offset)
    {
    }
}

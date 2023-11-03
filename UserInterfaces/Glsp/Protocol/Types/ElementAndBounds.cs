namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// The ElementAndBounds type is used to associate new bounds with a model element, which is referenced via its id.
    /// </summary>
    public class ElementAndBounds
    {
        /// <summary>
        ///  The identifier of the element.
        /// </summary>
        public string ElementId { get; init; }

        /// <summary>
        ///  The new size of the element.
        /// </summary>
        public Dimension? NewSize { get; init; }

        /// <summary>
        ///  The new position of the element.
        /// </summary>
        public Point? NewPosition { get; init; }
    }
}

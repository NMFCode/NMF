namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// A marker represents the validation result for a single model element.
    /// </summary>
    public class Marker
    {
        /// <summary>
        ///  Short label describing this marker message, e.g., short validation message
        /// </summary>
        public string Label { get; init; }

        /// <summary>
        ///  Full description of this marker, e.g., full validation message
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        ///  Id of the model element this marker refers to
        /// </summary>
        public string ElementId { get; init; }

        /// <summary>
        ///  Marker kind, e.g., info, warning, error or custom kind
        /// </summary>
        public string Kind { get; init; }
    }
}

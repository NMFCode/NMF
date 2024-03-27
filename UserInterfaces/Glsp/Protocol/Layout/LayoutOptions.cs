namespace NMF.Glsp.Protocol.Layout
{
    /// <summary>
    /// Denotes layout options for nodes
    /// </summary>
    public class LayoutOptions
    {
        /// <summary>
        /// Determines the padding right
        /// </summary>
        public double? PaddingRight { get; init; }

        /// <summary>
        /// Gets the preferred width
        /// </summary>
        public double? PrefWidth { get; init; }

        /// <summary>
        /// Gets the preferred height
        /// </summary>
        public double? PrefHeight { get; init; }
    }
}

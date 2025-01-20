namespace NMF.Glsp.Language
{
    /// <summary>
    /// Container for default Sprotty type names
    /// </summary>
    public static class DefaultTypes
    {
        /// <summary>
        /// The default type for a graph
        /// </summary>
        public const string Graph = "graph";

        /// <summary>
        /// The default type for a node
        /// </summary>
        public const string Node = "node";

        /// <summary>
        /// The default type for an edge
        /// </summary>
        public const string Edge = "edge";

        /// <summary>
        /// The default type for a port
        /// </summary>
        public const string Port = "port";

        /// <summary>
        /// The default type for a label
        /// </summary>
        public const string Label = "label";

        /// <summary>
        /// The default type for a compartment
        /// </summary>
        public const string Compartment = "comp";

        /// <summary>
        /// The default type for the header compartment
        /// </summary>
        public const string CompartmentHeader = "comp:header";

        /// <summary>
        /// The default type for a button
        /// </summary>
        public const string Button = "button";

        /// <summary>
        /// Thze default type for an expander button
        /// </summary>
        public const string ExpandButton = "button:expand";

        /// <summary>
        /// The default type for an issue marker
        /// </summary>
        public const string IssueMarker = "marker";

        // shapes
        /// <summary>
        /// The default type for circular node
        /// </summary>
        public const string NodeCircle = "node:circle";

        /// <summary>
        /// The default type for a rectangular node
        /// </summary>
        public const string NodeRectangle = "node:rectangle";

        /// <summary>
        /// The default type for a diamond node
        /// </summary>
        public const string NodeDiamond = "node:diamond";

        // types present on the client

        /// <summary>
        /// The default type for a routing point
        /// </summary>
        public const string RoutingPoint = "routing-point";

        /// <summary>
        /// The default type for a volatile routing point
        /// </summary>
        public const string VolatileRoutingPoint = "volatile-routing-point";

        /// <summary>
        /// The default type for a html element
        /// </summary>
        public const string Html = "html";

        /// <summary>
        /// The default type for a foreign object
        /// </summary>
        public const string ForeignObject = "foreign-object";

        /// <summary>
        /// The default type for a pre-rendered object
        /// </summary>
        public const string PreRendered = "pre-rendered";

        /// <summary>
        /// The default type for a pre-rendered shape
        /// </summary>
        public const string ShapePreRendered = "shape-pre-rendered";

        /// <summary>
        /// The type for SVG
        /// </summary>
        public const string Svg = "svg";
    }
}

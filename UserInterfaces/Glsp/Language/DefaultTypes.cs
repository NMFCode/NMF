using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Container for default Sprotty type names
    /// </summary>
    public static class DefaultTypes
    {

        public const string Graph = "graph";
        public const string Node = "node";
        public const string Edge = "edge";
        public const string Port = "port";
        public const string Label = "label";
        public const string Compartment = "comp";
        public const string CompartmentHeader = "comp:header";
        public const string Button = "button";
        public const string ExpandButton = "button:expand";
        public const string IssueMarker = "marker";

        // shapes
        public const string NodeCircle = "node:circle";
        public const string NodeRectangle = "node:rectangle";
        public const string NodeDiamond = "node:diamond";

        // types present on the client
        public const string RoutingPoint = "routing-point";
        public const string VolatileRoutingPoint = "volatile-routing-point";
        public const string Html = "html";
        public const string ForeignObject = "foreign-object";
        public const string PreRendered = "pre-rendered";
        public const string ShapePreRendered = "shape-pre-rendered";
        public const string Svg = "svg";
    }
}

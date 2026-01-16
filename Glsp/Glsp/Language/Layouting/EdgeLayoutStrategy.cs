using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;

namespace NMF.Glsp.Language.Layouting
{
    /// <summary>
    /// A custom layout strategy that sticks child elements to the edges of their element.
    /// Each child element can define a desired edge by setting the "edge" detail property 
    /// (with expected values "top", "bottom", "left", or "right"). If the detail is not provided,
    /// the element defaults to the top edge.
    /// </summary>
    internal class EdgeLayoutStrategy : LayoutStrategy
    {
        public static readonly EdgeLayoutStrategy Instance = new EdgeLayoutStrategy();

        /// <inheritdoc/>
        /// <remarks>Indicates that this layout strategy requires layout recalculation.</remarks>
        public override bool NeedsLayout => true;

        /// <inheritdoc/>
        /// <remarks>Marks the element with the "edge-stick" layout.</remarks>
        public override void Apply(GElement container)
        {
            container.Details["layout"] = "edge-stick";
        }

        /// <inheritdoc/>
        /// <remarks>Directly sets the position of a child element.</remarks>
        public override void SetPosition(GElement element, Point position, Dimension? size)
        {
            element.Position = position;
            element.Parent?.UpdateLayout();
        }

        /// <inheritdoc/>
        public override void Update(GElement element)
        {
            if (element == null)
            {
                return;
            }

            double containerWidth = element.Size?.Width ?? 0;
            double containerHeight = element.Size?.Height ?? 0;

            foreach (var child in element.Children)
            {
                string edge = "top";
                if (child.Details.TryGetValue("edge", out object edgeObj))
                {
                    edge = edgeObj?.ToString()?.ToLowerInvariant() ?? "top";
                }

                double childWidth = child.Size?.Width ?? 0;
                double childHeight = child.Size?.Height ?? 0;
                double x;
                double y;

                switch (edge)
                {
                    case "bottom":
                        x = (containerWidth - childWidth) / 2;
                        y = containerHeight - childHeight;
                        break;
                    case "left":
                        x = 0;
                        y = (containerHeight - childHeight) / 2;
                        break;
                    case "right":
                        x = containerWidth - childWidth;
                        y = (containerHeight - childHeight) / 2;
                        break;
                    default:
                        // Defaults to top if the provided value is unrecognized.
                        x = (containerWidth - childWidth) / 2;
                        y = 0;
                        break;
                }

                child.Position = new Point(x - (child.Position?.X ?? 0), y - (child.Position?.Y ?? 0));

            }
        }
    }
}

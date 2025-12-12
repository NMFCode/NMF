using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Linq;

namespace NMF.Glsp.Language.Layouting
{
    internal class AbsolutePositioningStrategy : LayoutStrategy
    {
        public static readonly AbsolutePositioningStrategy Instance = new AbsolutePositioningStrategy();

        public override bool NeedsLayout => true;

        public override void Apply(GElement container)
        {
        }

        public override void SetPosition(GElement element, Point position, Dimension? size)
        {
            element.Position = CalculateLocalPoint(position, element.Parent);
            element.Size = size ?? element.Size;
            if (element.Parent != null)
            {
                element.Parent.UpdateLayout();
            }
        }

        private static Point CalculateLocalPoint(Point position, GElement reference)
        {
            while (reference != null && reference.Position.HasValue)
            {
                position -= reference.Position.Value;
                reference = reference.Parent;
            }
            return position;
        }

        public override void Update(GElement element)
        {
            var size = element.Size ?? new Dimension(0, 0);
            var width = size.Width;
            var height = size.Height;

            foreach (var child in element.Children.Where(e => e.Size.HasValue && e.Position.HasValue))
            {
                width = Math.Max(width, child.Position.Value.X + child.Size.Value.Width);
                height = Math.Max(height, child.Position.Value.Y + child.Size.Value.Height);
            }

            element.Size = new Dimension(width, height);
            if ((width != size.Width || height != size.Height) && element.Parent != null)
            {
                element.Parent.UpdateLayout();
            }
        }
    }
}

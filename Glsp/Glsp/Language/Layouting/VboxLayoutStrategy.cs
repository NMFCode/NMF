using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;

namespace NMF.Glsp.Language.Layouting
{
    internal class VboxLayoutStrategy : LayoutStrategy
    {
        public static readonly VboxLayoutStrategy Instance = new VboxLayoutStrategy();

        public override bool NeedsLayout => false;

        public override void Apply(GElement container)
        {
            container.Details["layout"] = "vbox";
        }

        public override void SetPosition(GElement element, Point position, Dimension? size)
        {
            if (element.Parent == null) { return; }
            var pos = position;
            foreach (var child in element.Parent.Children)
            {
                if (child != element)
                {
                    pos = new Point(pos.X, pos.Y + (child.Size?.Height ?? 0));
                }
                else
                {
                    break;
                }
            }
            element.Position = pos;
            element.Size = size ?? element.Size;
            element.Parent.UpdateLayout();
        }

        public override void Update(GElement element)
        {
            if (element == null) { return; }
            var height = 0.0;
            var width = 0.0;
            foreach (var child in element.Children)
            {
                if (!child.Position.HasValue || child.Position.Value.Y < height)
                {
                    child.Position = new Point(child.Position?.X ?? 0.0, height);
                }
                height = Math.Max(height + (child.Size?.Height ?? 0), child.Position?.Y ?? 0);
                width = Math.Max(width, child.Size?.Width ?? 0);
            }

            element.Size = new Dimension(Math.Max(width, element.Size?.Width ?? 0), Math.Max(height, element.Size?.Height ?? 0));

            if (element.Parent != null)
            {
                element.Parent.UpdateLayout();
            }
        }
    }
}

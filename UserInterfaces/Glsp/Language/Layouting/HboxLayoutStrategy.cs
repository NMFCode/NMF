using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language.Layouting
{
    internal class HboxLayoutStrategy : LayoutStrategy
    {
        public static readonly HboxLayoutStrategy Instance = new HboxLayoutStrategy();

        public override void Apply(GElement container)
        {
            container.Details["layout"] = "hbox";
        }

        public override void SetPosition(GElement element, Point position)
        {
            if (element.Parent == null) { return; }
            var pos = position;
            foreach (var child in element.Parent.Children)
            {
                if (child != element)
                {
                    pos = new Point(pos.X + child.Size?.Width ?? 0, pos.Y);
                }
                else
                {
                    break;
                }
            }
            element.Position = pos;
            element.Parent.UpdateLayout();
        }

        public override void Update(GElement element)
        {
            if (element == null) { return; }
            var height = 0.0;
            var width = 0.0;
            foreach (var child in element.Children)
            {
                if (!child.Position.HasValue || child.Position.Value.X < width)
                {
                    child.Position = new Point(width, child.Position?.Y ?? 0.0);
                }
                height = Math.Max(height, child.Size?.Height ?? 0);
                width = Math.Max(width + (child.Size?.Width ?? 0), child.Position?.X ?? 0);
            }

            element.Size = new Dimension(Math.Max(width, element.Size?.Width ?? 0), Math.Max(height, element.Size?.Height ?? 0));

            if (element.Parent != null)
            {
                element.Parent.UpdateLayout();
            }
        }
    }
}

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
        }
    }
}

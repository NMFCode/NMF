using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language.Layouting
{
    internal class AbsolutePositioningStrategy : LayoutStrategy
    {
        public static readonly AbsolutePositioningStrategy Instance = new AbsolutePositioningStrategy();

        public override void Apply(GElement container)
        {
        }

        public override void SetPosition(GElement element, Point position)
        {
            element.Position = position;
        }
    }
}

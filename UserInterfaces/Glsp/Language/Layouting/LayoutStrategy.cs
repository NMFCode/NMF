using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language.Layouting
{
    public abstract class LayoutStrategy
    {
        public static LayoutStrategy Vbox => VboxLayoutStrategy.Instance;
        public static LayoutStrategy Hbox => HboxLayoutStrategy.Instance;
        public static LayoutStrategy FreeForm => AbsolutePositioningStrategy.Instance;

        public abstract void SetPosition(GElement element, Point position);

        public abstract void Apply(GElement container);
    }
}

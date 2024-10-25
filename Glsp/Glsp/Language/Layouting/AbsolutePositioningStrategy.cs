using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;

namespace NMF.Glsp.Language.Layouting
{
    internal class AbsolutePositioningStrategy : LayoutStrategy
    {
        public static readonly AbsolutePositioningStrategy Instance = new AbsolutePositioningStrategy();

        public override bool NeedsLayout => true;

        public override void Apply(GElement container)
        {
        }

        public override void SetPosition(GElement element, Point position)
        {
            element.Position = position;
        }

        public override void Update(GElement element)
        {
        }
    }
}

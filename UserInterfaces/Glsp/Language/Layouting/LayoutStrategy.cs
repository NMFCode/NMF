using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;

namespace NMF.Glsp.Language.Layouting
{
    /// <summary>
    /// Denotes a layout strategy
    /// </summary>
    public abstract class LayoutStrategy
    {
        /// <summary>
        /// Gets the implementation of the Vbox layout strategy
        /// </summary>
        public static LayoutStrategy Vbox => VboxLayoutStrategy.Instance;

        /// <summary>
        /// Gets the implementation of the Hbox layout strategy
        /// </summary>
        public static LayoutStrategy Hbox => HboxLayoutStrategy.Instance;

        /// <summary>
        /// Gets the implementation of the free form layout strategy
        /// </summary>
        public static LayoutStrategy FreeForm => AbsolutePositioningStrategy.Instance;

        public abstract void SetPosition(GElement element, Point position);

        public abstract void Update(GElement element);

        public abstract void Apply(GElement container);
    }
}

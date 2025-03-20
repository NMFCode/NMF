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

        /// <summary>
        /// Gets the implementation of the edge layout strategy
        /// </summary>
        public static LayoutStrategy Edge => EdgeLayoutStrategy.Instance;

        /// <summary>
        /// Sets the position for the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="position">The position</param>
        public abstract void SetPosition(GElement element, Point position);

        /// <summary>
        /// Updates the layout for the given element
        /// </summary>
        /// <param name="element">the element</param>
        public abstract void Update(GElement element);

        /// <summary>
        /// Applies the layout for the given container
        /// </summary>
        /// <param name="container">The container element</param>
        public abstract void Apply(GElement container);

        /// <summary>
        /// True, if the layout strategy needs automatic layout calculation, otherwise false
        /// </summary>
        public abstract bool NeedsLayout { get; }
    }
}

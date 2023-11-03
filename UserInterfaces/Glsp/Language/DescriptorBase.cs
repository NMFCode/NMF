using NMF.Glsp.Protocol.Types;
using System.Collections.Generic;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes the base class for graph element descriptors
    /// </summary>
    public abstract class DescriptorBase
    {
        internal GraphicalLanguage Language;

        /// <summary>
        /// Resolves the given descriptor type to a descriptor
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <returns>The descriptor instance, if any</returns>
        protected T D<T>() where T : DescriptorBase
        {
            return Language?.Descriptor<T>();
        }

        /// <summary>
        /// Called by the graphical language setup to initialize the layout described by this descriptor
        /// </summary>
        protected internal abstract void DefineLayout();

        /// <summary>
        /// Calculates edge hints for this rule
        /// </summary>
        /// <returns>A collection of edge hints</returns>
        protected internal abstract IEnumerable<EdgeTypeHint> CalculateEdgeHints();

        /// <summary>
        /// Calculates shape hints for this rule
        /// </summary>
        /// <returns>A collection of shape hints</returns>
        protected internal abstract IEnumerable<ShapeTypeHint> CalculateShapeHints();
    }
}

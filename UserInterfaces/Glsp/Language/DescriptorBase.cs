using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
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
        /// Creates the graph for the given semantic root element
        /// </summary>
        /// <param name="semanticRoot">The semantic root element</param>
        /// <param name="diagram">The notation diagram</param>
        /// <param name="trace">A skeleton trace</param>
        /// <returns>The Graph instance</returns>
        protected internal abstract GGraph CreateGraph(object semanticRoot, IDiagram diagram, ISkeletonTrace trace);

        /// <summary>
        /// Called by the graphical language setup to initialize the layout described by this descriptor
        /// </summary>
        protected internal abstract void DefineLayout();

        /// <summary>
        /// Calculates type hints for this rule
        /// </summary>
        /// <returns>A collection of type hints</returns>
        protected internal abstract IEnumerable<TypeHint> CalculateTypeHints();

        internal abstract GElementSkeletonBase GetRootSkeleton();
    }
}

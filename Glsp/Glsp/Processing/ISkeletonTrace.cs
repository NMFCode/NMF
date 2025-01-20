using NMF.Glsp.Graph;

namespace NMF.Glsp.Processing
{
    /// <summary>
    /// Denotes a trace for GLSP skeletons
    /// </summary>
    public interface ISkeletonTrace
    {
        /// <summary>
        /// Finds the graphical element created for the given semantic element and the given skeleton
        /// </summary>
        /// <param name="element">the semantic element</param>
        /// <param name="skeleton">the skeleton</param>
        /// <returns>the graphical element or null, if it was not found</returns>
        GElement ResolveElement(object element, object skeleton);

        /// <summary>
        /// Traces the given combination of semantic element and graphical element
        /// </summary>
        /// <param name="element">the semantic element</param>
        /// <param name="gElement">the graphical element</param>
        void Trace(object element, GElement gElement);

        /// <summary>
        /// Finds the graphical element created for the given semantic element and the given skeleton and removes it from the trace
        /// </summary>
        /// <param name="element">the semantic element</param>
        /// <param name="skeleton">the skeleton</param>
        /// <returns>the graphical element or null, if it was not found</returns>
        GElement RemoveElement(object element, object skeleton);
    }
}
using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Contracts
{
    /// <summary>
    /// Denotes an interface for a component that can calculate layout information
    /// </summary>
    public interface ILayoutEngine
    {
        /// <summary>
        /// Calculates a layout for the given graph
        /// </summary>
        /// <param name="graph">the graph which must be laid out</param>
        void CalculateLayout(GGraph graph);

        /// <summary>
        /// Calculates a layout for the given elements
        /// </summary>
        /// <param name="elements">the elements that should be laid out</param>
        void CalculateLayout(IEnumerable<GElement> elements);
    }
}

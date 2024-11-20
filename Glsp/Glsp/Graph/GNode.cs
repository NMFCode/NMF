using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Graph
{
    /// <summary>
    /// Denotes a node
    /// </summary>
    public class GNode : GElement
    {
        /// <summary>
        /// Creates a new element
        /// </summary>
        public GNode() : base() { }

        /// <summary>
        /// Creates a new element
        /// </summary>
        /// <param name="id">The id of the new element</param>
        /// <remarks>If the id is null, a new id is generated</remarks>
        public GNode(string id) : base(id) { }
    }
}

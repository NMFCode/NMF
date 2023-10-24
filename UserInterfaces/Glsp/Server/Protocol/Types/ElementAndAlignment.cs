using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// The ElementAndAlignment type is used to associate a new alignment with a model element, which is referenced via its id.
    /// </summary>
    public class ElementAndAlignment
    {
        /// <summary>
         ///  The identifier of an element.
         /// </summary>
        public string ElementId { get; init; }

        /// <summary>
         ///  The new alignment of the element.
         /// </summary>
        public Point NewAlignment { get; init; }
    }
}

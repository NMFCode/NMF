using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// Type hints for shapes
    /// </summary>
    public class ShapeTypeHint : TypeHint
    {

        /// <summary>
         ///  Specifies whether the element can be resized.
         /// </summary>
        public bool Resizable { get; init; }

        /// <summary>
         ///  Specifies whether the element can be moved to another parent
         /// </summary>
        public bool Reparentable { get; init; }

        /// <summary>
         ///  The types of elements that can be contained by this element (if any)
         /// </summary>
        public string[] ContainableElementTypeIds { get; init; }
    }
}

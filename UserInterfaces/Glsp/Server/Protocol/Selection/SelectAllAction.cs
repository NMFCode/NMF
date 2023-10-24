using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Selection
{
    /// <summary>
    /// Used for selecting or deselecting all elements.
    /// </summary>
    public class SelectAllAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "allSelected";

        /// <summary>
         ///  If `select` is true, all elements are selected, otherwise they are deselected.
         /// </summary>
        public bool Select { get; set; }
    }
}

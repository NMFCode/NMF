using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Validation
{
    /// <summary>
    /// To remove markers for elements a client or server may send a DeleteMarkersAction with all markers that should be removed.
    /// </summary>
    public class DeleteMarkersAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "deleteMarkers";


        /// <summary>
         ///  The list of markers that should be deleted.
         /// </summary>
       public Marker[] Markers { get; init; }
    }
}

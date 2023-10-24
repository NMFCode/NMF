using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Clipboard
{
    /// <summary>
    /// Server response to a RequestClipboardDataAction containing the selected elements as clipboard-compatible format.
    /// </summary>
    public class SetClipboardDataAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setClipboardData";


        /// <summary>
         ///  The selected elements from the editor context as clipboard data.
         /// </summary>
        public IDictionary<string, string> ClipboardData { get; } = new Dictionary<string, string>();
    }
}

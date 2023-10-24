using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Context
{
    /// <summary>
    /// The SetContextActions is the response to a RequestContextActions containing all actions for the queried context.
    /// </summary>
    public class SetContextActions : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setContextActions";

        /// <summary>
         ///  The actions available in the queried context.
         /// </summary>
        public LabeledAction[] Actions { get; init; }

        /// <summary>
         ///  Custom arguments.
         /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}

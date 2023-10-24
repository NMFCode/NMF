using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// Sent from the server to the client in order to provide hints certain modifications are allowed for a specific element type.
    /// </summary>
    public class SetTypeHintsAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setTypeHints";

        /// <summary>
         ///  The hints for shape types.
         /// </summary>
        public ShapeTypeHint[] ShapeHints { get; init; }

        /// <summary>
         ///  The hints for edge types.
         /// </summary>
        public EdgeTypeHint[] EdgeHints { get; init; }
    }
}

using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Layout
{
    /// <summary>
    /// Centers the viewport on the elements with the given identifiers. It changes the scroll setting of the viewport
    /// accordingly and resets the zoom to its default. This action can also be created on the client but it can also 
    /// be sent by the server in order to perform such a viewport change remotely.
    /// </summary>
    public class CenterAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "center";

        /// <summary>
         ///  The identifier of the elements on which the viewport should be centered.
         /// </summary>
        public string[] ElementIds { get; set; }

        /// <summary>
         ///  Indicate if the modification of the viewport should be realized with or without support of animations.
         /// </summary>
        public bool Animate { get; set; } = true;

        /// <summary>
         ///  Indicates whether the zoom level should be kept.
         /// </summary>
        public bool RetainZoom { get; set; } = false;
    }
}

using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Layout
{
    /// <summary>
    /// Triggers to fit all or a list of elements into the available drawing area. The resulting fit-to-screen command 
    /// changes the zoom and scroll settings of the viewport so the model can be shown completely. This action can also 
    /// be sent from the server to the client in order to perform such a viewport change programmatically.
    /// </summary>
    public class FitToScreenAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "fit";


        /// <summary>
         ///  The identifier of the elements to fit on screen.
         /// </summary>
        public string[] ElementIds { get; set; }

        /// <summary>
         ///  The padding that should be visible on the viewport.
         /// </summary>
        public double? Padding { get; set; }

        /// <summary>
         ///  The max zoom level authorized.
         /// </summary>
        public double? MaxZoom { get; set; }

        /// <summary>
         ///  Indicate if the action should be performed with animation support or not.
         /// </summary>
        public bool Animate { get; set; } = true;
    }
}

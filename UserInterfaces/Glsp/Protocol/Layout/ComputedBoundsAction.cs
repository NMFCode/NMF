using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;

namespace NMF.Glsp.Protocol.Layout
{
    /// <summary>
    /// Sent from the client to the server to transmit the result of bounds computation as a response 
    /// to a RequestBoundsAction. If the server is responsible for parts of the layout, it can do so 
    /// after applying the computed bounds received with this action. Otherwise there is no need to 
    /// send the computed bounds to the server, so they can be processed locally by the client.
    /// </summary>
    public class ComputedBoundsAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ComputedBoundsActionKind = "computedBounds";

        /// <inheritdoc/>
        public override string Kind => ComputedBoundsActionKind;


        /// <summary>
        ///  The new bounds of the model elements.
        /// </summary>
        public ElementAndBounds[] Bounds { get; set; }

        //// 
        ///  The revision number.
        /// </summary>
        public int? revision { get; set; }

        /// <summary>
        ///  The new alignment of the model elements.
        /// </summary>
        public ElementAndAlignment[] Alignments { get; set; }

        /// <summary>
        ///  The route of the model elements.
        /// </summary>
        public ElementAndRoutingPoints[] Routes { get; set; }
    }
}

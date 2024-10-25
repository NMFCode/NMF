using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// ent from the server to the client in order to set the model. If a model is already present, it is replaced.
    /// </summary>
    public class SetModelAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetModelActionKind = "setModel";

        /// <inheritdoc/>
        public override string Kind => SetModelActionKind;

        /// <summary>
        ///  The new graphical model elements.
        /// </summary>
        public GGraph NewRoot { get; set; }
    }
}

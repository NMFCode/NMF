using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// Sent from the server to the client in order to update the model. If no model is present yet, this behaves 
    /// the same as a SetModelAction. The transition from the old model to the new one can be animated.
    /// </summary>
    public class UpdateModelAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string UpdateModelActionKind = "updateModel";

        /// <inheritdoc/>
        public override string Kind => UpdateModelActionKind;


        /// <summary>
        ///  The new root element of the graphical model.
        /// </summary>
        public GGraph NewRoot { get; set; }

        /// <summary>
        ///  Boolean flag to indicate wether updated/changed elements should be animated in the diagram.
        /// </summary>
        public bool Animate { get; set; }
    }
}

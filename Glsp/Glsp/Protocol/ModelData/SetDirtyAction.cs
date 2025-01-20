using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// The server sends a SetDirtyStateAction to indicate to the client that the current model state on the server 
    /// does not correspond to the persisted model state of the source model. A client may ignore such an action or 
    /// use it to indicate to the user the dirty state.
    /// </summary>
    public class SetDirtyAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetDirtyActionKind = "setDirtyState";

        /// <inheritdoc/>
        public override string Kind => SetDirtyActionKind;


        /// <summary>
        ///  True if the current model state is dirty
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        ///  A string indicating the reason for the dirty state change e.g 'operation', 'undo' ...
        /// </summary>
        public string Reason { get; set; }
    }
}

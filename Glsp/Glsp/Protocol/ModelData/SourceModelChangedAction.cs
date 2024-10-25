using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// Sent from the server to the client in order to indicate that the source model has changed. 
    /// The source model denotes the data source from which the diagram has been originally derived 
    /// (such as a file, a database, etc.). Typically clients would react to such an action by asking 
    /// the user whether she wants to reload the diagram or ignore the changes and continue editing. 
    /// If the editor has no changes (i.e. is not dirty), clients may also choose to directly refresh 
    /// the editor by sending a RequestModelAction.
    /// </summary>
    public class SourceModelChangedAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SourceModelChangedActionKind = "sourceModelChanged";

        /// <inheritdoc/>
        public override string Kind => SourceModelChangedActionKind;

        /// <summary>
        ///  A human readable name of the source model (e.g. the file name).
        /// </summary>
        public string SourceModelName { get; set; }
    }
}

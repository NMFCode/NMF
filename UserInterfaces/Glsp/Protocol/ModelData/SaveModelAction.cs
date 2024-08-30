using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// Sent from the client to the server in order to persist the current model state back to the 
    /// source model. A new fileUri can be defined to save the model to a new destination different 
    /// from its original source model.
    /// </summary>
    public class SaveModelAction : ExecutableAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SaveModelActionKind = "saveModel";

        /// <inheritdoc/>
        public override string Kind => SaveModelActionKind;

        /// <summary>
        ///   The optional destination file uri.
        /// </summary>
        public string FileUri { get; set; }

        /// <inheritdoc />
        public override Task ExecuteAsync(IGlspSession session)
        {
            var uri = FileUri != null ? new Uri(FileUri, UriKind.RelativeOrAbsolute) : null;
            session.Save(uri);
            return Task.CompletedTask;
        }
    }
}

using System;
using System.Threading.Tasks;
using LspTypes;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <summary>
        ///     Sends the <c>window/showDocument</c> request to the client.
        /// </summary>
        /// <param name="uri">The URI of the document to show.</param>
        /// <param name="selection">The optional selection range in the document.</param>
        /// <param name="external">If true, requests to open the document externally.</param>
        /// <param name="takeFocus">If true, requests the client to take focus.</param>
        protected internal async Task ShowDocument(string uri, Range selection = null, bool external = false,
            bool takeFocus = false)
        {
            if (!_clientCapabilities.Window.ShowDocument.Support)
            {
                _ = SendLogMessage(MessageType.Warning, "Client does not support ShowDocument.");
                return;
            }

            var showDocumentParams = new ShowDocumentParams
            {
                Uri = uri,
                Selection = selection,
                External = external,
                TakeFocus = takeFocus
            };
            
            try
            {
                var result =
                    await _rpc.InvokeWithParameterObjectAsync<ShowDocumentResult>(MethodConstants.WindowShowDocument,
                        showDocumentParams);
                _ = SendLogMessage(MessageType.Info,
                    result.Success ? $"Success ShowDocument {uri}" : $"Failed To ShowDocument {uri}");
            }
            catch (Exception e)
            {
                _ = SendLogMessage(MessageType.Error, e.Message);
            }
        }
    }
}
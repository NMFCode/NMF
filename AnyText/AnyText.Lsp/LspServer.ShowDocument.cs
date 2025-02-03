using System;
using LspTypes;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private async void ShowDocument(string uri, Range selection = null, bool external = false,
            bool takeFocus = false)
        {
            if (!_clientCapabilities.Window.ShowDocument.Support)
            {
                SendLogMessage(MessageType.Warning, "Client does not support ShowDocument.");
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
                SendLogMessage(MessageType.Info,
                    result.Success ? $"Success ShowDocument {uri}" : $"Failed To ShowDocument {uri}");
            }
            catch (Exception e)
            {
                SendLogMessage(MessageType.Error, e.Message);
            }
        }
    }
}
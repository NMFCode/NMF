using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LspTypes;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private async Task<MessageActionItem> ShowMessageRequestAsync(string message, MessageType messageType,
            IEnumerable<string> titles)
        {
            var supportsMessageAction =
                _clientCapabilities.Window.ShowMessage.AnnotationId.AdditionalPropertiesSupport == true;
            if (!supportsMessageAction)
            {
                SendLogMessage(MessageType.Warning, "Client does not support ShowMessageRequest");
                return null;
            }

            var messageParams = new ShowMessageRequestParams
            {
                Message = message,
                Type = (int)messageType,
                Actions = titles.Select(title => new MessageActionItem
                {
                    Title = title
                }).ToArray()
            };
            
            try
            {
                var result =
                    await _rpc.InvokeWithParameterObjectAsync<MessageActionItem>(Methods.WindowShowMessageRequestName,
                        messageParams);
                return result;
            }
            catch (Exception e)
            {
                SendLogMessage(MessageType.Error, e.Message);
                return null;
            }
        }

        private void ShowMessageNotify(string message, MessageType messageType)
        {
            var messageParams = new ShowMessageParams
            {
                Message = message,
                MessageType = messageType
            };

            _rpc.NotifyWithParameterObjectAsync(Methods.WindowShowMessageName, messageParams);
        }
    }
}
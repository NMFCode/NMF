using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LspTypes;

using MessageType = LspTypes.MessageType;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <summary>
        /// Sends a message request <c>window/showMessageRequest</c> to the client and awaits a response.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="messageType">The type of message (Info, Warning, Error, Log).</param>
        /// <param name="titles">A collection of action button titles the user can choose from.</param>
        /// <returns>The selected <see cref="MessageActionItem"/> if the client supports the request; otherwise, null.</returns>
        protected internal async Task<MessageActionItem> ShowMessageRequestAsync(string message, MessageType messageType,
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
        
        /// <summary>
        /// Sends a message notification <c>window/showMessage</c> to the client without expecting a response.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="messageType">The type of message (Info, Warning, Error, Log).</param>
        protected internal Task ShowMessageNotify(string message, MessageType messageType)
        {
            var messageParams = new ShowMessageParams
            {
                Message = message,
                MessageType = ConvertMessageType(messageType)
            };

            return _rpc.NotifyWithParameterObjectAsync(Methods.WindowShowMessageName, messageParams);
        }
    }
}
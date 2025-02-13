using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class LspCommandArguments : ExecuteCommandArguments
    {
        private readonly LspServer _lsp;

        public LspCommandArguments(LspServer lsp)
        {
            _lsp = lsp;
        }

        public override Task SendLog(string message, MessageType messageType = MessageType.Info)
        {
            return _lsp.SendLogMessage(messageType, message);
        }

        public override Task ShowDocument(string uri, ParseRange? selection = null, bool external = false, bool takeFocus = false)
        {
            return _lsp.ShowDocument(uri, null, external, takeFocus);
        }

        public async override Task<string> ShowRequest(string message, MessageType messageType = MessageType.Info, params string[] buttons)
        {
            var answer = await _lsp.ShowMessageRequestAsync(message, messageType, buttons);
            return answer.Title;
        }

        public override Task ShowNotification(string message, MessageType messageType = MessageType.Info)
        {
            return _lsp.ShowMessageNotify(message, messageType);
        }
    }
}

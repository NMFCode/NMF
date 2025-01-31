using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        ///     Handles the <c>textDocument/rename</c> request from the client. This is used to determine the locations of a symbol
        ///     in a workspace to perform a workspace-wide rename.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (RenameParams)</param>
        /// <returns>A <see cref="WorkspaceEdit" /> object containing the changes to be performed by the client.</returns>
        [JsonRpcMethod(Methods.TextDocumentRenameName)]
        WorkspaceEdit QueryRenameWorkspaceEdit(JToken arg);
    }
}

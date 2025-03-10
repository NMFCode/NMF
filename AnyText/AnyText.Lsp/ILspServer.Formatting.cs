using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        /// Handles the <c>textDocument/formatting</c> request from the client.
        /// </summary>
        /// <param name="textDocument">The identifier of the text document to be formatted.</param>
        /// <param name="options">The formatting options provided by the client, such as indentation and spacing settings.</param>
        /// <returns>An array of <see cref="TextEdit"/> objects representing the formatting changes.</returns>
        [JsonRpcMethod(Methods.TextDocumentFormattingName)]
        LspTypes.TextEdit[] Formatting(TextDocumentIdentifier textDocument, FormattingOptions options);

        /// <summary>
        /// Handles the <c>textDocument/rangeFormatting</c> request from the client.
        /// </summary>
        /// <param name="textDocument">The identifier of the text document to be formatted.</param>
        /// <param name="range">The range within the document that should be formatted.</param>
        /// <param name="options">The formatting options provided by the client, such as indentation and spacing settings.</param>
        /// <returns>An array of <see cref="TextEdit"/> objects representing the formatting changes within the specified range.</returns>
        [JsonRpcMethod(Methods.TextDocumentRangeFormattingName)]
        LspTypes.TextEdit[] FormattingRange(TextDocumentIdentifier textDocument, Range range, FormattingOptions options);
    }
}
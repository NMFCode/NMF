using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        ///     Handles the <c>textDocument/semanticTokens/full</c> request from the client. This is used to retrieve all semantic
        ///     tokens for a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SemanticTokensParams)</param>
        /// <returns>A <see cref="SemanticTokens" /> object containing the full set of semantic tokens for the document.</returns>
        [JsonRpcMethod(Methods.TextDocumentSemanticTokensFull)]
        SemanticTokens QuerySemanticTokens(JToken arg);

        /// <summary>
        ///     Handles the <c>textDocument/semanticTokens/full/delta</c> request from the client. This is used to retrieve only
        ///     the changes (delta) in semantic tokens for a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SemanticTokensDeltaParams)</param>
        /// <returns>A <see cref="SemanticTokensDelta" /> object containing only the delta of semantic tokens for the document.</returns>
        [JsonRpcMethod(Methods.TextDocumentSemanticTokensFullDelta)]
        SemanticTokensDelta QuerySemanticTokensDelta(JToken arg);
        
        /// <summary>
        ///     Handles the <c>textDoocument/semanticTokens/range</c> request from the client. This is used to retrieve semantic
        ///     tokens within a specific range in the document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the range request. (SemanticTokensRangeParams)</param>
        /// <returns>A <see cref="SemanticTokens" /> object containing the semantic tokens within the specified range.</returns>
        [JsonRpcMethod(Methods.TextDocumentSemanticTokensRange)]
        SemanticTokens QuerySemanticTokensRange(JToken arg);
    }
}
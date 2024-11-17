using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private readonly Dictionary<string, uint[]> _previousSemanticTokens = new();

        /// <summary>
        ///     Handles the <c>textDocument/semanticTokens/full</c> request from the client. This is used to retrieve all semantic
        ///     tokens for a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SemanticTokensParams)</param>
        /// <returns>A <see cref="SemanticTokens" /> object containing the full set of semantic tokens for the document.</returns>
        public SemanticTokens QuerySemanticTokens(JToken arg)
        {
            var semanticTokensParams = arg.ToObject<SemanticTokensParams>();
            var uri = semanticTokensParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
                return new SemanticTokens { ResultId = null, Data = Array.Empty<uint>() };

            var semanticElements = document.GetSemanticElementsFromRoot().ToArray();

            _previousSemanticTokens[uri] = semanticElements;

            return new SemanticTokens
            {
                ResultId = Guid.NewGuid().ToString(),
                Data = semanticElements
            };
        }

        /// <summary>
        ///     Handles the <c>textDocument/semanticTokens/full/delta</c> request from the client. This is used to retrieve only
        ///     the changes (delta) in semantic tokens for a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SemanticTokensDeltaParams)</param>
        /// <returns>A <see cref="SemanticTokensDelta" /> object containing only the delta of semantic tokens for the document.</returns>
        public SemanticTokensDelta QuerySemanticTokensDelta(JToken arg)
        {
            var semanticTokensParams = arg.ToObject<SemanticTokensDeltaParams>();
            var uri = semanticTokensParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
                return new SemanticTokensDelta { ResultId = null, Edits = Array.Empty<SemanticTokensEdit>() };
            _previousSemanticTokens.TryGetValue(uri, out var previousTokens);
            var currentTokens = document.GetSemanticElementsFromRoot().ToArray();

            var edits = new List<SemanticTokensEdit>();
            int prevIndex = 0, currentIndex = 0;

            while (prevIndex < previousTokens.Length || currentIndex < currentTokens.Length)
                if (prevIndex < previousTokens.Length && currentIndex < currentTokens.Length)
                {
                    if (currentTokens[currentIndex] != previousTokens[prevIndex])
                    {
                        // Generate an edit if the tokens are different
                        var edit = new SemanticTokensEdit
                        {
                            Start = (uint)currentIndex,
                            DeleteCount = 1,
                            Data = new[] { currentTokens[currentIndex] }
                        };
                        edits.Add(edit);
                        prevIndex++;
                        currentIndex++;
                    }
                    else
                    {
                        prevIndex++;
                        currentIndex++;
                    }
                }
                else if (prevIndex < previousTokens.Length)
                {
                    // If there are more tokens in the previous set, remove them
                    var edit = new SemanticTokensEdit
                    {
                        Start = (uint)currentIndex,
                        DeleteCount = (uint)(previousTokens.Length - prevIndex),
                        Data = Array.Empty<uint>()
                    };
                    edits.Add(edit);
                    break;
                }
                else if (currentIndex < currentTokens.Length)
                {
                    // If there are more tokens in the current set, add them
                    var edit = new SemanticTokensEdit
                    {
                        Start = (uint)currentIndex,
                        DeleteCount = 0,
                        Data = currentTokens.Skip(currentIndex).ToArray()
                    };
                    edits.Add(edit);
                    break;
                }

            _previousSemanticTokens[uri] = currentTokens;

            return new SemanticTokensDelta
            {
                ResultId = Guid.NewGuid().ToString(),
                Edits = edits.ToArray()
            };
        }

        /// <summary>
        ///     Handles the <c>textDocument/semanticTokens/range</c> request from the client. This is used to retrieve semantic
        ///     tokens within a specific range in the document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the range request. (SemanticTokensRangeParams)</param>
        /// <returns>A <see cref="SemanticTokens" /> object containing the semantic tokens within the specified range.</returns>
        public SemanticTokens QuerySemanticTokensRange(JToken arg)
        {
            var semanticTokensRangeParams = arg.ToObject<SemanticTokensRangeParams>();
            var uri = semanticTokensRangeParams.TextDocument.Uri;
            var range = semanticTokensRangeParams.Range;

            if (!_documents.TryGetValue(uri, out var document))
                return new SemanticTokens { ResultId = null, Data = Array.Empty<uint>() };


            var allTokens = document.GetSemanticElementsFromRoot().ToArray();

            var filteredTokens = new List<uint>();
            uint line = 0;
            uint startChar = 0;

            for (var i = 0; i < allTokens.Length; i += 5)
            {
                line += allTokens[i];
                startChar = allTokens[i] == 0 ? startChar + allTokens[i + 1] : allTokens[i + 1];

                var length = allTokens[i + 2];
                var tokenType = allTokens[i + 3];
                var modifiers = allTokens[i + 4];

                var inRange = line >= range.Start.Line && line <= range.End.Line &&
                              (line > range.Start.Line || startChar >= range.Start.Character) &&
                              (line < range.End.Line || startChar + length <= range.End.Character);

                if (inRange)
                {
                    filteredTokens.Add(allTokens[i]);
                    filteredTokens.Add(allTokens[i + 1]);
                    filteredTokens.Add(length);
                    filteredTokens.Add(tokenType);
                    filteredTokens.Add(modifiers);
                }
            }

            return new SemanticTokens
            {
                ResultId = Guid.NewGuid().ToString(),
                Data = filteredTokens.ToArray()
            };
        }

        /// <summary>
        ///     Creates the registration for semantic token requests.
        /// </summary>
        /// <param name="languageId">The language identifier for the document.</param>
        /// <param name="parser">The parser used to extract token types and modifiers.</param>
        /// <returns>A <see cref="Registration" /> object with semantic token registration options.</returns>
        private Registration CreateSemanticTokenRegistration(string languageId, Parser parser)
        {
            var registrationOptions = new SemanticTokensRegistrationOptions
            {
                DocumentSelector = new[]
                {
                    new DocumentFilter
                    {
                        Language = languageId
                    }
                },
                Full = new { delta = true },
                Range = true,
                Legend = new SemanticTokensLegend
                {
                    tokenTypes = parser.Context.Grammar.TokenTypes,
                    tokenModifiers = parser.Context.Grammar.TokenModifiers
                }
            };
            return new Registration
            {
                RegisterOptions = registrationOptions,
                Id = Guid.NewGuid().ToString(),
                Method = "textDocument/semanticTokens"
            };
        }
    }
}
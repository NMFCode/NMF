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

            var resultId = Guid.NewGuid().ToString();
            _previousSemanticTokens[resultId] = semanticElements;

            return new SemanticTokens
            {
                ResultId = resultId,
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
            _previousSemanticTokens.TryGetValue(semanticTokensParams.PreviousResultId, out var previousTokens);
            
            var currentTokens = document
                .GetSemanticElementsFromRoot().ToArray();
            _previousSemanticTokens[semanticTokensParams.PreviousResultId] = currentTokens;

            var originalLength = previousTokens?.Length ?? 0;
            var modifiedLength = currentTokens.Length;

            var startIndex = 0;

            // Find the matching prefix
            while (startIndex < originalLength && startIndex < modifiedLength &&
                   previousTokens[startIndex] == currentTokens[startIndex])
                startIndex++;

            // If both sequences have differences
            if (startIndex < originalLength && startIndex < modifiedLength)
            {
                var originalEndIndex = originalLength - 1;
                var modifiedEndIndex = modifiedLength - 1;

                // Find the matching suffix
                while (originalEndIndex >= startIndex && modifiedEndIndex >= startIndex &&
                       previousTokens[originalEndIndex] == currentTokens[modifiedEndIndex])
                {
                    originalEndIndex--;
                    modifiedEndIndex--;
                }

                // Adjust indices if they moved behind the start index
                if (originalEndIndex < startIndex || modifiedEndIndex < startIndex)
                {
                    originalEndIndex++;
                    modifiedEndIndex++;
                }

                var deleteCount = originalEndIndex - startIndex + 1;
                var newData = currentTokens.Skip(startIndex).Take(modifiedEndIndex - startIndex + 1).ToArray();

                // Handle edge case: single new data item matches the last original token
                if (newData.Length == 1 && previousTokens[originalEndIndex] == newData[0])
                    return new SemanticTokensDelta
                    {
                        ResultId = Guid.NewGuid().ToString(),
                        Edits = new[]
                        {
                            new SemanticTokensEdit
                            {
                                Start = (uint)startIndex,
                                DeleteCount = (uint)(deleteCount - 1)
                            }
                        }
                    };

                // Return edits for both delete and replace
                return new SemanticTokensDelta
                {
                    ResultId = Guid.NewGuid().ToString(),
                    Edits = new[]
                    {
                        new SemanticTokensEdit
                        {
                            Start = (uint)startIndex,
                            DeleteCount = (uint)deleteCount,
                            Data = newData
                        }
                    }
                };
            }

            // If there are only new tokens to add
            if (startIndex < modifiedLength)
                return new SemanticTokensDelta
                {
                    ResultId = Guid.NewGuid().ToString(),
                    Edits = new[]
                    {
                        new SemanticTokensEdit
                        {
                            Start = (uint)startIndex,
                            DeleteCount = 0,
                            Data = currentTokens.Skip(startIndex).ToArray()
                        }
                    }
                };

            // If there are only tokens to delete
            if (startIndex < originalLength)
                return new SemanticTokensDelta
                {
                    ResultId = Guid.NewGuid().ToString(),
                    Edits = new[]
                    {
                        new SemanticTokensEdit
                        {
                            Start = (uint)startIndex,
                            DeleteCount = (uint)(originalLength - startIndex)
                        }
                    }
                };

            // If no changes, return an empty delta
            return new SemanticTokensDelta
            {
                ResultId = Guid.NewGuid().ToString(),
                Edits = Array.Empty<SemanticTokensEdit>()
            };
        }

        /// <summary>
        ///     Handles the <c>textDoocument/semanticTokens/range</c> request from the client. This is used to retrieve semantic
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


            var tokens = document.GetSemanticElementsFromRoot(AsParsePosition(range.Start), AsParsePosition(range.End))
                .ToArray();

            return new SemanticTokens
            {
                ResultId = Guid.NewGuid().ToString(),
                Data = tokens
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
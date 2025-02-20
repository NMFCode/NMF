using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using NMF.AnyText.Grammars;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private readonly Dictionary<string, uint[]> _previousSemanticTokens = new();

        /// <inheritdoc cref="ILspServer.QuerySemanticTokens"/>
        public SemanticTokens QuerySemanticTokens(JToken arg)
        {
            _ = SendLogMessage(MessageType.Info, "Received request for full semantic tokens.");
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
        /// <inheritdoc cref="ILspServer.QuerySemanticTokensDelta"/>
        public SemanticTokensDelta QuerySemanticTokensDelta(JToken arg)
        {
            _ = SendLogMessage(MessageType.Info, "Received request for semantic tokens delta.");
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

            var startIndex = FindMatchingPrefix(previousTokens, currentTokens, originalLength, modifiedLength);

            if (startIndex < originalLength && startIndex < modifiedLength)
            {
                return HandleDifferences(previousTokens, currentTokens, startIndex, originalLength, modifiedLength);
            }

            if (startIndex < modifiedLength)
            {
                return HandleAddedTokens(currentTokens, startIndex);
            }

            if (startIndex < originalLength)
            {
                return HandleRemovedTokens(originalLength, startIndex);
            }

            return new SemanticTokensDelta
            {
                ResultId = Guid.NewGuid().ToString(),
                Edits = Array.Empty<SemanticTokensEdit>()
            };
        }

        /// <inheritdoc cref="ILspServer.QuerySemanticTokensDelta"/>
        public SemanticTokens QuerySemanticTokensRange(JToken arg)
        {
            _ = SendLogMessage(MessageType.Info, "Received request for semantic tokens in a range.");
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
        /// <param name="language">The language identifier for the Registration.</param>
        /// <returns>A <see cref="Registration" /> object with semantic token registration options.</returns>
        private Registration CreateSemanticTokenRegistration(Grammar language)
        {
            var registrationOptions = new SemanticTokensRegistrationOptions
            {
                DocumentSelector = new[]
                {
                    new DocumentFilter
                    {
                        Language = language.LanguageId
                    }
                },
                Full = true,
                Range = true,
                Legend = new SemanticTokensLegend
                {
                    tokenTypes = language.TokenTypes,
                    tokenModifiers = language.TokenModifiers
                }
            };
            return new Registration
            {
                RegisterOptions = registrationOptions,
                Id = Guid.NewGuid().ToString(),
                Method = MethodConstants.RegisterSemanticTokens
            };
        }
        
        private int FindMatchingPrefix(uint[] previousTokens, uint[] currentTokens, int previousLength, int currentLength)
        {
            int startIndex = 0;
            while (startIndex < previousLength && startIndex < currentLength &&
                   previousTokens[startIndex] == currentTokens[startIndex])
            {
                startIndex++;
            }
            return startIndex;
        }
        private SemanticTokensDelta HandleDifferences(uint[] previousTokens, uint[] currentTokens, int startIndex, int originalLength, int modifiedLength)
        {
            var originalEndIndex = originalLength - 1;
            var modifiedEndIndex = modifiedLength - 1;

            while (originalEndIndex >= startIndex && modifiedEndIndex >= startIndex &&
                   previousTokens[originalEndIndex] == currentTokens[modifiedEndIndex])
            {
                originalEndIndex--;
                modifiedEndIndex--;
            }

            if (originalEndIndex < startIndex || modifiedEndIndex < startIndex)
            {
                originalEndIndex++;
                modifiedEndIndex++;
            }

            var deleteCount = originalEndIndex - startIndex + 1;
            var newData = currentTokens.Skip(startIndex).Take(modifiedEndIndex - startIndex + 1).ToArray();

            if (newData.Length == 1 && previousTokens[originalEndIndex] == newData[0])
            {
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
            }

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
        private SemanticTokensDelta HandleAddedTokens(uint[] currentTokens, int startIndex)
        {
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
        }
        
        private SemanticTokensDelta HandleRemovedTokens(int originalLength, int startIndex)
        {
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
        }
    }
}
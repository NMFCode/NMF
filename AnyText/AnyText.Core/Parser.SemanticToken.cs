using System;
using System.Collections.Generic;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        ///     Retrieves semantic elements from the root application with delta encoding for Language Server Protocol (LSP).
        /// </summary>
        /// <param name="start">
        ///     An optional <see cref="ParsePosition"/> specifying the starting range of tokens to include.
        ///     If null, all tokens from the beginning are considered.
        /// </param>
        /// <param name="end">
        ///     An optional <see cref="ParsePosition"/> specifying the ending range of tokens to include.
        ///     If null, all tokens to the end are considered.
        /// </param>
        /// <returns>
        ///     A list of semantic tokens represented as unsigned integers,
        ///     including delta-encoded line and character positions, length, token type, and modifiers.
        /// </returns>
        public IEnumerable<uint> GetSemanticElementsFromRoot(ParsePosition? start = null, ParsePosition? end = null)
        {
            var semanticTokens = new List<uint>();
            
            var rootApplication = Context.RootRuleApplication;
            uint previousLine = 0;
            uint previousStartChar = 0;
            ParsePosition? retryPosition = null;
            rootApplication.IterateLiterals(literalRuleApp =>
            {
                if (!literalRuleApp.IsActive)
                {
                    retryPosition = literalRuleApp.CurrentPosition;
                    return;
                }

                if (retryPosition is not null && literalRuleApp.CurrentPosition > retryPosition)
                {
                    return;
                }
                retryPosition = null;

                if (start.HasValue && end.HasValue && !IsTokenInRange(literalRuleApp.CurrentPosition, literalRuleApp.ExaminedTo, start.Value, end.Value))
                {
                    return;
                }
                
                var line = (uint)literalRuleApp.CurrentPosition.Line;
                var startChar = (uint)literalRuleApp.CurrentPosition.Col;
                var modifiers = GetTokenModifierIndexFromHierarchy(literalRuleApp) ?? 0;
                var tokenType = GetTokenTypeIndexFromHierarchy(literalRuleApp) ?? 0;
                
                if (literalRuleApp.Length.Line == 0)
                {
                    ProcessToken(semanticTokens, literalRuleApp.Literal, line, modifiers, tokenType, ref previousLine, ref previousStartChar, ref startChar);
                }
                else
                {
                    //split literal into lines for multiline literals to assign tokens (comments)
                    var lines = literalRuleApp.Literal.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    foreach (var lineText in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(lineText))
                        {
                            ProcessToken(semanticTokens, lineText, line, modifiers, tokenType, ref previousLine, ref previousStartChar, ref startChar);
                        }
                        line++;
                    }
                }
            });


            return semanticTokens;
        }

        private static void ProcessToken(List<uint> semanticTokens, string tokenText, uint lineIndex, uint modifiers, uint tokenType, ref uint previousLine, ref uint previousStartChar, ref uint startChar)
        {
            if (lineIndex < previousLine || (lineIndex == previousLine && startChar < previousStartChar))
            {
                return;
            }

            var currentLength = (uint)tokenText.Length;

            var deltaLine = lineIndex - previousLine;
            var deltaStartChar = deltaLine == 0 ? startChar - previousStartChar : startChar;

            semanticTokens.Add(deltaLine);
            semanticTokens.Add(deltaStartChar);
            semanticTokens.Add(currentLength);
            semanticTokens.Add(tokenType);
            semanticTokens.Add(modifiers);

            previousLine = lineIndex;
            previousStartChar = startChar;

            startChar = 0;
        }

        /// <summary>
        ///     Checks whether a token's range is within a specified range.
        /// </summary>
        private static bool IsTokenInRange(ParsePosition start, ParsePositionDelta examinedTo, ParsePosition rangeStart, ParsePosition rangeEnd)
        {
            var tokenEndLine = start.Line + examinedTo.Line;
            var tokenEndChar = start.Col + examinedTo.Col;

            bool isStartInRange = (start.Line > rangeStart.Line || 
                                   (start.Line == rangeStart.Line && start.Col>= rangeStart.Col));

            bool isEndInRange = (tokenEndLine < rangeEnd.Line || 
                                 (tokenEndLine == rangeEnd.Line && tokenEndChar <= rangeEnd.Col));

            return isStartInRange && isEndInRange;
        }

        /// <summary>
        ///     Traverses the parent hierarchy of a <see cref="RuleApplication" /> to find the first non-null TokenModifierIndex.
        /// </summary>
        /// <param name="application">The starting <see cref="RuleApplication" /> from which to begin the traversal.</param>
        /// <returns>
        ///     The first non-null TokenModifierIndex encountered in the hierarchy, or <c>null</c> if no non-null index is
        ///     found.
        /// </returns>
        private static uint? GetTokenModifierIndexFromHierarchy(RuleApplication application)
        {
            var currentApplication = application.Parent;
            uint? result = application.Rule.TokenModifierIndex;
            while (currentApplication != null && currentApplication.CurrentPosition == application.CurrentPosition)
            {
                var rule = currentApplication.Rule;
                if (rule.TokenModifierIndex != null)
                {
                    result = rule.TokenModifierIndex;
                }
                currentApplication = currentApplication.Parent;
            }

            return result;
        }

        /// <summary>
        ///     Traverses the parent hierarchy of a <see cref="RuleApplication" /> to find the first non-null TokenTypeIndex.
        /// </summary>
        /// <param name="application">The starting <see cref="RuleApplication" /> from which to begin the traversal.</param>
        /// <returns>The first non-null TokenTypeIndex encountered in the hierarchy, or <c>null</c> if no non-null index is found.</returns>
        private static uint? GetTokenTypeIndexFromHierarchy(RuleApplication application)
        {
            var currentApplication = application.Parent;
            uint? result = application.Rule.TokenTypeIndex;
            while (currentApplication != null && currentApplication.CurrentPosition == application.CurrentPosition)
            {
                var rule = currentApplication.Rule;
                if (rule.TokenTypeIndex != null)
                {
                    result = rule.TokenTypeIndex;
                }
                currentApplication = currentApplication.Parent;
            }

            return result;
        }
    }
}
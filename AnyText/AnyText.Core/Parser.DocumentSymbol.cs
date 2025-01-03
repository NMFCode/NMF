using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Rules;
using System.Diagnostics;
using NMF.AnyText.Model;
using System.Text.RegularExpressions;

namespace NMF.AnyText
{
    public partial class Parser
    {
        private static Regex regex = new Regex(@"(\w+) '(\w+)'");

        public ParsedDocumentSymbol[] GetDocumentSymbolsFromRoot()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            if (rootApplication.IsPositive)
            {
                var parsedDocumentSymbols = GetDocumentSymbols(rootApplication);
                return parsedDocumentSymbols;
            }

            return Array.Empty<ParsedDocumentSymbol>();
        }

        private ParsedDocumentSymbol[] GetDocumentSymbols(RuleApplication ruleApplication)
        {
            if (ruleApplication is MultiRuleApplication multiRuleApplication)
            {
                if (ruleApplication.Rule is OneOrMoreRule)
                {
                    return multiRuleApplication.Inner.Select(innerRuleApplication => GetDocumentSymbols(innerRuleApplication)).SelectMany(i => i).ToArray();
                }

                if (ruleApplication.Rule.SymbolKind == SymbolKind.Null)
                {
                    return Array.Empty<ParsedDocumentSymbol>();
                }

                var literal = ruleApplication.GetValue(Context).ToString();
                var match = regex.Match(literal);
                var type = match.Groups[1].Value;
                var name = match.Groups[2].Value;

                return new ParsedDocumentSymbol[]
{
                    new ParsedDocumentSymbol()
                    {
                        Name = name,
                        Detail = type,
                        Kind = multiRuleApplication.Rule.SymbolKind,
                        Tags = Array.Empty<SymbolTag>(),
                        Range = new DocumentSymbolRange()
                        {
                            Start = new DocumentSymbolPosition()
                            {
                                Line = (uint)multiRuleApplication.CurrentPosition.Line,
                                Character = (uint)multiRuleApplication.CurrentPosition.Col
                            },
                            End = new DocumentSymbolPosition()
                            {
                                Line = (uint)(multiRuleApplication.CurrentPosition.Line + multiRuleApplication.ExaminedTo.Line - 1),
                                Character = (uint)(multiRuleApplication.CurrentPosition.Col + multiRuleApplication.ExaminedTo.Col)
                            }
                        },
                        SelectionRange = new DocumentSymbolRange()
                        {
                            Start = new DocumentSymbolPosition()
                            {
                                Line = (uint)multiRuleApplication.CurrentPosition.Line,
                                Character = (uint)multiRuleApplication.CurrentPosition.Col
                            },
                            End = new DocumentSymbolPosition()
                            {
                                Line = (uint)(multiRuleApplication.CurrentPosition.Line + multiRuleApplication.ExaminedTo.Line - 1),
                                Character = (uint)(multiRuleApplication.CurrentPosition.Col + multiRuleApplication.ExaminedTo.Col)
                            }
                        },
                        Children = multiRuleApplication.Inner.Select(innerRuleApplication => GetDocumentSymbols(innerRuleApplication)).SelectMany(i => i).ToArray()
                    }
                };
            }
            else if (ruleApplication is SingleRuleApplication singleRuleApplication)
            {
                return GetDocumentSymbols(singleRuleApplication.Inner);
            }

            return Array.Empty<ParsedDocumentSymbol>();
        }

        public class ParsedDocumentSymbol
        {
            public string Name;
            public string? Detail;
            public SymbolKind Kind;
            public SymbolTag[]? Tags;
            public DocumentSymbolRange Range;
            public DocumentSymbolRange SelectionRange;
            public ParsedDocumentSymbol[]? Children;
        }

        public enum SymbolTag
        {
            Deprecated
        }

        public class DocumentSymbolRange
        {
            public DocumentSymbolPosition Start;
            public DocumentSymbolPosition End;
        }

        public class DocumentSymbolPosition
        {
            public uint Line;
            public uint Character;
        }
    }
}

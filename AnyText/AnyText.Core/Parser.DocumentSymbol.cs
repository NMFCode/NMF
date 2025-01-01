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

namespace NMF.AnyText
{
    public partial class Parser
    {
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

        private LiteralRuleApplication GetFirstLiteralRuleApplication(RuleApplication ruleApplication)
        {
            if (ruleApplication.Rule.IsLiteral)
            {
                return (LiteralRuleApplication)ruleApplication;
            }

            if (ruleApplication is MultiRuleApplication multiRuleApplication)
            {
                foreach (var inner in multiRuleApplication.Inner)
                {
                    var innerResult = GetFirstLiteralRuleApplication(inner);
                    if (innerResult != null)
                    {
                        return innerResult;
                    }
                }
            }

            if (ruleApplication is SingleRuleApplication singleRuleApplication)
            {
                return GetFirstLiteralRuleApplication(singleRuleApplication.Inner);
            }

            return null;
        }

        private ParsedDocumentSymbol[] GetDocumentSymbols(RuleApplication ruleApplication)
        {
            if (ruleApplication is MultiRuleApplication multiRuleApplication)
            {
                var firstLiteralRuleApplication = GetFirstLiteralRuleApplication(multiRuleApplication);

                if (firstLiteralRuleApplication == null || multiRuleApplication.Rule.SymbolKind == SymbolKind.Null)
                {
                    return Array.Empty<ParsedDocumentSymbol>();
                }

                return new ParsedDocumentSymbol[]
{
                    new ParsedDocumentSymbol()
                    {
                        Name = firstLiteralRuleApplication.Literal,
                        Detail = multiRuleApplication.Rule.ToString(),
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

                /*return new ParsedDocumentSymbol[]
                {
                    new ParsedDocumentSymbol()
                    {
                        Name = "singleruleapp",
                        Detail = "def",
                        Kind = SymbolKind.Package,
                        Tags = new SymbolTag[]
                        {
                            SymbolTag.Deprecated,
                        },
                        Range = new DocumentSymbolRange() { Start = new DocumentSymbolPosition() { Line = 0, Character = 0}, End = new DocumentSymbolPosition() { Line = 0, Character = 0}},
                        SelectionRange = new DocumentSymbolRange() { Start = new DocumentSymbolPosition() { Line = 0, Character = 0}, End = new DocumentSymbolPosition() { Line = 0, Character = 0}},
                        Children = GetDocumentSymbols(singleRuleApplication.Inner)
                    }
                };*/
            }
            /*else
            {
                return new ParsedDocumentSymbol[]
                {
                    new ParsedDocumentSymbol()
                    {
                        Name = "everythingelse",
                        Detail = "def",
                        Kind = SymbolKind.Package,
                        Tags = new SymbolTag[]
                        {
                            SymbolTag.Deprecated,
                        },
                        Range = new DocumentSymbolRange() { Start = new DocumentSymbolPosition() { Line = 0, Character = 0}, End = new DocumentSymbolPosition() { Line = 0, Character = 0}},
                        SelectionRange = new DocumentSymbolRange() { Start = new DocumentSymbolPosition() { Line = 0, Character = 0}, End = new DocumentSymbolPosition() { Line = 0, Character = 0}},
                        Children = Array.Empty<ParsedDocumentSymbol>()
                    }
                };
            }*/

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

        public enum SymbolKind
        {
            File = 1,
            Module = 2,
            Namespace = 3,
            Package = 4,
            Class = 5,
            Method = 6,
            Property = 7,
            Field = 8,
            Constructor = 9,
            Enum = 10,
            Interface = 11,
            Function = 12,
            Variable = 13,
            Constant = 14,
            String = 15,
            Number = 16,
            Boolean = 17,
            Array = 18,
            Object = 19,
            Key = 20,
            Null = 21,
            EnumMember = 22,
            Struct = 23,
            Event = 24,
            Operator = 25,
            TypeParameter = 26,
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

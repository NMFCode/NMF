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

        public DocumentSymbol[] GetDocumentSymbolsFromRoot()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            if (rootApplication.IsPositive)
            {
                var parsedDocumentSymbols = GetDocumentSymbols(rootApplication);
                return parsedDocumentSymbols;
            }

            return Array.Empty<DocumentSymbol>();
        }

        private DocumentSymbol[] GetDocumentSymbols(RuleApplication ruleApplication)
        {
            if (ruleApplication is MultiRuleApplication multiRuleApplication)
            {
                if (ruleApplication.Rule is OneOrMoreRule)
                {
                    return multiRuleApplication.Inner.Select(innerRuleApplication => GetDocumentSymbols(innerRuleApplication)).SelectMany(i => i).ToArray();
                }

                if (ruleApplication.Rule.SymbolKind == SymbolKind.Null)
                {
                    return Array.Empty<DocumentSymbol>();
                }

                var literal = ruleApplication.GetValue(Context).ToString();
                var match = regex.Match(literal);
                var type = match.Groups[1].Value;
                var name = match.Groups[2].Value;

                return new DocumentSymbol[]
{
                    new DocumentSymbol()
                    {
                        Name = name,
                        Detail = type,
                        Kind = multiRuleApplication.Rule.SymbolKind,
                        Tags = Array.Empty<SymbolTag>(),
                        Range = new Range()
                        {
                            Start = new Position()
                            {
                                Line = (uint)multiRuleApplication.CurrentPosition.Line,
                                Character = (uint)multiRuleApplication.CurrentPosition.Col
                            },
                            End = new Position()
                            {
                                Line = (uint)(multiRuleApplication.CurrentPosition.Line + multiRuleApplication.ExaminedTo.Line - 1),
                                Character = (uint)(multiRuleApplication.CurrentPosition.Col + multiRuleApplication.ExaminedTo.Col)
                            }
                        },
                        SelectionRange = new Range()
                        {
                            Start = new Position()
                            {
                                Line = (uint)multiRuleApplication.CurrentPosition.Line,
                                Character = (uint)multiRuleApplication.CurrentPosition.Col
                            },
                            End = new Position()
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

            return Array.Empty<DocumentSymbol>();
        }
    }
}

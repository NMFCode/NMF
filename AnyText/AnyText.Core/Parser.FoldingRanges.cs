using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        public ParsedFoldingRange[] GetFoldingRangesFromRoot()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            if (rootApplication.IsPositive)
            {
                var parsedFoldingRanges = GetFoldingRangesFromSequence(rootApplication);
                return parsedFoldingRanges;
            }

            return Array.Empty<ParsedFoldingRange>();
        }

        private ParsedFoldingRange[] GetFoldingRangesFromSequence(RuleApplication ruleApplication)
        {
            var result = new List<ParsedFoldingRange>();

            if (ruleApplication.Comments != null)
            {
                foreach (var commentRuleApplication in ruleApplication.Comments)
                {
                    if (commentRuleApplication.Rule is MultilineCommentRule)
                    {
                        var commentsFoldingRange = new ParsedFoldingRange()
                        {
                            StartLine = (uint)commentRuleApplication.CurrentPosition.Line,
                            StartCharacter = (uint)commentRuleApplication.CurrentPosition.Col,
                            EndLine = (uint)(commentRuleApplication.CurrentPosition.Line + commentRuleApplication.ExaminedTo.Line),
                            EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can therefore lead to wrap around when casting to uint
                            Kind = "comment"
                        };

                        result.Add(commentsFoldingRange);
                    }
                }
            }

            if (ruleApplication.Rule is ZeroOrMoreRule zeroOrMoreRule)
            {
                if (zeroOrMoreRule.InnerRule.IsImports())
                {
                    var firstInner = (ruleApplication as MultiRuleApplication).Inner.First();
                    var lastInner = (ruleApplication as MultiRuleApplication).Inner.Last();

                    var importsFoldingRange = new ParsedFoldingRange()
                    {
                        StartLine = (uint)firstInner.CurrentPosition.Line,
                        StartCharacter = (uint)firstInner.CurrentPosition.Col,
                        EndLine = (uint)lastInner.CurrentPosition.Line,
                        EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can therefore lead to wrap around when casting to uint
                        Kind = "imports"
                    };

                    result.Add(importsFoldingRange);
                }
            }

            if (ruleApplication is MultiRuleApplication multiRuleApplication)
            {
                if (multiRuleApplication.Rule is SequenceRule sequenceRule)
                {
                    var firstInner = multiRuleApplication.Inner.First();
                    var lastInner = multiRuleApplication.Inner.Last();

                    if (sequenceRule.IsRegion())
                    {
                        var regionFoldingRange = new ParsedFoldingRange()
                        {
                            StartLine = (uint)firstInner.CurrentPosition.Line,
                            StartCharacter = (uint)firstInner.CurrentPosition.Col,
                            EndLine = (uint)lastInner.CurrentPosition.Line,
                            EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can therefore lead to wrap around when casting to uint
                            Kind = "region"
                        };

                        result.Add(regionFoldingRange);
                    }
                    else if (multiRuleApplication.Rule is ParanthesesRule || sequenceRule.IsFoldingRange())
                    {
                        var foldingRange = new ParsedFoldingRange()
                        {
                            StartLine = (uint)firstInner.CurrentPosition.Line,
                            StartCharacter = (uint)firstInner.CurrentPosition.Col,
                            EndLine = (uint)lastInner.CurrentPosition.Line,
                            EndCharacter = 0 // determining the end character using length or examinedTo is inconsistent and can therefore lead to wrap around when casting to uint
                        };

                        result.Add(foldingRange);
                    }
                }

                foreach (var innerRuleApplication in multiRuleApplication.Inner)
                {
                    var innerFoldingRanges = GetFoldingRangesFromSequence(innerRuleApplication);
                    result.AddRange(innerFoldingRanges);
                }
            }
            else if (ruleApplication is SingleRuleApplication singleRuleApplication)
            {
                var innerFoldingRanges = GetFoldingRangesFromSequence(singleRuleApplication.Inner);
                result.AddRange(innerFoldingRanges);
            }

            return result.ToArray();
        }

        public class ParsedFoldingRange
        {
            public uint StartLine;
            public uint? StartCharacter;
            public uint EndLine;
            public uint? EndCharacter;
            public string? Kind;
        }
    }
}

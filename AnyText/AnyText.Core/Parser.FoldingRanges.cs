using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

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
                result.AddRange(GetCommentFoldingRanges(ruleApplication));
            }

            if (ruleApplication.Rule is ZeroOrMoreRule zeroOrMoreRule && zeroOrMoreRule.InnerRule.IsImports())
            {
                result.Add(GetImportsFoldingRange(ruleApplication));
            }

            if (ruleApplication is MultiRuleApplication multiRuleApplication)
            {
                if (multiRuleApplication.Rule is SequenceRule sequenceRule)
                {
                    if (sequenceRule.IsRegion())
                    {
                        result.Add(GetRegionFoldingRange(multiRuleApplication));
                    }
                    else if (multiRuleApplication.Rule is ParanthesesRule || sequenceRule.IsFoldable())
                    {
                        result.Add(GetFoldingRange(multiRuleApplication));
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

        private ParsedFoldingRange[] GetCommentFoldingRanges(RuleApplication ruleApplication)
        {
            var result = new List<ParsedFoldingRange>();

            for (var i = 0; i < ruleApplication.Comments.Count; i++)
            {
                var commentRuleApplication = ruleApplication.Comments[i];

                if (commentRuleApplication.Rule is MultilineCommentRule)
                {
                    var commentsFoldingRange = new ParsedFoldingRange()
                    {
                        StartLine = (uint)commentRuleApplication.CurrentPosition.Line,
                        StartCharacter = (uint)commentRuleApplication.CurrentPosition.Col,
                        EndLine = (uint)(commentRuleApplication.CurrentPosition.Line + commentRuleApplication.ExaminedTo.Line),
                        EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can lead to wrap around when casting to uint
                        Kind = "comment"
                    };

                    result.Add(commentsFoldingRange);
                }
                else
                {
                    RuleApplication endCommentRuleApplication;
                    do
                    {
                        endCommentRuleApplication = ruleApplication.Comments[i++];
                    }
                    while (endCommentRuleApplication.Rule is not MultilineCommentRule
                        && endCommentRuleApplication.CurrentPosition.Col == commentRuleApplication.CurrentPosition.Col
                        && i < ruleApplication.Comments.Count);

                    if (commentRuleApplication == endCommentRuleApplication) continue;

                    var commentsFoldingRange = new ParsedFoldingRange()
                    {
                        StartLine = (uint)commentRuleApplication.CurrentPosition.Line,
                        StartCharacter = (uint)commentRuleApplication.CurrentPosition.Col,
                        EndLine = (uint)(endCommentRuleApplication.CurrentPosition.Line),
                        EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can lead to wrap around when casting to uint
                        Kind = "comment"
                    };

                    result.Add(commentsFoldingRange);
                }
            }

            return result.ToArray();
        }

        private ParsedFoldingRange GetImportsFoldingRange(RuleApplication ruleApplication)
        {
            var firstInner = (ruleApplication as MultiRuleApplication).Inner.First();
            var lastInner = (ruleApplication as MultiRuleApplication).Inner.Last();

            var importsFoldingRange = new ParsedFoldingRange()
            {
                StartLine = (uint)firstInner.CurrentPosition.Line,
                StartCharacter = (uint)firstInner.CurrentPosition.Col,
                EndLine = (uint)lastInner.CurrentPosition.Line,
                EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can lead to wrap around when casting to uint
                Kind = "imports"
            };

            return importsFoldingRange;
        }

        private ParsedFoldingRange GetRegionFoldingRange(MultiRuleApplication multiRuleApplication)
        {
            var firstInner = multiRuleApplication.Inner.First();
            var lastInner = multiRuleApplication.Inner.Last();

            var regionFoldingRange = new ParsedFoldingRange()
            {
                StartLine = (uint)firstInner.CurrentPosition.Line,
                StartCharacter = (uint)firstInner.CurrentPosition.Col,
                EndLine = (uint)lastInner.CurrentPosition.Line,
                EndCharacter = 0, // determining the end character using length or examinedTo is inconsistent and can lead to wrap around when casting to uint
                Kind = "region"
            };

            return regionFoldingRange;
        }

        private ParsedFoldingRange GetFoldingRange(MultiRuleApplication multiRuleApplication)
        {
            var firstInner = multiRuleApplication.Inner.First();
            var lastInner = multiRuleApplication.Inner.Last();

            var foldingRange = new ParsedFoldingRange()
            {
                StartLine = (uint)firstInner.CurrentPosition.Line,
                StartCharacter = (uint)firstInner.CurrentPosition.Col,
                EndLine = (uint)lastInner.CurrentPosition.Line,
                EndCharacter = 0 // determining the end character using length or examinedTo is inconsistent and can lead to wrap around when casting to uint
            };

            return foldingRange;
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

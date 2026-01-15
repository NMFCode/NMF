using AnyText.Tests.UniversalVariability;
using NMF.AnyText;
using NMF.AnyText.Rules;
using NMF.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Diffs
{
    [TestFixture]
    public class ChangeTrackerTests
    {
        [TestCase("busybox", 1)]
        [TestCase("busybox", 2)]
        [TestCase("busybox", 3)]
        [TestCase("busybox", 4)]
        [TestCase("busybox", 5)]
        [TestCase("busybox", 6)]
        [TestCase("busybox", 7)]
        [TestCase("busybox", 8)]
        [TestCase("busybox", 9)]
        [TestCase("busybox", 10)]
        [TestCase("busybox", 11)]
        [TestCase("automotive02", 1)]
        [TestCase("automotive02", 2)]
        [TestCase("automotive02", 3)]
        [TestCase("demo", 1)]
        public void ChangeTracker_CorrectlyIdentifiesObsoleteItems(string model, int baseVersion)
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion:00}.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);

            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion + 1:00}.diff")));
            var diffs = new List<TextEdit>();

            foreach (var line in diff)
            {
                diffs.Add(line);
                var positions = new List<RuleApplication>();
                for (int i = line.Start.Line; i < line.End.Line; i++)
                {
                    var pos = parser.Context.Matcher.NextTokenPosition(new ParsePosition(i, 0));
                    var ruleApplication = parser.Context.Matcher.GetLiteralAt(pos);
                    if (ruleApplication != null)
                    {
                        positions.Add(ruleApplication);
                    }
                }
                parser.Context.Matcher.Apply(line);
                parser.Context.ChangeTracker.SetEdits(diffs);

                foreach (var ruleApplication in positions)
                {
                        Assert.That(parser.Context.ChangeTracker.IsObsoleted(ruleApplication, parser.Context));
                }
            }
        }


        [TestCase("busybox", 1)]
        [TestCase("busybox", 2)]
        [TestCase("busybox", 3)]
        [TestCase("busybox", 4)]
        [TestCase("busybox", 5)]
        [TestCase("busybox", 6)]
        [TestCase("busybox", 7)]
        [TestCase("busybox", 8)]
        [TestCase("busybox", 9)]
        [TestCase("busybox", 10)]
        [TestCase("busybox", 11)]
        [TestCase("automotive02", 1)]
        [TestCase("automotive02", 2)]
        [TestCase("automotive02", 3)]
        [TestCase("demo", 1)]
        public void ChangeTracker_CorrectlyIdentifiesInsertionItems(string model, int baseVersion)
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion+1:00}.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);

            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion + 1:00}.diff")))
                .ToArray();
            parser.Context.ChangeTracker.SetEdits(diff);

            foreach (var line in diff)
            {
                for (int i = 0; i < line.NewText.Length - 1; i++)
                {
                    var pos = parser.Context.Matcher.NextTokenPosition(new ParsePosition(line.Start.Line + i, 0));
                    if (pos.Line != i + line.Start.Line) continue;
                    var ruleApplication = parser.Context.Matcher.GetLiteralAt(pos);
                    if (ruleApplication != null)
                    {
                        var isInsertion = parser.Context.ChangeTracker.IsInsertion(ruleApplication, parser.Context);
                        if (!isInsertion)
                        {

                        }
                        Assert.That(isInsertion);
                    }
                }
            }
        }

        [TestCase("busybox", 1)]
        [TestCase("busybox", 2)]
        [TestCase("busybox", 3)]
        [TestCase("busybox", 4)]
        [TestCase("busybox", 5)]
        [TestCase("busybox", 6)]
        [TestCase("busybox", 7)]
        [TestCase("busybox", 8)]
        [TestCase("busybox", 9)]
        [TestCase("busybox", 10)]
        [TestCase("busybox", 11)]
        [TestCase("automotive02", 1)]
        [TestCase("automotive02", 2)]
        [TestCase("automotive02", 3)]
        [TestCase("demo", 1)]
        public void ChangeTracker_CorrectlyIdentifiesNonInsertionItems(string model, int baseVersion)
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion+1:00}.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);

            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion + 1:00}.diff"))).ToArray();
            parser.Context.ChangeTracker.SetEdits(diff);

            var index = 0;

            foreach (var line in diff)
            {
                var positions = new List<RuleApplication>();
                var lastInserted = null as RuleApplication;
                for (int i = index; i < line.Start.Line; i++)
                {
                    var pos = parser.Context.Matcher.NextTokenPosition(new ParsePosition(i, 0));
                    var ruleApplication = parser.Context.Matcher.GetLiteralAt(pos);
                    if (ruleApplication != null && ruleApplication.CurrentPosition.Line == i && ruleApplication != lastInserted)
                    {
                        positions.Add(ruleApplication);
                        lastInserted = ruleApplication;
                    }
                }

                foreach (var ruleApplication in positions)
                {
                    var isInsertion = parser.Context.ChangeTracker.IsInsertion(ruleApplication, parser.Context);
                    if (isInsertion)
                    {

                    }
                    Assert.That(isInsertion, Is.False);
                }

                index = line.Start.Line + line.NewText.Length - 1;
            }
        }


        [TestCase("busybox", 1)]
        [TestCase("busybox", 2)]
        [TestCase("busybox", 3)]
        [TestCase("busybox", 4)]
        [TestCase("busybox", 5)]
        [TestCase("busybox", 6)]
        [TestCase("busybox", 7)]
        [TestCase("busybox", 8)]
        [TestCase("busybox", 9)]
        [TestCase("busybox", 10)]
        [TestCase("busybox", 11)]
        [TestCase("automotive02", 1)]
        [TestCase("automotive02", 2)]
        [TestCase("automotive02", 3)]
        [TestCase("demo", 1)]
        public void ChangeTracker_CorrectlyIdentifiesNonObsoleteItems(string model, int baseVersion)
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion:00}.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);

            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion + 1:00}.diff")));
            var diffs = new List<TextEdit>();
            var dummyRule = new DummyRule();

            var index = 0;

            foreach (var line in diff)
            {
                diffs.Add(line);
                var positions = new List<RuleApplication>();
                var lastInserted = null as RuleApplication;
                for (int i = index; i < line.Start.Line; i++)
                {
                    var pos = parser.Context.Matcher.NextTokenPosition(new ParsePosition(i, 0));
                    var ruleApplication = parser.Context.Matcher.GetLiteralAt(pos);
                    if (ruleApplication != null && ruleApplication.CurrentPosition.Line == i && ruleApplication != lastInserted)
                    {
                        positions.Add(ruleApplication);
                        lastInserted = ruleApplication;
                    }
                    else
                    {
                        var pos2 = new ParsePosition(i, 1);
                        ruleApplication = parser.Context.Matcher.MatchCore(dummyRule, null, parser.Context, ref pos2);
                        positions.Add(ruleApplication);
                    }
                }
                parser.Context.Matcher.Apply(line);
                parser.Context.ChangeTracker.SetEdits(diffs);

                MakeSureMatcherHasSomeTokens(parser, dummyRule, index, line);

                foreach (var ruleApplication in positions)
                {
                    var isObsolete = parser.Context.ChangeTracker.IsObsoleted(ruleApplication, parser.Context);
                    if (isObsolete)
                    {

                    }
                    Assert.That(isObsolete, Is.False);
                }

                index = line.Start.Line + line.NewText.Length - 1;
            }
        }

        private static void MakeSureMatcherHasSomeTokens(Parser parser, DummyRule dummyRule, int index, TextEdit line)
        {
            var maxLine = Math.Max(line.Start.Line + line.NewText.Length, line.End.Line + 1);
            for (int i = 0; i < maxLine; i++)
            {
                var pos2 = new ParsePosition(i, 1);
                parser.Context.Matcher.MatchCore(dummyRule, null, parser.Context, ref pos2);
            }
        }

        private class DummyRule : LiteralRule
        {
            public DummyRule() : base("DummyFeatureWithSufficientLength")
            {
            }

            public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
            {
                return new LiteralRuleApplication(this, Literal, default);
            }
        }
    }
}

using AnyText.Tests.UniversalVariability;
using NMF.AnyText;
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
    public class DiffTests
    {
        [TestCase("busybox", 1)]
        [TestCase("demo", 1)]
        public void DiffResolvesCorrectly(string model, int baseVersion)
        {
            var uvl = new UniversalVariabilityGrammar();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion:00}.uvl"));
            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion+1:00}.diff")));

            foreach (var line in diff)
            {
                fileContents = line.Apply(fileContents);
            }

            var result = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion+1:00}.uvl"));

            AssertSame(fileContents, result);
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
        public void Uvl_ProcessDiff(string model, int baseVersion)
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion:00}.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);

            var fm = (FeatureModel)parsed;
            AssertNoBrokenFeatures(fm, parser.Context);

            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion + 1:00}.diff")));
            var parsed2 = parser.Update(diff);

            AssertNoBrokenFeatures(fm, parser.Context);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed2, Is.Not.Null);
            Assert.That(parsed2, Is.EqualTo(parsed));
        }

        private static void AssertNoBrokenFeatures(FeatureModel fm, ParseContext context)
        {
            var brokenFeature = fm.Descendants().OfType<Feature>().FirstOrDefault(f => f.Name == null);
            if (brokenFeature != null)
            {
                context.TryGetDefinitions(brokenFeature, out var definitions);
                Assert.Pass();
            }
            Assert.That(brokenFeature, Is.Null);

            var unresolvedFeatureConstraint = fm.Descendants().OfType<FeatureConstraint>().FirstOrDefault(fc => fc.Feature == null);
            if (unresolvedFeatureConstraint != null)
            {
                context.TryGetDefinitions(unresolvedFeatureConstraint, out var definitions);
                Assert.Pass();
            }
            Assert.That(unresolvedFeatureConstraint, Is.Null);
        }

        private void AssertSame(string[] actual, string[] expected)
        {
            Assert.That(actual, Is.Not.Null);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.That(actual[i], Is.EqualTo(expected[i]), message: $"Lines {i} are different");
            }
        }
    }
}

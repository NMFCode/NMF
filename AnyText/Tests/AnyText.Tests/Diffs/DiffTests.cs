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
            AssertNoUnnamedFeatures(fm, parser.Context);

            var diff = DiffParser.ToTextEdits(File.ReadAllLines(Path.Combine("TestDocuments", $"{model}_{baseVersion + 1:00}.diff")));
            var parsed2 = parser.Update(diff);

            AssertNoUnnamedFeatures(fm, parser.Context);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed2, Is.Not.Null);
            Assert.That(parsed2, Is.EqualTo(parsed));
        }

        private static void AssertNoUnnamedFeatures(FeatureModel fm, ParseContext context)
        {
            var brokenFeature = fm.Descendants().OfType<Feature>().FirstOrDefault(f => f.Name == null);
            if (brokenFeature != null)
            {
                context.TryGetDefinitions(brokenFeature, out var definitions);
                Assert.Pass();
            }
            Assert.That(brokenFeature, Is.Null);

            var unresolvedFeatureConstraint = fm.Descendants().OfType<FeatureConstraint>().FirstOrDefault(fc => fc.Feature == null);
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

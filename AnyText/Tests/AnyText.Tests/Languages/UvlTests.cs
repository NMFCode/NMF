using AnyText.Tests.SimpleJava;
using AnyText.Tests.UniversalVariability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public class UvlTests
    {

        [Test]
        public void Uvl_CanLoadComplexUvlFile()
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", "automotive01.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }

        [Test]
        public void Uvl_CanLoadSimpleUvlFile()
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", "busybox_01.uvl"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }
    }
}

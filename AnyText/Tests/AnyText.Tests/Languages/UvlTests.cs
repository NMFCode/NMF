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
        [TestCase("demo_01.uvl")]
        [TestCase("demo_02.uvl")]
        [TestCase("automotive01.uvl")]
        [TestCase("busybox_01.uvl")]
        [TestCase("busybox_02.uvl")]
        public void Uvl_CanLoadUvlFile(string name)
        {
            var uvl = new UniversalVariabilityGrammar();
            var parser = uvl.CreateParser();
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", name));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }
    }
}

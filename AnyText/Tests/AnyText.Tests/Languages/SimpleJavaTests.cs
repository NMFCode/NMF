using AnyText.Tests.SimpleJava;
using NMF.AnyText;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public class SimpleJavaTests
    {

        [Test]
        public void SimpleJava_CanLoadSimpleJavaFile()
        {
            var simpleJava = new SimpleJavaGrammar();
            var parser = new Parser(new ModelParseContext(simpleJava));
            var fileContents = File.ReadAllLines(Path.Combine("TestDocuments", "IntRange.java"));

            var parsed = parser.Initialize(fileContents);
            Assert.That(parser.Context.Errors.Where(e => e.Source == DiagnosticSources.Parser), Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }
    }
}

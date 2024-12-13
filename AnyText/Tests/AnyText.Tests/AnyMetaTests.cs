using NMF.AnyText.AnyMeta;
using NMF.Models.Meta;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests
{
    [TestFixture]
    public class AnyMetaTests
    {
        [Test]
        public void AnyMeta_SynthesizesNMeta()
        {
            var grammar = new AnyMetaGrammar();
            var parser = grammar.CreateParser();

            var nmeta = MetaElement.ClassInstance.Namespace;
            var synthesis = grammar.GetRule<AnyMetaGrammar.NamespaceRule>().Synthesize(nmeta, parser.Context);

            Assert.IsNotNull(synthesis);

            var lines = synthesis.Split(Environment.NewLine);
            var parsed = parser.Initialize(lines);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.IsNotNull(parsed);
        }
    }
}

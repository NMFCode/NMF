using NMF.AnyText.AnyMeta;
using NMF.Models.Meta;
using NMF.Models.Repository;
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

            Assert.That(synthesis, Is.Not.Null);
            File.WriteAllText("NMeta.anymeta", synthesis);

            var lines = synthesis.Split(Environment.NewLine);
            var parsed = parser.Initialize(lines);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }

        [TestCase("schema")]
        [TestCase("61850")]
        [TestCase("KDM")]
        [TestCase("COSEM")]
        public void AnyMeta_SynthesizesMetamodel(string fileName)
        {
            var grammar = new AnyMetaGrammar();
            var parser = grammar.CreateParser();
            var repo = new ModelRepository();
            var ns = (INamespace)repo.Resolve($"{fileName}.nmeta").RootElements.First();
            
            // KDM metamodel does not have a namespace
            if (ns.Uri == null)
            {
                ns.Uri = new Uri("about:kdm", UriKind.Absolute);
                ns.Prefix = "kdm";
                foreach (var child in ns.ChildNamespaces)
                {
                    child.Prefix = "kdm" + child.Name;
                    child.Uri = new Uri("about:kdm/" + child.Name, UriKind.Absolute);
                }
            }
            var synthesis = grammar.GetRule<AnyMetaGrammar.NamespaceRule>().Synthesize(ns, parser.Context);

            Assert.That(synthesis, Is.Not.Null);

            File.WriteAllText($"{fileName}.anymeta", synthesis);

            var lines = synthesis.Split(Environment.NewLine);
            var parsed = parser.Initialize(lines);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }
    }
}

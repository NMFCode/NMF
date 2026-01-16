using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Features
{
    [TestFixture]
    class DocumentHighlightsTests
    {
        [Test]
        public void GetDocumentHighlights_Symbol()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");

            var parsed = parser.Initialize(grammar);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetDocumentHighlights(new ParsePosition(15, 30));
            var expected = new List<DocumentHighlight>()
            {
                new DocumentHighlight()
                {
                    Range = new ParseRange(new ParsePosition(15, 25), new ParsePosition(15, 46)),
                    Kind = DocumentHighlightKind.Read
                },
                new DocumentHighlight()
                {
                    Range = new ParseRange(new ParsePosition(17, 0), new ParsePosition(17, 21)),
                    Kind = DocumentHighlightKind.Read
                }
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                AssertAreEqual(actual.ElementAt(i), expected[i]);
            }
        }

        [Test]
        public void GetDocumentHighlights_Text()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");

            var parsed = parser.Initialize(grammar);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetDocumentHighlights(new ParsePosition(18, 14));
            var expected = new List<DocumentHighlight>()
            {
                new DocumentHighlight()
                {
                    Range = new ParseRange(new ParsePosition(18, 12), new ParsePosition(18, 17)),
                    Kind = DocumentHighlightKind.Text
                },
                new DocumentHighlight()
                {
                    Range = new ParseRange(new ParsePosition(21, 12), new ParsePosition(21, 17)),
                    Kind = DocumentHighlightKind.Text
                }
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                AssertAreEqual(actual.ElementAt(i), expected[i]);
            }
        }

        [Test]
        public void GetDocumentHighlights_NotFound()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");

            var parsed = parser.Initialize(grammar);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetDocumentHighlights(new ParsePosition(13, 0));

            Assert.That(actual, Is.Null);
        }

        private static void AssertAreEqual(DocumentHighlight actual, DocumentHighlight expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actual.Range, Is.EqualTo(expected.Range));
                Assert.That(actual.Kind, Is.EqualTo(expected.Kind));
            });
        }
    }
}

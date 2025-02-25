using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests
{
    [TestFixture]
    class DocumentSymbolTests
    {
        [Test]
        public void GetDocumentSymbols()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Model

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetDocumentSymbolsFromRoot();
            var expected = new List<DocumentSymbol>()
            {
                new DocumentSymbol()
                {
                    Name = "HelloWorld",
                    Kind = SymbolKind.File,
                    Range = new ParseRange(new ParsePosition(0, 0), new ParsePosition(12, 0)),
                    SelectionRange = new ParseRange(new ParsePosition(0, 8), new ParsePosition(0, 18)),
                    Children = [
                        new DocumentSymbol()
                        {
                            Name = "Model",
                            Kind = SymbolKind.Class,
                            Range = new ParseRange(new ParsePosition(2, 0), new ParsePosition(5, 0)),
                            SelectionRange = new ParseRange(new ParsePosition(2, 0), new ParsePosition(2, 5))
                        },
                        new DocumentSymbol()
                        {
                            Name = "Person",
                            Kind = SymbolKind.Class,
                            Range = new ParseRange(new ParsePosition(5, 0), new ParsePosition(8, 0)),
                            SelectionRange = new ParseRange(new ParsePosition(5, 0), new ParsePosition(5, 6))
                        },
                        new DocumentSymbol()
                        {
                            Name = "Greeting",
                            Kind = SymbolKind.Class,
                            Range = new ParseRange(new ParsePosition(8, 0), new ParsePosition(11, 0)),
                            SelectionRange = new ParseRange(new ParsePosition(8, 0), new ParsePosition(8, 8))
                        },
                        new DocumentSymbol()
                        {
                            Name = "ID",
                            Kind = SymbolKind.Constant,
                            Range = new ParseRange(new ParsePosition(11, 0), new ParsePosition(12, 0)),
                            SelectionRange = new ParseRange(new ParsePosition(11, 9), new ParsePosition(11, 11))
                        }
                    ]
                }
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                AssertAreEqual(actual.ElementAt(i), expected[i]);
            }
        }

        private static string[] SplitIntoLines(string grammar)
        {
            var lines = grammar.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r');
            }
            return lines;
        }

        private static void AssertAreEqual(DocumentSymbol actual, DocumentSymbol expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(expected.Name));
                Assert.That(actual.Detail, Is.EqualTo(expected.Detail));
                Assert.That(actual.Kind, Is.EqualTo(expected.Kind));
                CollectionAssert.AreEqual(actual.Tags, expected.Tags);
                Assert.That(actual.Range, Is.EqualTo(expected.Range));
                Assert.That(actual.SelectionRange, Is.EqualTo(expected.SelectionRange));
            });

            if (actual.Children == null || expected.Children == null)
            {
                Assert.That(actual.Children, Is.EqualTo(expected.Children));
                return;
            }

            for (var i = 0; i < expected.Children.Count(); i++)
            {
                AssertAreEqual(actual.Children.ElementAt(i), expected.Children.ElementAt(i));
            }
        }
    }
}

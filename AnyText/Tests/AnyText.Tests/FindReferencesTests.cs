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
    class FindReferencesTests
    {
        [Test]
        public void GetReferences_Default()
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
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetReferences(new ParsePosition(3, 37));
            var expected = new List<ParseRange>()
            {
                new ParseRange(new ParsePosition(8, 0), new ParsePosition(8, 8)),
                new ParseRange(new ParsePosition(3, 34), new ParsePosition(3, 42))
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.That(actual.ElementAt(i), Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void FindReferences_Terminal()
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
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetReferences(new ParsePosition(6, 19));
            var expected = new List<ParseRange>()
            {
                new ParseRange(new ParsePosition(11, 9), new ParsePosition(11, 11)),
                new ParseRange(new ParsePosition(6, 18), new ParsePosition(6, 20))
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.That(actual.ElementAt(i), Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void GetReferences_FromDefinition()
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
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetReferences(new ParsePosition(5, 2));
            var expected = new List<ParseRange>()
            {
                new ParseRange(new ParsePosition(5, 0), new ParsePosition(5, 6)),
                new ParseRange(new ParsePosition(3, 14), new ParsePosition(3, 20)),
                new ParseRange(new ParsePosition(9, 20), new ParsePosition(9, 26))
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.That(actual.ElementAt(i), Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void GetReferences_NotFound()
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
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetReferences(new ParsePosition(7, 0));

            Assert.That(actual, Is.Null);
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
    }
}

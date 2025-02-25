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
    class FoldingRangeTests
    {
        [Test]
        public void GetFoldingRanges()
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

            var actual = parser.GetFoldingRangesFromRoot();
            var expected = new List<FoldingRange>()
            {
                new FoldingRange() { StartLine = 0, StartCharacter = 0, EndLine = 12, EndCharacter = 0},
                new FoldingRange() { StartLine = 2, StartCharacter = 0, EndLine = 3, EndCharacter = 45},
                new FoldingRange() { StartLine = 3, StartCharacter = 4, EndLine = 3, EndCharacter = 43},
                new FoldingRange() { StartLine = 5, StartCharacter = 0, EndLine = 6, EndCharacter = 21},
                new FoldingRange() { StartLine = 8, StartCharacter = 0, EndLine = 9, EndCharacter = 32},
                new FoldingRange() { StartLine = 11, StartCharacter = 0, EndLine = 11, EndCharacter = 31}
            };

            Assert.That(actual.Count(), Is.EqualTo(expected.Count));

            for (int i = 0; i < expected.Count; i++)
            {
                AssertAreEqual(actual.ElementAt(i), expected[i]);
            }
        }

        [Test]
        public void GetFoldingRanges_RespondToCommentLineAddition()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld root Model
Model:
    (persons+=Person | greetings+=Greeting)*;

/*

this is a multiline comment

*/

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            parser.Update([new TextEdit(new ParsePosition(7, 0), new ParsePosition(7, 0), ["", ""])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var foldingRanges = parser.GetFoldingRangesFromRoot();
            Assert.That(foldingRanges.Any(foldingRange =>
                foldingRange.StartLine == 4 && foldingRange.EndLine == 9
            ));
        }

        [Test]
        public void GetFoldingRanges_RespondToCommentLineRemoval()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld root Model
Model:
    (persons+=Person | greetings+=Greeting)*;

/*

this is a multiline comment

*/

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            parser.Update([new TextEdit(new ParsePosition(6, 27), new ParsePosition(7, 0), [""])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var foldingRanges = parser.GetFoldingRangesFromRoot();
            Assert.That(foldingRanges.Any(foldingRange =>
                foldingRange.StartLine == 4 && foldingRange.EndLine == 7
            ));
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

        private static void AssertAreEqual(FoldingRange actual, FoldingRange expected)
        {
            Assert.That(actual.StartLine, Is.EqualTo(expected.StartLine));
            Assert.That(actual.StartCharacter, Is.EqualTo(expected.StartCharacter));
            Assert.That(actual.EndLine, Is.EqualTo(expected.EndLine));
            Assert.That(actual.EndCharacter, Is.EqualTo(expected.EndCharacter));
            Assert.That(actual.Kind, Is.EqualTo(expected.Kind));
        }
    }
}

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
    class CommentChangeTests
    {
        [Test]
        public void CommentChange_RespondToLineAddition()
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
        public void CommentChange_RespondToLineRemoval()
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
    }
}

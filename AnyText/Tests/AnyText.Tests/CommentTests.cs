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
    public class CommentTests
    {
        [Test]
        public void AnyText_ParseUpdateComment()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Model
// SingleLineComment
Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            var literalPositionsStart = new List<ParsePosition>();
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                literalPositionsStart.Add(literal.CurrentPosition);
            });
            parser.Update([new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 0), [" "])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var index = 0;
            var comments = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                if (literal.Rule.IsComment) comments++;
                var beforeUpdateLiteralLine = literalPositionsStart[index].Line;
                var beforeUpdateLiteralCol = literalPositionsStart[index].Col;
                if (beforeUpdateLiteralLine == 1)
                    Assert.That(literal.CurrentPosition.Col, Is.EqualTo(beforeUpdateLiteralCol + 1));
                else
                    Assert.That(literal.CurrentPosition.Col, Is.EqualTo(beforeUpdateLiteralCol));

                Assert.That(literal.CurrentPosition.Line, Is.EqualTo(beforeUpdateLiteralLine));

                index++;
            });
            Assert.That(comments, Is.EqualTo(1));

        }
        [Test]
        public void AnyText_ParseUpdateCommentMultiLineComment()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Model
/*
MultiLineComment
*/
Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            parser.Update([new TextEdit(new ParsePosition(3, 0), new ParsePosition(3, 0), [" "])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

        }
        [Test]
        public void AnyText_ParseUpdateAddComment()
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

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            parser.Update([new TextEdit(new ParsePosition(2, 6), new ParsePosition(2, 6), ["// Added Comment"])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var comments = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                if (literal.Rule.IsComment) comments++;

            });
            Assert.That(comments, Is.EqualTo(1));
        }
        [Test]
        public void AnyText_ParseUpdateAddNewLineInComment()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Model
/*
MultiLineComment
*/
Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            parser.Update([new TextEdit(new ParsePosition(3, 0), new ParsePosition(3, 0), ["", ""])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var comments = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                if (literal.Rule.IsComment) comments++;

            });
            Assert.That(comments, Is.EqualTo(1));
        }

        [Test]
        public void AnyText_ParseUpdateAddNewLineBeforeComment()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Model
/*
MultiLineComment
*/
Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            parser.Update([new TextEdit(new ParsePosition(0, 29), new ParsePosition(0, 29), ["", ""])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var comments = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                if (literal.Rule.IsComment)
                {
                    comments++;
                    Assert.That(literal.CurrentPosition.Line, Is.EqualTo(2));
                }

            });
            Assert.That(comments, Is.EqualTo(1));
        }
    }
}

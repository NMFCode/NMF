using NMF.AnyText;
using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;
using NMF.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public class AnyTextGrammarTests
    {
        [Test]
        public void AnyText_CanLoadLangiumPlaygroundExample()
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
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }

        [Test]
        public void AnyText_CanLoadAnyText()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");
            var parsed = parser.Initialize(grammar);
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.Not.Null);
        }

        [Test]
        public void AnyText_PrettyPrintItself()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");
            var parsed = parser.Initialize(grammar) as NMF.AnyText.Metamodel.Grammar;

            var synthesized = anyText.GetRule<AnyTextGrammar.GrammarRule>().Synthesize(parsed, parser.Context);
            Assert.That(synthesized, Is.Not.Null);
            var joined = string.Join(Environment.NewLine, grammar) + Environment.NewLine;
            Assert.That(synthesized, Is.EqualTo(joined));
        }

        [Test]
        public void AnyText_ParseUpdateRemoveLineBreak()
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
            var examinedStart = new ParsePositionDelta(11, 31);
            Assert.That(parsed, Is.Not.Null);
            AssertAtLeast(parser.Context.RootRuleApplication.ExaminedTo, examinedStart);
            var literalPositionsStart = new List<ParsePosition>();
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                literalPositionsStart.Add(literal.CurrentPosition);
            });
            parser.Update([new TextEdit(new ParsePosition(0, 29), new ParsePosition(1, 0), [string.Empty])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var index = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                var beforeUpdateLiteralLine = literalPositionsStart[index].Line;
                if (beforeUpdateLiteralLine != 0)
                    Assert.That(literal.CurrentPosition.Line, Is.EqualTo(beforeUpdateLiteralLine - 1));
                else
                    Assert.That(literal.CurrentPosition.Line, Is.EqualTo(beforeUpdateLiteralLine));
                index++;
            });
            var examinedUpdate = new ParsePositionDelta(10, 31);
            AssertAtLeast(parser.Context.RootRuleApplication.ExaminedTo, examinedUpdate);

        }

        private static void AssertAtLeast(ParsePositionDelta actual, ParsePositionDelta expected)
        {
            Assert.That(actual.Line, Is.AtLeast(expected.Line));
            if (actual.Line == expected.Line)
            {
                Assert.That(actual.Col, Is.AtLeast(expected.Col));
            }
        }


        [Test]
        public void AnyText_ParseUpdateAddLines()
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
            var examinedStart = new ParsePositionDelta( 11, 31);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            AssertAtLeast(parser.Context.RootRuleApplication.ExaminedTo, examinedStart);
            var newTextFirstLine = "terminal Char:";
            var newTextSecondLine = @"  /\S/;";
            parser.Update([new TextEdit(new ParsePosition(12, 0), new ParsePosition(12, 0), [newTextFirstLine, newTextSecondLine])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var examinedUpdate = new ParsePositionDelta( 13, 7);
            AssertAtLeast(parser.Context.RootRuleApplication.ExaminedTo, examinedUpdate);

        }

        [Test]
        public void AnyText_ParseUpdateInnerRule()
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
            //remove last s of 'persons'
            parser.Update([new TextEdit(new ParsePosition(3, 11), new ParsePosition(3, 12), [string.Empty])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void AnyText_ParseUpdateLiteralPositionAndParent()
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

            var positions = new List<ParsePosition>();

            parser.Context.RootRuleApplication.IterateLiterals(literal => positions.Add(literal.CurrentPosition));

            parser.Update([new TextEdit(new ParsePosition(0, 29), new ParsePosition(1, 0), [string.Empty])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var literalPositions = new List<ParsePosition>();

            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                literalPositions.Add(literal.CurrentPosition);
                Assert.That(literal.Parent, Is.Not.Null);
                Assert.That(literal.CurrentPosition.Col, Is.AtLeast(0));
            });

        }

        [Test]
        public void AnyText_ParseUpdateOnComment()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Model
/*

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
            parser.Update([new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 0), [" "])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);


        }
        [Test]
        public void AnyText_ParseUpdateInsertLineBreakAtStartOfLine()
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
            var literalPositionsStart = new List<ParsePosition>();
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                literalPositionsStart.Add(literal.CurrentPosition);
            });
            parser.Update([new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 0), [string.Empty, string.Empty])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var index = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                var beforeUpdateLiteralLine = literalPositionsStart[index].Line;
                Assert.That(literal.CurrentPosition.Line, Is.EqualTo(beforeUpdateLiteralLine + 1));
                index++;
            });

        }
        [Test]
        public void AnyText_ParseUpdateInsertAtStartOfLine()
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
            var literalPositionsStart = new List<ParsePosition>();
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                literalPositionsStart.Add(literal.CurrentPosition);
            });
            parser.Update([new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 0), [" "])]);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
            var index = 0;
            parser.Context.RootRuleApplication.IterateLiterals(literal =>
            {
                var beforeUpdateLiteralLine = literalPositionsStart[index].Line;
                var beforeUpdateLiteralCol = literalPositionsStart[index].Col;
                if (beforeUpdateLiteralLine == 0)
                    Assert.That(literal.CurrentPosition.Col, Is.EqualTo(beforeUpdateLiteralCol + 1));
                else
                    Assert.That(literal.CurrentPosition.Col, Is.EqualTo(beforeUpdateLiteralCol));

                Assert.That(literal.CurrentPosition.Line, Is.EqualTo(beforeUpdateLiteralLine));

                index++;
            });

        }
    }
}

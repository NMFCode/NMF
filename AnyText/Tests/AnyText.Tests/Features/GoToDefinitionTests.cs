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
    class GoToDefinitionTests
    {
        [Test]
        public void GetDefinition_Default()
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

            var actualRuleApplication = parser.GetDefinitions(new ParsePosition(3, 37)).First();
            var actual = new ParseRange(actualRuleApplication.CurrentPosition, actualRuleApplication.CurrentPosition + actualRuleApplication.Length);
            var expected = new ParseRange(new ParsePosition(8, 0), new ParsePosition(8, 8));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetDefinition_Terminal()
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

            var actualRuleApplication = parser.GetDefinitions(new ParsePosition(6, 19)).First();
            var actual = new ParseRange(actualRuleApplication.CurrentPosition, actualRuleApplication.CurrentPosition + actualRuleApplication.Length);
            var expected = new ParseRange(new ParsePosition(11, 9), new ParsePosition(11, 11));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetDefinition_FromDefinition()
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

            var actualRuleApplication = parser.GetDefinitions(new ParsePosition(5, 2)).First();
            var actual = new ParseRange(actualRuleApplication.CurrentPosition, actualRuleApplication.CurrentPosition + actualRuleApplication.Length);
            var expected = new ParseRange(new ParsePosition(5, 0), new ParsePosition(5, 6));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetDefinition_NotFound()
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

            var actual = parser.GetDefinitions(new ParsePosition(7, 0));

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

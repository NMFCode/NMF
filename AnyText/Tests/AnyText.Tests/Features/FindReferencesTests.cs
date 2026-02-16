using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
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
    public class FindReferencesTests
    {
        [Test]
        public void GetReferences_Synthesized()
        {
            var s1 = new State { Name = "S1" };
            var t1 = new Transition { StartState = s1, EndState = s1, Input = "a" };
            var sm = new StateMachine
            {
                Id = "test",
                States = { s1 },
                Transitions = { t1 }
            };

            var grammar = new StateMachineGrammar();
            var parser = grammar.CreateParser();
            parser.Initialize(sm);

            Assert.That(parser.Context.TryGetDefinitions(s1, out var s1Defs));
            Assert.That(parser.Context.TryGetDefinitions(t1, out var t1Defs));
            Assert.That(parser.Context.TryGetDefinitions(sm, out var smDefs));
            Assert.That(s1Defs.Count, Is.EqualTo(1));
            Assert.That(t1Defs.Count, Is.EqualTo(1));
            Assert.That(smDefs.Count, Is.EqualTo(1));
           
            Assert.That(parser.Context.TryGetReferences(s1, out var s1Refs));
            Assert.That(s1Refs.Count, Is.EqualTo(3));
        }

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

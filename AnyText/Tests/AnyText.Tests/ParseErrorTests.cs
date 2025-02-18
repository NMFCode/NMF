using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;

namespace AnyText.Tests
{
    [TestFixture]
    public class ParseErrorTests
    {
        [Test]
        public void AnyText_ErrorUpdateReferenceError()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Mode

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            parser.Update([new TextEdit(new ParsePosition(0, 28), new ParsePosition(0, 28), ["l"])]);
            Assert.That(parser.Context.Errors, Is.Empty);

        }
        [Test]
        public void AnyText_ErrorUpdateAfterReferenceError()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Mode

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            Assert.That(parser.Context.Errors.Count, Is.EqualTo(1));
            parser.Update([new TextEdit(new ParsePosition(0, 28), new ParsePosition(0, 28), [" "])]);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            Assert.That(parser.Context.Errors.Count, Is.EqualTo(1));

        }
        [Test]
        public void AnyText_ErrorUpdateBeforeReferenceError()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Mode

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            Assert.That(parser.Context.Errors.Count, Is.EqualTo(1));
            parser.Update([new TextEdit(new ParsePosition(0, 24), new ParsePosition(0, 24), [" "])]);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            Assert.That(parser.Context.Errors.Count, Is.EqualTo(1));

        }
        [Test]
        public void AnyText_ErrorUpdateReference()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Mode

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            parser.Update([new TextEdit(new ParsePosition(2, 4), new ParsePosition(2, 5), [string.Empty])]);
            Assert.That(parser.Context.Errors, Is.Empty);

        }
        
        [Test]
        public void AnyText_ErrorUpdateErrorPosition()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar HelloWorld root Mode

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            var oldPosition = parser.Context.Errors[0].Position;
            parser.Update([new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 0), [string.Empty, string.Empty])]);
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            Assert.That(parser.Context.Errors[0].Position.Line, Is.EqualTo(oldPosition.Line+1)); ;

        }
        [Test]
        public void AnyText_ErrorCorrectUpdateShouldHaveNoErrors()
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

terminal ID: /[_a-zA-Z][\w_]*/";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parser.Context.Errors, Is.Not.Empty);
            parser.Update([new TextEdit(new ParsePosition(11, 30), new ParsePosition(11, 30), [";"])]);
            Assert.That(parser.Context.Errors, Is.Empty);

        }
    }
}
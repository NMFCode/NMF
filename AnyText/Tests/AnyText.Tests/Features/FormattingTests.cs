using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;

namespace AnyText.Tests.Features
{
    [TestFixture]
    public class FormattingTests
    {
        [Test]
        public void AnyText_FormattingTest()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar =
                @"grammar HelloWorld root Model Model:(persons+=Person | greetings+=Greeting)*; Person:'person' name=ID;
Greeting:
    'Hello' person=[Person] '!'; terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(TestUtils.SplitIntoLines(grammar));
            Assert.That(parsed, Is.Not.Null);
            var result = parser.Format();
            var expected = new[]
            {
                new TextEdit(new ParsePosition(0, 0), new ParsePosition(0, 102), ["grammar HelloWorld"]),
                new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 9), ["root Model"]),
                new TextEdit(new ParsePosition(2, 0), new ParsePosition(2, 65), [""]),
                new TextEdit(new ParsePosition(3, 0), new ParsePosition(3, 0),
                [
                    "", "Model:", "  ( persons+=Person | greetings+=Greeting )*;", "", "Person:",
                    "  'person' name=ID;", "", "Greeting:", "  'Hello' person=[Person] '!';", "", "terminal ID:",
                    "  /[_a-zA-Z][\\w_]*/;", ""
                ])
            };
            Assert.That(result, Is.EqualTo(expected).Using<TextEdit>((a, b) =>
                a.Start.Equals(b.Start) &&
                a.End.Equals(b.End) &&
                a.NewText.SequenceEqual(b.NewText)
                    ? 0
                    : 1));
        }

        [Test]
        public void AnyText_FormattingRangeTest()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld 
root Model


Model:( persons+=Person | greetings+=Greeting )*;

Person:
  'person' name=ID;

Greeting:
  'Hello' person=[Person] '!';

terminal ID:
  /[_a-zA-Z][\w_]*/;
";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));
            var result = parser.Format(new ParsePosition(4, 0), new ParsePosition(5, 0));
            var expected = new[]
            {
                new TextEdit(new ParsePosition(4, 0), new ParsePosition(4, 49), ["Model:"]),
                new TextEdit(new ParsePosition(5, 0), new ParsePosition(5, 0),
                    ["  ( persons+=Person | greetings+=Greeting )*;"])
            };
            Assert.That(result, Is.EqualTo(expected).Using<TextEdit>((a, b) =>
                a.Start.Equals(b.Start) &&
                a.End.Equals(b.End) &&
                a.NewText.SequenceEqual(b.NewText)
                    ? 0
                    : 1));
        }

        [Test]
        public void AnyText_FormattingNoChangeTest()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld
root Model


Model:
  ( persons+=Person | greetings+=Greeting )*;

Person:
  'person' name=ID;

Greeting:
  'Hello' person=[Person] '!';

terminal ID:
  /[_a-zA-Z][\w_]*/;
";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));
            var result = parser.Format();
            Assert.That(result, Is.Empty);
        }
        [Test]
        public void AnyText_FormattingRangeNoChangeTest()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld
root Model


Model:( persons+=Person | greetings+=Greeting )*;

Person:
  'person' name=ID;

Greeting:
  'Hello' person=[Person] '!';

terminal ID:
  /[_a-zA-Z][\w_]*/;
";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));
            var result = parser.Format(new ParsePosition(4,0), new ParsePosition(4, 15));
            Assert.That(result, Is.Empty);
        }
        [Test]
        public void AnyText_FormattingOptionsTest()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld
root Model


Model:
  ( persons+=Person | greetings+=Greeting )*;

Person:
  'person' name=ID;

Greeting:
  'Hello' person=[Person] '!';

terminal ID:
  /[_a-zA-Z][\w_]*/;
     



 ";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));
            var result = parser.Format(indentationString:"\t",trimFinalNewlines:true, insertFinalNewline:true);
            var expected = new[]
            {
                new TextEdit(new ParsePosition(5, 0), new ParsePosition(5, 45), ["\t( persons+=Person | greetings+=Greeting )*;"]),
                new TextEdit(new ParsePosition(8, 0), new ParsePosition(8, 19), ["\t'person' name=ID;"]),
                new TextEdit(new ParsePosition(11, 0), new ParsePosition(11, 30), ["\t'Hello' person=[Person] '!';"]),
                new TextEdit(new ParsePosition(14, 0), new ParsePosition(14, 20), ["\t/[_a-zA-Z][\\w_]*/;\r\n"]),
                new TextEdit(new ParsePosition(15, 0), new ParsePosition(20, 0), [string.Empty])

            };
            Assert.That(result, Is.EqualTo(expected).Using<TextEdit>((a, b) =>
                a.Start.Equals(b.Start) &&
                a.End.Equals(b.End) &&
                a.NewText.SequenceEqual(b.NewText)
                    ? 0
                    : 1));
        }
    }
}
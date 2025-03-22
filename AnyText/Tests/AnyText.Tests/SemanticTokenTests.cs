using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;

namespace AnyText.Tests
{
    [TestFixture]
    public class SemanticTokenTests
    {
        [Test]
        public void AnyText_SemanticTokenTest()
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
            var result = parser.GetSemanticElementsFromRoot();
            var expected = new uint[]
            {
                0, 8, 10, 4, 0, 0, 11, 4, 3, 0, 0, 5, 5, 1, 0, 2, 0, 5, 1, 2, 0, 5, 1, 11, 0, 1, 4,
                1, 11, 0, 0, 1, 7, 9, 2, 0, 7, 2, 11, 0, 0, 2, 6, 1, 0, 0, 7, 1, 11, 0, 0, 2, 9, 9, 2, 0, 9, 2, 11, 0,
                0, 2, 8, 1, 0, 0, 8, 1, 11, 0, 0, 1, 1, 11, 0, 0, 1, 1, 11, 0, 2, 0, 6, 1, 2, 0, 6, 1, 11, 0, 1, 4, 8,
                3, 1, 0, 9, 4, 9, 2, 0, 4, 1, 11, 0, 0, 1, 2, 1, 0, 0, 2, 1, 11, 0, 2, 0, 8, 1, 2, 0, 8, 1, 11, 0, 1, 4,
                7, 3, 1, 0, 8, 6, 9, 2, 0, 6, 1, 11, 0, 0, 1, 1, 11, 0, 0, 1, 6, 4, 0, 0, 6, 1, 11, 0, 0, 2, 3, 3, 1, 0,
                3, 1, 11, 0, 2, 0, 8, 3, 0, 0, 9, 2, 1, 2, 0, 2, 1, 11, 0, 0, 2, 17, 6, 0, 0, 17, 1, 11, 0
            };
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void AnyText_SemanticTokenRangeTest()
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
            var result = parser.GetSemanticElementsFromRoot(new ParsePosition(0, 0), new ParsePosition(1, 0));
            var expected = new uint[]
            {
                0, 8, 10, 4, 0, 0, 11, 4, 3, 0, 0, 5, 5, 1, 0
            };
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
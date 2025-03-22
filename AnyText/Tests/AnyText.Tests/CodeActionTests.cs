using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;

namespace AnyText.Tests
{
    [TestFixture]
    public class CodeActionTests
    {
        [Test]
        public void AnyText_CodeAction()
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

            parser.Initialize(TestUtils.SplitIntoLines(grammar));
            var start = new ParsePosition(0, 0);
            var end = new ParsePosition(0, 1);
            var actions = parser.GetCodeActionInfo(start, end).ToList();


            var uniqueIdentifiers = actions
                .Select(c => c.Action.CommandIdentifier)
                .Distinct()
                .ToList();

            Assert.That(actions.Count, Is.EqualTo(1));

            var expectedIdentifiers = new List<string> { "editor.action.addCommentHeader" };
            Assert.That(expectedIdentifiers, Is.EquivalentTo(uniqueIdentifiers));
            foreach (var identifier in expectedIdentifiers)
                Assert.That(parser.Context.Grammar.ExecutableActions.ContainsKey(identifier), Is.True,
                    $"ExecutableActions dictionary does not contain the expected identifier: {identifier}");
        }
    }
}
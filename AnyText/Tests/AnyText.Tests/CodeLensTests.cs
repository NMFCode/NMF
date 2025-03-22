using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;

namespace AnyText.Tests
{
    [TestFixture]
    public class CodeLensTests
    {
        [Test]
        public void AnyText_CodeLensTest()
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
            var codeLensInfos = new List<CodeLensApplication>();
            
            parser.Context.RootRuleApplication.AddCodeLenses(codeLensInfos);
            var uniqueIdentifiers = codeLensInfos
                .Select(c => c.CodeLens.CommandIdentifier)
                .Distinct()
                .ToList();
            
            Assert.That(codeLensInfos.Count, Is.EqualTo(5));
            
            var expectedIdentifiers = new List<string> { "codelens.reference.ModelRule", "codelens.reference.Grammar", "codelens.reference.DataRule" };
            Assert.That(expectedIdentifiers, Is.EquivalentTo(uniqueIdentifiers));
            foreach (var identifier in expectedIdentifiers)
            {
                Assert.That(parser.Context.Grammar.ExecutableActions.ContainsKey(identifier), Is.True,
                    $"ExecutableActions dictionary does not contain the expected identifier: {identifier}");
            }
        }
       
    }
}
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
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
    public class CompletionTests
    {
        [TestCase("", "grammar")]
        [TestCase("grammar Test ", "(")]
        [TestCase("grammar Test (t", ")")]
        [TestCase("grammar Test (t)", "root")]
        [TestCase("grammar Test (t) root Start Start ", "returns")]
        public void AnyText_SyntacticCompletion_CompletionIncludesBestNextMatch(string line, string expected)
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = new[] { line };
            parser.Initialize(grammar);

            var completionSuggestion = parser.SuggestCompletions(new ParsePosition(0, line.Length));

            Assert.That(completionSuggestion, Has.Some.EqualTo(expected));
        }

        [TestCase("", "namespace")]
        [TestCase("namespace Test", "(")]
        [TestCase("namespace Test (t", ")")]
        [TestCase("namespace Test (t)", "=")]
        [TestCase("namespace Test (t)", "{")]
        public void AnyMeta_SyntacticCompletion_CompletionIncludesBestNextMatch(string line, string expected)
        {
            var anyMeta = new AnyMetaGrammar();
            var parser = anyMeta.CreateParser();

            var grammar = new[] { line };
            parser.Initialize(grammar);

            var completionSuggestion = parser.SuggestCompletions(new ParsePosition(0, line.Length));

            Assert.That(completionSuggestion, Has.Some.EqualTo(expected));
        }
    }
}

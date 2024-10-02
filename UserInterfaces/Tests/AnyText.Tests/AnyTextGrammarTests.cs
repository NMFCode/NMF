using NMF.AnyText;
using NMF.AnyText.Grammar;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests
{
    [TestFixture]
    public class AnyTextGrammarTests
    {
        [Test]
        public void AnyText_CanLoadLangiumPlaygroundExample()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(anyText.GetRule<AnyTextGrammar.GrammarRule>());
            var grammar = @"grammar HelloWorld

Model:
    (persons+=Person | greetings+=Greeting)*;

Person:
    'person' name=ID;

Greeting:
    'Hello' person=[Person] '!';

terminal ID: /[_a-zA-Z][\w_]*/;";

            var parsed = parser.Initialize(SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
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

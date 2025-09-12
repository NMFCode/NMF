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
    public class RecoveryTests
    {
        [Test]
        public void AnyText_RecoveryFirstRuleWorks()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld root Model
Model: ???????   ???;
Person:
    'person' name=ID;
Greeting:
    'Hello' person=[Person] '!';
terminal ID: /[_a-zA-Z][\w_]*/;";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));

            var tokens = new HashSet<string>();
            parser.Context.RootRuleApplication.IterateLiterals(lit => tokens.Add(lit.Literal));

            Assert.That(tokens.Contains("Person"));

            var errors = parser.Context.Errors.ToList();
            Assert.That(errors, Is.Not.Empty);
        }

        [Test]
        public void AnyText_RecoverySecondRuleWorks()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld root Model
Model:
    (persons+=Person | greetings+=Greeting)*;
Person:
    ???????   ???;
Greeting:
    'Hello' person=[Person] '!';
terminal ID: /[_a-zA-Z][\w_]*/;";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));

            var tokens = new HashSet<string>();
            parser.Context.RootRuleApplication.IterateLiterals(lit => tokens.Add(lit.Literal));

            Assert.That(tokens.Contains("Greeting"));

            var errors = parser.Context.Errors.ToList();
            Assert.That(errors, Is.Not.Empty);
        }


        [Test]
        public void AnyText_RecoveryTwoRulesWorks()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));

            var grammar = @"grammar HelloWorld root Model
Model:
    ???????   ???;
Person:
    ???????   ???;
Greeting:
    'Hello' person=[Person] '!';
terminal ID: /[_a-zA-Z][\w_]*/;";

            parser.Initialize(TestUtils.SplitIntoLines(grammar));

            var tokens = new HashSet<string>();
            parser.Context.RootRuleApplication.IterateLiterals(lit => tokens.Add(lit.Literal));

            Assert.That(tokens.Contains("Greeting"));

            var errors = parser.Context.Errors.ToList();
            Assert.That(errors, Is.Not.Empty);
        }
    }
}

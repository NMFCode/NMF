﻿using NMF.AnyText;
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
    public class AnyTextGrammarTests
    {
        [Test]
        public void AnyText_CanLoadLangiumPlaygroundExample()
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
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void AnyText_CanLoadAnyText()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");
            var parsed = parser.Initialize(grammar);
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void AnyText_PrettyPrintItself()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");
            var parsed = parser.Initialize(grammar) as NMF.AnyText.Metamodel.Grammar;

            var synthesized = anyText.GetRule<AnyTextGrammar.GrammarRule>().Synthesize(parsed, parser.Context);
            Assert.That(synthesized, Is.Not.Null);
            var joined = string.Join(Environment.NewLine, grammar);
            Assert.That(synthesized, Is.EqualTo(joined));
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

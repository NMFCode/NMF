using Microsoft.CSharp;
using NMF.AnyText;
using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Transformation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnyText.Tests.CodeGeneration
{
    [TestFixture]
    public class CodeGeneratorTests
    {
        [Test]
        public void AnyText_GeneratedGrammer_MatchesExisting()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");
            var parsed = parser.Initialize(grammar) as IGrammar;

            if (parsed == null)
            {
                Assert.Fail($"Failed with {string.Join(",", parser.Context.Errors)}");
            }
            Assert.That(parsed, Is.Not.Null);

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings { Namespace = "NMF.AnyText.Grammars" });
            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter("AnyTextGrammar.cs"))
            {
                csharp.GenerateCodeFromCompileUnit(unit, writer, new System.CodeDom.Compiler.CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }

            var allText = File.ReadAllText("AnyTextGrammar.cs");
            var reference = File.ReadAllText(Path.Combine("Reference", "AnyTextGrammar.cs"));

            Assert.That(EliminateWhitespaces(allText), Is.EqualTo(EliminateWhitespaces(reference)));
        }
        [Test]
        public void AnyText_GeneratedExpressionGrammer_MatchesExisting()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("Expressions.anytext");
            var parsed = parser.Initialize(grammar) as IGrammar;

            if (parsed == null)
            {
                Assert.Fail($"Failed with {string.Join(",", parser.Context.Errors)}");
            }
            Assert.That(parsed, Is.Not.Null);

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings
            {
                Namespace = "AnyText.Tests.ExpressionGrammar",
                ImportedNamespaces = { "AnyText.Test.Metamodel.Expressions" }
            });
            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter("Expressions.cs"))
            {
                csharp.GenerateCodeFromCompileUnit(unit, writer, new System.CodeDom.Compiler.CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }

            var allText = File.ReadAllText("Expressions.cs");
            var reference = File.ReadAllText(Path.Combine("Reference", "Expressions.cs"));

            Assert.That(EliminateWhitespaces(allText), Is.EqualTo(EliminateWhitespaces(reference)));
        }

        private static string EliminateWhitespaces(string input)
        {
            return Regex.Replace(Regex.Replace(input, "//[^/].*", string.Empty), @"\s+", string.Empty);
        }
    }
}

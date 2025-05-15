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

        [Test]
        public void AnyText_GeneratedBasketsGrammer_MatchesExisting()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("Baskets.anytext");
            var parsed = parser.Initialize(grammar) as IGrammar;

            if (parsed == null)
            {
                Assert.Fail($"Failed with {string.Join(",", parser.Context.Errors)}");
            }
            Assert.That(parsed, Is.Not.Null);

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings
            {
                Namespace = "AnyText.Tests.BasketsGrammar",
                ImportedNamespaces = { "AnyText.Test.Metamodel.Baskets" }
            });
            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter("Baskets.cs"))
            {
                csharp.GenerateCodeFromCompileUnit(unit, writer, new System.CodeDom.Compiler.CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }

            var allText = File.ReadAllText("Baskets.cs");
            // AnyText code generator currently does not take automated name mangling into account
            allText = allText.Replace("return semanticElement.Baskets", "return semanticElement.Baskets_");
            var reference = File.ReadAllText(Path.Combine("Reference", "Baskets.cs"));

            Assert.That(EliminateWhitespaces(allText), Is.EqualTo(EliminateWhitespaces(reference)));
        }

        [Test]
        public void AnyText_GeneratedListExpressionsGrammer_MatchesExisting()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("ListExpressions.anytext");
            var parsed = parser.Initialize(grammar) as IGrammar;

            if (parsed == null)
            {
                Assert.Fail($"Failed with {string.Join(",", parser.Context.Errors)}");
            }
            Assert.That(parsed, Is.Not.Null);

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings
            {
                Namespace = "AnyText.Tests.ListExpressions"
            });
            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter("ListExpressions.cs"))
            {
                csharp.GenerateCodeFromCompileUnit(unit, writer, new System.CodeDom.Compiler.CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }

            var allText = File.ReadAllText("ListExpressions.cs");
            var reference = File.ReadAllText(Path.Combine("Reference", "ListExpressions.cs"));

            Assert.That(EliminateWhitespaces(allText), Is.EqualTo(EliminateWhitespaces(reference)));
        }

        [Test]
        public void AnyText_GeneratedUvlGrammer_MatchesExisting()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("UVL.anytext");
            var parsed = parser.Initialize(grammar) as IGrammar;

            if (parsed == null)
            {
                Assert.Fail($"Failed with {string.Join(",", parser.Context.Errors)}");
            }
            Assert.That(parsed, Is.Not.Null);

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings
            {
                Namespace = "AnyText.Tests.UniversalVariability",
            });
            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter("UVL.cs"))
            {
                csharp.GenerateCodeFromCompileUnit(unit, writer, new System.CodeDom.Compiler.CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }

            var allText = File.ReadAllText("UVL.cs");
            var reference = File.ReadAllText(Path.Combine("Reference", "UVL.cs"));

            Assert.That(EliminateWhitespaces(allText), Is.EqualTo(EliminateWhitespaces(reference)));
        }

        [Test]
        public void AnyText_GeneratedSimpleJavaGrammer_MatchesExisting()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("SimpleJava.anytext");
            var parsed = parser.Initialize(grammar) as IGrammar;

            if (parsed == null)
            {
                Assert.Fail($"Failed with {string.Join(",", parser.Context.Errors)}");
            }
            Assert.That(parsed, Is.Not.Null);

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings
            {
                Namespace = "AnyText.Tests.SimpleJava",
            });
            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter("SimpleJava.cs"))
            {
                csharp.GenerateCodeFromCompileUnit(unit, writer, new System.CodeDom.Compiler.CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }

            var allText = File.ReadAllText("SimpleJava.cs");
            // AnyText code generator currently does not take automated name mangling into account
            allText = allText.Replace("semanticElement.DoStatement", "semanticElement.DoStatement_");
            var reference = File.ReadAllText(Path.Combine("Reference", "SimpleJava.cs"));

            Assert.That(EliminateWhitespaces(allText), Is.EqualTo(EliminateWhitespaces(reference)));
        }

        private static string EliminateWhitespaces(string input)
        {
            return Regex.Replace(Regex.Replace(input, "//[^/].*", string.Empty), @"\s+", string.Empty);
        }
    }
}

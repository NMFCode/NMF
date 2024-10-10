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

            var unit = CodeGenerator.Compile(parsed, new CodeGeneratorSettings());
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
        }
    }
}

using CommandLine;
using Microsoft.CSharp;
using NMF.AnyText.Transformation;
using System.CodeDom.Compiler;
using CodeGenerator = NMF.AnyText.Transformation.CodeGenerator;

namespace NMF.AnyTextGen.Verbs
{
    [Verb("generate-parser", HelpText = "Generates a parser for an AnyText specification")]
    internal class GenerateParserVerb : VerbBase
    {
        [Option('n',"namespace", Required = false, HelpText = "The namespace of the generated parser class")]
        public string? Namespace { get; set; }

        [Value(1, Required = false, HelpText = "The output path, defaults to the same name as the AnyText specification but with changed extension.")]
        public string? OutputPath { get; set; }

        protected override void ExecuteCore()
        {
            var grammar = LoadGrammar();
            var compileUnit = CodeGenerator.Compile(grammar, new CodeGeneratorSettings
            {
                Namespace = Namespace ?? "Generated"
            });

            if (OutputPath == null)
            {
                OutputPath = Path.ChangeExtension(AnyTextPath, ".cs");
            }

            var csharp = new CSharpCodeProvider();

            using (var writer = new StreamWriter(OutputPath!))
            {
                csharp.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    IndentString = "    ",
                });
            }
        }
    }
}

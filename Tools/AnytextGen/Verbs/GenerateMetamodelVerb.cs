using CommandLine;
using NMF.AnyText.Transformation;
using NMF.Models.Meta;
using NMF.Models.Repository;

namespace NMF.AnyTextGen.Verbs
{
    [Verb("generate-metamodel", HelpText = "Generates a metamodel from an AnyText specification")]
    internal class GenerateMetamodelVerb : VerbBase
    {

        [Value(1, Required = false, HelpText = "The output path, defaults to the same name as the AnyText specification but with changed extension.")]
        public string? OutputPath { get; set; }

        [Option('i', "identifierNames", Required = false, HelpText = "A list of identifier names, multiple entries separated by ';'. If omitted, attributes named 'name' or 'id' (case-insensitive) will be assumed to be identifiers.", Separator = ';')]
        public IEnumerable<string>? IdentifierNames { get; set; }

        protected override void ExecuteCore()
        {
            var metamodel = CreateNamespace();
            if (metamodel != null)
            {
                var repository = new ModelRepository();
                repository.Save(metamodel, OutputPath!);
            }
        }

        public INamespace? CreateNamespace()
        {
            var grammar = LoadGrammar();
            var metamodel = CodeGenerator.CreateNamespace(grammar, IdentifierNames);
            if (metamodel == null)
            {
                Console.WriteLine($"The AnyText specification at '{AnyTextPath}' does not define its own abstract syntax.");
                return null;
            }

            if (OutputPath == null)
            {
                OutputPath = Path.ChangeExtension(AnyTextPath, ".nmeta");
            }

            return metamodel;
        }
    }
}

using CommandLine;
using NMF.AnyText.Transformation;
using NMF.Models.Repository;

namespace NMF.AnyTextGen.Verbs
{
    [Verb("generate-metamodel", HelpText = "Generates a metamodel from an AnyText specification")]
    internal class GenerateMetamodelVerb : VerbBase
    {

        [Value(1, Required = false, HelpText = "The output path, defaults to the same name as the AnyText specification but with changed extension.")]
        public string? OutputPath { get; set; }

        protected override void ExecuteCore()
        {
            var grammar = LoadGrammar();
            var metamodel = CodeGenerator.CreateNamespace(grammar);
            if (metamodel == null)
            {
                throw new InvalidOperationException($"The AnyText specification at '{AnyTextPath}' does not define its own abstract syntax.");
            }

            if (OutputPath == null)
            {
                OutputPath = Path.ChangeExtension(AnyTextPath, ".nmeta");
            }

            var repository = new ModelRepository();
            repository.Save(metamodel, OutputPath!);
        }
    }
}

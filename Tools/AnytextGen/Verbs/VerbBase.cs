using CommandLine;
using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;

namespace NMF.AnyTextGen.Verbs
{
    internal abstract class VerbBase
    {
        [Value(0, Required = true, HelpText = "The path to the AnyText specification")]
        public string? AnyTextPath { get; set; }

        protected abstract void ExecuteCore();

        protected IGrammar LoadGrammar()
        {
            var directory = Path.GetDirectoryName(AnyTextPath);
            var actualPath = Path.GetFullPath(AnyTextPath!);
            if (!string.IsNullOrEmpty(directory))
            {
                Environment.CurrentDirectory = directory;
            }

            var anyText = new AnyTextGrammar();
            var parser = anyText.CreateParser();
            var grammar = File.ReadAllLines(actualPath);
            var parsed = parser.Initialize(grammar) as IGrammar;
            if (parsed == null || parser.Context.Errors.Any())
            {
                throw new InvalidOperationException($"Contents of {actualPath} could not be parsed as AnyText grammar. {parser.Context.Errors.OrderByDescending(e => e.Position).FirstOrDefault()}");
            }
            return parsed;
        }

        public int Execute()
        {
            try
            {
                if (!File.Exists(AnyTextPath))
                {
                    throw new InvalidOperationException($"'{Path.GetFullPath(AnyTextPath!)}' does not exist.");
                }
                ExecuteCore();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}

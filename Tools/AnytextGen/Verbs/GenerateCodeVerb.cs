using CommandLine;
using Microsoft.CSharp;
using NMF.AnyText.AnyMeta;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyTextGen.Verbs
{
    [Verb("generate-code", HelpText = "Generates metamodel code from an AnyText specification or NMeta metamodel, specified in XMI or AnyMeta")]
    internal class GenerateCodeVerb : VerbBase
    {

        [Value(1, Required = true, HelpText = "The output path.")]
        public string? OutputPath { get; set; }

        [Option('n', "namespace", Required = false, HelpText = "The namespace of the generated parser class")]
        public string? Namespace { get; set; }

        [Option('x', "save-metamodel-xmi", Required = false, HelpText = "If set, the metamodel is kept in XMI format. This flag is ignored if the input is specified as XMI file.")]
        public bool SaveMetamodel { get; set; }

        protected override void ExecuteCore()
        {
            var extension = Path.GetExtension(AnyTextPath)?.ToLowerInvariant();
            var ns = LoadNamespace(extension);
            if (ns != null)
            {
                if (SaveMetamodel && extension != ".nmeta")
                {
                    MetaRepository.Instance.Serializer.Serialize(ns, Path.ChangeExtension(AnyTextPath, ".nmeta"));
                }

                var code = MetaFacade.CreateCode(ns, Namespace ?? "Generated");
                MetaFacade.GenerateCode(code, new CSharpCodeProvider(), OutputPath, Directory.Exists(OutputPath));
            }
        }

        private INamespace? LoadNamespace(string? extension)
        {
            INamespace? ns;
            if (extension == ".anytext")
            {
                var generateMetamodel = new GenerateMetamodelVerb() { AnyTextPath = AnyTextPath };
                ns = generateMetamodel.CreateNamespace();
                if (ns != null && ns.Model == null)
                {
                    var model = new Model { ModelUri = ns.Uri };
                    model.RootElements.Add(ns);
                }
            }
            else
            {
                if (extension == ".anymeta")
                {
                    var anymetaGrammar = new AnyMetaGrammar();
                    var parser = anymetaGrammar.CreateParser();
                    ns = parser.Initialize(File.ReadAllLines(AnyTextPath!)) as INamespace;
                    if (parser.Context.Errors.Any())
                    {
                        var errors = parser.Context.Errors.Select(e => $"{e.Message} (line {e.Position.Line}, col {e.Position.Col})");
                        throw new InvalidOperationException($"Parsing the given file as an AnyMeta document failed: {string.Join(", ", errors)}");
                    }
                }
                else if (extension == ".nmeta")
                {
                    var repository = new ModelRepository();
                    ns = repository.Resolve(AnyTextPath)?.RootElements.FirstOrDefault() as INamespace;
                }
                else
                {
                    throw new ArgumentException($"The file extension '{extension}' is not supported. Supported metamodel formats are anytext, anymeta and nmeta.");
                }
                if (ns == null)
                {
                    throw new InvalidOperationException($"Failed to load contents of {AnyTextPath} as a metamodel.");
                }
            }

            return ns;
        }
    }
}

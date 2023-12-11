using CommandLine;
using CommandLine.Text;
using NMF.Expressions;
using NMF.Interop;
using NMF.Interop.Ecore;
using NMF.Interop.Ecore.Transformations;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Transformations.Parallel;
using NMF.Utilities;
using PythonCodeGenerator.CodeDom;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using CmofPackage = NMF.Interop.Cmof.IPackage;
using UmlPackage = NMF.Interop.Uml.IPackage;
using LegacyCmofPackage = NMF.Interop.Legacy.Cmof.IPackage;


namespace Ecore2Code
{
    class Options
    {
        public Options()
        {
            OverallNamespace = "GeneratedCode";
        }

        [Option('n', "namespace", HelpText = "The root namespace")]
        public string OverallNamespace { get; set; }

        [Option('l', "language", Default = SupportedLanguage.CS, HelpText = "The language in which the code should be generated")]
        public SupportedLanguage Language { get; set; }

        [Option('f', "folder", HelpText = "Determines whether the code for classes should be separated to multiple files")]
        public bool UseFolders { get; set; }

        [Option('p', "parallel", HelpText = "If specified, runs the code generator in parallel mode (in incubation)")]
        public bool Parallel { get; set; }

        [Option('x', "force", HelpText = "If specified, the code is generated regardless of existing code")]
        public bool Force { get; set; }

        [Option('u', "model-uri", HelpText = "If specified, overrides the uri of the base package.")]
        public string Uri { get; set; }

        [Option('p', "primitive-types", HelpText = "If set, Ecore Data types are transformed to primitive types")]
        public bool PrimitiveTypes { get; set; }

        [Value(0)]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output file/folder in which the code should be generated")]
        public string OutputFile { get; set; }

        [Option('m', "metamodel", Required = false, HelpText = "Specify this argument if you want to serialize the NMeta metamodel possibly generated from Ecore")]
        public string NMeta { get; set; }

        [Option('r', "resolve", Required = false, HelpText = "A list of namespace remappings with optional code base namespace override in the syntax URI@baseNamespace=file, multiple entries separated by ';'", Separator = ';')]
        public IEnumerable<string> NamespaceMappings { get; set; }

        [Option('t', "type-mapping", Required = false, HelpText = "A list of type mappings in the syntax <Ecore Instance Class>=<.NET class>, multiple entries separated by ';'", Separator = ';')]
        public IEnumerable<string> TypeMappings { get; set; }

        [Option('i', "input-only", HelpText = "If set, generate code for input files only")]
        public bool InputOnly { get; set; }

        [Option("no-events", HelpText = "If set, no dedicated events are generated for properties. This has the same effect as no-changed and no-changing together")]
        public bool NoEvents { get; set; }

        [Option("no-changed", HelpText = "If set, no Changed events are generated for events")]
        public bool NoChanged { get; set; }

        [Option("no-changing", HelpText = "If set, no Changed events are generated for events")]
        public bool NoChanging { get; set; }

        [Option("collectionsAreElements", HelpText = "If set, collections are all rendered as elements")]
        public bool CollectionsAsElements { get; set; }
    }

    public enum SupportedLanguage
    {
        CS,
        VB,
        PY
    }

    class Ecore2Code
    {
        private readonly Options options;
        private ModelRepository repository;

        public Ecore2Code(Options options)
        {
            this.options = options;
        }

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            switch (result.Tag)
            {
                case ParserResultType.Parsed:
                    var options = (result as Parsed<Options>).Value;

                    var gen = new Ecore2Code(options);
#if DEBUG
                    gen.GenerateCode();
#else
                    try
                    {
                        gen.GenerateCode();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
#endif
                    break;
                case ParserResultType.NotParsed:
                default:
                    Console.WriteLine("You are using me wrongly!");
                    Console.WriteLine("Usage: Ecore2Code [Options] -o [Output File or directory] [Inputfiles]");
                    Console.WriteLine("Input files may either be in NMeta or Ecore format.");
                    Console.WriteLine("Example: Ecore2Code -f -n NMF.Models -o Meta NMeta.nmf");
                    Console.WriteLine(HelpText.AutoBuild(result).ToString());
                    break;
            }
        }

        private void GenerateCode()
        {
            var packageTransform = new Meta2ClassesTransformation();
            var stopWatch = new Stopwatch();

            packageTransform.ForceGeneration = options.Force;
            packageTransform.DefaultNamespace = options.OverallNamespace;
            packageTransform.GenerateForInputOnly = options.InputOnly;
            packageTransform.GenerateChangedEvents = !(options.NoEvents || options.NoChanged);
            packageTransform.GenerateChangingEvents = !(options.NoEvents || options.NoChanging);
            packageTransform.GenerateCollectionsAsElements = options.CollectionsAsElements;

            LoadTypeMappings();

            Dictionary<Uri, string> fileMappings = new Dictionary<Uri, string>();
            LoadNamespaceMappings(fileMappings, packageTransform.NamespaceMap);

            var metaPackage = LoadPackageFromFiles(fileMappings);
            SetUri(metaPackage);

            Model model = metaPackage.Model;
            if (model == null) throw new InvalidOperationException("The namespace was not loaded correctly.");
            if (options.NMeta != null)
            {
                using (var fs = File.Create(options.NMeta))
                {
                    MetaRepository.Instance.Serializer.Serialize(model, fs);
                }
            }

            stopWatch.Start();
            var compileUnit = TransformationEngine.Transform<INamespace, CodeCompileUnit>(metaPackage,
                options.Parallel
                   ? (ITransformationEngineContext)new ParallelTransformationContext(packageTransform)
                   : new TransformationContext(packageTransform));
            stopWatch.Stop();

            Console.WriteLine("Operation took {0}ms", stopWatch.Elapsed.TotalMilliseconds);

            OutputGeneratedCode(compileUnit);

            Console.WriteLine("Code generated successfully!");
        }

        private void LoadNamespaceMappings(Dictionary<Uri, string> fileMappings, Dictionary<Uri, string> namespaceMappings)
        {
            if (options.NamespaceMappings != null && options.NamespaceMappings.Any())
            {
                foreach (var mapping in options.NamespaceMappings)
                {
                    AddMapping(fileMappings, namespaceMappings, mapping);
                }
            }
        }

        private static void AddMapping(Dictionary<Uri, string> fileMappings, Dictionary<Uri, string> namespaceMappings, string mapping)
        {
            if (string.IsNullOrEmpty(mapping)) return;
            var lastIdx = mapping.LastIndexOf('=');
            if (lastIdx == -1)
            {
                Console.WriteLine("Namespace mapping {0} is missing required separator =", mapping);
                return;
            }
            string file = mapping.Substring(lastIdx + 1);
            string[] uriNs = mapping.Substring(0, lastIdx).Split('@');

            if (!Uri.TryCreate(uriNs[0], UriKind.Absolute, out Uri uri))
            {
                uri = new Uri(uriNs[0], UriKind.Relative);
            }
            fileMappings.Add(uri, file);

            if (uriNs.Length == 2)
            {
                namespaceMappings.Add(uri, uriNs[1]);
            }
        }

        private void LoadTypeMappings()
        {
            Ecore2MetaTransformation.GeneratePrimitiveTypes = options.PrimitiveTypes;
            if (options.TypeMappings != null && options.TypeMappings.Any())
            {
                var typeMapping = new Dictionary<string, string>();
                foreach (var mapping in options.TypeMappings)
                {
                    if (string.IsNullOrEmpty(mapping)) continue;
                    var lastIdx = mapping.LastIndexOf('=');
                    if (lastIdx == -1)
                    {
                        Console.WriteLine("Type mapping {0} is missing required separator =", mapping);
                        continue;
                    }
                    typeMapping.Add(mapping.Substring(0, lastIdx), mapping.Substring(lastIdx + 1));
                }
                Ecore2MetaTransformation.CustomTypesMap = typeMapping;
            }
        }

        private void SetUri(INamespace metaPackage)
        {
            if (options.Uri != null)
            {
                if (Uri.TryCreate(options.Uri, UriKind.Absolute, out Uri uri))
                {
                    metaPackage.Model.ModelUri = uri;
                }
                else
                {
                    Console.Error.WriteLine("The provided string {0} could not be parsed as an absolute URI.", options.Uri);
                }
            }
            if (metaPackage.Uri == null)
            {
                Console.Error.WriteLine("Warning: There is no base Uri for the provided metamodels. Some features of the generated code will be disabled.");
            }
        }

        private void OutputGeneratedCode(CodeCompileUnit compileUnit)
        {
            CodeDomProvider generator;
            switch (options.Language)
            {
                case SupportedLanguage.CS:
                    generator = new Microsoft.CSharp.CSharpCodeProvider();
                    break;
                case SupportedLanguage.VB:
                    generator = new Microsoft.VisualBasic.VBCodeProvider();
                    break;
                case SupportedLanguage.PY:
                    generator = new PythonProvider();
                    break;
                default:
                    Console.WriteLine("Unknown language detected. Falling back to default C#");
                    generator = new Microsoft.CSharp.CSharpCodeProvider();
                    break;
            }

            var genOptions = new CodeGeneratorOptions()
            {
                BlankLinesBetweenMembers = true,
                VerbatimOrder = false,
                ElseOnClosing = false,
                BracingStyle = "C",
                IndentString = "    "
            };
            if (options.UseFolders)
            {
                foreach (var file in MetaFacade.SplitCompileUnit(compileUnit))
                {
                    var fileInfo = new FileInfo(Path.Combine(options.OutputFile, file.Key) + "." + generator.FileExtension);
                    CheckDirectoryExists(fileInfo.Directory);
                    using (var sw = new StreamWriter(fileInfo.Create()))
                    {
                        generator.GenerateCodeFromCompileUnit(file.Value, sw, genOptions);
                    }
                }
            }
            else
            {
                using (var sw = new StreamWriter(options.OutputFile))
                {
                    generator.GenerateCodeFromCompileUnit(compileUnit, sw, genOptions);
                }
            }
        }

        private static void CheckDirectoryExists(DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                CheckDirectoryExists(directoryInfo.Parent);
                directoryInfo.Create();
            }
        }

        public INamespace LoadPackageFromFiles(IDictionary<Uri, string> resolveMappings)
        {
            var files = options.InputFiles;
            if (files == null || !files.Any()) return null;

            var packages = new List<INamespace>();
            repository = new ModelRepository(EcoreInterop.Repository);

            if (resolveMappings != null)
            {
                repository.Locators.Add(new FileMapLocator(resolveMappings));
            }

            foreach (var ecoreFile in files)
            {
                var model = repository.Resolve(ecoreFile);
                if (model == null)
                {
                    Console.WriteLine($"Metamodel {ecoreFile} could not be found.");
                    Environment.ExitCode = 1;
                    continue;
                }
                foreach (var item in model.RootElements)
                {
                    switch (item)
                    {
                        case EPackage ecorePackage:
#if DEBUG
                            packages.Add(EcoreInterop.Transform2Meta(ecorePackage, AddMissingPackage));
#else
                    try
                    {
                        packages.Add(EcoreInterop.Transform2Meta(ecorePackage, AddMissingPackage));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred reading the Ecore file. The error message was: " + ex.Message);
                        Environment.ExitCode = 1;
                    }
#endif
                            break;
                        case Namespace nmetaNamespace:
                            packages.Add(nmetaNamespace);
                            break;
                        case CmofPackage cmofPackage:
                            packages.Add(UmlInterop.Transform(cmofPackage, AddMissingPackage));
                            break;
                        case UmlPackage umlPackage:
                            packages.Add(UmlInterop.Transform(umlPackage, AddMissingPackage));
                            break;
                        case LegacyCmofPackage legacyCmofPackage:
                            packages.Add(UmlInterop.Transform(legacyCmofPackage, AddMissingPackage));
                            break;
                        default:
                            Console.Error.WriteLine($"{item} (resolved from {ecoreFile}) is not a supported metamodel.");
                            break;
                    }
                }
            }

            if (packages.Count == 0)
            {
                throw new InvalidOperationException("No package could be found.");
            }
            else if (packages.Count == 1)
            {
                return packages.First();
            }
            else
            {
                var package = new Namespace() { Name = options.OverallNamespace };
                package.ChildNamespaces.AddRange(packages);
                return package;
            }
        }

        private void AddMissingPackage(IEPackage package, INamespace metaNamespace)
        {
            AddMissingPackage(package?.Model?.ModelUri, metaNamespace);
        }

        private void AddMissingPackage(CmofPackage cmofPackage, INamespace metaNamespace)
        {
            AddMissingPackage(cmofPackage?.Model?.ModelUri, metaNamespace);
        }

        private void AddMissingPackage(UmlPackage umlPackage, INamespace metaNamespace)
        {
            AddMissingPackage(umlPackage?.Model?.ModelUri, metaNamespace);
        }

        private void AddMissingPackage(LegacyCmofPackage cmofPackage, INamespace metaNamespace)
        {
            AddMissingPackage(cmofPackage.Model?.ModelUri, metaNamespace);
        }

        private void AddMissingPackage(Uri packageUri, INamespace metaNamespace)
        {
            if (options.NMeta != null && packageUri != null && packageUri.IsAbsoluteUri && packageUri.IsFile)
            {
                var path = packageUri.LocalPath;
                var dir = Path.GetDirectoryName(options.NMeta);
                var extension = Path.GetExtension(options.NMeta);
                var file = Path.GetFileNameWithoutExtension(path);
                repository.Save(metaNamespace, Path.Combine(dir, file + extension));
            }
        }
    }
}

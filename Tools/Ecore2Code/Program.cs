using CommandLine;
using CommandLine.Text;
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


namespace Ecore2Code
{
    class Options
    {
        public Options()
        {
            OverallNamespace = "GeneratedCode";
        }

        [Option('n', "namespace", HelpText="The root namespace")]
        public string OverallNamespace { get; set; }

        [Option('l', "language", DefaultValue = SupportedLanguage.CS, HelpText = "The language in which the code should be generated")]
        public SupportedLanguage Language { get; set; }

        [Option('f', "folder", HelpText = "Determines whether the code for classes should be separated to multiple files")]
        public bool UseFolders { get; set; }

        [Option('p', "parallel", HelpText = "If specified, runs the code generator in parallel mode (in incubation)")]
        public bool Parallel { get; set; }

        [Option('x', "force", HelpText = "If specified, the code is generated regardless of existing code")]
        public bool Force { get; set; }

        [Option('g', "operations", HelpText = "If specified, the code generator generates stubs for operations")]
        public bool Operations { get; set; }

        [Option('u', "model-uri", HelpText ="If specified, overrides the uri of the base package.")]
        public string Uri { get; set; }

        [Option('p', "primitive-types", HelpText = "If set, Ecore Data types are transformed to primitive types")]
        public bool PrimitiveTypes { get; set; }

        [ValueList(typeof(List<string>))]
        public IList<string> InputFiles { get; set; }

        [Option('o', "output", Required=true, HelpText="The output file/folder in which the code should be generated")]
        public string OutputFile { get; set; }

        [Option('m', "metamodel", Required=false, HelpText="Specify this argument if you want to serialize the NMeta metamodel possibly generated from Ecore")]
        public string NMeta { get; set; }

        [OptionList('r', "resolve", Required=false, HelpText="A list of namespace remappings in the syntax URI=file, multiple entries separated by ';'", Separator=';')]
        public List<string> NamespaceMappings { get; set; }

        [OptionList('t', "type-mapping", Required = false, HelpText = "A list of type mappings in the syntax <Ecore Instance Class>=<.NET class>, multiple entries separated by ';'", Separator =';')]
        public List<string> TypeMappings { get; set; }

        public string GetHelp()
        {
            return HelpText.AutoBuild(this);
        }
    }

    public enum SupportedLanguage
    {
        CS,
        VB,
        CPP,
        JS,
        PY
    }

    class Ecore2Code
    {
        private Options options;
        private ModelRepository repository;

        public Ecore2Code(Options options)
        {
            this.options = options;
        }

        static void Main(string[] args)
        {            
            Options options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
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
            }
            else
            {
                Console.WriteLine("You are using me wrongly!");
                Console.WriteLine("Usage: Ecore2Code [Options] -o [Output File or directory] [Inputfiles]");
                Console.WriteLine("Input files may either be in NMeta or Ecore format.");
                Console.WriteLine("Example: Ecore2Code -f -n NMF.Models -o Meta NMeta.nmf");
                Console.WriteLine(options.GetHelp());
            }
        }

        private void GenerateCode()
        {
            var packageTransform = new NMF.Models.Meta.Meta2ClassesTransformation();
            var stopWatch = new Stopwatch();

            packageTransform.ForceGeneration = options.Force;
            packageTransform.CreateOperations = options.Operations;
            packageTransform.DefaultNamespace = options.OverallNamespace;

            LoadTypeMappings();

            Dictionary<Uri, string> mappings = LoadNamespaceMappings();

            var metaPackage = LoadPackageFromFiles(mappings);
            SetUri(metaPackage);

            Model model = EncapsulateNamespace(metaPackage);
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

        private Dictionary<Uri, string> LoadNamespaceMappings()
        {
            Dictionary<Uri, string> mappings = null;
            if (options.NamespaceMappings != null && options.NamespaceMappings.Count > 0)
            {
                mappings = new Dictionary<Uri, string>();
                foreach (var mapping in options.NamespaceMappings)
                {
                    if (string.IsNullOrEmpty(mapping)) continue;
                    var lastIdx = mapping.LastIndexOf('=');
                    if (lastIdx == -1)
                    {
                        Console.WriteLine("Namespace mapping {0} is missing required separator =", mapping);
                        continue;
                    }
                    Uri uri;
                    if (!Uri.TryCreate(mapping.Substring(0, lastIdx), UriKind.Absolute, out uri))
                    {
                        uri = new Uri(mapping.Substring(0, lastIdx), UriKind.Relative);
                    }
                    mappings.Add(uri, mapping.Substring(lastIdx + 1));
                }
            }

            return mappings;
        }

        private void LoadTypeMappings()
        {
            Ecore2MetaTransformation.GeneratePrimitiveTypes = options.PrimitiveTypes;
            if (options.TypeMappings != null && options.TypeMappings.Count > 0)
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

        private static Model EncapsulateNamespace(INamespace metaPackage)
        {
            var model = metaPackage.Model;
            if (model == null)
            {
                model = new Model();
                model.RootElements.Add(metaPackage);
            }
            model.ModelUri = metaPackage.Uri;
            return model;
        }

        private void SetUri(INamespace metaPackage)
        {
            if (options.Uri != null)
            {
                Uri uri;
                if (Uri.TryCreate(options.Uri, UriKind.Absolute, out uri))
                {
                    metaPackage.Uri = uri;
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
            CodeDomProvider generator = null;

            switch (options.Language)
            {
                case SupportedLanguage.CS:
                    generator = new Microsoft.CSharp.CSharpCodeProvider();
                    break;
                case SupportedLanguage.VB:
                    generator = new Microsoft.VisualBasic.VBCodeProvider();
                    break;
                case SupportedLanguage.CPP:
                    generator = new Microsoft.VisualC.CppCodeProvider();
                    break;
                case SupportedLanguage.JS:
                    generator = new Microsoft.JScript.JScriptCodeProvider();
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
            if (files == null || files.Count == 0) return null;

            var packages = new List<INamespace>();
            repository = new ModelRepository(EcoreInterop.Repository);
            
            if (resolveMappings != null)
            {
                repository.Locators.Add(new FileMapLocator(resolveMappings));
            }

            foreach (var ecoreFile in files)
            {
                if (Path.GetExtension(ecoreFile) == ".ecore")
                {
#if DEBUG
                    var model = repository.Resolve(ecoreFile);
                    var ePackages = model.RootElements.OfType<EPackage>();
                    foreach (var ePackage in ePackages)
                    {
                        packages.Add(EcoreInterop.Transform2Meta(ePackage, AddMissingPackage));
                    }
#else
                    try
                    {
                        var ePackages = repository.Resolve(ecoreFile).RootElements.OfType<EPackage>();
                        foreach (var ePackage in ePackages)
                        {
                            packages.Add(EcoreInterop.Transform2Meta(ePackage, AddMissingPackage));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred reading the Ecore file. The error message was: " + ex.Message);
                        Environment.ExitCode = 1;
                    }
#endif
                }
                else if (Path.GetExtension(ecoreFile) == ".nmf" || Path.GetExtension(ecoreFile) == ".nmeta")
                {
#if DEBUG
                    packages.AddRange(repository.Resolve(ecoreFile).RootElements.OfType<INamespace>());
#else
                    try
                    {
                        packages.AddRange(repository.Resolve(ecoreFile).RootElements.OfType<INamespace>());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred reading the NMeta file. The error message was: " + ex.Message);
                        Environment.ExitCode = 1;
                    }
#endif
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
            if (options.NMeta != null && package.Model != null && package.Model.ModelUri != null && package.Model.ModelUri.IsAbsoluteUri && package.Model.ModelUri.IsFile)
            {
                var path = package.Model.ModelUri.LocalPath;
                var dir = Path.GetDirectoryName(options.NMeta);
                var extension = Path.GetExtension(options.NMeta);
                var file = Path.GetFileNameWithoutExtension(path);
                repository.Save(EncapsulateNamespace(metaNamespace), Path.Combine(dir, file + extension));
            }
        }
    }
}

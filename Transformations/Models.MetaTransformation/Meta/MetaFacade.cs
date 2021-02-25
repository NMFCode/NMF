using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Transformations;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    /// <summary>
    /// Denotes a facade for the code generator
    /// </summary>
    public static class MetaFacade
    {
        private static Meta2ClassesTransformation meta2Classes = new Meta2ClassesTransformation()
        {
            SeparateImplementations = true,
            OnlyNested = false
        };

        /// <summary>
        /// Generates the code for the given namespace
        /// </summary>
        /// <param name="ns">the namespace for which code should be generated</param>
        /// <param name="overAllNamespace">The namespace that should be prepended to the generated code</param>
        /// <returns>A compilation unit with the generated code</returns>
        public static CodeCompileUnit CreateCode(INamespace ns, string overAllNamespace)
        {
            meta2Classes.DefaultNamespace = overAllNamespace;
            return TransformationEngine.Transform<INamespace, CodeCompileUnit>(ns, meta2Classes);
        }

        /// <summary>
        /// Splits the provided code compile unit into chunks such that it contains one class per file
        /// </summary>
        /// <param name="unit">The unit containing all the code</param>
        /// <returns>A dictionary of paths and code compile units</returns>
        public static IDictionary<string, CodeCompileUnit> SplitCompileUnit(CodeCompileUnit unit)
        {
            var dict = new Dictionary<string, CodeCompileUnit>();
            var namespaces = unit.Namespaces.Cast<CodeNamespace>().Where(n => !string.IsNullOrEmpty(n.Name));
            if (!namespaces.Any()) return dict;
            string baseNamespace = namespaces.First().Name;
            CodeNamespace globalNamespace = null;
            foreach (CodeNamespace ns in unit.Namespaces)
            {
                if (string.IsNullOrEmpty(ns.Name))
                {
                    globalNamespace = ns;
                    continue;
                }
                if (ns.Name.StartsWith(baseNamespace)) continue;
                if (baseNamespace.StartsWith(ns.Name))
                {
                    baseNamespace = ns.Name;
                }
                else
                {
                    var commonLength = 0;
                    while (true)
                    {
                        var idx1 = baseNamespace.IndexOf('.', commonLength + 1);
                        var idx2 = ns.Name.IndexOf('.', commonLength + 1);
                        if (idx1 == -1 || idx2 == -1 || idx1 != idx2)
                        {
                            break;
                        }
                        idx1 -= commonLength;
                        if (baseNamespace.Substring(commonLength, idx1) != ns.Name.Substring(commonLength, idx1))
                        {
                            break;
                        }
                        commonLength = idx2;
                    }
                    baseNamespace = baseNamespace.Substring(0, commonLength);
                }
            }
            int offset = baseNamespace.Length;
            if (offset != 0) offset++;
            foreach (CodeNamespace ns in unit.Namespaces)
            {
                if (string.IsNullOrEmpty(ns.Name)) continue;
                foreach (CodeTypeDeclaration type in ns.Types)
                {
                    var newUnit = new CodeCompileUnit();
                    var newNamespace = new CodeNamespace();
                    var imports = new CodeNamespace();
                    newNamespace.Name = ns.Name;
                    for (int i = 0; i < ns.Imports.Count; i++)
                    {
                        imports.Imports.Add(ns.Imports[i]);
                    }
                    if (globalNamespace != null)
                    {
                        imports.Imports.AddRange(globalNamespace.Imports.Cast<CodeNamespaceImport>().ToArray());
                    }
                    newUnit.Namespaces.Add(newNamespace);
                    newUnit.Namespaces.Add(imports);
                    newNamespace.Types.Add(type);
                    string fileName = string.Empty;
                    if (ns.Name != baseNamespace)
                    {
                        fileName = ns.Name.Substring(offset).Replace('.', '\\') + "\\";
                    }
                    dict.Add(fileName + type.Name, newUnit);
                }
            }
            return dict;
        }

        /// <summary>
        /// Generates code for the given unit with the given code provider
        /// </summary>
        /// <param name="unit">The code compile unit</param>
        /// <param name="generator">The code provider</param>
        /// <param name="target">The target path</param>
        /// <param name="splitToFolders">True, if the unit should be split up in one class per file, otherwise false</param>
        public static void GenerateCode(CodeCompileUnit unit, CodeDomProvider generator, string target, bool splitToFolders)
        {
            var genOptions = new CodeGeneratorOptions()
            {
                BlankLinesBetweenMembers = true,
                VerbatimOrder = false,
                ElseOnClosing = false,
                BracingStyle = "C",
                IndentString = "    "
            };

            if (splitToFolders)
            {
                foreach (var file in SplitCompileUnit(unit))
                {
                    var fileInfo = new FileInfo(Path.Combine(target, file.Key) + "." + generator.FileExtension);
                    CheckDirectoryExists(fileInfo.Directory);
                    using (var sw = fileInfo.CreateText())
                    {
                        generator.GenerateCodeFromCompileUnit(file.Value, sw, genOptions);
                    }
                }
            }
            else
            {
                var fileInfo = new FileInfo(target);
                CheckDirectoryExists(fileInfo.Directory);
                using (var sw = fileInfo.CreateText())
                {
                    generator.GenerateCodeFromCompileUnit(unit, sw, genOptions);
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
    }
}

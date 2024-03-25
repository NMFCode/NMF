using NMF.CodeGen;
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
        private static readonly Meta2ClassesTransformation meta2Classes = new Meta2ClassesTransformation()
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
            return CodeDomHelper.SplitCompileUnit(unit);
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

using NMF.AnyText.Metamodel;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Transformation
{
    /// <summary>
    /// Facade class for the AnyText code generator
    /// </summary>
    public static class CodeGenerator
    {
        private static AnytextCodeGenerator _transformation = new AnytextCodeGenerator();

        [ThreadStatic]
        internal static AnytextMetamodelTrace _trace;

        [ThreadStatic]
        internal static CodeGeneratorSettings _settings;

        /// <summary>
        /// Creates a namespace for the given grammar model
        /// </summary>
        /// <param name="grammar">the grammar model</param>
        /// <returns>the metamodel extracted from the grammar</returns>
        public static INamespace CreateNamespace(IGrammar grammar)
        {
            var trace = new AnytextMetamodelTrace();
            return trace.CreateNamespace(grammar, new ModelRepository());
        }

        /// <summary>
        /// Compiles the given grammar into a code model
        /// </summary>
        /// <param name="grammar">the grammar model that should be compiled</param>
        /// <param name="settings">code generator settings</param>
        /// <returns>A code model with the generated code</returns>
        public static CodeCompileUnit Compile(IGrammar grammar, CodeGeneratorSettings settings)
        {
            var globNs = new CodeNamespace();
            var context = new TransformationContext(_transformation);
            _settings = settings ?? new CodeGeneratorSettings { Namespace = "Generated" };
            var trace = new AnytextMetamodelTrace();
            _trace = trace;
            trace.CreateNamespace(grammar, new ModelRepository());
            var grammarNs = TransformationEngine.Transform<IGrammar, CodeNamespace>(grammar, context);

            MoveImports(grammarNs, globNs);

            _trace = null;
            return new CodeCompileUnit
            {
                Namespaces =
                {
                    globNs, grammarNs
                }
            };
        }

        private static void MoveImports(CodeNamespace fromNamespace, CodeNamespace toNamespace)
        {
            for (int i = 0; i < fromNamespace.Imports.Count; i++)
            {
                toNamespace.Imports.Add(fromNamespace.Imports[i]);
            }
            fromNamespace.Imports.Clear();
        }
    }
}

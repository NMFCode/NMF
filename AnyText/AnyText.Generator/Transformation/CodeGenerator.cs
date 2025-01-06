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
    public static class CodeGenerator
    {
        private static AnytextCodeGenerator _transformation = new AnytextCodeGenerator();

        [ThreadStatic]
        internal static AnytextMetamodelTrace _trace;

        [ThreadStatic]
        internal static CodeGeneratorSettings _settings;

        public static INamespace CreateNamespace(IGrammar grammar)
        {
            var trace = new AnytextMetamodelTrace();
            return trace.CreateNamespace(grammar, new ModelRepository());
        }

        public static CodeCompileUnit Compile(IGrammar grammar, CodeGeneratorSettings settings)
        {
            var globNs = new CodeNamespace();
            var context = new TransformationContext(_transformation);
            _settings = settings;
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

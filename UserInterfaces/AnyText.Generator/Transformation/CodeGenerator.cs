﻿using NMF.AnyText.Metamodel;
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

        public static CodeCompileUnit Compile(IGrammar grammar, CodeGeneratorSettings settings)
        {
            var globNs = new CodeNamespace();

            foreach (var nsImport in settings.ImportedNamespaces)
            {
                globNs.Imports.Add(new CodeNamespaceImport(nsImport));
            }
            var context = new TransformationContext(_transformation);
            var trace = new AnytextMetamodelTrace();
            _trace = trace;
            trace.CreateNamespace(grammar, new ModelRepository());
            var grammarNs = TransformationEngine.Transform<IGrammar, CodeNamespace>(grammar, context);

            _trace = null;
            return new CodeCompileUnit
            {
                Namespaces =
                {
                    globNs, grammarNs
                }
            };
        }
    }
}
using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta2
{
    public partial class Meta2ClassesTransformation
    {
        public class Namespace2CompileUnit : TransformationRule<Namespace, CodeCompileUnit>
        {
            private Namespace2Namespace ns2ns;

            public override void Transform(Namespace input, CodeCompileUnit output, ITransformationContext context)
            {
                AddNamespaceRecursively(input, output, context);
            }

            private void AddNamespaceRecursively(Namespace ns, CodeCompileUnit unit, ITransformationContext context)
            {
                var codeNs = context.Trace.ResolveIn(ns2ns, ns);
                unit.Namespaces.Add(codeNs);
                foreach (var subNs in ns.ChildNamespaces)
                {
                    AddNamespaceRecursively(subNs, unit, context);
                }
            }

            public override void RegisterDependencies()
            {
                ns2ns = Rule<Namespace2Namespace>();

                Require(ns2ns);
            }
        }
    }
}

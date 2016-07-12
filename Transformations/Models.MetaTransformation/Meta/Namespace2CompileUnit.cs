using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// Gets or sets a flag determing whether code should be generated regardless of existing code
        /// </summary>
        public bool ForceGeneration { get; set; }

        /// <summary>
        /// The transformation rule to generate a namespace to a single code compile unit
        /// </summary>
        public class Namespace2CompileUnit : TransformationRule<INamespace, CodeCompileUnit>
        {
            /// <summary>
            /// Initializes the generated code compile unit
            /// </summary>
            /// <param name="input">The NMeta base namespace</param>
            /// <param name="output">The generated code compile unit</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(INamespace input, CodeCompileUnit output, ITransformationContext context)
            {
                var t = Transformation as Meta2ClassesTransformation;
                if (t == null || t.OnlyNested)
                {
                    AddNamespace(input, output, context, t);
                }
                else
                {
                    foreach (var item in context.Trace.TraceAllIn(ns2ns))
                    {
                        if (AddToCompileUnit(item.GetInput(0) as INamespace, t))
                        {
                            output.Namespaces.Add(item.Output as CodeNamespace);
                        }
                    }
                }
            }

            private static void AddNamespace(INamespace ns, CodeCompileUnit unit, ITransformationContext context, Meta2ClassesTransformation t)
            {
                if (AddToCompileUnit(ns, t))
                {
                    var codeNamespace = context.Trace.ResolveIn(ns2ns, ns);
                    unit.Namespaces.Add(codeNamespace);
                }
                foreach (var child in ns.ChildNamespaces)
                {
                    AddNamespace(child, unit, context, t);
                }
            }

            private static bool AddToCompileUnit(INamespace n, Meta2ClassesTransformation t)
            {
                if (t == null) return true;
                return t.AddToCompileUnit(n);
            }

            private static Namespace2Namespace ns2ns;

            /// <summary>
            /// Registers the dependencies, i.e. transform the base namespace
            /// </summary>
            public override void RegisterDependencies()
            {
                Require(Rule<Namespace2Namespace>());

                ns2ns = Rule<Namespace2Namespace>();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the given namespace should be added to the compile unit
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        protected virtual bool AddToCompileUnit(INamespace n)
        {
            if (ForceGeneration)
            {
                return true;
            }
            else
            {
                var uri = n.Uri;
                if (uri == null) return true;
                return !MetaRepository.Instance.Models.ContainsKey(uri);
            }
        }
    }
}

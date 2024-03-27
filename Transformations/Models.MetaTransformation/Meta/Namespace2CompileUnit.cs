using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using System.CodeDom;

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
                        var ns = item.GetInput(0) as INamespace;
                        if (ns != input && t.GenerateForInputOnly)
                        {
                            continue;
                        }

                        if (AddToCompileUnit(ns, t))
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

#pragma warning disable S2696 // Instance members should not write to "static" fields
                ns2ns = Rule<Namespace2Namespace>();
#pragma warning restore S2696 // Instance members should not write to "static" fields
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
                var absoluteUri = n.AbsoluteUri;
                return uri != null && (!MetaRepository.Instance.Models.ContainsKey(uri))
                    && (absoluteUri == null || !MetaRepository.Instance.Models.ContainsKey(absoluteUri));
            }
        }
    }
}

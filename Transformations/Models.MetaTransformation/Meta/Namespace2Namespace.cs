using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Utilities;
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
        /// The transformation rule to transform namespaces to code namespaces
        /// </summary>
        public class Namespace2Namespace : NamespaceGenerator<INamespace>
        {
            /// <summary>
            /// Gets the default imported namespaces
            /// </summary>
            public override IEnumerable<string> DefaultImports
            {
                get
                {
                    yield return "System";
                    yield return "System.Collections";
                    yield return "System.Collections.Generic";
                    yield return "System.Collections.ObjectModel";
                    yield return "System.ComponentModel";
                    yield return "System.Diagnostics";
                    yield return "System.Linq";
                    yield return "NMF.Expressions";
                    yield return "NMF.Expressions.Linq";
                    yield return "NMF.Models";
                    yield return "NMF.Models.Collections";
                    yield return "NMF.Collections.Generic";
                    yield return "NMF.Collections.ObjectModel";
                    yield return "NMF.Serialization";
                    yield return "NMF.Utilities";
                }
            }

            /// <summary>
            /// Gets the name of the given namespace
            /// </summary>
            /// <param name="input">The NMeta namespace</param>
            /// <returns>The fully qualified namespace</returns>
            protected override string GetName(INamespace input)
            {
                string baseName;
                if (input.ParentNamespace == null)
                {
                    var t = Transformation as Meta2ClassesTransformation;
                    if (t != null)
                    {
                        baseName = t.DefaultNamespace;
                    }
                    else
                    {
                        baseName = "GeneratedCode";
                    }
                }
                else
                {
                    baseName = GetName(input.ParentNamespace);
                }
                return baseName + "." + input.Name.ToPascalCase();
            }

            /// <summary>
            /// Registers the dependencies, i.e. generates child namespaces and types
            /// </summary>
            public override void RegisterDependencies()
            {
                RequireMany(this, n => n.ChildNamespaces);
                RequireTypes(Rule<Type2Type>(), n => n.Types);
            }
        }
    }
}

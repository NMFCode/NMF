﻿using NMF.CodeGen;
using NMF.Utilities;
using System.Collections.Generic;

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
                    if (Transformation is Meta2ClassesTransformation t)
                    {
                        return t.SystemImports;
                    }
                    else
                    {
                        return DefaultSystemImports;
                    }
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
                    if (Transformation is Meta2ClassesTransformation t && input.Uri != null)
                    {
                        if (!t.NamespaceMap.TryGetValue(input.Uri, out baseName))
                        {
                            baseName = t.DefaultNamespace;
                        }
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
                var nsName = input.Name.ToPascalCase();
                return string.IsNullOrEmpty(baseName) ? nsName : baseName + "." + nsName;
            }

            /// <summary>
            /// Registers the dependencies, i.e. generates child namespaces and types
            /// </summary>
            public override void RegisterDependencies()
            {
                TransformationDelayLevel = 2;
                RequireMany(this, n => n.ChildNamespaces);
                RequireTypes(Rule<Type2Type>(), n => n.Types);
            }
        }
    }
}

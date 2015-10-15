using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.CodeGen
{
    /// <summary>
    /// Represents a transformation rule that generates namespaces
    /// </summary>
    /// <typeparam name="T">The model element type from which namespaces are being generated</typeparam>
    public abstract class NamespaceGenerator<T> : TransformationRule<T, CodeNamespace>
        where T : class
    {
        protected NamespaceGenerator()
        {
            TransformationDelayLevel = 1;
        }

        /// <summary>
        /// Creates the uninitialized transformation rule output
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="context">The transformation context</param>
        /// <returns>A blank code namespace with the correct name</returns>
        public override CodeNamespace CreateOutput(T input, ITransformationContext context)
        {
            return new CodeNamespace(GetName(input));
        }

        /// <summary>
        /// Gets the default namespace imports in a derived class
        /// </summary>
        public abstract IEnumerable<string> DefaultImports
        {
            get;
        }

        /// <summary>
        /// Gets the name for the code namespace generated from the given model element
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <returns>The name of the code namespace</returns>
        protected abstract string GetName(T input);


        /// <summary>
        /// Initializes the generated code namespace
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="output">The generated code namespace</param>
        /// <param name="context">The transformation context</param>
        public override void Transform(T input, CodeNamespace output, ITransformationContext context)
        {
            var imports = new List<string>(DefaultImports);

            foreach (var item in imports)
            {
                output.Imports.Add(new CodeNamespaceImport(item));
            }

            var usings = new HashSet<string>();

            var initialTypeCount = output.Types.Count;
            for (int i = 0; i < initialTypeCount; i++)
            {
                RecursivelyAddTypes(output.Types[i], output, usings);
            }

            usings.Remove(output.Name);
            foreach (var item in usings)
            {
                output.Imports.Add(new CodeNamespaceImport(item));
            }
        }

        private void RecursivelyAddTypes(CodeTypeMember member, CodeNamespace output, HashSet<string> usings)
        {
            var codeType = member as CodeTypeDeclaration;
            if (codeType != null)
            {
                FixClassNamespaces(output, usings, codeType);
                var iFace = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(codeType, CodeDomHelper.InterfaceKey);
                if (iFace != null)
                {
                    FixClassNamespaces(output, usings, iFace);
                }
            }

            var dependentMembers = CodeDomHelper.DependentMembers(member, false);
            if (dependentMembers != null)
            {
                foreach (var dependent in dependentMembers)
                {
                    RecursivelyAddTypes(dependent, output, usings);
                }
            }

            var dependentTypes = CodeDomHelper.DependentTypes(member, false);
            if (dependentTypes != null)
            {
                foreach (var dependentType in dependentTypes)
                {
                    if (!output.Types.Contains(dependentType))
                    {
                        AddType(output, dependentType);
                    }
                }
            }
        }

        private void FixClassNamespaces(CodeNamespace output, HashSet<string> usings, CodeTypeDeclaration type)
        {
            for (int i = 0; i < type.BaseTypes.Count; i++)
            {
                var baseRef = type.BaseTypes[i];
                type.BaseTypes[i] = AddNamespaceUse(baseRef, output, usings);
            }

            for (int i = 0; i < type.Members.Count; i++)
            {
                var childMember = type.Members[i];
                AddMemberUses(childMember, output, usings);
            }
        }

        private void AddMemberUses(CodeTypeMember member, CodeNamespace ns, HashSet<string> usings)
        {
            var memberProperty = member as CodeMemberProperty;
            if (memberProperty != null)
            {
                memberProperty.Type = AddNamespaceUse(memberProperty.Type, ns, usings);
                return;
            }
            var memberField = member as CodeMemberField;
            if (memberField != null)
            {
                memberField.Type = AddNamespaceUse(memberField.Type, ns, usings);
                return;
            }
            var memberMethod = member as CodeMemberMethod;
            if (memberMethod != null)
            {
                memberMethod.ReturnType = AddNamespaceUse(memberMethod.ReturnType, ns, usings);
                for (int j = 0; j < memberMethod.Parameters.Count; j++)
                {
                    memberMethod.Parameters[j].Type = AddNamespaceUse(memberMethod.Parameters[j].Type, ns, usings);
                }
                return;
            }
            var memberEvent = member as CodeMemberEvent;
            if (memberEvent != null)
            {
                memberEvent.Type = AddNamespaceUse(memberEvent.Type, ns, usings);
                return;
            }
            var nestedType = member as CodeTypeDeclaration;
            if (nestedType != null)
            {
                for (int i = 0; i < nestedType.Members.Count; i++)
                {
                    AddMemberUses(nestedType.Members[i], ns, usings);
                }
            }
        }

        private CodeTypeReference AddNamespaceUse(CodeTypeReference typeRef, CodeNamespace ns , HashSet<string> usings)
        {
            if (typeRef == null) return null;
            var refns = CodeDomHelper.Namespace(typeRef);
            if (typeRef.TypeArguments.Count == 0)
            {
                if (IsNamespaceConflict(typeRef.BaseType, ns) && refns != null)
                {
                    return new CodeTypeReference(refns + "." + typeRef.BaseType);
                }
                else
                {
                    if (refns != null) usings.Add(refns);
                    return typeRef;
                }
            }
            else
            {
                var newRef = new CodeTypeReference(typeRef.BaseType);
                for (int i = 0; i < typeRef.TypeArguments.Count; i++)
                {
                    newRef.TypeArguments.Add(AddNamespaceUse(typeRef.TypeArguments[i], ns, usings));
                }
                return newRef;
            }
        }

        /// <summary>
        /// Creates a dependency that the given subsequent model element is transformed to a type
        /// </summary>
        /// <typeparam name="TType">The model element of the input type</typeparam>
        /// <param name="rule">The transformation rule used to generate the type</param>
        /// <param name="selector">A function selecting the correct subsequent model element</param>
        /// <returns>The created transformation rule dependency</returns>
        public ITransformationRuleDependency RequireType<TType>(TransformationRuleBase<TType, CodeTypeDeclaration> rule, Func<T, TType> selector)
            where TType : class
        {
            return Require(rule, selector, AddType);
        }

        /// <summary>
        /// Gets called when the given type is added to the output namespace, e.g. to prevent name conflicts
        /// </summary>
        /// <param name="type"></param>
        private void AddType(CodeNamespace ns, CodeTypeDeclaration type)
        {
            var name = CreateNewValidName(ns, type.Name);
            var typeRef = type.GetReferenceForType();
            var isSelfPointing = false;
            if (typeRef != null)
            {
                isSelfPointing = typeRef.BaseType == type.Name;
                if (CodeDomHelper.Namespace(typeRef) == null)
                {
                    CodeDomHelper.SetNamespace(typeRef, ns.Name);
                }
            }
            if (name != type.Name)
            {
                type.Name = name;
                if (isSelfPointing)
                {
                    typeRef.BaseType = name;
                }
            }
            ns.Types.Add(type);
        }

        private string CreateNewValidName(CodeNamespace ns, string originalName)
        {
            var name = originalName;
            var nameCounter = 0;
            while (!IsValidName(name, ns))
            {
                name = originalName + nameCounter.ToString();
                nameCounter++;
            }
            return name;
        }

        /// <summary>
        /// Decides whether the given name is a valid class name for a type
        /// </summary>
        /// <param name="name">The proposed name</param>
        /// <param name="ns">The proposed namespace</param>
        /// <returns>True if the name is valid, otherwise False</returns>
        protected virtual bool IsValidName(string name, CodeNamespace ns)
        {
            for (int i = 0; i < ns.Types.Count; i++)
            {
                if (ns.Types[i].Name == name) return false;
            }
            return !DefaultImports.Contains(name);
        }

        /// <summary>
        /// Decides whether the given name causes a namespace conflict
        /// </summary>
        /// <param name="name">The proposed name</param>
        /// <param name="ns">The namespace</param>
        /// <returns>True, if there is a namespace conflict so that the given name must be fully qualified, otherwise False</returns>
        protected virtual bool IsNamespaceConflict(string name, CodeNamespace ns)
        {
            foreach (var import in DefaultImports)
            {
                var t = Type.GetType(import + "." + name);
                if (t != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a dependency that the given subsequent model elements are transformed to types
        /// </summary>
        /// <typeparam name="TType">The model elements input type</typeparam>
        /// <param name="rule">The transformation rule used to generate the types</param>
        /// <param name="selector">A function that selects the subsequent model elements</param>
        /// <returns>The created transformation rule dependency</returns>
        public ITransformationRuleDependency RequireTypes<TType>(TransformationRuleBase<TType, CodeTypeDeclaration> rule, Func<T, IEnumerable<TType>> selector)
            where TType : class
        {
            return RequireMany(rule, selector, (ns, types) =>
            {
                foreach (var decl in types)
                {
                    AddType(ns, decl);
                }
            });
        }
    }
}

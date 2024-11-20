using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace NMF.CodeGen
{
    /// <summary>
    /// Represents a transformation rule that generates namespaces
    /// </summary>
    /// <typeparam name="T">The model element type from which namespaces are being generated</typeparam>
    public abstract class NamespaceGenerator<T> : TransformationRule<T, CodeNamespace>
        where T : class
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
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
            foreach (var item in DefaultImports)
            {
                output.Imports.Add(new CodeNamespaceImport(item));
            }

            Dictionary<string, string> nameConflicts = new Dictionary<string, string>(LoadOrGenerateNameConflicts(context));
            FillNameConflictsWithDefaultAssemblies(nameConflicts, Enumerable.Repeat(output.Name, 1));
            foreach (var type in output.Types.OfType<CodeTypeDeclaration>())
            {
                RegisterConflict(output, nameConflicts, type);
            }
            var usings = new HashSet<string>();
            VisitNamespace(output, reference => CorrectNamespace(reference, output, usings, nameConflicts));

#pragma warning disable S4158 // Empty collections should not be accessed or iterated
            usings.Remove(output.Name);
#pragma warning restore S4158 // Empty collections should not be accessed or iterated
            if (usings.Count != 0)
            {
                FillNameConflictsWithDefaultAssemblies(nameConflicts, usings);
                VisitNamespace(output, reference => CorrectNamespace(reference, output, usings, nameConflicts));
            }
            
            foreach (var item in usings)
            {
                output.Imports.Add(new CodeNamespaceImport(item));
            }
        }

        private void RecursivelyAddTypes(CodeTypeMember member, CodeNamespace output)
        {
            var dependentMembers = CodeDomHelper.DependentMembers(member, false);
            if (dependentMembers != null)
            {
                foreach (var dependent in dependentMembers)
                {
                    RecursivelyAddTypes(dependent, output);
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

        private CodeTypeReference CorrectNamespace(CodeTypeReference reference, CodeNamespace output, HashSet<string> usings, Dictionary<string, string> nameConflicts)
        {
            var refNs = reference.Namespace();
            if (refNs != null)
            {
                string chosenNs;
                if (refNs != output.Name)
                {
                    if (nameConflicts.TryGetValue(reference.BaseType, out chosenNs) && chosenNs != refNs)
                    {
                        // We are in a namespace conflict and need to fully qualify the reference
                        reference.BaseType = refNs + "." + reference.BaseType;
                    }
                    else
                    {
                        usings.Add(refNs);
                    }
                }
            }
            for (int i = 0; i < reference.TypeArguments.Count; i++)
            {
                reference.TypeArguments[i] = CorrectNamespace(reference.TypeArguments[i], output, usings, nameConflicts);
            }
            return reference;
        }

        /// <summary>
        /// Gets the identifier for namespace conflicts
        /// </summary>
        /// <returns>An object that is used to obtain namespace conflicts</returns>
        protected virtual object GetNamespaceConflictsIdentifier()
        {
            return typeof(NamespaceGenerator<>);
        }

        private Dictionary<string, string> LoadOrGenerateNameConflicts(ITransformationContext context)
        {
            object nameConflictsObject;
            object key = GetNamespaceConflictsIdentifier() ?? typeof(NamespaceGenerator<>);
            Dictionary<string, string> nameConflicts;
            if (context.Data.TryGetValue(key, out nameConflictsObject))
            {
                nameConflicts = nameConflictsObject as Dictionary<string, string>;
                if (nameConflicts == null)
                {
                    lock (key)
                    {
                        nameConflicts = context.Data[key] as Dictionary<string, string>;
                        if (nameConflicts == null)
                        {
                            nameConflicts = GenerateNameConflicts(context);
                            context.Data[key] = nameConflicts;
                        }
                    }
                }
            }
            else
            {
                lock (key)
                {
                    if (context.Data.TryGetValue(key, out nameConflictsObject))
                    {
                        nameConflicts = nameConflictsObject as Dictionary<string, string>;
                        if (nameConflicts == null)
                        {
                            nameConflicts = GenerateNameConflicts(context);
                            context.Data[key] = nameConflicts;
                        }
                    }
                    else
                    {
                        nameConflicts = GenerateNameConflicts(context);
                        context.Data.Add(key, nameConflicts);
                    }
                }
            }

            return nameConflicts;
        }

        private Dictionary<string, string> GenerateNameConflicts(ITransformationContext context)
        {
            var nameDict = new Dictionary<string, string>();
            FillNameConflictsWithDefaultAssemblies(nameDict, DefaultImports);
            List<NamespaceGenerator<T>> rules = CalculateDependentRuleTypes(context);
            // Second pass: Populate name conflicts
            foreach (var rule in rules)
            {
                CalculateConflictsInRule(context, nameDict, rule);
            }
            return nameDict;
        }

        private void FillNameConflictsWithDefaultAssemblies(Dictionary<string, string> nameDict, IEnumerable<string> usings)
        {
            foreach (var ass in AssembliesToCheck)
            {
                foreach (var type in ass.GetExportedTypes())
                {
                    if (usings.Contains(type.Namespace))
                    {
                        RegisterConflict(type.Name, nameDict, type.Namespace, false);
                    }
                }
            }
        }

        private void CalculateConflictsInRule(ITransformationContext context, Dictionary<string, string> nameDict, NamespaceGenerator<T> rule)
        {
            foreach (var codeNs in context.Trace.FindAllIn(rule))
            {
                VisitNamespace(codeNs, reference =>
                {
                    RegisterConflicts(reference, nameDict);
                    for (int i = 0; i < reference.TypeArguments.Count; i++)
                    {
                        RegisterConflicts(reference.TypeArguments[i], nameDict);
                    }
                    return reference;
                });
                foreach (CodeTypeDeclaration codeType in codeNs.Types)
                {
                    RegisterConflict(codeType.Name, nameDict, codeNs.Name, true);
                }
            }
        }

        private List<NamespaceGenerator<T>> CalculateDependentRuleTypes(ITransformationContext context)
        {
            var rules = context.Transformation.Rules.OfType<NamespaceGenerator<T>>().ToList();
            // First pass: Add all dependent types
            foreach (var rule in rules)
            {
                foreach (var codeNs in context.Trace.FindAllIn(rule))
                {
                    for (int i = codeNs.Types.Count - 1; i >= 0; i--)
                    {
                        RecursivelyAddTypes(codeNs.Types[i], codeNs);
                    }
                }
            }

            return rules;
        }

        private void RegisterConflicts(CodeTypeReference reference, Dictionary<string, string> nameDict)
        {
            var refNs = reference.Namespace();
            if (refNs != null)
            {
                RegisterConflict(reference.BaseType, nameDict, refNs, true);
            }
        }

        private void RegisterConflict(CodeNamespace output, Dictionary<string, string> nameConflicts, CodeTypeDeclaration type)
        {
            RegisterConflict(type.Name, nameConflicts, output.Name, false);
            foreach (var nested in type.Members.OfType<CodeTypeDeclaration>())
            {
                RegisterConflict(output, nameConflicts, nested);
            }
        }

        private void RegisterConflict(string typeName, Dictionary<string, string> nameDict, string refNs, bool checkSystemConflict)
        {
            string chosenClass;
            if (nameDict.TryGetValue(typeName, out chosenClass))
            {
                if (refNs != chosenClass)
                {
                    nameDict[typeName] = null;
                }
            }
            else
            {
                nameDict.Add(typeName, checkSystemConflict && IsSystemNameConflict(typeName, refNs) ? null : refNs);
            }
        }

        /// <summary>
        /// Determines whether the given type name is a conflict with a system type
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="refNs">the suggested namespace</param>
        /// <returns></returns>
        protected virtual bool IsSystemNameConflict(string typeName, string refNs)
        {
            foreach (var defaultNamespace in DefaultImports)
            {
                var type = Type.GetType(defaultNamespace + "." + typeName, false);
                if (type != null && type.Namespace != refNs)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the assemblies in which to check for system name conflicts
        /// </summary>
        protected virtual IEnumerable<System.Reflection.Assembly> AssembliesToCheck
        {
            get
            {
                yield return typeof(object).Assembly;
            }
        }

        private static void VisitNamespace(CodeNamespace ns, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            for (int i = 0; i < ns.Types.Count; i++)
            {
                VisitClass(ns.Types[i], referenceConversion);
            }
        }

        private static void VisitClass(CodeTypeDeclaration type, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            for (int i = 0; i < type.BaseTypes.Count; i++)
            {
                var baseRef = type.BaseTypes[i];
                type.BaseTypes[i] = referenceConversion(baseRef);
            }

            for (int i = 0; i < type.Members.Count; i++)
            {
                var childMember = type.Members[i];
                VisitMember(childMember, referenceConversion);
            }
        }

        private static void VisitMember(CodeTypeMember member, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            if (member is CodeMemberProperty memberProperty)
            {
                memberProperty.Type = referenceConversion(memberProperty.Type);
                VisitStatements(memberProperty.GetStatements, referenceConversion);
                VisitStatements(memberProperty.SetStatements, referenceConversion);
                return;
            }
            if (member is CodeMemberField memberField)
            {
                memberField.Type = referenceConversion(memberField.Type);
                VisitExpression(memberField.InitExpression, referenceConversion);
                return;
            }
            if (member is CodeMemberMethod memberMethod)
            {
                memberMethod.ReturnType = referenceConversion(memberMethod.ReturnType);
                for (int j = 0; j < memberMethod.Parameters.Count; j++)
                {
                    memberMethod.Parameters[j].Type = referenceConversion(memberMethod.Parameters[j].Type);
                }
                VisitStatements(memberMethod.Statements, referenceConversion);
                return;
            }
            if (member is CodeMemberEvent memberEvent)
            {
                memberEvent.Type = referenceConversion(memberEvent.Type);
                return;
            }
            if (member is CodeTypeDeclaration nestedType)
            {
                VisitClass(nestedType, referenceConversion);
            }
        }

        private static void VisitStatements(CodeStatementCollection statements, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            if (statements == null) return;
            for (int i = 0; i < statements.Count; i++)
            {
                var statement = statements[i];
                VisitStatement(statement, referenceConversion);
            }
        }

        private static void VisitStatement(CodeStatement statement, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            switch (statement)
            {
                case CodeExpressionStatement expressionStatement:
                    VisitExpression(expressionStatement.Expression, referenceConversion);
                    return;
                case CodeConditionStatement ifStmt:
                    VisitExpression(ifStmt.Condition, referenceConversion);
                    VisitStatements(ifStmt.TrueStatements, referenceConversion);
                    VisitStatements(ifStmt.FalseStatements, referenceConversion);
                    return;
                case CodeVariableDeclarationStatement decl:
                    VisitExpression(decl.InitExpression, referenceConversion);
                    return;
                case CodeAssignStatement assign:
                    VisitExpression(assign.Left, referenceConversion);
                    VisitExpression(assign.Right, referenceConversion);
                    return;
                case CodeAttachEventStatement attach:
                    VisitExpression(attach.Event, referenceConversion);
                    VisitExpression(attach.Listener, referenceConversion);
                    return;
                case CodeRemoveEventStatement detach:
                    VisitExpression(detach.Event, referenceConversion);
                    VisitExpression(detach.Listener, referenceConversion);
                    return;
                case CodeMethodReturnStatement ret:
                    VisitExpression(ret.Expression, referenceConversion);
                    return;
                case CodeThrowExceptionStatement throwE:
                    VisitExpression(throwE.ToThrow, referenceConversion);
                    return;
            }
        }

        private static void VisitExpression(CodeExpression expression, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            if (expression == null)
            {
                return;
            }

            switch (expression)
            {
                case CodeMethodInvokeExpression mce:
                    VisitExpression(mce.Method.TargetObject, referenceConversion);
                    for (int i = 0; i < mce.Parameters.Count; i++)
                    {
                        VisitExpression(mce.Parameters[i], referenceConversion);
                    }
                    return;
                case CodeBinaryOperatorExpression bin:
                    VisitExpression(bin.Left, referenceConversion);
                    VisitExpression(bin.Right, referenceConversion);
                    return;
                case CodeCastExpression cast:
                    cast.TargetType = referenceConversion(cast.TargetType);
                    VisitExpression(cast.Expression, referenceConversion);
                    return;
                case CodeTypeReferenceExpression typeExp:
                    typeExp.Type = referenceConversion(typeExp.Type);
                    return;
                case CodePropertyReferenceExpression propertyRef:
                    VisitExpression(propertyRef.TargetObject, referenceConversion);
                    return;
                case CodeFieldReferenceExpression fieldRef:
                    VisitExpression(fieldRef.TargetObject, referenceConversion);
                    return;
                case CodeDelegateCreateExpression dce:
                    VisitExpression(dce.TargetObject, referenceConversion);
                    dce.DelegateType = referenceConversion(dce.DelegateType);
                    return;
                case CodeDelegateInvokeExpression die:
                    VisitExpression(die.TargetObject, referenceConversion);
                    for (int i = 0; i < die.Parameters.Count; i++)
                    {
                        VisitExpression(die.Parameters[i], referenceConversion);
                    }
                    return;
                case CodeTypeOfExpression typeOf:
                    typeOf.Type = referenceConversion(typeOf.Type);
                    return;
                case CodeObjectCreateExpression objectCreate:
                    objectCreate.CreateType = referenceConversion(objectCreate.CreateType);
                    for (int i = 0; i < objectCreate.Parameters.Count; i++)
                    {
                        VisitExpression(objectCreate.Parameters[i], referenceConversion);
                    }
                    return;
                case CodeArrayCreateExpression arrayCreate:
                    arrayCreate.CreateType = referenceConversion(arrayCreate.CreateType);
                    for (int i = 0; i < arrayCreate.Initializers.Count; i++)
                    {
                        VisitExpression(arrayCreate.Initializers[i], referenceConversion);
                    }
                    return;
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

        private void AddType(CodeNamespace ns, CodeTypeDeclaration type)
        {
            var name = CreateNewValidName(ns, type.Name);
            var typeRef = type.GetReferenceForType();
            var isSelfPointing = false;
            if (typeRef != null && typeRef.UserData.Contains(CodeDomHelper.NamespaceKey))
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

﻿using NMF.Transformations;
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

            Dictionary<string, string> nameConflicts = LoadOrGenerateNameConflicts(context);
            var usings = new HashSet<string>();

            VisitNamespace(output, reference => CorrectNamespace(reference, output, usings, nameConflicts));

            usings.Remove(output.Name);
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
            var rules = context.Transformation.Rules.OfType<NamespaceGenerator<T>>().ToList();
            // First pass: Add all dependent types
            foreach (var rule in rules)
            {
                foreach (var codeNs in context.Trace.FindAllIn(rule))
                {
                    for (int i = codeNs.Types.Count - 1; i >= 0 ; i--)
                    {
                        RecursivelyAddTypes(codeNs.Types[i], codeNs);
                    }
                }
            }
            // Second pass: Populate name conflicts
            foreach (var rule in rules)
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
                        RegisterConflict(codeType.Name, nameDict, codeNs.Name);
                    }
                }
            }
            return nameDict;
        }

        private void RegisterConflicts(CodeTypeReference reference, Dictionary<string, string> nameDict)
        {
            var refNs = reference.Namespace();
            if (refNs != null)
            {
                RegisterConflict(reference.BaseType, nameDict, refNs);
            }
        }

        private void RegisterConflict(string typeName, Dictionary<string, string> nameDict, string refNs)
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
                nameDict.Add(typeName, IsSystemNameConflict(typeName) ? null : refNs);
            }
        }

        /// <summary>
        /// Determines whether the given type name is a conflict with a system type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual bool IsSystemNameConflict(string typeName)
        {
            foreach (var ass in AssembliesToCheck)
            {
                foreach (var defaultNamespace in DefaultImports)
                {
                    if (Type.GetType(defaultNamespace + "." + typeName, false) != null)
                    {
                        return true;
                    }
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

        private void VisitNamespace(CodeNamespace ns, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            for (int i = 0; i < ns.Types.Count; i++)
            {
                VisitClass(ns.Types[i], referenceConversion);
            }
        }

        private void VisitClass(CodeTypeDeclaration type, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
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

        private void VisitMember(CodeTypeMember member, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            var memberProperty = member as CodeMemberProperty;
            if (memberProperty != null)
            {
                memberProperty.Type = referenceConversion(memberProperty.Type);
                VisitStatements(memberProperty.GetStatements, referenceConversion);
                VisitStatements(memberProperty.SetStatements, referenceConversion);
                return;
            }
            var memberField = member as CodeMemberField;
            if (memberField != null)
            {
                memberField.Type = referenceConversion(memberField.Type);
                VisitExpression(memberField.InitExpression, referenceConversion, true);
                return;
            }
            var memberMethod = member as CodeMemberMethod;
            if (memberMethod != null)
            {
                memberMethod.ReturnType = referenceConversion(memberMethod.ReturnType);
                for (int j = 0; j < memberMethod.Parameters.Count; j++)
                {
                    memberMethod.Parameters[j].Type = referenceConversion(memberMethod.Parameters[j].Type);
                }
                VisitStatements(memberMethod.Statements, referenceConversion);
                return;
            }
            var memberEvent = member as CodeMemberEvent;
            if (memberEvent != null)
            {
                memberEvent.Type = referenceConversion(memberEvent.Type);
                return;
            }
            var nestedType = member as CodeTypeDeclaration;
            if (nestedType != null)
            {
                for (int i = 0; i < nestedType.Members.Count; i++)
                {
                    VisitMember(nestedType.Members[i], referenceConversion);
                }
            }
        }

        private void VisitStatements(CodeStatementCollection statements, Func<CodeTypeReference, CodeTypeReference> referenceConversion)
        {
            if (statements == null) return;
            for (int i = 0; i < statements.Count; i++)
            {
                var statement = statements[i];
                var expressionStatement = statement as CodeExpressionStatement;
                if (expressionStatement != null)
                {
                    VisitExpression(expressionStatement.Expression, referenceConversion);
                    continue;
                }
                var ifStmt = statement as CodeConditionStatement;
                if (ifStmt != null)
                {
                    VisitExpression(ifStmt.Condition, referenceConversion);
                    VisitStatements(ifStmt.TrueStatements, referenceConversion);
                    VisitStatements(ifStmt.FalseStatements, referenceConversion);
                    continue;
                }
                var decl = statement as CodeVariableDeclarationStatement;
                if (decl != null)
                {
                    VisitExpression(decl.InitExpression, referenceConversion);
                    continue;
                }
                var assign = statement as CodeAssignStatement;
                if (assign != null)
                {
                    VisitExpression(assign.Left, referenceConversion);
                    VisitExpression(assign.Right, referenceConversion);
                    continue;
                }
                var attach = statement as CodeAttachEventStatement;
                if (attach != null)
                {
                    VisitExpression(attach.Event, referenceConversion);
                    VisitExpression(attach.Listener, referenceConversion);
                    continue;
                }
                var detach = statement as CodeRemoveEventStatement;
                if (detach != null)
                {
                    VisitExpression(detach.Event, referenceConversion);
                    VisitExpression(detach.Listener, referenceConversion);
                    continue;
                }
                var ret = statement as CodeMethodReturnStatement;
                if (ret != null)
                {
                    VisitExpression(ret.Expression, referenceConversion, true);
                    continue;
                }
                var throwE = statement as CodeThrowExceptionStatement;
                if (throwE != null)
                {
                    VisitExpression(throwE.ToThrow, referenceConversion);
                    continue;
                }
            }
        }

        private void VisitExpression(CodeExpression expression, Func<CodeTypeReference, CodeTypeReference> referenceConversion, bool mayBeNull = false)
        {
            if (expression == null)
            {
                if (!mayBeNull)
                {

                }
                return;
            }
            var mce = expression as CodeMethodInvokeExpression;
            if (mce != null)
            {
                VisitExpression(mce.Method.TargetObject, referenceConversion);
                return;
            }
            var bin = expression as CodeBinaryOperatorExpression;
            if (bin != null)
            {
                VisitExpression(bin.Left, referenceConversion);
                VisitExpression(bin.Right, referenceConversion);
                return;
            }
            var tre = expression as CodeTypeReferenceExpression;
            if (tre != null)
            {
                tre.Type = referenceConversion(tre.Type);
                return;
            }
            var cast = expression as CodeCastExpression;
            if (cast != null)
            {
                cast.TargetType = referenceConversion(cast.TargetType);
                VisitExpression(cast.Expression, referenceConversion);
                return;
            }
            var typeExp = expression as CodeTypeReferenceExpression;
            if (typeExp != null)
            {
                typeExp.Type = referenceConversion(typeExp.Type);
                return;
            }
            var propertyRef = expression as CodePropertyReferenceExpression;
            if (propertyRef != null)
            {
                VisitExpression(propertyRef.TargetObject, referenceConversion);
                return;
            }
            var fieldRef = expression as CodeFieldReferenceExpression;
            if (fieldRef != null)
            {
                VisitExpression(fieldRef.TargetObject, referenceConversion, true);
                return;
            }
            var dce = expression as CodeDelegateCreateExpression;
            if (dce != null)
            {
                VisitExpression(dce.TargetObject, referenceConversion, true);
                dce.DelegateType = referenceConversion(dce.DelegateType);
                return;
            }
            var die = expression as CodeDelegateInvokeExpression;
            if (die != null)
            {
                VisitExpression(die.TargetObject, referenceConversion, true);
                for (int i = 0; i < die.Parameters.Count; i++)
                {
                    VisitExpression(die.Parameters[i], referenceConversion);
                }
                return;
            }
            var typeOf = expression as CodeTypeOfExpression;
            if (typeOf != null)
            {
                typeOf.Type = referenceConversion(typeOf.Type);
                return;
            }
            var objectCreate = expression as CodeObjectCreateExpression;
            if (objectCreate != null)
            {
                objectCreate.CreateType = referenceConversion(objectCreate.CreateType);
                for (int i = 0; i < objectCreate.Parameters.Count; i++)
                {
                    VisitExpression(objectCreate.Parameters[i] as CodeExpression, referenceConversion, true);
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

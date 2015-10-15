using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System.Diagnostics;
using System.Collections;

namespace NMF.CodeGen
{
    /// <summary>
    /// Represents a base class for a transformation rule that generates classes
    /// </summary>
    /// <typeparam name="T">The model element type from which the classes are generated</typeparam>
    public abstract class ClassGenerator<T> : TransformationRule<T, CodeTypeDeclaration>
        where T : class
    {
        /// <summary>
        /// Creates the output the transformation rule
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="context">The transformaton context</param>
        /// <returns>A new code type declaration with set reference that references back to the code declaration</returns>
        public override CodeTypeDeclaration CreateOutput(T input, ITransformationContext context)
        {
            return CodeDomHelper.CreateTypeDeclarationWithReference(GetName(input));
        }

        /// <summary>
        /// Create a separate public interface for the given input
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="generatedType">The generated type declaration</param>
        /// <returns>A new type declaration without members or null, if no interface shall be created.</returns>
        protected virtual CodeTypeDeclaration CreateSeparatePublicInterface(T input, CodeTypeDeclaration generatedType)
        {
            return null;
        }

        /// <summary>
        /// Modifies the given code declaration to respect that the generated class is the root of an inheritance tree 
        /// </summary>
        /// <param name="generatedType">The generated class declaration</param>
        /// <remarks>This method can be overridden so that the derived transformation rule can inherit common base functionality</remarks>
        protected virtual void AddImplementationBaseClass(CodeTypeDeclaration generatedType) { }

        /// <summary>
        /// Gets the name of the class generated for the given model element
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <returns>The name of the class</returns>
        protected abstract string GetName(T input);

        /// <summary>
        /// Initializes the generated type declaration
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="generatedType">The generated class declaration</param>
        /// <param name="context">The transformation context</param>
        /// <remarks>Can be overridden to refine code generation</remarks>
        public override void Transform(T input, CodeTypeDeclaration generatedType, ITransformationContext context)
        {
            HashSet<CodeTypeMember> shadows;
            var ownShadows = generatedType.Shadows(false);
            if (ownShadows != null)
            {
                shadows = new HashSet<CodeTypeMember>(ownShadows);
            }
            else
            {
                shadows = new HashSet<CodeTypeMember>();
            }
            var constructor = CodeDomHelper.GetOrCreateDefaultConstructor(generatedType, () => new CodeConstructor() { Attributes = MemberAttributes.Public });
            var dependends = generatedType.DependentMembers(false);
            if (dependends != null)
            {
                foreach (var member in dependends)
                {
                    RecursivelyAddDependentMembers(generatedType.Members, constructor.Statements, member, shadows);
                }
            }

            var interfaceDecl = CreateSeparatePublicInterface(input, generatedType);
            if (interfaceDecl != null)
            {
                CodeDomHelper.SetUserItem(generatedType, CodeDomHelper.InterfaceKey, interfaceDecl);
                var dependentTypes = CodeDomHelper.DependentTypes(generatedType, true);
                dependentTypes.Add(interfaceDecl);

                var typeReference = generatedType.GetReferenceForType();
                typeReference.BaseType = interfaceDecl.Name;

                CreateInterfaceMembers(generatedType, interfaceDecl);

                for (int i = generatedType.BaseTypes.Count - 1; i >= 0; i--)
                {
                    var baseTypeRef = generatedType.BaseTypes[i];
                    var baseType = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(baseTypeRef, CodeDomHelper.ClassKey);
                    if (baseType == null) continue;
                    interfaceDecl.BaseTypes.Add(baseTypeRef);
                    generatedType.BaseTypes.RemoveAt(i);
                }
                generatedType.BaseTypes.Add(typeReference);

                if (!EnsureBaseMembers(generatedType, generatedType, shadows, constructor))
                {
                    AddImplementationBaseClass(generatedType);
                }
            }

            if (constructor.Statements.Count > 0)
            {
                CodeDomHelper.SetUserItem(generatedType, CodeDomHelper.ConstructorKey, constructor);
                generatedType.Members.Add(constructor);
            }
        }

        protected virtual bool ShouldContainMembers(CodeTypeDeclaration generatedType, CodeTypeReference baseType)
        {
            return true;
        }

        private bool EnsureBaseMembers(CodeTypeDeclaration generatedType, CodeTypeDeclaration current, HashSet<CodeTypeMember> shadows, CodeConstructor constructor)
        {
            var addedBaseClass = false;
            var baseCollection = current.BaseTypes;
            var iFace = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(current, CodeDomHelper.InterfaceKey);
            if (iFace != null) baseCollection = iFace.BaseTypes;
            for (int i = 0; i < baseCollection.Count; i++)
            {
                var baseTypeRef = baseCollection[i];
                if (!ShouldContainMembers(generatedType, baseTypeRef)) continue;
                var baseType = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(baseTypeRef, CodeDomHelper.ClassKey);
                if (baseType == null || baseType == generatedType) continue;
                var noShadows = true;
                var dependent = baseType.DependentMembers(false);
                if (!addedBaseClass)
                {
                    var members = new List<CodeTypeMember>();
                    var constructorStatements = new List<CodeStatement>();
                    if (dependent != null)
                    {
                        foreach (var inheritedMember in dependent)
                        {
                            noShadows &= RecursivelyAddDependentMembers(members, constructorStatements, inheritedMember, shadows);
                        }
                    }
                    if (noShadows)
                    {
                        var implementationRef = new CodeTypeReference();
                        implementationRef.BaseType = baseType.Name;
                        var n = baseTypeRef.Namespace();
                        if (n != null && n.EndsWith(baseType.Name))
                        {
                            implementationRef.BaseType = n + "." + baseType.Name;
                        }
                        else
                        {
                            implementationRef.SetNamespace(n);
                        }
                        generatedType.BaseTypes.Insert(0, implementationRef);
                        shadows.AddRange(members);
                        addedBaseClass = true;
                        ShadowMembers(baseType, shadows);
                    }
                    else
                    {
                        foreach (var member in members)
                        {
                            if (!generatedType.Members.Contains(member))
                            {
                                generatedType.Members.Add(member);
                            }
                        }
                        foreach (var stmt in constructorStatements)
                        {
                            constructor.Statements.Add(stmt);
                        }
                        addedBaseClass = EnsureBaseMembers(generatedType, baseType, shadows, constructor);
                    }
                }
                else
                {
                    if (dependent != null)
                    {
                        foreach (var inheritedMember in dependent)
                        {
                            RecursivelyAddDependentMembers(generatedType.Members, constructor.Statements, inheritedMember, shadows);
                        }
                    }
                    AddBaseMembers(baseType, generatedType, shadows, constructor);
                }
            }
            return addedBaseClass;
        }

        private void ShadowMembers(CodeTypeDeclaration codeType, HashSet<CodeTypeMember> shadows)
        {
            var baseCollection = codeType.BaseTypes;
            var iFace = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(codeType, CodeDomHelper.InterfaceKey);
            if (iFace != null) baseCollection = iFace.BaseTypes;
            for (int i = 0; i < baseCollection.Count; i++)
            {
                var baseTypeRef = baseCollection[i];
                var baseType = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(baseTypeRef, CodeDomHelper.ClassKey);
                if (baseType == null || baseType == codeType) continue;
                var dependent = baseType.DependentMembers(false);
                if (dependent != null)
                {
                    shadows.AddRange(dependent);
                }
                ShadowMembers(baseType, shadows);
            }
        }

        private void AddBaseMembers(CodeTypeDeclaration type, CodeTypeDeclaration targetType, HashSet<CodeTypeMember> shadows, CodeConstructor constructor)
        {
            var baseCollection = type.BaseTypes;
            var iFace = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(type, CodeDomHelper.InterfaceKey);
            if (iFace != null) baseCollection = iFace.BaseTypes;
            for (int i = 0; i < baseCollection.Count; i++)
            {
                var baseTypeRef = baseCollection[i];
                if (!ShouldContainMembers(targetType, baseTypeRef)) continue;
                var baseType = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(baseTypeRef, CodeDomHelper.ClassKey);
                if (baseType == null || baseType == type) continue;
                var dependent = baseType.DependentMembers(false);
                if (dependent != null)
                {
                    foreach (var inheritedMember in dependent)
                    {
                        RecursivelyAddDependentMembers(targetType.Members, constructor.Statements, inheritedMember, shadows);
                    }
                }
                AddBaseMembers(baseType, targetType, shadows, constructor);
            }
        }

        private bool RecursivelyAddDependentMembers(IList members, IList constructorStatements, CodeTypeMember current, HashSet<CodeTypeMember> shadows)
        {
            var noShadows = true;
            if (!shadows.Contains(current))
            {
                if (!members.Contains(current))
                {
                    members.Add(current);
                    var conStmts = current.ImpliedConstructorStatements(false);
                    if (conStmts != null)
                    {
                        foreach (var stmt in conStmts)
                        {
                            constructorStatements.Add(stmt);
                        }
                    }
                    var dependent = CodeDomHelper.DependentMembers(current, false);
                    if (dependent != null)
                    {
                        foreach (var item in dependent)
                        {
                            noShadows &= RecursivelyAddDependentMembers(members, constructorStatements, item, shadows);
                        }
                    }
                    var shadowsOfCurrent = CodeDomHelper.Shadows(current, false);
                    if (shadowsOfCurrent != null)
                    {
                        shadows.AddRange(shadowsOfCurrent);
                    }
                }
            }
            else
            {
                noShadows = false;
            }
            return noShadows;
        }

        private static void CreateInterfaceMembers(CodeTypeDeclaration generatedType, CodeTypeDeclaration interfaceDecl)
        {
            foreach (CodeTypeMember item in generatedType.Members)
            {
                if ((item.Attributes & (MemberAttributes.Public | MemberAttributes.Abstract | MemberAttributes.Override)) == MemberAttributes.Public)
                {
                    var property = item as CodeMemberProperty;
                    if (property != null)
                    {
                        var newProperty = new CodeMemberProperty()
                        {
                            Attributes = MemberAttributes.Public | MemberAttributes.Final,
                            Name = property.Name,
                            Type = property.Type,
                            HasGet = property.HasGet,
                            HasSet = property.HasSet
                        };
                        newProperty.Comments.AddRange(property.Comments);
                        interfaceDecl.Members.Add(newProperty);
                        continue;
                    }
                    var method = item as CodeMemberMethod;
                    if (method != null)
                    {
                        var newMethod = new CodeMemberMethod()
                        {
                            Name = method.Name,
                            ReturnType = method.ReturnType,
                            Attributes = MemberAttributes.Final
                        };
                        newMethod.Parameters.AddRange(method.Parameters);
                        newMethod.Comments.AddRange(method.Comments);
                        interfaceDecl.Members.Add(newMethod);
                        continue;
                    }
                    var ev = item as CodeMemberEvent;
                    if (ev != null)
                    {
                        var newEvent = new CodeMemberEvent()
                        {
                            Name = ev.Name,
                            Type = ev.Type,
                            Attributes = MemberAttributes.Final
                        };
                        newEvent.Comments.AddRange(ev.Comments);
                        interfaceDecl.Members.Add(newEvent);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a dependency to generate a property from a given subsequent model element
        /// </summary>
        /// <typeparam name="TProp">The model element type from which to generate a property</typeparam>
        /// <param name="rule">The property generator</param>
        /// <param name="selector">The selector function that selects the model element from which to generate the property</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireGenerateProperty<TProp>(TransformationRuleBase<TProp, CodeMemberProperty> rule, Func<T, TProp> selector)
            where TProp : class
        {
            return Require(rule, selector, persistor: (cl, prop) => cl.DependentMembers(true).Add(prop));
        }

        /// <summary>
        /// Creates a dependency to generate properties from subsequent model elements
        /// </summary>
        /// <typeparam name="TProp">The model element type from which to generate a property</typeparam>
        /// <param name="rule">The transformation rule for the property generation rule</param>
        /// <param name="selector">The selector function to select the subsequent model elements</param>
        /// <returns>The created transformation rule dependency</returns>
        public ITransformationRuleDependency RequireGenerateProperties<TProp>(TransformationRuleBase<TProp, CodeMemberProperty> rule, Func<T, IEnumerable<TProp>> selector)
            where TProp : class
        {
            return RequireMany(rule, selector, persistor: (cl, props) => cl.DependentMembers(true).AddRange(props));
        }

        /// <summary>
        /// Cregates a dependency to generate a method from a given subsequent model element
        /// </summary>
        /// <typeparam name="TMeth">The model element type from which to generate a method</typeparam>
        /// <param name="rule">The method generator</param>
        /// <param name="selector">The selector function that selects the model element from which to generate the method</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireGenerateMethod<TMeth>(TransformationRuleBase<TMeth, CodeMemberMethod> rule, Func<T, TMeth> selector)
            where TMeth : class
        {
            return Require(rule, selector, (cl, meth) => cl.DependentMembers(true).Add(meth));
        }

        /// <summary>
        /// Creates a dependency to generate methods from subsequent model elements
        /// </summary>
        /// <typeparam name="TMeth">The model elements type from which to generate methods</typeparam>
        /// <param name="rule">The method generator</param>
        /// <param name="selector">The selector function that selects the model elements from which to generate the methods</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireGenerateMethods<TMeth>(TransformationRuleBase<TMeth, CodeMemberMethod> rule, Func<T, IEnumerable<TMeth>> selector)
            where TMeth : class
        {
            return RequireMany(rule, selector, (cl, methods) => cl.DependentMembers(true).AddRange(methods));
        }

        /// <summary>
        /// Creates a dependency to generate an event from a given subsequent model element
        /// </summary>
        /// <typeparam name="TEvent">The model element type from which to generate an event</typeparam>
        /// <param name="rule">The transformation rule that is used to generate the event</param>
        /// <param name="selector">The selector function that selects the model element from which to generate an event</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireEvent<TEvent>(TransformationRuleBase<TEvent, CodeMemberEvent> rule, Func<T, TEvent> selector)
            where TEvent : class
        {
            return Require(rule, selector, (cl, ev) => cl.DependentMembers(true).Add(ev));
        }

        /// <summary>
        /// Creates a dependency to generate events from given subsequent model elements
        /// </summary>
        /// <typeparam name="TEvent">The model element type from which to generate events</typeparam>
        /// <param name="rule">The transformation rule to generate events</param>
        /// <param name="selector">The selector function that selects the model elements from which to generate events</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireEvents<TEvent>(TransformationRuleBase<TEvent, CodeMemberEvent> rule, Func<T, IEnumerable<TEvent>> selector)
            where TEvent : class
        {
            return RequireMany(rule, selector, (cl, events) => cl.DependentMembers(true).AddRange(events));
        }

        /// <summary>
        /// Creates a dependency to generate the code for the base classes of the current class
        /// </summary>
        /// <typeparam name="TClass">The model element type from which to generate the base class</typeparam>
        /// <param name="rule">The transformation rule that is used to generate the class</param>
        /// <param name="selector">A function used to select the model element from which to generate a base class</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireBaseClass<TClass>(TransformationRuleBase<TClass, CodeTypeDeclaration> rule, Func<T, TClass> selector)
            where TClass : class
        {
            return Require(rule, selector, (cl, baseClass) =>
            {
                var typeRef = CodeDomHelper.GetOrCreateUserItem<CodeTypeReference>(baseClass, CodeDomHelper.TypeReferenceKey, () => new CodeTypeReference());
                cl.BaseTypes.Add(typeRef);
            });
        }

        /// <summary>
        /// Creates a dependency to generate the code for the base classes of the current class
        /// </summary>
        /// <typeparam name="TClass">The model element type from which to generate the base classes</typeparam>
        /// <param name="rule">The transformation rule that is used to generate the base classes</param>
        /// <param name="selector">The function used to select the model elements from which to generate the base classes</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireBaseClasses<TClass>(TransformationRuleBase<TClass, CodeTypeDeclaration> rule, Func<T, IEnumerable<TClass>> selector)
            where TClass : class
        {
            return RequireMany(rule, selector, (cl, baseClasses) =>
            {
                foreach (var baseClass in baseClasses)
                {
                    var typeRef = CodeDomHelper.GetOrCreateUserItem<CodeTypeReference>(baseClass, CodeDomHelper.TypeReferenceKey, () => new CodeTypeReference());
                    cl.BaseTypes.Add(typeRef);
                }
            });
        }

        /// <summary>
        /// Creates a dependency to generate the code for the base classes of the current class
        /// </summary>
        /// <typeparam name="TClass">The model element type from which to generate the base class</typeparam>
        /// <param name="selector">A function used to select the model element from which to generate a base class</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireBaseClass(Func<T, T> selector)
        {
            return RequireBaseClass(this, selector);
        }

        /// <summary>
        /// Creates a dependency to generate the code for the base classes of the current class
        /// </summary>
        /// <typeparam name="TClass">The model element type from which to generate the base classes</typeparam>
        /// <param name="selector">The function used to select the model elements from which to generate the base classes</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireBaseClasses(Func<T, IEnumerable<T>> selector)
        {
            return RequireBaseClasses(this, selector);
        }
    }
}

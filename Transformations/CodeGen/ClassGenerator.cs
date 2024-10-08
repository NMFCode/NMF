﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System.Collections;
using NMF.Analyses;

#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations

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
            return CodeDomHelper.CreateTypeDeclarationWithReference(GetName(input), true);
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
            constructor.WriteDocumentation("Creates a new instance");
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

                CreateInterfaceMembers(input, generatedType, interfaceDecl, context);

                for (int i = generatedType.BaseTypes.Count - 1; i >= 0; i--)
                {
                    var baseTypeRef = generatedType.BaseTypes[i];
                    var baseType = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(baseTypeRef, CodeDomHelper.ClassKey);
                    if (baseType == null) continue;
                    interfaceDecl.BaseTypes.Add(baseTypeRef);
                    generatedType.BaseTypes.RemoveAt(i);
                }
                generatedType.BaseTypes.Add(typeReference);

                ResolveMultipleInheritanceMembers(generatedType, shadows, constructor);
            }
            else
            {
                CodeDomHelper.SetUserItem(generatedType, CodeDomHelper.BaseClassesKey, generatedType.Closure(GetBaseClasses).Except(generatedType));
            }

            if (constructor.Statements.Count > 0)
            {
                CodeDomHelper.SetUserItem(generatedType, CodeDomHelper.ConstructorKey, constructor);
                generatedType.Members.Add(constructor);
            }
        }

        /// <summary>
        /// Determines whether the provided type should contain members of the given base type
        /// </summary>
        /// <param name="generatedType">The type that is currently being generated</param>
        /// <param name="baseType">The base types</param>
        /// <returns>True if so, otherwise false</returns>
        protected virtual bool ShouldContainMembers(CodeTypeDeclaration generatedType, CodeTypeDeclaration baseType)
        {
            var baseClasses = CodeDomHelper.GetOrCreateUserItem<IEnumerable<CodeTypeDeclaration>>(generatedType, CodeDomHelper.BaseClassesKey);
            if (baseClasses == null)
            {
                return true;
            }
            else
            {
                return !baseClasses.Contains(baseType);
            }
        }

        private static IEnumerable<CodeTypeDeclaration> GetBaseClasses(CodeTypeDeclaration type)
        {
            var interfaceType = CodeDomHelper.GetOrCreateUserItem<CodeTypeDeclaration>(type, CodeDomHelper.InterfaceKey);
            if (interfaceType == null)
            {
                interfaceType = type;
            }
            return interfaceType.BaseTypes.Cast<CodeTypeReference>().Select(r => r.GetTypeForReference()).Where(c => c != null);
        }

        private static IEnumerable<CodeTypeMember> AllFeatures(CodeTypeDeclaration typeDeclaration)
        {
            return typeDeclaration.Closure(GetBaseClasses).SelectMany(AllDeclaredFeatures);
        }

        private static IEnumerable<CodeTypeMember> Refinements(CodeTypeDeclaration typeDeclaration)
        {
            return AllDeclaredFeatures(typeDeclaration).SelectMany(t =>
            {
                var shadows = t.Shadows(false);
                if (shadows == null)
                {
                    return Enumerable.Empty<CodeTypeMember>();
                }
                else
                {
                    return shadows;
                }
            });
        }

        private static IEnumerable<CodeTypeMember> AllDeclaredFeatures(CodeTypeDeclaration typeDeclaration)
        {
            return typeDeclaration.Closure<CodeTypeMember>(t =>
            {
                var dependents = t.DependentMembers(false);
                if (dependents == null)
                {
                    return Enumerable.Empty<CodeTypeMember>();
                }
                else
                {
                    return dependents;
                }
            });
        }

        private static IEnumerable<CodeTypeDeclaration> Edges(CodeTypeDeclaration typeDeclaration, IEnumerable<CodeTypeDeclaration> candidates)
        {
            var ancestors = typeDeclaration.Closure(GetBaseClasses);
            var refinements = Refinements(typeDeclaration);
            var conflicting = from cand in candidates
                              where !cand.Closure(GetBaseClasses).Contains(typeDeclaration) &&
                                    refinements.IntersectsWith(AllFeatures(cand))
                              select cand;

            return ancestors.Union(conflicting);
        }

        /// <summary>
        /// Resolves the members inherited from multiple base classes
        /// </summary>
        /// <param name="generatedType">The generated type</param>
        /// <param name="shadows">The members that are shadowed by others</param>
        /// <param name="constructor">The constructor to which initializations should be added</param>
        protected virtual void ResolveMultipleInheritanceMembers(CodeTypeDeclaration generatedType, HashSet<CodeTypeMember> shadows, CodeConstructor constructor)
        {
            var allClasses = generatedType.Closure(GetBaseClasses);
            var layering = Layering<CodeTypeDeclaration>.CreateLayers(generatedType, c => Edges(c, allClasses));
            CodeTypeDeclaration implBaseType = FindSuitableImplementationBaseType(generatedType, shadows, layering);
            IEnumerable<CodeTypeDeclaration> inheritedBaseClasses;
            if (implBaseType != null)
            {
                inheritedBaseClasses = AddBaseClasses(generatedType, implBaseType);
            }
            else
            {
                inheritedBaseClasses = Enumerable.Empty<CodeTypeDeclaration>();
                AddImplementationBaseClass(generatedType);
            }
            CodeDomHelper.SetUserItem(generatedType, CodeDomHelper.BaseClassesKey, inheritedBaseClasses);
            for (int i = layering.Count - 1; i >= 0; i--)
            {
                foreach (var baseType in layering[i])
                {
                    AddMembersOfBaseClass(generatedType, shadows, constructor, baseType);
                }

            }
        }

        private void AddMembersOfBaseClass(CodeTypeDeclaration generatedType, HashSet<CodeTypeMember> shadows, CodeConstructor constructor, CodeTypeDeclaration baseType)
        {
            if (baseType != generatedType && ShouldContainMembers(generatedType, baseType))
            {
                var dependent = baseType.DependentMembers(false);
                if (dependent != null)
                {
                    foreach (var inheritedMember in dependent)
                    {
                        RecursivelyAddDependentMembers(generatedType.Members, constructor.Statements, inheritedMember, shadows);
                    }
                }
                var shadowsOfBase = baseType.Shadows(false);
                if (shadowsOfBase != null)
                {
                    shadows.UnionWith(shadowsOfBase);
                }
            }
        }

        private static IEnumerable<CodeTypeDeclaration> AddBaseClasses(CodeTypeDeclaration generatedType, CodeTypeDeclaration implBaseType)
        {
            IEnumerable<CodeTypeDeclaration> inheritedBaseClasses = implBaseType.Closure(GetBaseClasses);
            var implementationRef = new CodeTypeReference();
            implementationRef.BaseType = implBaseType.Name;
            var n = implBaseType.GetReferenceForType().Namespace();
            if (n != null && n.EndsWith(implBaseType.Name))
            {
                implementationRef.BaseType = n + "." + implBaseType.Name;
            }
            else
            {
                implementationRef.SetNamespace(n);
            }
            generatedType.BaseTypes.Insert(0, implementationRef);
            return inheritedBaseClasses;
        }

        private static CodeTypeDeclaration FindSuitableImplementationBaseType(CodeTypeDeclaration generatedType, HashSet<CodeTypeMember> shadows, IList<ICollection<CodeTypeDeclaration>> layering)
        {
            CodeTypeDeclaration implBaseType = null;
            int layerIndex;
            for (layerIndex = layering.Count - 1; layerIndex >= 0; layerIndex--)
            {
                var layer = layering[layerIndex];
                if (layer.Count == 1 && layer.First() != generatedType && !shadows.IntersectsWith(AllFeatures(layer.First())))
                {
                    implBaseType = layer.First();
                    break;
                }
                foreach (var cl in layer)
                {
                    shadows.UnionWith(Refinements(cl));
                }
            }

            return implBaseType;
        }

        private void RecursivelyAddDependentMembers(IList members, IList constructorStatements, CodeTypeMember current, HashSet<CodeTypeMember> shadows)
        {
            if (!shadows.Add(current))
            {
                return;
            }
            var conflict = members.Cast<CodeTypeMember>().FirstOrDefault(member => AreConflicting(member, current));
            if (conflict == null)
            {
                members.Add(current);
            }
            else
            {
                ResolveConflict(members, current, conflict);
            }
            AddImpliedConstructorStatements(constructorStatements, current);
            AddDependentMembers(members, constructorStatements, current, shadows);
            AddShadows(current, shadows);
        }

        private static void AddShadows(CodeTypeMember current, HashSet<CodeTypeMember> shadows)
        {
            var shadowing = CodeDomHelper.Shadows(current, false);
            if (shadowing != null)
            {
                shadows.AddRange(shadowing);
            }
        }

        private void AddDependentMembers(IList members, IList constructorStatements, CodeTypeMember current, HashSet<CodeTypeMember> shadows)
        {
            var dependent = CodeDomHelper.DependentMembers(current, false);
            if (dependent != null)
            {
                foreach (var item in dependent)
                {
                    RecursivelyAddDependentMembers(members, constructorStatements, item, shadows);
                }
            }
        }

        private static void AddImpliedConstructorStatements(IList constructorStatements, CodeTypeMember current)
        {
            var conStmts = current.ImpliedConstructorStatements(false);
            if (conStmts != null)
            {
                foreach (var stmt in conStmts)
                {
                    constructorStatements.Add(stmt);
                }
            }
        }

        private static void ResolveConflict(IList members, CodeTypeMember current, CodeTypeMember conflict)
        {
            members.Remove(conflict);
            try
            {
                members.Add(conflict.Merge(current));
            }
            catch (Exception e)
            {
                members.Add(conflict);
                current.Name += "_";
                members.Add(current);
                Console.Error.WriteLine(string.Format("Exception joining two members {0} and {1}: {2}", current.Name, conflict.Name, e.Message), e);
            }
        }

        private bool AreConflicting(CodeTypeMember member, CodeTypeMember current)
        {
            if (member.Name != current.Name) return false;

            if (member.GetType() != current.GetType()) return false;

            if (member is CodeSnippetTypeMember || current is CodeSnippetTypeMember) return false;

            if (member is CodeMemberMethod memberMethod && current is CodeMemberMethod currentMethod)
            {
                if (memberMethod.Parameters.Count != currentMethod.Parameters.Count) return false;
                if (memberMethod.TypeParameters.Count != currentMethod.TypeParameters.Count) return false;

                for (int i = 0; i < memberMethod.Parameters.Count; i++)
                {
                    if (memberMethod.Parameters[i].Type.BaseType != currentMethod.Parameters[i].Type.BaseType) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Generates the interface members for the given type
        /// </summary>
        /// <param name="input">The input for which the interface members need to be generated</param>
        /// <param name="generatedType">The generated type</param>
        /// <param name="interfaceDecl">The interface declaration</param>
        /// <param name="context">The transformation context</param>
        protected virtual void CreateInterfaceMembers(T input, CodeTypeDeclaration generatedType, CodeTypeDeclaration interfaceDecl, ITransformationContext context)
        {
            foreach (CodeTypeMember item in generatedType.Members)
            {
                if ((item.Attributes & (MemberAttributes.Public | MemberAttributes.Override)) != MemberAttributes.Public)
                {
                    continue;
                }

                if (item is CodeMemberProperty property)
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
                    newProperty.CustomAttributes.AddRange(property.CustomAttributes);
                    interfaceDecl.Members.Add(newProperty);
                }
                else if (item is CodeMemberMethod method)
                {
                    var newMethod = new CodeMemberMethod()
                    {
                        Name = method.Name,
                        ReturnType = method.ReturnType,
                        Attributes = MemberAttributes.Final
                    };
                    newMethod.Parameters.AddRange(method.Parameters);
                    newMethod.Comments.AddRange(method.Comments);
                    newMethod.CustomAttributes.AddRange(method.CustomAttributes);
                    interfaceDecl.Members.Add(newMethod);
                }
                else if (item is CodeMemberEvent ev)
                {
                    var newEvent = new CodeMemberEvent()
                    {
                        Name = ev.Name,
                        Type = ev.Type,
                        Attributes = MemberAttributes.Final
                    };
                    newEvent.Comments.AddRange(ev.Comments);
                    interfaceDecl.Members.Add(newEvent);
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
            return Require(rule, selector, (cl, meth) => { if (meth != null) cl.DependentMembers(true).Add(meth); });
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
            return RequireMany(rule, selector, (cl, methods) =>
            {
                cl.DependentMembers(true).AddRange(methods.Where(m => m != null));
            });
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
        /// <param name="selector">The function used to select the model elements from which to generate the base classes</param>
        /// <returns>The transformation rule dependency</returns>
        public ITransformationRuleDependency RequireBaseClasses(Func<T, IEnumerable<T>> selector)
        {
            return RequireBaseClasses(this, selector);
        }
    }
}

#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations
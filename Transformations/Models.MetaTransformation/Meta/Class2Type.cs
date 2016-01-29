using NMF.CodeGen;
using NMF.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// Represents the transformation rule to transform classes
        /// </summary>
        public class Class2Type : ClassGenerator<IClass>
        {
            /// <summary>
            /// Gets the name of the generated class for the given NMeta class
            /// </summary>
            /// <param name="input">The NMeta class</param>
            /// <returns>The name of the generated class</returns>
            protected override string GetName(IClass input)
            {
                return input.Name.ToPascalCase();
            }

            public override CodeTypeDeclaration CreateOutput(IClass input, ITransformationContext context)
            {
                var generatedType = base.CreateOutput(input, context);
                var mapping = input.GetExtension<MappedType>();
                if (mapping != null)
                {
                    var reference = generatedType.GetReferenceForType();
                    reference.BaseType = mapping.SystemType.Name;
                    reference.SetNamespace(mapping.SystemType.Namespace);
                }
                return generatedType;
            }

            protected override bool ShouldContainMembers(CodeTypeDeclaration generatedType, CodeTypeReference baseType)
            {
                return baseType.BaseType != "IModelElement";
            }

            /// <summary>
            /// Registers the dependencies: Marks the rule instantiating for Type2Type and requires to generate members and base classes
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());

                RequireBaseClasses(cl => cl.BaseTypes);

                var t = Transformation as Meta2ClassesTransformation;
                if (t != null && t.CreateOperations)
                {
                    RequireGenerateMethods(Rule<Operation2Method>(), cl => cl.Operations);
                }

                RequireGenerateProperties(Rule<Attribute2Property>(), cl => cl.Attributes);
                RequireGenerateProperties(Rule<Reference2Property>(), cl => cl.References);

                RequireMany(Rule<RefinedReferenceGenerator>(), cl => cl.ReferenceConstraints.Select(c => Tuple.Create(cl, c.Constrains)));
                RequireMany(Rule<RefinedAttributeGenerator>(), cl => cl.AttributeConstraints.Select(c => Tuple.Create(cl, c.Constrains)));

                Call(Rule<Class2Children>(), (decl, childDecl) => { if (childDecl != null) decl.Members.Add(childDecl); });
                Call(Rule<Class2Referenced>(), (decl, refDecl) => { if (refDecl != null) decl.Members.Add(refDecl); });
            }

            /// <summary>
            /// Initializes the generated class
            /// </summary>
            /// <param name="input">The input NMeta class</param>
            /// <param name="generatedType">The generated type</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                if (input.IsAbstract) generatedType.TypeAttributes = System.Reflection.TypeAttributes.Abstract | System.Reflection.TypeAttributes.Public;

                if (input.Namespace.Uri != null)
                {
                    generatedType.AddAttribute(typeof(XmlNamespaceAttribute), input.Namespace.Uri.ConvertToString());
                }
                if (input.Namespace.Prefix != null)
                {
                    generatedType.AddAttribute(typeof(XmlNamespacePrefixAttribute), input.Namespace.Prefix);
                }
                var uri = input.AbsoluteUri;
                if (uri != null && uri.IsAbsoluteUri)
                {
                    generatedType.AddAttribute(typeof(ModelRepresentationClassAttribute), uri.AbsoluteUri);
                }

                generatedType.WriteDocumentation(input.Summary ?? string.Format("The representation of the {0} class", input.Name), input.Remarks);

                base.Transform(input, generatedType, context);

                AdjustTypeReference(input, generatedType, context);

                generatedType.BaseTypes.Add(typeof(IModelElement).Name);

                var members = generatedType.Members;

                // Create Children
                var childrenProperty = CreateChildren(input, generatedType, context);
                if (childrenProperty != null) members.Add(childrenProperty);

                // Create GetRelativeUriForChild
                var getRelativeUriForChild = CreateGetRelativePathForNonIdentifiedChild(input, generatedType, context);
                if (getRelativeUriForChild != null) members.Add(getRelativeUriForChild);

                // Create GetModelElementForUri
                var getModelElementForUri = CreateGetModelElementForReference(input, generatedType, context);
                if (getModelElementForUri != null) members.Add(getModelElementForUri);

                // Create References Property
                var referencesProperty = CreateReferencesProperty(input, generatedType, context);
                if (referencesProperty != null) members.Add(referencesProperty);

                // Create GetClass
                var getClass = CreateGetClass(input);
                if (getClass != null) members.Add(getClass);

                ImplementIdentifier(input, generatedType);

                var inputModel = input.Model;
                if (!input.BaseTypes.Any(c => c.Model == inputModel))
                {
                    lock (context.Data)
                    {
                        context.GetRootClasses(true).Add(input);
                    }
                }
            }

            private void AdjustTypeReference(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var reference = generatedType.GetReferenceForType();
                var ns2ns = Rule<Namespace2Namespace>();
                if (ns2ns != null)
                {
                    var baseType = reference.BaseType;
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var ns in ns2ns.DefaultImports)
                    {
                        var typeName = ns + "." + baseType;
                        if (System.Type.GetType(typeName, false) != null)
                        {
                            reference.BaseType = context.Trace.ResolveIn(ns2ns, input.Namespace).Name + "." + baseType;
                        }
                        foreach (var ass in assemblies)
                        {
                            if (ass.GetType(typeName, false) != null)
                            {
                                reference.BaseType = context.Trace.ResolveIn(ns2ns, input.Namespace).Name + "." + baseType;
                                return;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Creates the GetClass method that obtains the class structure of the generated type
            /// </summary>
            /// <param name="input">The NMeta class</param>
            /// <returns>A code method definition for the GetClass method</returns>
            protected virtual CodeMemberMethod CreateGetClass(IClass input)
            {
                var getClass = new CodeMemberMethod()
                {
                    Name = "GetClass",
                    ReturnType = new CodeTypeReference(typeof(IClass)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                var absoluteUri = input.AbsoluteUri;
                if (absoluteUri != null && absoluteUri.IsAbsoluteUri)
                {
                    var repositoryRef = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(MetaRepository)), "Instance");
                    var classRef = new CodeMethodInvokeExpression(repositoryRef, "ResolveClass", new CodePrimitiveExpression(absoluteUri.AbsoluteUri));
                    getClass.Statements.Add(new CodeMethodReturnStatement(classRef));
                }
                else
                {
                    getClass.ThrowException<NotSupportedException>();
                }
                getClass.WriteDocumentation("Gets the Class element that describes the structure of the current model element");
                return getClass;
            }

            /// <summary>
            /// Creates the public interface for the given NMeta class
            /// </summary>
            /// <param name="input">The NMeta class</param>
            /// <param name="generatedType">The generated type</param>
            /// <returns>A new type declaration that has attributes set same like the original type</returns>
            protected override CodeTypeDeclaration CreateSeparatePublicInterface(IClass input, CodeTypeDeclaration generatedType)
            {
                var t = Transformation as Meta2ClassesTransformation;
                if (t != null && !t.SeparateImplementations) return null;
                var iface = new CodeTypeDeclaration()
                {
                    Name = "I" + generatedType.Name,
                    Attributes = MemberAttributes.Public,
                    IsInterface = true
                };
                iface.BaseTypes.Add(typeof(IModelElement).Name);
                iface.AddAttribute(typeof(DefaultImplementationTypeAttribute), new CodeTypeOfExpression(generatedType.Name));
                iface.AddAttribute(typeof(XmlDefaultImplementationTypeAttribute), new CodeTypeOfExpression(generatedType.Name));
                iface.WriteDocumentation("The public interface for " + input.Name);
                return iface;
            }

            /// <summary>
            /// Adds the implementation base class for the framework, i.e. ModelElement
            /// </summary>
            /// <param name="generatedType">The generated type</param>
            protected override void AddImplementationBaseClass(CodeTypeDeclaration generatedType)
            {
                generatedType.BaseTypes.Insert(0, new CodeTypeReference(typeof(ModelElement).Name));
            }

            /// <summary>
            /// Creates the property for the referenced elements
            /// </summary>
            /// <param name="input">The input NMeta class</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The code property that implements the references property</returns>
            protected virtual CodeMemberProperty CreateReferencesProperty(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var referencedClass = context.Trace.ResolveIn(Rule<Class2Referenced>(), input);
                if (referencedClass == null) return null;

                var thisRef = new CodeThisReferenceExpression();
                var referencedProperty = new CodeMemberProperty()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Type = new CodeTypeReference(typeof(IEnumerableExpression<>).Name, new CodeTypeReference(typeof(IModelElement).Name)),
                    HasGet = true,
                    HasSet = false,
                    Name = "ReferencedElements"
                };
                referencedProperty.WriteDocumentation("Gets the referenced model elements of this model element");
                var referenced = new CodeObjectCreateExpression(referencedClass.GetReferenceForType(), thisRef);
                var baseReferenced = new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), "ReferencedElements");
                referencedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(baseReferenced, "Concat", referenced)));
                return referencedProperty;
            }

            private static CodeExpression AddReference(CodeExpression expression, IReference reference)
            {
                return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(expression, "Concat", new CodeTypeReference(typeof(IModelElement).Name)),
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), reference.Name.ToPascalCase()));
            }

            /// <summary>
            /// Implements the members necessary for the identifier mechanism of the given NMeta class
            /// </summary>
            /// <param name="class">The NMeta class</param>
            /// <param name="generatedType">The generated type for the NMeta class</param>
            protected virtual void ImplementIdentifier(IClass @class, CodeTypeDeclaration generatedType)
            {
                if (@class.Identifier != null)
                {
                    var isIdentifiedProperty = new CodeMemberProperty();
                    isIdentifiedProperty.Name = "IsIdentified";
                    isIdentifiedProperty.Type = new CodeTypeReference(typeof(bool));
                    isIdentifiedProperty.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                    isIdentifiedProperty.HasGet = true;
                    isIdentifiedProperty.HasSet = false;
                    isIdentifiedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                    isIdentifiedProperty.WriteDocumentation("Gets a value indicating whether the current model element can be identified by an attribute value");
                    generatedType.Members.Add(isIdentifiedProperty);

                    var toIdentifierString = new CodeMemberMethod()
                    {
                        Name = "ToIdentifierString",
                        Attributes = MemberAttributes.Public | MemberAttributes.Override,
                        ReturnType = new CodeTypeReference(typeof(string))
                    };
                    toIdentifierString.WriteDocumentation("Gets the identifier string for this model element", "The identifier string");
                    var identifiedObject = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), @class.Identifier.Name.ToPascalCase());
                    var toString = new CodeMethodInvokeExpression(identifiedObject, "ToString");
                    var t = Transformation as Meta2ClassesTransformation;
                    if (!IsString(@class.Identifier.Type) && t != null && !t.IsValueType(@class.Identifier.Type))
                    {
                        var nullRef = new CodePrimitiveExpression(null);
                        toIdentifierString.Statements.Add(new CodeConditionStatement(
                            new CodeBinaryOperatorExpression(identifiedObject, CodeBinaryOperatorType.IdentityEquality, nullRef),
                            new CodeMethodReturnStatement(nullRef)));
                    }
                    toIdentifierString.Statements.Add(new CodeMethodReturnStatement(toString));
                    generatedType.Members.Add(toIdentifierString);
                }

                var identifier = @class.RetrieveIdentifier();
                if (identifier == null || generatedType.IsInterface) return;
                generatedType.AddAttribute(typeof(DebuggerDisplayAttribute), string.Format("{0} {{{1}}}", generatedType.Name, identifier.Name.ToPascalCase()));
            }

            /// <summary>
            /// Generates the method to resolve referenced model elements by reference name and index
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The GetModelElementsForReference method</returns>
            protected virtual CodeMemberMethod CreateGetModelElementForReference(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getModelElementForUri = new CodeMemberMethod()
                {
                    Name = "GetModelElementForReference",
                    ReturnType = new CodeTypeReference(typeof(IModelElement).Name),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                getModelElementForUri.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "reference"));
                getModelElementForUri.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
                AddReferencesOfClass(input, generatedType, AddToGetModelElementForUri, getModelElementForUri, true, context);
                getModelElementForUri.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetModelElementForReference", new CodeArgumentReferenceExpression("reference"), new CodeArgumentReferenceExpression("index"))));
                if (getModelElementForUri.Statements.Count == 1) return null;
                getModelElementForUri.WriteDocumentation("Resolves the given URI to a child model element", "The model element or null if it could not be found", new Dictionary<string, string>() 
                {
                    {"reference", "The requested reference name"},
                    {"index", "The index of this reference"}
                });
                return getModelElementForUri;
            }

            /// <summary>
            /// Generates the method to create a relative path for a given child model element
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The GetRelativePathForNonIdentifiedChild method</returns>
            protected virtual CodeMemberMethod CreateGetRelativePathForNonIdentifiedChild(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getRelativeUriForChild = new CodeMemberMethod()
                {
                    Name = "GetRelativePathForNonIdentifiedChild",
                    ReturnType = new CodeTypeReference(typeof(string)),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                getRelativeUriForChild.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IModelElement).Name, "element"));
                getRelativeUriForChild.WriteDocumentation("Gets the relative URI fragment for the given child model element", "A fragment of the relative URI", new Dictionary<string, string>() 
                    {
                        {
                            "element", "The element that should be looked for"
                        }
                    });
                AddReferencesOfClass(input, generatedType, AddToGetRelativeUriForChild, getRelativeUriForChild, true, context);
                if (getRelativeUriForChild.Statements.Count == 0) return null;
                getRelativeUriForChild.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetRelativePathForNonIdentifiedChild", new CodeArgumentReferenceExpression("element"))));
                return getRelativeUriForChild;
            }

            /// <summary>
            /// Generates the Children property for the given class
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the Children property</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The Children property for the given NMeta class</returns>
            protected virtual CodeMemberProperty CreateChildren(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var childrenClass = context.Trace.ResolveIn(Rule<Class2Children>(), input);
                if (childrenClass == null) return null;

                var thisRef = new CodeThisReferenceExpression();
                var childrenProperty = new CodeMemberProperty()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Type = new CodeTypeReference(typeof(IEnumerableExpression<>).Name, new CodeTypeReference(typeof(IModelElement).Name)),
                    HasGet = true,
                    HasSet = false,
                    Name = "Children"
                };
                childrenProperty.WriteDocumentation("Gets the child model elements of this model element");
                var children = new CodeObjectCreateExpression(childrenClass.GetReferenceForType(), thisRef);
                var baseChildren = new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), "Children");
                childrenProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(baseChildren, "Concat", children)));
                return childrenProperty;
            }

            private T AddReferencesOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IReference, T> action, T initial, bool containmentsOnly, ITransformationContext context)
            {
                var t = Transformation as Meta2ClassesTransformation;
                return AddReferencesOfClass<T>(input, typeDeclaration, action, initial, true, new HashSet<IClass>(), containmentsOnly, context, Rule<Reference2Property>());
            }

            private static T AddReferencesOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IReference, T> action, T initial, bool inherit, HashSet<IClass> visited, bool containmentsOnly, ITransformationContext context, Reference2Property r2p)
            {
                if (visited.Add(input))
                {
                    IEnumerable<IReference> references = input.References;
                    if (containmentsOnly) references = references.Where(r => r.IsContainment);
                    foreach (var r in references)
                    {
                        var property = context.Trace.ResolveIn(r2p, r);
                        if (typeDeclaration.Members.Contains(property))
                        {
                            initial = action(initial, r);
                        }
                    }
                    if (inherit)
                    {
                        foreach (var baseClass in input.BaseTypes.Where(type => !type.IsInterface))
                        {
                            initial = AddReferencesOfClass<T>(baseClass, typeDeclaration, action, initial, inherit, visited, containmentsOnly, context, r2p);
                        }
                    }
                }
                return initial;
            }

            private static CodeMemberMethod AddToGetRelativeUriForChild(CodeMemberMethod method, IReference containment)
            {
                if (containment.UpperBound == 1)
                {
                    var ifIdentical = new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("element"),
                            CodeBinaryOperatorType.IdentityEquality,
                            new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), containment.Name.ToPascalCase())));

                    ifIdentical.TrueStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).Name), "CreatePath",
                        new CodePrimitiveExpression(containment.Name.ToPascalCase()))));

                    method.Statements.Add(ifIdentical);
                }
                else if (containment.IsOrdered)
                {
                    var idxVarName = containment.Name.ToCamelCase() + "Index";
                    var idxVar = new CodeVariableDeclarationStatement(typeof(int), idxVarName, new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).Name), "IndexOfReference",
                        new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), containment.Name.ToPascalCase()), new CodeArgumentReferenceExpression("element")));
                    var idxRef = new CodeVariableReferenceExpression(idxVarName);

                    var ifContained = new CodeConditionStatement(new CodeBinaryOperatorExpression(idxRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(-1)));
                    ifContained.TrueStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).Name), "CreatePath",
                        new CodePrimitiveExpression(containment.Name),
                        idxRef)));

                    method.Statements.Add(idxVar);
                    method.Statements.Add(ifContained);
                }
                return method;
            }

            private static CodeMemberMethod AddToGetModelElementForUri(CodeMemberMethod method, IReference containment)
            {
                if (containment.UpperBound == 1 || containment.IsOrdered)
                {
                    var propRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), containment.Name.ToPascalCase());
                    var ifStmt = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("reference"),
                        CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(containment.Name.ToUpperInvariant())));
                    if (containment.UpperBound == 1)
                    {
                        ifStmt.TrueStatements.Add(new CodeMethodReturnStatement(propRef));
                    }
                    else
                    {
                        var indexCondition = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("index"), CodeBinaryOperatorType.LessThan,
                            new CodePropertyReferenceExpression(propRef, "Count"));
                        var innerIf = new CodeConditionStatement(indexCondition);
                        innerIf.TrueStatements.Add(new CodeMethodReturnStatement(new CodeIndexerExpression(propRef, new CodeVariableReferenceExpression("index"))));
                        innerIf.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
                        ifStmt.TrueStatements.Add(innerIf);
                    }
                    method.Statements.Add(ifStmt);
                }
                return method;
            }
        }
    }
}

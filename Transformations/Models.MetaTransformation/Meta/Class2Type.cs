using NMF.CodeGen;
using NMF.Expressions;
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
using System.Reflection;


#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        private static bool SetTypeReferenceForMappedType(IType type, CodeTypeReference reference)
        {
            var mapping = type.GetExtension<MappedType>();
            if (mapping != null)
            {
                reference.BaseType = mapping.SystemType.Name;
                reference.SetNamespace(mapping.SystemType.Namespace);
                return true;
            }
            return false;
        }

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

            /// <inheritdoc />
            public override CodeTypeDeclaration CreateOutput(IClass input, ITransformationContext context)
            {
                var generatedType = base.CreateOutput(input, context);
                SetTypeReferenceForMappedType(input, generatedType.GetReferenceForType());
                return generatedType;
            }

            /// <inheritdoc />
            protected override bool ShouldContainMembers(CodeTypeDeclaration generatedType, CodeTypeDeclaration baseType)
            {
                return base.ShouldContainMembers(generatedType, baseType) && baseType.Name != "ModelElement";
            }

            private static IEnumerable<Tuple<IClass, IReference>> FindReferencesToOverride(IClass current)
            {
                if (current.BaseTypes.Count <= 1)
                {
                    return Enumerable.Empty<Tuple<IClass, IReference>>();
                }

                var refinedReferencesList = new List<HashSet<IReference>>();
                foreach (var baseType in current.BaseTypes)
                {
                    var refinements = new HashSet<IReference>();
                    refinedReferencesList.Add(refinements);
                    PickUpRefinedReferences(baseType, refinements);
                }

                var toRefine = new HashSet<IReference>();
                for (int i = 0; i < refinedReferencesList.Count; i++)
                {
                    for (int j = i + 1; j < refinedReferencesList.Count; j++)
                    {
                        foreach (var reference in refinedReferencesList[i].Where(reference => refinedReferencesList[j].Remove(reference)))
                        {
                            toRefine.Add(reference);
                        }
                    }
                }

                return toRefine.Select(r => Tuple.Create(current, r));
            }

            private static IEnumerable<Tuple<IClass, IAttribute>> FindAttributesToOverride(IClass current)
            {
                if (current.BaseTypes.Count <= 1)
                {
                    return Enumerable.Empty<Tuple<IClass, IAttribute>>();
                }

                var refinedAttributesList = new List<HashSet<IAttribute>>();
                foreach (var baseType in current.BaseTypes)
                {
                    var refinements = new HashSet<IAttribute>();
                    refinedAttributesList.Add(refinements);
                    PickUpRefinedAttributes(baseType, refinements);
                }

                var toRefine = new HashSet<IAttribute>();
                for (int i = 0; i < refinedAttributesList.Count; i++)
                {
                    for (int j = i + 1; j < refinedAttributesList.Count; j++)
                    {
                        foreach (var attribute in refinedAttributesList[i].Where(attribute => refinedAttributesList[j].Remove(attribute)))
                        {
                            toRefine.Add(attribute);
                        }
                    }
                }

                return toRefine.Select(r => Tuple.Create(current, r));
            }

            private static void PickUpRefinedReferences(IClass scope, HashSet<IReference> references)
            {
                foreach (var constraint in scope.ReferenceConstraints)
                {
                    references.Add(constraint.Constrains);
                }

                foreach (var reference in scope.References.Where(reference => reference.Refines != null))
                {
                    references.Add(reference.Refines);
                }

                foreach (var baseType in scope.BaseTypes)
                {
                    PickUpRefinedReferences(baseType, references);
                }
            }

            private static void PickUpRefinedAttributes(IClass scope, HashSet<IAttribute> references)
            {
                foreach (var constraint in scope.AttributeConstraints)
                {
                    references.Add(constraint.Constrains);
                }

                foreach (var attribute in scope.Attributes.Where(attribute => attribute.Refines != null))
                {
                    references.Add(attribute.Refines);
                }

                foreach (var baseType in scope.BaseTypes)
                {
                    PickUpRefinedAttributes(baseType, references);
                }
            }

            /// <summary>
            /// Registers the dependencies: Marks the rule instantiating for Type2Type and requires to generate members and base classes
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());

                RequireBaseClasses(cl => cl.BaseTypes);

                RequireGenerateMethods(Rule<Operation2Method>(), cl => cl.Operations);
                RequireGenerateProperties(Rule<Attribute2Property>(), cl => cl.Attributes);
                RequireGenerateProperties(Rule<Reference2Property>(), cl => cl.References);

                RequireMany(Rule<RefinedReferenceGenerator>(), cl =>
                {
                    return cl.ReferenceConstraints.Select(c => Tuple.Create(cl, c.Constrains))
                            .Concat(FindReferencesToOverride(cl));
                });
                RequireMany(Rule<RefinedAttributeGenerator>(), cl =>
                {
                    return cl.AttributeConstraints.Select(c => Tuple.Create(cl, c.Constrains))
                            .Concat(FindAttributesToOverride(cl));
                });

                RequireMany(Rule<Feature2Proxy>(), cl => cl.Attributes.Where(a => a.UpperBound == 1), (cl, proxies) => cl.DependentMembers(true).AddRange(proxies));
                RequireMany(Rule<Feature2Proxy>(), cl => cl.References.Where(r => r.UpperBound == 1), (cl, proxies) => cl.DependentMembers(true).AddRange(proxies));

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
                if (input.IsAbstract) generatedType.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
                generatedType.IsPartial = true;

                GenerateBasicAttributes(input, generatedType);

                generatedType.WriteDocumentation(input.Summary ?? string.Format("The default implementation of the {0} class", input.Name), input.Remarks);

                var members = generatedType.Members;
                if (input.InstanceOf != null)
                {
                    GenerateInstanceOf(input, generatedType, context, members);
                }

                base.Transform(input, generatedType, context);

                AdjustTypeReference(input, generatedType, context);

                generatedType.BaseTypes.Add(typeof(IModelElement).ToTypeReference());

                AddIfNotNull(members, CreateChildren(input, generatedType, context));
                AddIfNotNull(members, CreateGetRelativePathForNonIdentifiedChild(input, generatedType, context));
                AddIfNotNull(members, CreateGetModelElementForReference(input, generatedType, context));
                AddIfNotNull(members, CreateReferencesProperty(input, generatedType, context));
                AddIfNotNull(members, CreateGetAttributeValue(input, generatedType, context));
                AddIfNotNull(members, CreateGetCollectionForFeature(input, generatedType, context));
                AddIfNotNull(members, CreateSetFeature(input, generatedType, context));
                AddIfNotNull(members, CreateGetExpressionForAttribute(input, generatedType, context));
                AddIfNotNull(members, CreateGetExpressionForReference(input, generatedType, context));
                AddIfNotNull(members, CreateGetCompositionName(input, generatedType, context));

                CodeMemberField codeField = CreateClassField();
                AddIfNotNull(members, codeField);
                var baseClasses = input.Closure(cl => cl.BaseTypes);
                var isModelElementContained = false;
                foreach (var cl in baseClasses)
                {
                    if (cl.RelativeUri == ModelExtensions.ClassModelElement.RelativeUri) isModelElementContained = true;
                    if (cl.InstanceOf != null && cl != input)
                    {
                        GenerateInstanceOfMethodsForBaseClass(input, generatedType, context, members, codeField, cl);
                    }
                }
                if (!isModelElementContained)
                {
                    AddIfNotNull(members, CreateOverriddenGetClassMethod(input, ModelExtensions.ClassModelElement.InstanceOf, context, codeField, true));
                    AddIfNotNull(members, CreateClassInstanceProperty(input, ModelExtensions.ClassModelElement.InstanceOf, context, codeField, true));
                }

                ImplementIdentifier(input, generatedType, context);

                AddRootClass(input, context);

                var serializationName = input.Name;
                var serializationInfo = input.GetExtension<SerializationInformation>();
                if (serializationInfo != null)
                {
                    serializationName = serializationInfo.SerializationName;
                }

                if (serializationName != generatedType.Name)
                {
                    generatedType.AddAttribute(typeof(XmlElementNameAttribute), serializationName);
                }
            }

            private static void AddRootClass(IClass input, ITransformationContext context)
            {
                var inputModel = input.Model;
                if (!input.BaseTypes.Any(c => c.Model == inputModel))
                {
                    lock (context.Data)
                    {
                        context.GetRootClasses(true).Add(input);
                    }
                }
            }

            private void GenerateInstanceOfMethodsForBaseClass(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context, CodeTypeMemberCollection members, CodeMemberField codeField, IClass cl)
            {
                var isOverride = cl.RelativeUri == ModelExtensions.ClassModelElement.RelativeUri ||
                                            !base.ShouldContainMembers(generatedType, context.Trace.ResolveIn(this, cl));

                AddIfNotNull(members, CreateOverriddenGetClassMethod(input, cl.InstanceOf, context, codeField, isOverride));
                AddIfNotNull(members, CreateClassInstanceProperty(input, cl.InstanceOf, context, codeField, isOverride));

                var referenceImplementations = cl.InstanceOf.MostSpecificRefinement(ModelExtensions.ReferenceTypeReferencesReference);
                if (!referenceImplementations.Contains(ModelExtensions.ReferenceTypeReferencesReference))
                {
                    foreach (var referenceImplementation in referenceImplementations)
                    {
                        AddIfNotNull(members, CreateOverriddenReferenceImplementation(cl, referenceImplementation, context, isOverride));
                        AddIfNotNull(members, CreateOverriddenReferenceProxyImplementation(cl, referenceImplementation, context, isOverride));
                    }
                }
            }

            private void GenerateInstanceOf(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context, CodeTypeMemberCollection members)
            {
                AddIfNotNull(members, CreateAbstractGetClassMethod(input.InstanceOf, context));

                var referenceImplementations = input.InstanceOf.MostSpecificRefinement(ModelExtensions.ReferenceTypeReferencesReference);
                if (!referenceImplementations.Contains(ModelExtensions.ReferenceTypeReferencesReference))
                {
                    foreach (var referenceImplementation in referenceImplementations)
                    {
                        AddIfNotNull(members, CreateAbstractReferenceImplementation(input.InstanceOf, referenceImplementation, context, generatedType.GetReferenceForType()));
                        AddIfNotNull(members, CreateAbstractReferenceProxyImplementation(input.InstanceOf, referenceImplementation, context));
                    }
                }
            }

            private static void GenerateBasicAttributes(IClass input, CodeTypeDeclaration generatedType)
            {
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
            }

            private static CodeTypeMember CreateOverriddenReferenceImplementation(IClass scope, IReference referenceImplementation, ITransformationContext context, bool isOverride)
            {
                if (referenceImplementation.ReferenceType is not IClass referenceType) return null;

                var upperBound = referenceType.GetUpperBoundConstraintValue();

                if (upperBound.HasValue && upperBound.Value == 1)
                {
                    // We know that the reference is single-valued
                    IReferenceType referencedType = FindTargetTypeForReferenceClass(referenceType);

                    var overriddenReferenceImplementation = new CodeMemberMethod()
                    {
                        Attributes = MemberAttributes.Public,
                        Name = "Get" + referenceImplementation.Name.ToPascalCase() + "Value",
                        ReturnType = CreateReference(referencedType, true, context)
                    };
                    if (isOverride)
                    {
                        overriddenReferenceImplementation.Attributes |= MemberAttributes.Override;
                    }
                    overriddenReferenceImplementation.Parameters.Add(new CodeParameterDeclarationExpression(CreateReference(referenceImplementation.ReferenceType, true, context), "reference"));
                    overriddenReferenceImplementation.WriteDocumentation(string.Format("Gets the referenced value for a {0} of the enclosing {1}.", referenceImplementation.Name, scope.InstanceOf.Name));

                    overriddenReferenceImplementation.Statements.Add(new CodeMethodReturnStatement(new CodeCastExpression(overriddenReferenceImplementation.ReturnType,
                        new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetReferencedElement", new CodeArgumentReferenceExpression("reference")))));

                    return overriddenReferenceImplementation;
                }
                return null;
            }

            private static CodeTypeMember CreateOverriddenReferenceProxyImplementation(IClass scope, IReference referenceImplementation, ITransformationContext context, bool isOverride)
            {
                if (referenceImplementation.ReferenceType is not IClass referenceType) return null;

                var upperBound = referenceType.GetUpperBoundConstraintValue();

                if (upperBound.HasValue && upperBound.Value == 1)
                {
                    // We know that the reference is single-valued
                    IReferenceType referencedType = FindTargetTypeForReferenceClass(referenceType);

                    var innerReturnType = CreateReference(referencedType, true, context);
                    var overriddenReferenceImplementation = new CodeMemberMethod()
                    {
                        Attributes = MemberAttributes.Public,
                        Name = "Get" + referenceImplementation.Name.ToPascalCase() + "Proxy",
                        ReturnType = new CodeTypeReference(typeof(INotifyValue<>).Name, innerReturnType)
                    };
                    if (isOverride)
                    {
                        overriddenReferenceImplementation.Attributes |= MemberAttributes.Override;
                    }
                    overriddenReferenceImplementation.Parameters.Add(new CodeParameterDeclarationExpression(CreateReference(referenceImplementation.ReferenceType, true, context), "reference"));
                    overriddenReferenceImplementation.WriteDocumentation(string.Format("Gets the referenced value for a {0} of the enclosing {1}.", referenceImplementation.Name, scope.InstanceOf.Name));

                    var referenceRef = new CodeArgumentReferenceExpression("reference");

                    overriddenReferenceImplementation.Statements.Add(new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(referenceRef, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression()),
                        new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentOutOfRangeException).ToTypeReference(), new CodePrimitiveExpression("reference")))));

                    var castMethod = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(Observable).ToTypeReference()), "As", typeof(IModelElement).ToTypeReference(), innerReturnType);
                    var expression = new CodeMethodInvokeExpression(castMethod,
                        new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetExpressionForReference", new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(referenceRef, "Name"), "ToUpperInvariant")));

                    overriddenReferenceImplementation.Statements.Add(new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(referenceRef, "UpperBound"), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(1)),
                        new CodeMethodReturnStatement(expression)));

                    overriddenReferenceImplementation.ThrowException<NotSupportedException>();

                    return overriddenReferenceImplementation;
                }
                return null;
            }

            private static CodeTypeMember CreateAbstractReferenceImplementation(IClass instanceOf, IReference referenceImplementation, ITransformationContext context, CodeTypeReference selfReference)
            {
                if (referenceImplementation.ReferenceType is not IClass referenceType) return null;

                var upperBound = referenceType.GetUpperBoundConstraintValue();
                
                if (upperBound.HasValue && upperBound.Value == 1)
                {
                    // We know that the reference is single-valued
                    IReferenceType referencedType = FindTargetTypeForReferenceClass(referenceType);

                    var abstractReferenceImplementation = new CodeMemberMethod()
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                        Name = "Get" + referenceImplementation.Name.ToPascalCase() + "Value",
                        ReturnType = CreateReference(referencedType, true, context)
                    };
                    abstractReferenceImplementation.Parameters.Add(new CodeParameterDeclarationExpression(CreateReference(referenceImplementation.ReferenceType, true, context), "reference"));
                    abstractReferenceImplementation.WriteDocumentation(string.Format("Gets the referenced value for a {0} of the enclosing {1}.", referenceImplementation.Name, instanceOf.Name));
                    abstractReferenceImplementation.AddAttribute(typeof(ObservableProxyAttribute), new CodeTypeOfExpression(selfReference), "Get" + referenceImplementation.Name.ToPascalCase() + "Proxy");
                    return abstractReferenceImplementation;
                }
                return null;
            }

            private static CodeTypeMember CreateAbstractReferenceProxyImplementation(IClass instanceOf, IReference referenceImplementation, ITransformationContext context)
            {
                if (referenceImplementation.ReferenceType is not IClass referenceType) return null;

                var upperBound = referenceType.GetUpperBoundConstraintValue();

                if (upperBound.HasValue && upperBound.Value == 1)
                {
                    // We know that the reference is single-valued
                    IReferenceType referencedType = FindTargetTypeForReferenceClass(referenceType);

                    var abstractReferenceImplementation = new CodeMemberMethod()
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                        Name = "Get" + referenceImplementation.Name.ToPascalCase() + "Proxy",
                        ReturnType = new CodeTypeReference(typeof(INotifyValue<>).Name, CreateReference(referencedType, true, context))
                    };
                    abstractReferenceImplementation.Parameters.Add(new CodeParameterDeclarationExpression(CreateReference(referenceImplementation.ReferenceType, true, context), "reference"));
                    abstractReferenceImplementation.WriteDocumentation(string.Format("Gets a proxy for the referenced value for a {0} of the enclosing {1}.", referenceImplementation.Name, instanceOf.Name));
                    return abstractReferenceImplementation;
                }
                return null;
            }

            private static IReferenceType FindTargetTypeForReferenceClass(IClass referenceType)
            {
                // First look if there is a suitable constraint
                var referencedType = referenceType.GetReferenceTypeConstraintValue();
                // If not, search whether we can conclude on constrained base types
                if (referencedType == null)
                {
                    var targetTypeReferences = referenceType.MostSpecificRefinement(ModelExtensions.ReferenceReferenceTypeReference);
                    if (targetTypeReferences.Count == 1 && targetTypeReferences.First().ReferenceType is IClass targetType)
                    {
                        var constrainedBaseTypes = targetType.GetReferenceConstraintValue(ModelExtensions.ClassBaseTypesReference);
                        if (constrainedBaseTypes != null)
                        {
                            // It would be better to use the most specific of these here, though we hardly expect more than one
                            referencedType = constrainedBaseTypes.OfType<IClass>().FirstOrDefault();
                        }
                    }
                }
                return referencedType;
            }

            /// <summary>
            /// Gets the interface type for the given instantiating class
            /// </summary>
            /// <param name="instantiating">The instantiating class</param>
            /// <param name="context">The context in which the request is made</param>
            /// <returns></returns>
            protected virtual CodeTypeReference GetInterfaceType(IClass instantiating, ITransformationContext context)
            {
                var targetType = new CodeTypeReference();
                if (!SetTypeReferenceForMappedType(instantiating, targetType))
                {
                    targetType.BaseType = "I" + instantiating.Name.ToPascalCase();
                }
                return targetType;
            }

            private static CodeMemberField CreateClassField()
            {
                var classField = new CodeMemberField(typeof(IClass).ToTypeReference(), "_classInstance")
                {
                    Attributes = MemberAttributes.Private | MemberAttributes.Static
                };
                return classField;
            }

            /// <summary>
            /// Creates the overridden type-safe Get(Class) method
            /// </summary>
            /// <param name="input">The class for which the method is generated</param>
            /// <param name="instantiating">The return type of the method</param>
            /// <param name="context">The context in which the request is made</param>
            /// <param name="classField">The class field</param>
            /// <param name="isOverride">True, if this is an override, otherwise false</param>
            /// <returns>A method declaration</returns>
            protected virtual CodeMemberMethod CreateOverriddenGetClassMethod(IClass input, IClass instantiating, ITransformationContext context, CodeMemberField classField, bool isOverride)
            {
                var type = GetInterfaceType(instantiating, context);
                var getClass = new CodeMemberMethod()
                {
                    Name = "Get" + instantiating.Name.ToPascalCase(),
                    ReturnType = type,
                    Attributes = MemberAttributes.Public
                };
                if (isOverride)
                {
                    getClass.Attributes |= MemberAttributes.Override;
                }
                var absoluteUri = input.AbsoluteUri;
                if (classField != null && absoluteUri != null && absoluteUri.IsAbsoluteUri)
                {
                    GenerateClassFieldReferenceAndInitialization(instantiating, classField, type, getClass.Statements, absoluteUri);
                }
                else
                {
                    getClass.ThrowException<NotSupportedException>($"{input.Name} does not have an absolute URI and therefore cannot be resolved.");
                }
                getClass.WriteDocumentation(string.Format("Gets the {0} for this model element", instantiating.Name));
                return getClass;
            }

            private static void GenerateClassFieldReferenceAndInitialization(IClass instantiating, CodeMemberField classField, CodeTypeReference type, CodeStatementCollection statements, Uri absoluteUri)
            {
                var repositoryRef = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(MetaRepository).ToTypeReference()), "Instance");
                var classRef = new CodeCastExpression(typeof(IClass).ToTypeReference(), new CodeMethodInvokeExpression(repositoryRef, "Resolve", new CodePrimitiveExpression(absoluteUri.AbsoluteUri)));
                var classFieldRef = new CodeFieldReferenceExpression(null, classField.Name);
                var nullRef = new CodePrimitiveExpression(null);
                statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(classFieldRef, CodeBinaryOperatorType.IdentityEquality, nullRef),
                    new CodeAssignStatement(classFieldRef, classRef)));
                if (instantiating.AbsoluteUri == ModelExtensions.ClassModelElement.InstanceOf.AbsoluteUri)
                {
                    statements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(null, classField.Name)));
                }
                else
                {
                    statements.Add(new CodeMethodReturnStatement(new CodeCastExpression(type, new CodeFieldReferenceExpression(null, classField.Name))));
                }
            }

            /// <summary>
            /// Generates a class instance property
            /// </summary>
            /// <param name="input">The class for which the property shall be generated</param>
            /// <param name="instantiating">The type of class that should be returned</param>
            /// <param name="context">The context in which the request is made</param>
            /// <param name="classField">The class field</param>
            /// <param name="isOld">True, if there is already an inherited property with the same name, otherwise False</param>
            /// <returns>A property declaration or null</returns>
            protected virtual CodeMemberProperty CreateClassInstanceProperty(IClass input, IClass instantiating, ITransformationContext context, CodeMemberField classField, bool isOld)
            {
                var absoluteUri = input.AbsoluteUri;
                if (classField != null && absoluteUri != null && absoluteUri.IsAbsoluteUri)
                {
                    var type = GetInterfaceType(instantiating, context);
                    var classInstanceProperty = new CodeMemberProperty()
                    {
                        Name = instantiating.Name.ToPascalCase() + "Instance",
                        Type = type,
                        Attributes = MemberAttributes.Public | MemberAttributes.Static
                    };
                    if (input.InstanceOf != instantiating && isOld)
                    {
                        classInstanceProperty.Attributes |= MemberAttributes.New;
                    }
                    classInstanceProperty.HasGet = true;
                    classInstanceProperty.HasSet = false;
                    classInstanceProperty.WriteDocumentation(string.Format("Gets the {0} model for this type", instantiating.Name));
                    GenerateClassFieldReferenceAndInitialization(instantiating, classField, type, classInstanceProperty.GetStatements, absoluteUri);
                    return classInstanceProperty;
                }
                else
                {
                    return null;
                }
            }    

            /// <summary>
            /// Creates an abstract type-safe Get(Class) method
            /// </summary>
            /// <param name="input">The type of class that the method shall return</param>
            /// <param name="context">The context in which this call is made</param>
            /// <returns>A code method declaration</returns>
            protected virtual CodeMemberMethod CreateAbstractGetClassMethod(IClass input, ITransformationContext context)
            {
                var abstractGetClass = new CodeMemberMethod()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                    Name = "Get" + input.Name.ToPascalCase(),
                    ReturnType = context.Trace.ResolveIn(this, input).GetReferenceForType()
                };
                abstractGetClass.WriteDocumentation(string.Format("Gets the {0} for this model element", input.Name));
                return abstractGetClass;
            }

            private static void AddIfNotNull(CodeTypeMemberCollection members, CodeTypeMember member)
            {
                if (member != null)
                {
                    members.Add(member);
                }
            }

            private void AdjustTypeReference(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var reference = generatedType.GetReferenceForType();
                if (input.GetExtension<MappedType>() == null)
                {
                    var ns2ns = Rule<Namespace2Namespace>();
                    if (ns2ns != null)
                    {
                        var baseType = reference.BaseType;
                        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (var ns in ns2ns.DefaultImports)
                        {
                            ApplyUsing(input, context, reference, ns2ns, baseType, assemblies, ns);
                        }
                    }
                }
            }

            private static void ApplyUsing(IClass input, ITransformationContext context, CodeTypeReference reference, Namespace2Namespace ns2ns, string baseType, Assembly[] assemblies, string ns)
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

            /// <summary>
            /// Creates the public interface for the given NMeta class
            /// </summary>
            /// <param name="input">The NMeta class</param>
            /// <param name="generatedType">The generated type</param>
            /// <returns>A new type declaration that has attributes set same like the original type</returns>
            protected override CodeTypeDeclaration CreateSeparatePublicInterface(IClass input, CodeTypeDeclaration generatedType)
            {
                if (Transformation is Meta2ClassesTransformation t && !t.SeparateImplementations) return null;
                var iface = new CodeTypeDeclaration()
                {
                    Name = "I" + generatedType.Name,
                    Attributes = MemberAttributes.Public,
                    IsInterface = true,
                    IsPartial = true,
                };
                iface.BaseTypes.Add(typeof(IModelElement).ToTypeReference());
                iface.AddAttribute(typeof(DefaultImplementationTypeAttribute), new CodeTypeOfExpression(generatedType.Name));
                iface.AddAttribute(typeof(XmlDefaultImplementationTypeAttribute), new CodeTypeOfExpression(generatedType.Name));
                iface.WriteDocumentation("The public interface for " + input.Name);
                var uri = input.AbsoluteUri;
                if (uri != null && uri.IsAbsoluteUri)
                {
                    iface.AddAttribute(typeof(ModelRepresentationClassAttribute), uri.AbsoluteUri);
                }
                return iface;
            }

            /// <inheritdoc />
            protected override void CreateInterfaceMembers(IClass input, CodeTypeDeclaration generatedType, CodeTypeDeclaration interfaceDecl, ITransformationContext context)
            {
                base.CreateInterfaceMembers(input, generatedType, interfaceDecl, context);
                // we need to manually add public members for refined attributes and references
                var refinedAttributes = input.Attributes
                    .Select(a => a.Refines)
                    .Where(a => a != null && a.DeclaringType == input)
                    .Distinct().ToList();

                foreach (var att in refinedAttributes)
                {
                    var property = context.Trace.ResolveIn(Rule<Attribute2Property>(), att);
                    CopyProperty(property, interfaceDecl);
                }

                var refinedReferences = input.References
                    .Select(r => r.Refines)
                    .Where(r => r != null && r.DeclaringType == input)
                    .Distinct().ToList();

                foreach (var reference in refinedReferences)
                {
                    var property = context.Trace.ResolveIn(Rule<Reference2Property>(), reference);
                    CopyProperty(property, interfaceDecl);
                }
            }

            private void CopyProperty(CodeMemberProperty property, CodeTypeDeclaration interfaceDecl)
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

            /// <summary>
            /// Adds the implementation base class for the framework, i.e. ModelElement
            /// </summary>
            /// <param name="generatedType">The generated type</param>
            protected override void AddImplementationBaseClass(CodeTypeDeclaration generatedType)
            {
                generatedType.BaseTypes.Insert(0, typeof(ModelElement).ToTypeReference());
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
                    Type = new CodeTypeReference(typeof(IEnumerableExpression<>).Name, typeof(IModelElement).ToTypeReference()),
                    HasGet = true,
                    HasSet = false,
                    Name = "ReferencedElements"
                };
                referencedProperty.WriteDocumentation("Gets the referenced model elements of this model element");
                var referenced = new CodeObjectCreateExpression(referencedClass.GetReferenceForType(), thisRef);
                var baseReferenced = new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), "ReferencedElements");
                referencedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(baseReferenced, "Concat", referenced)));
                referencedProperty.SetMerge(other =>
                {
                    var newReferences = new CodeMemberProperty()
                    {
                        Attributes = referencedProperty.Attributes,
                        Type = referencedProperty.Type,
                        HasGet = referencedProperty.HasGet,
                        HasSet = referencedProperty.HasSet,
                        Name = referencedProperty.Name
                    };
                    newReferences.WriteDocumentation("Gets the referenced model elements of this model element");
                    var otherExpression = ((other as CodeMemberProperty).GetStatements[0] as CodeMethodReturnStatement).Expression;
                    newReferences.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(otherExpression, "Concat", referenced)));
                    return newReferences;
                });
                return referencedProperty;
            }

            /// <summary>
            /// Implements the members necessary for the identifier mechanism of the given NMeta class
            /// </summary>
            /// <param name="class">The NMeta class</param>
            /// <param name="context">The context in which the class is generated</param>
            /// <param name="generatedType">The generated type for the NMeta class</param>
            protected virtual void ImplementIdentifier(IClass @class, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var identifier = @class.RetrieveIdentifier();
                if (identifier.Identifier == null || generatedType.IsInterface) return;
                var identifierProp = context.Trace.ResolveIn(Rule<Attribute2Property>(), identifier.Identifier);
                generatedType.AddAttribute(typeof(DebuggerDisplayAttribute), string.Format("{0} {{{1}}}", @class.Name, identifierProp.Name));

                if (generatedType.Members.Contains(identifierProp))
                {
                    var isIdentifiedProperty = new CodeMemberProperty
                    {
                        Name = "IsIdentified",
                        Type = new CodeTypeReference(typeof(bool)),
                        Attributes = MemberAttributes.Public | MemberAttributes.Override,
                        HasGet = true,
                        HasSet = false
                    };
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
                    var identifierProperty = context.Trace.ResolveIn(Rule<Attribute2Property>(), identifier.Identifier);
                    var identifiedObject = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), identifierProperty.Name);
                    var toString = new CodeMethodInvokeExpression(identifiedObject, "ToString");
                    if (Transformation is Meta2ClassesTransformation t && !t.IsValueType(identifier.Identifier.Type))
                    {
                        var nullRef = new CodePrimitiveExpression(null);
                        toIdentifierString.Statements.Add(new CodeConditionStatement(
                            new CodeBinaryOperatorExpression(identifiedObject, CodeBinaryOperatorType.IdentityEquality, nullRef),
                            new CodeMethodReturnStatement(nullRef)));
                    }
                    toIdentifierString.Statements.Add(new CodeMethodReturnStatement(toString));
                    generatedType.Members.Add(toIdentifierString);

                    if (identifier.Scope == IdentifierScope.Global)
                    {
                        generatedType.Members.Add(CreateCreateUriWithFragment());
                        generatedType.Members.Add(CreatePropagateNewModel());
                        generatedType.Members.Add(CreateOnKeyChanged());
                    }
                }
            }

            private static CodeMemberMethod CreateOnKeyChanged()
            {
                var onKeyChanged = new CodeMemberMethod
                {
                    Name = "OnKeyChanged",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                onKeyChanged.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ValueChangedEventArgs).ToTypeReference(), "e"));
                var eRef = new CodeArgumentReferenceExpression("e");
                onKeyChanged.Statements.Add(new CodeMethodInvokeExpression(null, "UpdateRegisteredIdentifier", eRef));
                onKeyChanged.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), onKeyChanged.Name, eRef));
                onKeyChanged.WriteDocumentation("Notifies clients that the identifier changed");
                return onKeyChanged;
            }

            private static CodeMemberMethod CreatePropagateNewModel()
            {
                var propagateNewModel = new CodeMemberMethod
                {
                    Name = "PropagateNewModel",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                propagateNewModel.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Model).ToTypeReference(), "newModel"));
                propagateNewModel.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Model).ToTypeReference(), "oldModel"));
                propagateNewModel.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IModelElement).ToTypeReference(), "subtreeRoot"));
                var oldModelRef = new CodeArgumentReferenceExpression("oldModel");
                var newModelRef = new CodeArgumentReferenceExpression("newModel");
                var nullRef = new CodePrimitiveExpression(null);
                var thisRef = new CodeThisReferenceExpression();
                propagateNewModel.Statements.Add(new CodeVariableDeclarationStatement(typeof(string), "id", new CodeMethodInvokeExpression(thisRef, "ToIdentifierString")));
                var idRef = new CodeVariableReferenceExpression("id");
                propagateNewModel.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(oldModelRef, CodeBinaryOperatorType.IdentityInequality, nullRef),
                    new CodeExpressionStatement(new CodeMethodInvokeExpression(oldModelRef, "UnregisterId", idRef))));
                propagateNewModel.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(newModelRef, CodeBinaryOperatorType.IdentityInequality, nullRef),
                    new CodeExpressionStatement(new CodeMethodInvokeExpression(newModelRef, "RegisterId", idRef, thisRef))));
                propagateNewModel.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "PropagateNewModel", newModelRef, oldModelRef, new CodeArgumentReferenceExpression("subtreeRoot")));
                propagateNewModel.WriteDocumentation("Propagates through the composition hierarchy that an entire subtree has been added to a new model");
                return propagateNewModel;
            }

            private static CodeMemberMethod CreateCreateUriWithFragment()
            {
                var createUriWithFragment = new CodeMemberMethod
                {
                    Name = "CreateUriWithFragment",
                    ReturnType = typeof(Uri).ToTypeReference(),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                createUriWithFragment.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fragment"));
                createUriWithFragment.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "absolute"));
                createUriWithFragment.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IModelElement).ToTypeReference(), "baseElement"));
                createUriWithFragment.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(), "CreateUriFromGlobalIdentifier",
                    new CodeArgumentReferenceExpression("fragment"), new CodeArgumentReferenceExpression("absolute"))));
                createUriWithFragment.WriteDocumentation("Creates the uri with the given fragment starting from the current model element");
                return createUriWithFragment;
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
                    ReturnType = typeof(IModelElement).ToTypeReference(),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                getModelElementForUri.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "reference"));
                getModelElementForUri.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
                AddReferencesOfClass(input, generatedType, AddToGetModelElementForUri, getModelElementForUri, false, context);
                getModelElementForUri.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetModelElementForReference", new CodeArgumentReferenceExpression("reference"), new CodeArgumentReferenceExpression("index"))));
                if (getModelElementForUri.Statements.Count == 1)
                {
                    return null;
                }
                getModelElementForUri.WriteDocumentation("Resolves the given URI to a child model element", "The model element or null if it could not be found", new Dictionary<string, string>() 
                {
                    {"reference", "The requested reference name"},
                    {"index", "The index of this reference"}
                });
                return getModelElementForUri;
            }

            /// <summary>
            /// Generates the GetAttributeValue method
            /// </summary>
            /// /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The GetAttributeValue method for the given class</returns>
            protected virtual CodeMemberMethod CreateGetAttributeValue(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getAttributeValue = new CodeMemberMethod()
                {
                    Name = "GetAttributeValue",
                    ReturnType = new CodeTypeReference(typeof(object)),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                getAttributeValue.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "attribute"));
                getAttributeValue.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
                AddAttributesOfClass(input, generatedType, AddToGetAttributeValue, getAttributeValue, context);
                getAttributeValue.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetAttributeValue", new CodeArgumentReferenceExpression("attribute"), new CodeArgumentReferenceExpression("index"))));
                if (getAttributeValue.Statements.Count == 1) return null;
                getAttributeValue.WriteDocumentation("Resolves the given attribute name", "The attribute value or null if it could not be found", new Dictionary<string, string>()
                {
                    {"attribute", "The requested attribute name"},
                    {"index", "The index of this attribute"}
                });
                return getAttributeValue;
            }

            private CodeMemberMethod AddToGetAttributeValue(CodeMemberMethod method, IAttribute attribute, CodeMemberProperty property, ITransformationContext context)
            {
                if (attribute.UpperBound == 1 || attribute.IsOrdered)
                {
                    var propRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property.Name);
                    var ifStmt = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("attribute"),
                        CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(attribute.Name.ToUpperInvariant())));
                    if (attribute.UpperBound == 1)
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

            /// <summary>
            /// Generates the GetCollectionForFeature method
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The GetCollectionForFeature method</returns>
            protected virtual CodeMemberMethod CreateGetCollectionForFeature(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getCollectionForFeature = new CodeMemberMethod()
                {
                    Name = "GetCollectionForFeature",
                    ReturnType = new CodeTypeReference(typeof(System.Collections.IList)),
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                getCollectionForFeature.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "feature"));
                AddReferencesOfClass(input, generatedType, AddToCollectionsForFeature, getCollectionForFeature, false, context);
                AddAttributesOfClass(input, generatedType, AddToCollectionsForFeature, getCollectionForFeature, context);
                if (getCollectionForFeature.Statements.Count == 0)
                {
                    return null;
                }
                getCollectionForFeature.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetCollectionForFeature", new CodeArgumentReferenceExpression("feature"))));
                getCollectionForFeature.WriteDocumentation("Gets the Model element collection for the given feature", "A non-generic list of elements", new Dictionary<string, string>()
                {
                    { "feature", "The requested feature" }
                });
                return getCollectionForFeature;
            }

            private CodeMemberMethod AddToCollectionsForFeature(CodeMemberMethod method, ITypedElement feature, CodeMemberProperty property, ITransformationContext context)
            {
                if (feature.UpperBound != 1)
                {
                    var propRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "_" + property.Name.ToCamelCase());
                    var ifStmt = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("feature"),
                        CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(feature.Name.ToUpperInvariant())));
                    ifStmt.TrueStatements.Add(new CodeMethodReturnStatement(propRef));
                    method.Statements.Add(ifStmt);
                }
                return method;
            }

            /// <summary>
            /// Creates the SetFeature method
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The SetFeature method</returns>
            protected virtual CodeMemberMethod CreateSetFeature(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var setFeature = new CodeMemberMethod()
                {
                    Name = "SetFeature",
                    ReturnType = null,
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                setFeature.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "feature"));
                setFeature.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "value"));
                var thisRef = new CodeThisReferenceExpression();
                AddReferencesOfClass(input, generatedType, (m, f, p, _) => AddSetFeature(m, f, p, context, true, thisRef), setFeature, false, context);
                AddAttributesOfClass(input, generatedType, (m, f, p, _) => AddSetFeature(m, f, p, context, false, thisRef), setFeature, context);
                var type2Type = Rule<Type2Type>();
                AddRefinedReferencesOfClass(input, generatedType, (m, f, p, _) =>
                {
                    var type = context.Trace.ResolveIn(type2Type, f.Type);
                    return AddSetFeature(m, f, p, context, true, new CodeCastExpression(type.GetReferenceForType(), thisRef));
                }, setFeature, false, context);
                AddRefinedAttributesOfClass(input, generatedType, (m, f, p, _) =>
                {
                    var type = context.Trace.ResolveIn(type2Type, f.Type);
                    return AddSetFeature(m, f, p, context, false, new CodeCastExpression(type.GetReferenceForType(), thisRef));
                }, setFeature, context);
                if (setFeature.Statements.Count == 0)
                {
                    return null;
                }
                setFeature.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "SetFeature", new CodeArgumentReferenceExpression("feature"), new CodeArgumentReferenceExpression("value")));
                setFeature.WriteDocumentation("Sets a value to the given feature", null, new Dictionary<string, string>()
                {
                    { "feature", "The requested feature" },
                    { "value", "The value that should be set to that feature" }
                });
                return setFeature;
            }

            /// <summary>
            /// Creates the CallOperation method
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The CallOperation method</returns>
            protected virtual CodeMemberMethod CreateCallOperation(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var callOperation = new CodeMemberMethod()
                {
                    Name = "CallOperation",
                    ReturnType = typeof(object).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                callOperation.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IOperation).ToTypeReference(), "operation"));
                callOperation.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object[]), "arguments"));
                AddOperationsOfClass(input, generatedType, (_, op, ctx) =>
                {
                    var meth = context.Trace.ResolveIn(Rule<Operation2Method>(), op);
                    var ifStmt = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("operation"),
                        CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(op.Name.ToUpperInvariant())));
                    CodeExpression value = new CodeArgumentReferenceExpression("arguments");
                    var callArgs = new CodeExpression[meth.Parameters.Count];
                    for (int i = 0; i < meth.Parameters.Count; i++)
                    {
                        callArgs[i] = new CodeCastExpression(meth.Parameters[i].Type, new CodeIndexerExpression(value, new CodePrimitiveExpression(i)));
                    }
                    if (op.Type != null)
                    {
                        ifStmt.TrueStatements.Add(new CodeMethodReturnStatement(
                            new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), meth.Name, callArgs)));
                    }
                    else
                    {
                        ifStmt.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), meth.Name, callArgs));
                        ifStmt.TrueStatements.Add(new CodeMethodReturnStatement());
                    }
                    callOperation.Statements.Add(ifStmt);
                    return null;
                }, callOperation, context);
                if (callOperation.Statements.Count == 0)
                {
                    return null;
                }
                callOperation.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "CallOperation", new CodeArgumentReferenceExpression("operation"), new CodeArgumentReferenceExpression("arguments")));
                callOperation.WriteDocumentation("Calls the given operation", "The operation result or null, if the operation does not return any value", new Dictionary<string, string>()
                {
                    { "operation", "The operation that should be called" },
                    { "arguments", "The arguments used to call the operation" }
                });
                return callOperation;
            }

            private static CodeMemberMethod AddSetFeature(CodeMemberMethod method, ITypedElement feature, CodeMemberProperty property, ITransformationContext context, bool isReference, CodeExpression thisReference)
            {
                if (feature.UpperBound == 1)
                {
                    var propRef = new CodePropertyReferenceExpression(thisReference, property.Name);
                    var ifStmt = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("feature"),
                        CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(feature.Name.ToUpperInvariant())));
                    CodeExpression value = new CodeArgumentReferenceExpression("value");
                    if (feature.Type != null || isReference)
                    {
                        var targetType = CreateReference(feature.Type, isReference, context);
                        value = new CodeCastExpression(targetType, value);
                    }
                    ifStmt.TrueStatements.Add(new CodeAssignStatement(propRef, value));
                    ifStmt.TrueStatements.Add(new CodeMethodReturnStatement());
                    method.Statements.Add(ifStmt);
                }
                return method;
            }

            /// <summary>
            /// Creates the GetExpressionForReference method
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The GetExpressionForReference method</returns>
            protected virtual CodeMemberMethod CreateGetExpressionForReference(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getExpressionForReference = new CodeMemberMethod()
                {
                    Name = "GetExpressionForReference",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(INotifyExpression<IModelElement>))
                };
                getExpressionForReference.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "reference"));
                AddReferencesOfClass(input, generatedType, (m, r, p, _) => AddToExpressionForFeature(m, r, p, "reference"), getExpressionForReference, false, context);
                if (getExpressionForReference.Statements.Count == 0)
                {
                    return null;
                }
                getExpressionForReference.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetExpressionForReference", new CodeArgumentReferenceExpression("reference"))));
                getExpressionForReference.WriteDocumentation("Gets the property expression for the given reference", "An incremental property expression", new Dictionary<string, string>()
                {
                    { "reference", "The requested reference in upper case" }
                });
                return getExpressionForReference;
            }

            /// <summary>
            /// Generates the GetCompositionName method
            /// </summary>
            /// <param name="input">The class for which the method is generated</param>
            /// <param name="generatedType">The generated type declaration</param>
            /// <param name="context">The context in which the request is made</param>
            /// <returns>A method declaration</returns>
            protected virtual CodeMemberMethod CreateGetCompositionName(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getCompositionName = new CodeMemberMethod
                {
                    Name = "GetCompositionName",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(string))
                };
                getCompositionName.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "container"));
                var containerRef = new CodeArgumentReferenceExpression("container");
                AddReferencesOfClass(input, generatedType, (stmts, r, p, ctx) =>
                {
                    if (r.UpperBound != 1)
                    {
                        stmts.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(containerRef, CodeBinaryOperatorType.ValueEquality, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + p.Name.ToCamelCase())),
                            new CodeMethodReturnStatement(new CodePrimitiveExpression(r.Name))));
                    }
                    return stmts;
                }, getCompositionName.Statements, true, context);
                if (getCompositionName.Statements.Count == 0)
                {
                    return null;
                }
                getCompositionName.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetCompositionName", containerRef)));
                getCompositionName.WriteDocumentation("Gets the property name for the given container", "The name of the respective container reference", new Dictionary<string, string>()
                {
                    { "container", "The container object" }
                });
                return getCompositionName;
            }

            /// <summary>
            /// Creates the GetExpressionForAttribute method
            /// </summary>
            /// <param name="input">The NMeta class for which to generate the method</param>
            /// <param name="generatedType">The generated type for the class</param>
            /// <param name="context">The transformaion context</param>
            /// <returns>The GetExpressionForAttribute method</returns>
            protected virtual CodeMemberMethod CreateGetExpressionForAttribute(IClass input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getExpressionForAttribute = new CodeMemberMethod()
                {
                    Name = "GetExpressionForAttribute",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(INotifyExpression<object>))
                };
                getExpressionForAttribute.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "attribute"));
                AddAttributesOfClass(input, generatedType, (m, r, p, _) => AddToExpressionForFeature(m, r, p, "attribute"), getExpressionForAttribute, context);
                if (getExpressionForAttribute.Statements.Count == 0)
                {
                    return null;
                }
                getExpressionForAttribute.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "GetExpressionForAttribute", new CodeArgumentReferenceExpression("attribute"))));
                getExpressionForAttribute.WriteDocumentation("Gets the property expression for the given attribute", "An incremental property expression", new Dictionary<string, string>()
                {
                    { "attribute", "The requested attribute in upper case" }
                });
                return getExpressionForAttribute;
            }

            private static CodeMemberMethod AddToExpressionForFeature(CodeMemberMethod method, ITypedElement feature, CodeMemberProperty property, string parameterName)
            {
                if (feature.UpperBound == 1)
                {
                    var propTypeRef = new CodeTypeReference(feature.Name.ToPascalCase() + "Proxy");
                    var ifStmt = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(parameterName),
                        CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(property.Name.ToUpperInvariant())));
                    CodeExpression proxyExpression = new CodeObjectCreateExpression(propTypeRef, new CodeThisReferenceExpression());
                    if (feature is IAttribute && !IsString(feature.Type))
                    {
                        proxyExpression = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(Observable).ToTypeReference()),
                            "Box",
                            proxyExpression);
                    }
                    ifStmt.TrueStatements.Add(new CodeMethodReturnStatement(proxyExpression));
                    method.Statements.Add(ifStmt);
                }
                return method;
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
                getRelativeUriForChild.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IModelElement).ToTypeReference(), "element"));
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
                    Type = new CodeTypeReference(typeof(IEnumerableExpression<>).Name, typeof(IModelElement).ToTypeReference()),
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

            /// <summary>
            /// Iterates all references of the given class
            /// </summary>
            /// <typeparam name="T">The type of the result</typeparam>
            /// <param name="input">The start class</param>
            /// <param name="typeDeclaration">The type declaration in the context of which the references should be added</param>
            /// <param name="action">the action that should be performed for each reference</param>
            /// <param name="initial">an initial result object</param>
            /// <param name="containmentsOnly">if true, only containment references are considered</param>
            /// <param name="context">The context in which the references should be visited</param>
            /// <returns>the result object after is has been applied for all references</returns>
            protected T AddReferencesOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IReference, CodeMemberProperty, ITransformationContext, T> action, T initial, bool containmentsOnly, ITransformationContext context)
            {
                var r2p = Rule<Reference2Property>();
                foreach (var bcl in input.Closure(cl => cl.BaseTypes))
                {
                    foreach (var reference in bcl.References)
                    {
                        if (!containmentsOnly || reference.IsContainment)
                        {
                            var property = context.Trace.ResolveIn(r2p, reference);
                            if (typeDeclaration.Members.Contains(property))
                            {
                                initial = action(initial, reference, property, context);
                            }
                        }
                    }
                }
                return initial;
            }

            /// <summary>
            /// Iterates all references of the given class
            /// </summary>
            /// <typeparam name="T">The type of the result</typeparam>
            /// <param name="input">The start class</param>
            /// <param name="typeDeclaration">The type declaration in the context of which the references should be added</param>
            /// <param name="action">the action that should be performed for each reference</param>
            /// <param name="initial">an initial result object</param>
            /// <param name="containmentsOnly">if true, only containment references are considered</param>
            /// <param name="context">The context in which the references should be visited</param>
            /// <returns>the result object after is has been applied for all references</returns>
            protected T AddRefinedReferencesOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IReference, CodeMemberProperty, ITransformationContext, T> action, T initial, bool containmentsOnly, ITransformationContext context)
            {
                var r2p = Rule<RefinedReferenceGenerator>();
                foreach (var reference in input.Closure(c => c.BaseTypes)
                    .SelectMany(c => c.References)
                    .Select(r => r.Refines)
                    .Where(r => r != null)
                    .Distinct())
                {
                    if (!containmentsOnly || reference.IsContainment)
                    {
                        var property = context.Trace.ResolveIn(r2p, input, reference);
                        if (typeDeclaration.Members.Contains(property))
                        {
                            initial = action(initial, reference, property, context);
                        }
                    }
                }
                return initial;
            }

            /// <summary>
            /// Iterates all operations of the given class
            /// </summary>
            /// <typeparam name="T">The type of the result</typeparam>
            /// <param name="input">The input class</param>
            /// <param name="typeDeclaration">The class declaration in the context of which the operations are iterated</param>
            /// <param name="action">an action that should be performed for each operation</param>
            /// <param name="initial">the initial result</param>
            /// <param name="context">the context in which the operations are iterated</param>
            /// <returns>the result after all operations have been iterated</returns>
            protected T AddOperationsOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IOperation, ITransformationContext, T> action, T initial, ITransformationContext context)
            {
                var o2m = Rule<Operation2Method>();
                foreach (var bcl in input.Closure(cl => cl.BaseTypes))
                {
                    foreach (var op in bcl.Operations)
                    {
                        var method = context.Trace.ResolveIn(o2m, op);
                        if (typeDeclaration.Members.Contains(method))
                        {
                            initial = action(initial, op, context);
                        }
                    }
                }
                return initial;
            }

            /// <summary>
            /// Iterates the attributes of the given class
            /// </summary>
            /// <typeparam name="T">The result type</typeparam>
            /// <param name="input">The class whose attributes should be iterated</param>
            /// <param name="typeDeclaration">The generated type declaration for the input</param>
            /// <param name="action">The action that should be performed for each attribute</param>
            /// <param name="initial">The initial result</param>
            /// <param name="context">The context in which the attributes should be iterated</param>
            /// <returns>The result after all attributes have been processed</returns>
            protected T AddAttributesOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IAttribute, CodeMemberProperty, ITransformationContext, T> action, T initial, ITransformationContext context)
            {
                var a2p = Rule<Attribute2Property>();
                foreach (var bcl in input.Closure(cl => cl.BaseTypes))
                {
                    foreach (var att in bcl.Attributes)
                    {
                        var property = context.Trace.ResolveIn(a2p, att);
                        if (typeDeclaration.Members.Contains(property))
                        {
                            initial = action(initial, att, property, context);
                        }
                    }
                }
                return initial;
            }

            /// <summary>
            /// Iterates the attributes of the given class
            /// </summary>
            /// <typeparam name="T">The result type</typeparam>
            /// <param name="input">The class whose attributes should be iterated</param>
            /// <param name="typeDeclaration">The generated type declaration for the input</param>
            /// <param name="action">The action that should be performed for each attribute</param>
            /// <param name="initial">The initial result</param>
            /// <param name="context">The context in which the attributes should be iterated</param>
            /// <returns>The result after all attributes have been processed</returns>
            protected T AddRefinedAttributesOfClass<T>(IClass input, CodeTypeDeclaration typeDeclaration, Func<T, IAttribute, CodeMemberProperty, ITransformationContext, T> action, T initial, ITransformationContext context)
            {
                var a2p = Rule<RefinedAttributeGenerator>();
                foreach (var att in input.Closure(cl => cl.BaseTypes)
                    .SelectMany(c => c.Attributes)
                    .Select(a => a.Refines)
                    .Where(a => a != null)
                    .Distinct())
                {
                    var property = context.Trace.ResolveIn(a2p, input, att);
                    if (typeDeclaration.Members.Contains(property))
                    {
                        initial = action(initial, att, property, context);
                    }
                }
                return initial;
            }

            private static CodeMemberMethod AddToGetRelativeUriForChild(CodeMemberMethod method, IReference containment, CodeMemberProperty property, ITransformationContext context)
            {
                if (containment.UpperBound == 1)
                {
                    var ifIdentical = new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("element"),
                            CodeBinaryOperatorType.IdentityEquality,
                            new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property.Name)));

                    ifIdentical.TrueStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).ToTypeReference()), "CreatePath",
                        new CodePrimitiveExpression(containment.Name))));

                    method.Statements.Add(ifIdentical);
                }
                else if (containment.IsOrdered)
                {
                    var idxVarName = containment.Name.ToCamelCase() + "Index";
                    var idxVar = new CodeVariableDeclarationStatement(typeof(int), idxVarName, new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).ToTypeReference()), "IndexOfReference",
                        new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property.Name), new CodeArgumentReferenceExpression("element")));
                    var idxRef = new CodeVariableReferenceExpression(idxVarName);

                    var ifContained = new CodeConditionStatement(new CodeBinaryOperatorExpression(idxRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(-1)));
                    ifContained.TrueStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).ToTypeReference()), "CreatePath",
                        new CodePrimitiveExpression(containment.Name),
                        idxRef)));

                    method.Statements.Add(idxVar);
                    method.Statements.Add(ifContained);
                }
                return method;
            }

            private static CodeMemberMethod AddToGetModelElementForUri(CodeMemberMethod method, IReference containment, CodeMemberProperty property, ITransformationContext context)
            {
                if (containment.UpperBound == 1 || containment.IsOrdered)
                {
                    var propRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property.Name);
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


#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations

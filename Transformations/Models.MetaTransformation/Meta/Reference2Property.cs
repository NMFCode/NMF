using NMF.CodeGen;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Models.Collections;
using NMF.Models.Meta;
using NMF.Serialization;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform a reference to a code property
        /// </summary>
        public class Reference2Property : TransformationRule<IReference, CodeMemberProperty>
        {
            /// <summary>
            /// Registers the dependencies, i.e. transform the reference type and refined references
            /// </summary>
            public override void RegisterDependencies()
            {
                Require(Rule<Reference2Type>(), reference => reference.UpperBound != 1 && reference.Opposite != null,
                    (prop, decl) => CodeDomHelper.DependentTypes(prop, true).Add(decl));

                Require(Rule<RefinedReferenceGenerator>(), r => r.DeclaringType as IClass, r => r.Refines, r => r.Refines != null);

                Call(Rule<Type2Type>(), reference => reference.Type);
            }

            /// <summary>
            /// Initializes the generated property
            /// </summary>
            /// <param name="input">The NMeta reference</param>
            /// <param name="generatedProperty">The generated property</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IReference input, CodeMemberProperty generatedProperty, ITransformationContext context)
            {
                generatedProperty.Name = input.Name.ToPascalCase();
                generatedProperty.Attributes = MemberAttributes.Public;
                var summary = input.Summary;
                if (string.IsNullOrEmpty(summary)) summary = string.Format("The {0} property", input.Name);
                generatedProperty.WriteDocumentation(summary, input.Remarks);

                if (input.IsContainerReference() && input.DeclaringType is IClass && input.DeclaringType.References.Count(r => r.IsContainerReference()) == 1)
                {
                    GenerateOverriddenParentReference(input, generatedProperty, context);
                }
                else
                {
                    GenerateFullProperty(input, generatedProperty, context);
                }

                GenerateSerializationAttributes(input, generatedProperty, context);
            }

            private void GenerateOverriddenParentReference(IReference input, CodeMemberProperty generatedProperty, ITransformationContext context)
            {
                if (input.UpperBound != 1) throw new ArgumentException("The reference {0}.{1} is a container reference but has an upper bound greather than 1.");

                generatedProperty.Type = CreateTypeReference(input, att => generatedProperty.CustomAttributes.Add(att),
                    CreateCollectionInterfaceType, context);

                generatedProperty.HasGet = true;
                generatedProperty.HasSet = true;

                var parentRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Parent");

                generatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).ToTypeReference()), "CastAs", generatedProperty.Type), parentRef)));

                generatedProperty.SetStatements.Add(new CodeAssignStatement(parentRef, new CodePropertySetValueReferenceExpression()));

                GenerateOnParentChangedMethod(input, generatedProperty);
            }

            private void GenerateOnParentChangedMethod(IReference input, CodeMemberProperty property)
            {
                var onParentChanged = new CodeMemberMethod()
                {
                    Name = "OnParentChanged",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(void))
                };
                onParentChanged.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IModelElement).ToTypeReference(), "newParent"));
                onParentChanged.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IModelElement).ToTypeReference(), "oldParent"));

                var appendix = property.Name.ToPascalCase();
                if (appendix == "Parent") appendix += "Reference";
                var oldElementVar = new CodeVariableReferenceExpression("old" + appendix);
                var newElementVar = new CodeVariableReferenceExpression("new" + appendix);

                var castRef = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(ModelHelper).ToTypeReference()), "CastAs", property.Type);
                var nullRef = new CodePrimitiveExpression(null);
                var thisRef = new CodeThisReferenceExpression();

                onParentChanged.Statements.Add(new CodeVariableDeclarationStatement(property.Type, oldElementVar.VariableName,
                    new CodeMethodInvokeExpression(castRef, new CodeArgumentReferenceExpression("oldParent"))));
                onParentChanged.Statements.Add(new CodeVariableDeclarationStatement(property.Type, newElementVar.VariableName,
                    new CodeMethodInvokeExpression(castRef, new CodeArgumentReferenceExpression("newParent"))));

                CodeStatement unsetOld;
                CodeStatement setNew;

                string oppositeName = input.Opposite.Name.ToPascalCase();

                if (input.Opposite.UpperBound == 1)
                {
                    unsetOld = new CodeAssignStatement(new CodePropertyReferenceExpression(oldElementVar, oppositeName), nullRef);
                    setNew = new CodeAssignStatement(new CodePropertyReferenceExpression(newElementVar, oppositeName), thisRef);
                }
                else
                {
                    unsetOld = new CodeExpressionStatement(new CodeMethodInvokeExpression(
                        new CodePropertyReferenceExpression(oldElementVar, oppositeName), "Remove", thisRef));
                    setNew = new CodeExpressionStatement(new CodeMethodInvokeExpression(
                        new CodePropertyReferenceExpression(newElementVar, oppositeName), "Add", thisRef));
                }

                onParentChanged.Statements.Add(new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(oldElementVar, CodeBinaryOperatorType.IdentityInequality, nullRef), unsetOld));
                onParentChanged.Statements.Add(new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(newElementVar, CodeBinaryOperatorType.IdentityInequality, nullRef), setNew));

                var valueChangedEvArgs = typeof(ValueChangedEventArgs).ToTypeReference();
                var valueChangeDef = new CodeVariableDeclarationStatement(valueChangedEvArgs, "e",
                    new CodeObjectCreateExpression(valueChangedEvArgs, oldElementVar, newElementVar));
                var valueChangeRef = new CodeVariableReferenceExpression(valueChangeDef.Name);

                onParentChanged.Statements.Add(valueChangeDef);

                onParentChanged.Statements.Add(property.CreateOnChangedEventPattern(valueChangedEvArgs, valueChangeRef));
                onParentChanged.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "OnPropertyChanged",
                    new CodePrimitiveExpression(property.Name), valueChangeRef));

                onParentChanged.WriteDocumentation("Gets called when the parent model element of the current model element changes",
                    null, new Dictionary<string, string>() {
                        {
                            "oldParent", "The old parent model element"
                        },
                        {
                            "newParent", "The new parent model element"
                        }
                    });

                onParentChanged.SetMerge(other =>
                {
                    var mergedOnParent = new CodeMemberMethod()
                    {
                        Name = "OnParentChanged",
                        Attributes = MemberAttributes.Family | MemberAttributes.Override,
                        ReturnType = new CodeTypeReference(typeof(void))
                    };
                    for (int i = 0; i < onParentChanged.Parameters.Count; i++)
                    {
                        mergedOnParent.Parameters.Add(onParentChanged.Parameters[i]);
                    }
                    var otherCasted = other as CodeMemberMethod;
                    mergedOnParent.Statements.AddRange(otherCasted.Statements);
                    mergedOnParent.Statements.AddRange(onParentChanged.Statements);
                    mergedOnParent.Statements.Remove(valueChangeDef);
                    return mergedOnParent;
                });

                property.DependentMembers(true).Add(onParentChanged);
            }

            private void GenerateFullProperty(IReference property, CodeMemberProperty codeProperty, ITransformationContext context)
            {
                var fieldType = CreateTypeReference(property, att => codeProperty.CustomAttributes.Add(att),
                    CreateCollectionImplementationType, context);
                var fieldRef = codeProperty.CreateBackingField(fieldType, null);

                codeProperty.ImplementGetter(fieldRef);

                if (property.UpperBound == 1)
                {
                    codeProperty.Type = fieldType;
                    GenerateSetStatement(property, codeProperty, fieldRef);
                    GenerateResetMethod(codeProperty);
                }
                else
                {
                    codeProperty.Type = CreateTypeReference(property, null, CreateCollectionInterfaceType, context);
                    codeProperty.MarkCollectionProperty();

                    var newCollection = new CodeObjectCreateExpression(fieldType);
                    if (property.IsContainment || property.Opposite != null) newCollection.Parameters.Add(new CodeThisReferenceExpression());
                    var createEmptyCollection = new CodeAssignStatement(fieldRef, newCollection);
                    var constructorStmts = codeProperty.ImpliedConstructorStatements(true);
                    constructorStmts.Add(createEmptyCollection);
                    constructorStmts.Add(new CodeAttachEventStatement(fieldRef, "CollectionChanging",
                        GenerateCollectionBubbleHandler(codeProperty, "CollectionChanging", typeof(NotifyCollectionChangingEventArgs))));
                    constructorStmts.Add(new CodeAttachEventStatement(fieldRef, "CollectionChanged",
                        GenerateCollectionBubbleHandler(codeProperty, "CollectionChanged", typeof(NotifyCollectionChangedEventArgs))));
                }
            }

            private CodeMethodReferenceExpression GenerateCollectionBubbleHandler(CodeMemberProperty property, string suffix, System.Type eventArgsType)
            {
                var collectionBubbleHandler = new CodeMemberMethod()
                {
                    Name = property.Name + suffix,
                    Attributes = MemberAttributes.Private
                };
                collectionBubbleHandler.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
                collectionBubbleHandler.Parameters.Add(new CodeParameterDeclarationExpression(eventArgsType, "e"));
                collectionBubbleHandler.Statements.Add(new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(), "On" + suffix,
                    new CodePrimitiveExpression(property.Name), new CodeArgumentReferenceExpression("e")));
                collectionBubbleHandler.WriteDocumentation(string.Format("Forwards " + suffix + " notifications for the {0} property to the parent model element", property.Name), null,
                    new Dictionary<string, string>() {
                    { "sender", "The collection that raised the change" },
                    { "e", "The original event data" }});

                property.DependentMembers(true).Add(collectionBubbleHandler);
                return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), collectionBubbleHandler.Name);
            }

            /// <summary>
            /// Generates the set statements of a normal reference
            /// </summary>
            /// <param name="property">The NMeta reference</param>
            /// <param name="codeProperty">The generated code property</param>
            /// <param name="fieldReference">A reference to the backening field</param>
            /// <remarks>Normal means in this case that the reference is not an overridden container reference</remarks>
            protected virtual void GenerateSetStatement(IReference property, CodeMemberProperty codeProperty, CodeExpression fieldReference)
            {
                var ifStmt = new CodeConditionStatement();
                var val = new CodePropertySetValueReferenceExpression();
                ifStmt.Condition = new CodeBinaryOperatorExpression(fieldReference, CodeBinaryOperatorType.IdentityInequality, val);

                var assignment = new CodeAssignStatement(fieldReference, val);

                ifStmt.TrueStatements.Add(codeProperty.CreateOnChangingEventPattern());
                ifStmt.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "OnPropertyChanging",
                        new CodePrimitiveExpression(codeProperty.Name)));

                var oldDef = new CodeVariableDeclarationStatement(codeProperty.Type, "old", fieldReference);
                var oldRef = new CodeVariableReferenceExpression("old");

                ifStmt.TrueStatements.Add(oldDef);

                var targetClass = property.Type;
                if (targetClass != null)
                {
                    var nullRef = new CodePrimitiveExpression(null);
                    var thisRef = new CodeThisReferenceExpression();
                    var oldNotNull = new CodeBinaryOperatorExpression(oldRef, CodeBinaryOperatorType.IdentityInequality, nullRef);
                    var valueNotNull = new CodeBinaryOperatorExpression(val, CodeBinaryOperatorType.IdentityInequality, nullRef);

                    ifStmt.TrueStatements.Add(assignment);

                    var oldCheck = new CodeConditionStatement(oldNotNull);

                    var newCheck = new CodeConditionStatement(valueNotNull);

                    if (property.Opposite != null)
                    {
                        var oppositeName = property.Opposite.Name.ToPascalCase();
                        var oldOpposite = new CodePropertyReferenceExpression(oldRef, oppositeName);
                        var valOpposite = new CodePropertyReferenceExpression(val, oppositeName);

                        if (property.Opposite.UpperBound == 1)
                        {
                            oldCheck.TrueStatements.Add(new CodeAssignStatement(oldOpposite, nullRef));

                            newCheck.TrueStatements.Add(new CodeAssignStatement(valOpposite, thisRef));
                        }
                        else
                        {
                            oldCheck.TrueStatements.Add(new CodeMethodInvokeExpression(oldOpposite, "Remove", thisRef));

                            var addThis = new CodeMethodInvokeExpression(valOpposite, "Add", thisRef);
                            if (property.Opposite.IsUnique)
                            {
                                newCheck.TrueStatements.Add(addThis);
                            }
                            else
                            {
                                var ifNotContains = new CodeConditionStatement(new CodeBinaryOperatorExpression(
                                    new CodeMethodInvokeExpression(valOpposite, "Contains", thisRef), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(true)));

                                ifNotContains.TrueStatements.Add(addThis);
                                newCheck.TrueStatements.Add(ifNotContains);
                            }
                        }

                        if (property.Opposite.IsContainment)
                        {
                            ifStmt.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(thisRef, "Parent"), val));
                        }
                    }

                    if (property.IsContainment)
                    {
                        oldCheck.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(oldRef, "Parent"), nullRef));
                        newCheck.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(val, "Parent"), thisRef));
                    }

                    oldCheck.TrueStatements.Add(new CodeRemoveEventStatement(
                        new CodeEventReferenceExpression(oldRef, "Deleted"),
                        new CodeMethodReferenceExpression(thisRef, "OnReset" + codeProperty.Name)));

                    newCheck.TrueStatements.Add(new CodeAttachEventStatement(
                        new CodeEventReferenceExpression(val, "Deleted"),
                        new CodeMethodReferenceExpression(thisRef, "OnReset" + codeProperty.Name)));

                    ifStmt.TrueStatements.Add(oldCheck);
                    ifStmt.TrueStatements.Add(newCheck);
                }
                else
                {
                    // TODO: Refine?
                    ifStmt.TrueStatements.Add(assignment);
                }

                var valueChangeTypeRef = typeof(ValueChangedEventArgs).ToTypeReference();
                var valueChangeDef = new CodeVariableDeclarationStatement(valueChangeTypeRef, "e",
                    new CodeObjectCreateExpression(typeof(ValueChangedEventArgs).ToTypeReference(), oldRef, val));
                var valueChangeRef = new CodeVariableReferenceExpression(valueChangeDef.Name);

                ifStmt.TrueStatements.Add(valueChangeDef);

                ifStmt.TrueStatements.Add(codeProperty.CreateOnChangedEventPattern(typeof(ValueChangedEventArgs).ToTypeReference(),
                    valueChangeRef));

                ifStmt.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "OnPropertyChanged",
                        new CodePrimitiveExpression(codeProperty.Name), valueChangeRef));

                codeProperty.SetStatements.Add(ifStmt);
                codeProperty.HasSet = true;
            }


            private static void GenerateResetMethod(CodeMemberProperty generatedProperty)
            {
                var resetMember = new CodeMemberMethod()
                {
                    Name = "OnReset" + generatedProperty.Name,
                    Attributes = MemberAttributes.Private,
                    ReturnType = new CodeTypeReference(typeof(void))
                };
                resetMember.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
                resetMember.Parameters.Add(new CodeParameterDeclarationExpression(typeof(EventArgs).ToTypeReference(), "eventArgs"));

                resetMember.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(), generatedProperty.Name), new CodePrimitiveExpression(null)));

                resetMember.WriteDocumentation(string.Format("Handles the event that the {0} property must reset", generatedProperty.Name), null,
                    new Dictionary<string, string>() {
                        { "sender", "The object that sent this reset request"},
                        { "eventArgs", "The event data for the reset event"}
                    });

                generatedProperty.DependentMembers(true).Add(resetMember);
            }

            private void GenerateSerializationAttributes(IReference input, CodeMemberProperty output, ITransformationContext context)
            {
                if (input.Name != output.Name)
                {
                    output.AddAttribute(typeof(XmlElementNameAttribute), input.Name);
                }

                if (input.IsContainment)
                {
                    output.AddAttribute(typeof(XmlAttributeAttribute), false);
                    output.AddAttribute(typeof(ContainmentAttribute));
                }
                else
                {
                    if (input.Opposite != null && input.Opposite.IsContainment)
                    {
                        if (input.UpperBound == 1)
                        {
                            output.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DesignerSerializationVisibilityAttribute).ToTypeReference(), new CodeAttributeArgument(
                                new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DesignerSerializationVisibility).ToTypeReference()), DesignerSerializationVisibility.Hidden.ToString()))));
                        }
                        else
                        {
                            var attDecl = output.CustomAttributes.OfType<CodeAttributeDeclaration>().FirstOrDefault(att => att.AttributeType.BaseType == typeof(DesignerSerializationVisibilityAttribute).Name);
                            if (attDecl == null)
                            {
                                output.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DesignerSerializationVisibilityAttribute).ToTypeReference(), new CodeAttributeArgument(
                                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DesignerSerializationVisibility).ToTypeReference()), DesignerSerializationVisibility.Hidden.ToString()))));
                            }
                            else
                            {
                                attDecl.Arguments[0].Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DesignerSerializationVisibility).ToTypeReference()), DesignerSerializationVisibility.Hidden.ToString());
                            }
                        }
                    }
                    output.AddAttribute(typeof(XmlAttributeAttribute), true);
                }

                if (input.Opposite != null)
                {
                    var class2Type = Rule<Class2Type>();
                    var opposite = input.Opposite;
                    if (opposite.ReferenceType == input.DeclaringType)
                    {
                        output.AddAttribute(typeof(XmlOppositeAttribute), input.Opposite.Name);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                if (input.UpperBound != 1)
                {
                    output.AddAttribute(typeof(ConstantAttribute));
                }
            }


            private CodeTypeReference CreateCollectionImplementationType(ITypedElement arg, CodeTypeReference elementType, ITransformationContext context)
            {
                var reference = arg as IReference;

                if (reference.Opposite != null)
                {
                    var referenceType = context.Trace.ResolveIn(Rule<Reference2Type>(), reference);
                    return referenceType.GetReferenceForType();
                }
                else
                {
                    if (reference.IsUnique)
                    {
                        if (reference.IsOrdered)
                        {
                            return CreateOrderedSet(elementType, reference.IsContainment);
                        }
                        else
                        {
                            return CreateSet(elementType, reference.IsContainment);
                        }
                    }
                    else
                    {
                        if (reference.IsOrdered)
                        {
                            return CreateList(elementType, reference.IsContainment);
                        }
                        else
                        {
                            return CreateBag(elementType, reference.IsContainment);
                        }
                    }
                }
            }

            /// <summary>
            /// Creates the type reference for a bag with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <param name="isContainment">Indicates whether the reference is a containment</param>
            /// <returns>A code reference to the class that implements the collection</returns>
            protected virtual CodeTypeReference CreateBag(CodeTypeReference elementTypeReference, bool isContainment)
            {
                if (isContainment)
                {
                    return new CodeTypeReference(typeof(ObservableCompositionList<>).Name, elementTypeReference);
                }
                else
                {
                    return new CodeTypeReference(typeof(ObservableAssociationList<>).Name, elementTypeReference);
                }
            }

            /// <summary>
            /// Creates the type reference for a list with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <param name="isContainment">Indicates whether the reference is a containment</param>
            /// <returns>A code reference to the class that implements the collection</returns>
            protected virtual CodeTypeReference CreateList(CodeTypeReference elementTypeReference, bool isContainment)
            {
                if (isContainment)
                {
                    return new CodeTypeReference(typeof(ObservableCompositionList<>).Name, elementTypeReference);
                }
                else
                {
                    return new CodeTypeReference(typeof(ObservableAssociationList<>).Name, elementTypeReference);
                }
            }

            /// <summary>
            /// Creates the type reference for an unordered set with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <param name="isContainment">Indicates whether the reference is a containment</param>
            /// <returns>A code type reference to the class that implements the collection</returns>
            protected virtual CodeTypeReference CreateSet(CodeTypeReference elementTypeReference, bool isContainment)
            {
                if (isContainment)
                {
                    return new CodeTypeReference(typeof(ObservableCompositionSet<>).Name, elementTypeReference);
                }
                else
                {
                    return new CodeTypeReference(typeof(ObservableAssociationSet<>).Name, elementTypeReference);
                }
            }

            /// <summary>
            /// Creates the type reference for an ordered set with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <param name="isContainment">Indicates whether the reference is a containment</param>
            /// <returns>A code type reference to the class that implements the collection</returns>
            protected virtual CodeTypeReference CreateOrderedSet(CodeTypeReference elementTypeReference, bool isContainment)
            {
                if (isContainment)
                {
                    return new CodeTypeReference(typeof(ObservableCompositionOrderedSet<>).Name, elementTypeReference);
                }
                else
                {
                    return new CodeTypeReference(typeof(ObservableAssociationOrderedSet<>).Name, elementTypeReference);
                }
            }

            private CodeTypeReference CreateCollectionInterfaceType(ITypedElement arg, CodeTypeReference elementType, ITransformationContext context)
            {
                var feature = arg as IReference;
                if (feature.IsUnique)
                {
                    if (feature.IsOrdered)
                    {
                        return CreateOrderedSetInterfaceType(elementType);
                    }
                    else
                    {
                        return CreateSetInterfaceType(elementType);
                    }
                }
                else
                {
                    if (feature.IsOrdered)
                    {
                        return CreateListInterfaceType(elementType);
                    }
                    else
                    {
                        return CreateBagInterfaceType(elementType);
                    }
                }
            }

            /// <summary>
            /// Creates the interface type for a list with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <returns>A code type reference that represents a list with the given elements</returns>
            protected virtual CodeTypeReference CreateListInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(IListExpression<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the interface type for an unordered set with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <returns>A code type reference that represents an unordered set with the given elements</returns>
            protected virtual CodeTypeReference CreateSetInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ISetExpression<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the interface type for an ordered set with the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element tye reference</param>
            /// <returns>A code type reference that represents an ordered set</returns>
            protected virtual CodeTypeReference CreateOrderedSetInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(IOrderedSetExpression<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the interface type for a bag with the given element type
            /// </summary>
            /// <param name="elementTypeReference"></param>
            /// <returns></returns>
            protected virtual CodeTypeReference CreateBagInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ICollectionExpression<>).Name, elementTypeReference);
            }
        }
    }
}

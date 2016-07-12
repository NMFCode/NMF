using NMF.CodeGen;
using NMF.Expressions;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule that generates the collection class for a refined reference
        /// </summary>
        public class RefinedReferenceCollectionClassGenerator : TransformationRule<IClass, IReference, CodeTypeDeclaration>
        {
            /// <summary>
            /// Creates the uninitialized output type declaration
            /// </summary>
            /// <param name="scope">The scope in which the reference is refined</param>
            /// <param name="reference">The refined reference</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The newly created code type declaration</returns>
            public override CodeTypeDeclaration CreateOutput(IClass scope, IReference reference, ITransformationContext context)
            {
                return CodeDomHelper.CreateTypeDeclarationWithReference(scope.Name.ToPascalCase() + reference.Name.ToPascalCase() + "Collection");
            }

            private CodeFieldReferenceExpression parentRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_parent");

            /// <summary>
            /// Initializes the created type declaration
            /// </summary>
            /// <param name="scope">The scope in which the reference is refined</param>
            /// <param name="reference">The refined reference</param>
            /// <param name="generatedType">The generated type declaration</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IClass scope, IReference reference, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var scopeDeclaration = context.Trace.ResolveIn(Rule<Class2Type>(), scope);
                var elementType = CreateReference(reference.Type, true, context);
                var parent = new CodeMemberField(scopeDeclaration.GetReferenceForType(), "_parent");
                parent.Attributes = MemberAttributes.Private;
                generatedType.Members.Add(parent);

                generatedType.WriteDocumentation(string.Format("The collection class to implement the refined {0} reference for the {1} class", reference.Name, scope.Name));

                var implementingReferences = scope.References.Where(r => r.Refines == reference).ToList();
                var constraint = scope.ReferenceConstraints.FirstOrDefault(r => r.Constrains == reference);

                var constructor = new CodeConstructor();
                constructor.Attributes = MemberAttributes.Public;
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(scopeDeclaration.GetReferenceForType(), "parent"));
                var constrParentRef = new CodeArgumentReferenceExpression("parent");
                constructor.Statements.Add(new CodeAssignStatement(parentRef, constrParentRef));

                ImplementNotifications(generatedType, implementingReferences, constructor, constrParentRef);

                constructor.WriteDocumentation("Creates a new instance");

                CodeExpression standardValuesRef = null;

                if (constraint != null)
                {
                    var metaRepository = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(MetaRepository)), "Instance");
                    CodeExpression[] initializers = new CodeExpression[constraint.References.Count];
                    for (int i = 0; i < constraint.References.Count; i++)
                    {
                        var uri = constraint.References[i].AbsoluteUri;
                        if (uri == null) throw new InvalidOperationException("Model elements used in a reference constraint must have a valid absolute URI.");
                        var element = new CodeMethodInvokeExpression(metaRepository, "Resolve", new CodePrimitiveExpression(uri.AbsoluteUri));
                        initializers[i] = new CodeCastExpression(elementType, element);
                    }
                    var arrayType = new CodeTypeReference(elementType, 1);
                    var standardValues = new CodeMemberField(arrayType, "_standardValues");
                    standardValues.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                    standardValues.InitExpression = new CodeArrayCreateExpression(arrayType, initializers);
                    standardValuesRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(generatedType.GetReferenceForType()), standardValues.Name);
                    generatedType.Members.Add(standardValues);
                }

                generatedType.Members.Add(constructor);
                ImplementCollection(generatedType, elementType, standardValuesRef, reference, implementingReferences, constraint, context);

                if (implementingReferences.All(att => att.IsOrdered || att.UpperBound == 1) && reference.IsOrdered)
                {
                    ImplementList(generatedType, elementType, standardValuesRef, reference, implementingReferences, constraint, context);
                }

                ImplementNotifiable(generatedType, elementType);
            }

            protected virtual void ImplementNotifications(CodeTypeDeclaration generatedType, List<IReference> implementingAttributes, CodeConstructor constructor, CodeArgumentReferenceExpression constrParentRef)
            {
                generatedType.Members.Add(GenerateCollectionChangedEvent());
                generatedType.Members.Add(GenerateOnCollectionChangedMethod());
                CodeMemberMethod single = null;
                CodeMemberMethod multi = null;
                foreach (var att in implementingAttributes)
                {
                    if (att.UpperBound == 1)
                    {
                        if (single == null)
                        {
                            single = GenerateSingleValueChangedHandler();
                        }
                        constructor.Statements.Add(new CodeAttachEventStatement(constrParentRef, att.Name.ToPascalCase() + "Changed",
                            new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), single.Name)));
                    }
                    else
                    {
                        if (multi == null)
                        {
                            multi = GenerateMultiValueChangedHandler();
                        }
                        constructor.Statements.Add(new CodeAttachEventStatement(
                            new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(constrParentRef, att.Name.ToPascalCase()), "AsNotifiable"), "CollectionChanged",
                            new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), multi.Name)));
                    }
                }
                if (single != null) generatedType.Members.Add(single);
                if (multi != null) generatedType.Members.Add(multi);
            }

            private CodeMemberMethod GenerateSingleValueChangedHandler()
            {
                var handler = new CodeMemberMethod()
                {
                    Name = "HandleValueChange",
                    Attributes = MemberAttributes.Private
                };
                handler.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
                handler.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ValueChangedEventArgs), "eventArgs"));

                var argsRef = new CodeArgumentReferenceExpression("eventArgs");
                var eDef = new CodeVariableDeclarationStatement(typeof(NotifyCollectionChangedEventArgs), "collectionEvent");
                handler.Statements.Add(eDef);
                var eRef = new CodeVariableReferenceExpression(eDef.Name);

                var nullRef = new CodePrimitiveExpression(null);
                var oldRef = new CodePropertyReferenceExpression(argsRef, "OldValue");
                var newRef = new CodePropertyReferenceExpression(argsRef, "NewValue");

                var mainIf = new CodeConditionStatement(new CodeBinaryOperatorExpression(newRef, CodeBinaryOperatorType.IdentityInequality, nullRef));
                var innerIf = new CodeConditionStatement(new CodeBinaryOperatorExpression(oldRef, CodeBinaryOperatorType.IdentityInequality, nullRef));
                innerIf.TrueStatements.Add(new CodeAssignStatement(eRef, new CodeObjectCreateExpression(typeof(NotifyCollectionChangedEventArgs),
                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(NotifyCollectionChangedAction)), NotifyCollectionChangedAction.Replace.ToString()),
                    newRef, oldRef)));
                innerIf.FalseStatements.Add(new CodeAssignStatement(eRef, new CodeObjectCreateExpression(typeof(NotifyCollectionChangedEventArgs),
                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(NotifyCollectionChangedAction)), NotifyCollectionChangedAction.Add.ToString()),
                    newRef)));
                mainIf.TrueStatements.Add(innerIf);
                mainIf.FalseStatements.Add(new CodeAssignStatement(eRef, new CodeObjectCreateExpression(typeof(NotifyCollectionChangedEventArgs),
                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(NotifyCollectionChangedAction)), NotifyCollectionChangedAction.Remove.ToString()),
                    oldRef)));
                handler.Statements.Add(mainIf);

                handler.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "OnCollectionChanged", eRef));

                return handler;
            }

            private CodeMemberMethod GenerateMultiValueChangedHandler()
            {
                var handler = new CodeMemberMethod()
                {
                    Name = "HandleCollectionChange",
                    Attributes = MemberAttributes.Private
                };
                handler.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
                handler.Parameters.Add(new CodeParameterDeclarationExpression(typeof(NotifyCollectionChangedEventArgs), "eventArgs"));

                handler.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "OnCollectionChanged", new CodeArgumentReferenceExpression("eventArgs")));

                return handler;
            }

            private CodeMemberEvent GenerateCollectionChangedEvent()
            {
                var collectionChangedEvent = new CodeMemberEvent()
                {
                    Name = "CollectionChanged",
                    Type = typeof(NotifyCollectionChangedEventHandler).ToTypeReference(),
                    Attributes = MemberAttributes.Public
                };
                collectionChangedEvent.WriteDocumentation("Gets fired when the contents of this collection changes");
                return collectionChangedEvent;
            }

            private CodeMemberMethod GenerateOnCollectionChangedMethod()
            {
                var onCollectionChanged = new CodeMemberMethod()
                {
                    Name = "OnCollectionChanged",
                    Attributes = MemberAttributes.Family
                };
                onCollectionChanged.Parameters.Add(new CodeParameterDeclarationExpression(typeof(NotifyCollectionChangedEventArgs), "eventArgs"));
                var handler = new CodeVariableDeclarationStatement(typeof(NotifyCollectionChangedEventHandler), "handler", new CodeEventReferenceExpression(new CodeThisReferenceExpression(), "CollectionChanged"));
                var handlerRef = new CodeVariableReferenceExpression(handler.Name);
                onCollectionChanged.Statements.Add(handler);
                onCollectionChanged.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(handlerRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
                    new CodeExpressionStatement(new CodeMethodInvokeExpression(handlerRef, "Invoke", new CodeThisReferenceExpression(), new CodeArgumentReferenceExpression("eventArgs")))));
                onCollectionChanged.WriteDocumentation("Fires the CollectionChanged event");
                return onCollectionChanged;
            }

            /// <summary>
            /// Implements the IList interface
            /// </summary>
            /// <param name="generatedType">The generated code type declaration</param>
            /// <param name="elementType">The element tyoe reference</param>
            /// <param name="implementingReferences">The attributes implementing the collection</param>
            protected virtual void ImplementList(CodeTypeDeclaration generatedType, CodeTypeReference elementType, CodeExpression standardValuesRef, IReference original, List<IReference> implementingReferences, IReferenceConstraint constraint, ITransformationContext context)
            {
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(IListExpression<>).Name, elementType));
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(IList<>).Name, elementType));
                generatedType.Members.Add(GenerateIndexOf(implementingReferences, constraint, elementType, standardValuesRef));
                generatedType.Members.Add(GenerateInsert(original, implementingReferences, constraint, elementType, context));
                generatedType.Members.Add(GenerateRemoveAt(implementingReferences, constraint, elementType));
                generatedType.Members.Add(GenerateIndexer(original, implementingReferences, constraint, elementType, standardValuesRef, context));
            }

            /// <summary>
            /// Implements the ICollection interface
            /// </summary>
            /// <param name="generatedType">The generated code type declaration</param>
            /// <param name="elementType">The element type reference</param>
            /// <param name="implementingReferences">The attributes implementing the collection</param>
            protected virtual void ImplementCollection(CodeTypeDeclaration generatedType, CodeTypeReference elementType, CodeExpression standardValuesRef, IReference original, List<IReference> implementingReferences, IReferenceConstraint constraint, ITransformationContext context)
            {
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(ICollectionExpression<>).Name, elementType));
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(ICollection<>).Name, elementType));
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(INotifyCollection<>).Name, elementType));
                generatedType.Members.Add(GenerateAdd(original, implementingReferences, elementType, context));
                generatedType.Members.Add(GenerateClear(implementingReferences));
                generatedType.Members.Add(GenerateContains(implementingReferences, constraint, elementType, standardValuesRef));
                generatedType.Members.Add(GenerateCopyTo(original, implementingReferences, constraint, elementType, standardValuesRef));
                generatedType.Members.Add(GenerateCount(implementingReferences, constraint));
                generatedType.Members.Add(GenerateIsReadOnly(implementingReferences.Count == 0));
                generatedType.Members.Add(GenerateRemove(original, implementingReferences, elementType, context));
                generatedType.Members.Add(GenerateGenericGetEnumerator(implementingReferences, constraint, elementType, standardValuesRef));
                generatedType.Members.Add(GenerateObjectGetEnumerator());
            }

            /// <summary>
            /// Implement the IEnumerableExpression interface
            /// </summary>
            /// <param name="generatedType">The generated code type declaration</param>
            /// <param name="elementType">The element type reference</param>
            protected virtual void ImplementNotifiable(CodeTypeDeclaration generatedType, CodeTypeReference elementType)
            {
                generatedType.Members.Add(GenerateIsAttached());
                generatedType.Members.Add(GenerateCollectionAsNotifiableMethod(elementType));
                generatedType.Members.Add(GenerateEnumerableAsNotifiableMethod(elementType));
                generatedType.Members.Add(GenerateObjectAsNotifiableMethod());
                generatedType.Members.Add(GenerateAttachMethod());
                generatedType.Members.Add(GenerateDetachMethod());
            }

            private CodeMemberMethod GenerateAdd(IReference original, IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, ITransformationContext context)
            {
                var add = new CodeMemberMethod()
                {
                    Name = "Add",
                    Attributes = MemberAttributes.Public,
                    ReturnType = null
                };
                add.Parameters.Add(new CodeParameterDeclarationExpression(elementType, "item"));
                var itemRef = new CodeArgumentReferenceExpression("item");
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    var propertyTypeRef = CreateReference(reference.Type, true, context);
                    if (reference.UpperBound == 1)
                    {
                        var ifNull = new CodeConditionStatement();
                        ifNull.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
                        if (reference.Type != original.Type)
                        {
                            var variableName = reference.Name.ToCamelCase() + "Casted";
                            ifNull.TrueStatements.Add(new CodeVariableDeclarationStatement(propertyTypeRef, variableName, new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(itemRef, "As", propertyTypeRef))));
                            var varRef = new CodeVariableReferenceExpression(variableName);
                            var typeMatchIf = new CodeConditionStatement();
                            typeMatchIf.Condition = new CodeBinaryOperatorExpression(varRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                            typeMatchIf.TrueStatements.Add(new CodeAssignStatement(propertyRef, varRef));
                            typeMatchIf.TrueStatements.Add(new CodeMethodReturnStatement());
                            ifNull.TrueStatements.Add(typeMatchIf);
                        }
                        else
                        {
                            ifNull.TrueStatements.Add(new CodeAssignStatement(propertyRef, itemRef));
                            ifNull.TrueStatements.Add(new CodeMethodReturnStatement());
                        }
                        add.Statements.Add(ifNull);
                    }
                    else
                    {
                        if (reference.Type != original.Type)
                        {
                            var variableName = reference.Name.ToCamelCase() + "Casted";
                            add.Statements.Add(new CodeVariableDeclarationStatement(propertyTypeRef, variableName, new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(itemRef, "As", propertyTypeRef))));
                            var varRef = new CodeVariableReferenceExpression(variableName);
                            var typeMatchIf = new CodeConditionStatement();
                            typeMatchIf.Condition = new CodeBinaryOperatorExpression(varRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                            if (reference.UpperBound == -1)
                            {
                                typeMatchIf.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "Add", varRef));
                            }
                            else
                            {
                                var ifNotFull = new CodeConditionStatement();
                                ifNotFull.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(propertyRef, "Count"), CodeBinaryOperatorType.LessThan, new CodePrimitiveExpression(reference.UpperBound));
                                ifNotFull.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "Add", itemRef));
                                typeMatchIf.TrueStatements.Add(ifNotFull);
                            }
                            add.Statements.Add(typeMatchIf);
                        }
                        else
                        {
                            if (reference.UpperBound == -1)
                            {
                                add.Statements.Add(new CodeMethodInvokeExpression(propertyRef, "Add", itemRef));
                                break;
                            }
                            else
                            {
                                var ifNotFull = new CodeConditionStatement();
                                ifNotFull.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(propertyRef, "Count"), CodeBinaryOperatorType.LessThan, new CodePrimitiveExpression(reference.UpperBound));
                                ifNotFull.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "Add", itemRef));
                                add.Statements.Add(ifNotFull);
                            }
                        }
                    }
                }
                add.WriteDocumentation("Adds the given element to the collection", null, new Dictionary<string, string>() { { "item", "The item to add" } });
                return add;
            }

            private CodeMemberMethod GenerateClear(IEnumerable<IReference> implementingReferences)
            {
                var clear = new CodeMemberMethod()
                {
                    Name = "Clear",
                    Attributes = MemberAttributes.Public,
                    ReturnType = null
                };
                foreach (var reference in implementingReferences)
                {
                    var refRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        clear.Statements.Add(new CodeAssignStatement(refRef, new CodePrimitiveExpression(null)));
                    }
                    else
                    {
                        clear.Statements.Add(new CodeMethodInvokeExpression(refRef, "Clear"));
                    }
                }
                clear.WriteDocumentation("Clears the collection and resets all references that implement it.");
                return clear;
            }

            private CodeExpression GetPropertyReference(IReference reference)
            {
                return new CodePropertyReferenceExpression(parentRef, reference.Name.ToPascalCase());
            }

            private CodeMemberMethod GenerateContains(IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var contains = new CodeMemberMethod()
                {
                    Name = "Contains",
                    Attributes = MemberAttributes.Public,
                    ReturnType = new CodeTypeReference(typeof(bool))
                };
                contains.Parameters.Add(new CodeParameterDeclarationExpression(elementType, "item"));
                var itemRef = new CodeArgumentReferenceExpression("item");
                if (constraint != null)
                {
                    var constraintIf = new CodeConditionStatement();
                    constraintIf.Condition = new CodeMethodInvokeExpression(standardValuesRef, "Contains", itemRef);
                    constraintIf.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                    contains.Statements.Add(constraintIf);
                }
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    var refIf = new CodeConditionStatement();
                    if (reference.UpperBound == 1)
                    {
                        refIf.Condition = new CodeBinaryOperatorExpression(itemRef, CodeBinaryOperatorType.IdentityEquality, propertyRef);
                    }
                    else
                    {
                        refIf.Condition = new CodeMethodInvokeExpression(propertyRef, "Contains", itemRef);
                    }
                    refIf.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                    contains.Statements.Add(refIf);
                }
                contains.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
                contains.WriteDocumentation("Gets a value indicating whether the given element is contained in the collection", "True, if it is contained, otherwise False",
                    new Dictionary<string, string>() { { "item", "The item that should be looked out for" } });
                return contains;
            }

            private CodeMemberMethod GenerateCopyTo(IReference original, IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var copyTo = new CodeMemberMethod()
                {
                    Name = "CopyTo",
                    Attributes = MemberAttributes.Public,
                    ReturnType = null
                };
                copyTo.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(elementType, 1), "array"));
                copyTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "arrayIndex"));
                var arrayRef = new CodeArgumentReferenceExpression("array");
                var arrayIndexRef = new CodeArgumentReferenceExpression("arrayIndex");
                if (constraint != null)
                {
                    copyTo.Statements.Add(new CodeMethodInvokeExpression(standardValuesRef, "CopyTo", arrayRef, arrayIndexRef));
                    copyTo.Statements.Add(new CodeAssignStatement(arrayIndexRef, new CodeBinaryOperatorExpression(arrayIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(constraint.References.Count))));
                }
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        var ifNull = new CodeConditionStatement();
                        ifNull.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                        ifNull.TrueStatements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(arrayRef, arrayIndexRef), propertyRef));
                        ifNull.TrueStatements.Add(new CodeAssignStatement(arrayIndexRef, new CodeBinaryOperatorExpression(arrayIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                        copyTo.Statements.Add(ifNull);
                    }
                    else
                    {
                        if (reference.Type == original.Type)
                        {
                            copyTo.Statements.Add(new CodeMethodInvokeExpression(propertyRef, "CopyTo", arrayRef, arrayIndexRef));
                            copyTo.Statements.Add(new CodeAssignStatement(arrayIndexRef, new CodeBinaryOperatorExpression(arrayIndexRef, CodeBinaryOperatorType.Add, new CodePropertyReferenceExpression(propertyRef, "Count"))));
                        }
                        else
                        {
                            var usingEnumerator = new CodeTryCatchFinallyStatement();
                            var enumeratorDeclaration = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(IEnumerator<>).Name, elementType), reference.Name.ToCamelCase() + "Enumerator");
                            enumeratorDeclaration.InitExpression = new CodeMethodInvokeExpression(propertyRef, "GetEnumerator");
                            var enumeratorRef = new CodeVariableReferenceExpression(enumeratorDeclaration.Name);
                            var whileElements = new CodeIterationStatement();
                            whileElements.InitStatement = new CodeSnippetStatement("");
                            whileElements.IncrementStatement = new CodeSnippetStatement("");
                            whileElements.TestExpression = new CodeMethodInvokeExpression(enumeratorRef, "MoveNext");
                            whileElements.Statements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(arrayRef, arrayIndexRef), new CodePropertyReferenceExpression(enumeratorRef, "Current")));
                            whileElements.Statements.Add(new CodeAssignStatement(arrayIndexRef, new CodeBinaryOperatorExpression(arrayIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                            usingEnumerator.TryStatements.Add(whileElements);
                            usingEnumerator.FinallyStatements.Add(new CodeMethodInvokeExpression(enumeratorRef, "Dispose"));
                            copyTo.Statements.Add(enumeratorDeclaration);
                            copyTo.Statements.Add(usingEnumerator);
                        }
                    }
                }

                copyTo.WriteDocumentation("Copies the contents of the collection to the given array starting from the given array index", null,
                    new Dictionary<string, string>() { { "array", "The array in which the elements should be copied" }, { "arrayIndex", "The starting index" } });
                return copyTo;
            }

            private CodeMemberProperty GenerateCount(IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint)
            {
                var count = new CodeMemberProperty()
                {
                    Name = "Count",
                    Attributes = MemberAttributes.Public,
                    Type = new CodeTypeReference(typeof(int)),
                    HasGet = true,
                    HasSet = false
                };
                count.GetStatements.Add(new CodeVariableDeclarationStatement(typeof(int), "count", new CodePrimitiveExpression(constraint != null ? constraint.References.Count : 0)));
                var countRef = new CodeVariableReferenceExpression("count");
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        var ifNull = new CodeConditionStatement();
                        ifNull.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                        ifNull.TrueStatements.Add(new CodeAssignStatement(countRef, new CodeBinaryOperatorExpression(countRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                        count.GetStatements.Add(ifNull);
                    }
                    else
                    {
                        var propertyCount = new CodePropertyReferenceExpression(propertyRef, "Count");
                        count.GetStatements.Add(new CodeAssignStatement(countRef, new CodeBinaryOperatorExpression(countRef, CodeBinaryOperatorType.Add, propertyCount)));
                    }
                }
                count.GetStatements.Add(new CodeMethodReturnStatement(countRef));
                count.WriteDocumentation("Gets the amount of elements contained in this collection");
                return count;
            }

            private CodeMemberProperty GenerateIsReadOnly(bool isReadOnly)
            {
                var isReadOnlyProperty = new CodeMemberProperty()
                {
                    Name = "IsReadOnly",
                    Attributes = MemberAttributes.Public,
                    Type = new CodeTypeReference(typeof(bool)),
                    HasGet = true,
                    HasSet = false
                };
                isReadOnlyProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(isReadOnly)));
                isReadOnlyProperty.WriteDocumentation("Gets a value indicating that the collection is not read-only");
                return isReadOnlyProperty;
            }

            private CodeMemberMethod GenerateRemove(IReference original, IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, ITransformationContext context)
            {
                var remove = new CodeMemberMethod()
                {
                    Name = "Remove",
                    Attributes = MemberAttributes.Public,
                    ReturnType = new CodeTypeReference(typeof(bool))
                };
                remove.Parameters.Add(new CodeParameterDeclarationExpression(elementType, "item"));
                var itemRef = new CodeArgumentReferenceExpression("item");
                var typeDict = new Dictionary<IType, CodeExpression>();
                foreach (var reference in implementingReferences)
                {
                    var refIf = new CodeConditionStatement();
                    var propertyRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        refIf.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityEquality, itemRef);
                        refIf.TrueStatements.Add(new CodeAssignStatement(propertyRef, new CodePrimitiveExpression(null)));
                        refIf.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                    }
                    else
                    {
                        if (reference.Type == original.Type)
                        {
                            refIf.Condition = new CodeMethodInvokeExpression(propertyRef, "Remove", itemRef);
                        }
                        else
                        {
                            if (reference.Type == null) throw new InvalidOperationException(string.Format("Implementing reference {0} must have a type at this point!", reference.ToString()));
                            CodeExpression castedItemRef;
                            if (!typeDict.TryGetValue(reference.Type, out castedItemRef))
                            {
                                var typeRef = CreateReference(reference.Type, true, context);
                                var castedItem = new CodeVariableDeclarationStatement(typeRef, reference.Type.Name.ToCamelCase() + "Item");
                                castedItem.InitExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(itemRef, "As", typeRef));
                                remove.Statements.Add(castedItem);
                                castedItemRef = new CodeVariableReferenceExpression(castedItem.Name);
                                typeDict.Add(reference.Type, castedItemRef);
                            }
                            refIf.Condition = new CodeBinaryOperatorExpression(
                                new CodeBinaryOperatorExpression(castedItemRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
                                CodeBinaryOperatorType.BooleanAnd,
                                new CodeMethodInvokeExpression(propertyRef, "Remove", castedItemRef));
                        }
                        refIf.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                    }
                    remove.Statements.Add(refIf);
                }
                remove.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
                remove.WriteDocumentation("Removes the given item from the collection", "True, if the item was removed, otherwise False",
                    new Dictionary<string, string>() {
                        { "item", "The item that should be removed"}
                    });

                return remove;
            }

            private CodeMemberMethod GenerateGenericGetEnumerator(IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var getEnumerator = new CodeMemberMethod()
                {
                    Name = "GetEnumerator",
                    Attributes = MemberAttributes.Public,
                    ReturnType = new CodeTypeReference(typeof(IEnumerator<>).Name, elementType)
                };
                CodeExpression currentExpression;
                if (constraint != null)
                {
                    currentExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(standardValuesRef, "Cast", elementType));
                }
                else
                {
                    currentExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(Enumerable).ToTypeReference()), "Empty", elementType));
                }
                foreach (var reference in implementingReferences)
                {
                    currentExpression = new CodeMethodInvokeExpression(currentExpression, "Concat", GetPropertyReference(reference));
                }
                currentExpression = new CodeMethodInvokeExpression(currentExpression, "GetEnumerator");
                getEnumerator.Statements.Add(new CodeMethodReturnStatement(currentExpression));
                getEnumerator.WriteDocumentation("Gets an enumerator that enumerates the collection", "A generic enumerator");
                return getEnumerator;
            }

            private CodeMemberMethod GenerateObjectGetEnumerator()
            {
                var getEnumerator = new CodeMemberMethod()
                {
                    Attributes = MemberAttributes.Private,
                    PrivateImplementationType = new CodeTypeReference(typeof(System.Collections.IEnumerable).Name),
                    ReturnType = new CodeTypeReference(typeof(System.Collections.IEnumerator).Name),
                    Name = "GetEnumerator"
                };
                getEnumerator.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetEnumerator")));
                return getEnumerator;
            }

            private CodeMemberMethod GenerateIndexOf(IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var indexOf = new CodeMemberMethod()
                {
                    Name = "IndexOf",
                    Attributes = MemberAttributes.Public,
                    ReturnType = new CodeTypeReference(typeof(int))
                };
                indexOf.Parameters.Add(new CodeParameterDeclarationExpression(elementType, "item"));
                var itemRef = new CodeArgumentReferenceExpression("item");
                int runningIndexInit = 0;
                if (constraint != null && constraint.References.Count > 0)
                {
                    var iterateStandards = new CodeIterationStatement();
                    iterateStandards.InitStatement = new CodeVariableDeclarationStatement(typeof(int), "i", new CodePrimitiveExpression(0));
                    var iRef = new CodeVariableReferenceExpression("i");
                    iterateStandards.TestExpression = new CodeBinaryOperatorExpression(iRef, CodeBinaryOperatorType.LessThan, new CodePrimitiveExpression(constraint.References.Count));
                    iterateStandards.IncrementStatement = new CodeAssignStatement(iRef, new CodeBinaryOperatorExpression(iRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
                    iterateStandards.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeArrayIndexerExpression(standardValuesRef, iRef), CodeBinaryOperatorType.IdentityEquality, itemRef),
                        new CodeMethodReturnStatement(iRef)));
                    indexOf.Statements.Add(iterateStandards);
                    runningIndexInit = constraint.References.Count;
                }
                indexOf.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "runningIndex", new CodePrimitiveExpression(runningIndexInit)));
                indexOf.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "index"));
                var runningIndexRef = new CodeVariableReferenceExpression("runningIndex");
                var indexRef = new CodeVariableReferenceExpression("index");
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        var refIf = new CodeConditionStatement();
                        refIf.Condition = new CodeBinaryOperatorExpression(itemRef, CodeBinaryOperatorType.IdentityEquality, propertyRef);
                        refIf.TrueStatements.Add(new CodeMethodReturnStatement(runningIndexRef));
                        refIf.FalseStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
                            new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)))));
                        indexOf.Statements.Add(refIf);
                    }
                    else
                    {
                        indexOf.Statements.Add(new CodeAssignStatement(indexRef, new CodeMethodInvokeExpression(propertyRef, "IndexOf", itemRef)));
                        var found = new CodeConditionStatement();
                        found.Condition = new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(-1));
                        found.TrueStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePropertyReferenceExpression(propertyRef, "Count"))));
                        found.FalseStatements.Add(new CodeMethodReturnStatement(new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.Add, runningIndexRef)));
                        indexOf.Statements.Add(found);
                    }
                }
                indexOf.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(-1)));
                indexOf.WriteDocumentation("Gets the index of the given element", "The index of the given element or -1 if it was not found",
                    new Dictionary<string, string>() { { "item", "The item that should be looked for" } });
                return indexOf;
            }

            private CodeMemberMethod GenerateInsert(IReference original, IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType, ITransformationContext context)
            {
                var insert = new CodeMemberMethod()
                {
                    Name = "Insert",
                    Attributes = MemberAttributes.Public,
                    ReturnType = null
                };
                insert.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
                insert.Parameters.Add(new CodeParameterDeclarationExpression(elementType, "item"));
                insert.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "runningIndex", new CodePrimitiveExpression(constraint != null ? constraint.References.Count : 0)));
                var runningIndexRef = new CodeVariableReferenceExpression("runningIndex");
                var indexRef = new CodeArgumentReferenceExpression("index");
                var itemRef = new CodeArgumentReferenceExpression("item");
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        var ifNotNull = new CodeConditionStatement();
                        ifNotNull.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                        ifNotNull.TrueStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.ValueEquality, indexRef),
                            new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(NotSupportedException)))));
                        ifNotNull.TrueStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                        insert.Statements.Add(ifNotNull);
                    }
                    else
                    {
                        var ifInRange = new CodeConditionStatement();
                        var indexDiff = new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.Subtract, runningIndexRef);
                        ifInRange.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(propertyRef, "Count"), CodeBinaryOperatorType.LessThanOrEqual, indexDiff);
                        if (original.Type == reference.Type)
                        {
                            ifInRange.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "Insert", indexDiff, itemRef));
                        }
                        else
                        {
                            ifInRange.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "Insert", indexDiff, new CodeCastExpression(CreateReference(reference.Type, true, context), itemRef)));
                        }
                        ifInRange.TrueStatements.Add(new CodeMethodReturnStatement());
                        ifInRange.FalseStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePropertyReferenceExpression(propertyRef, "Count"))));
                        insert.Statements.Add(ifInRange);
                    }
                }
                insert.ThrowException<ArgumentOutOfRangeException>("index");
                insert.WriteDocumentation("Inserts the given item at the given index of the collection", null,
                    new Dictionary<string, string>() {
                        { "index", "The index where to add the item" },
                        { "item", "The item that should be added" }
                    });
                return insert;
            }

            private CodeMemberMethod GenerateRemoveAt(IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType)
            {
                var removeAt = new CodeMemberMethod()
                {
                    Name = "RemoveAt",
                    Attributes = MemberAttributes.Public,
                    ReturnType = null
                };
                removeAt.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
                removeAt.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "runningIndex", new CodePrimitiveExpression(constraint != null ? constraint.References.Count : 0)));
                var runningIndexRef = new CodeVariableReferenceExpression("runningIndex");
                var indexRef = new CodeArgumentReferenceExpression("index");
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    if (reference.UpperBound == 1)
                    {
                        var ifNotNull = new CodeConditionStatement();
                        ifNotNull.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                        ifNotNull.TrueStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.ValueEquality, indexRef),
                            new CodeAssignStatement(propertyRef, new CodePrimitiveExpression(null)),
                            new CodeMethodReturnStatement()));
                        ifNotNull.TrueStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                        removeAt.Statements.Add(ifNotNull);
                    }
                    else
                    {
                        var ifInRange = new CodeConditionStatement();
                        var indexDiff = new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.Subtract, runningIndexRef);
                        ifInRange.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(propertyRef, "Count"), CodeBinaryOperatorType.LessThanOrEqual, indexDiff);
                        ifInRange.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "RemoveAt", indexDiff));
                        ifInRange.TrueStatements.Add(new CodeMethodReturnStatement());
                        ifInRange.FalseStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePropertyReferenceExpression(propertyRef, "Count"))));
                        removeAt.Statements.Add(ifInRange);
                    }
                }
                removeAt.ThrowException<ArgumentOutOfRangeException>("index");
                removeAt.WriteDocumentation("Removes the item at the given position", null,
                    new Dictionary<string, string>() {
                        { "index", "The index where to remove the item"}
                    });
                return removeAt;
            }

            private CodeMemberProperty GenerateIndexer(IReference original, IEnumerable<IReference> implementingReferences, IReferenceConstraint constraint, CodeTypeReference elementType, CodeExpression standardValuesRef, ITransformationContext context)
            {
                var indexer = new CodeMemberProperty()
                {
                    Attributes = MemberAttributes.Public,
                    Name = "Item",
                    Type = elementType
                };
                indexer.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
                var indexRef = new CodeArgumentReferenceExpression("index");
                int runningIndexInit = 0;
                if (constraint != null && constraint.References.Count > 0)
                {
                    indexer.GetStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.LessThan, new CodePrimitiveExpression(constraint.References.Count)),
                        new CodeMethodReturnStatement(new CodeArrayIndexerExpression(standardValuesRef, indexRef))));
                    runningIndexInit = constraint.References.Count;
                }
                var defineRunningIndex = new CodeVariableDeclarationStatement(typeof(int), "runningIndex", new CodePrimitiveExpression(runningIndexInit));
                indexer.GetStatements.Add(defineRunningIndex);
                indexer.SetStatements.Add(defineRunningIndex);
                var runningIndexRef = new CodeVariableReferenceExpression("runningIndex");
                foreach (var reference in implementingReferences)
                {
                    var propertyRef = GetPropertyReference(reference);
                    CodeExpression setValue;
                    if (original.Type == reference.Type)
                    {
                        setValue = new CodePropertySetValueReferenceExpression();
                    }
                    else
                    {
                        setValue = new CodeCastExpression(CreateReference(reference.Type, true, context), new CodePropertySetValueReferenceExpression());
                    }
                    if (reference.UpperBound == 1)
                    {
                        var getIfNotNull = new CodeConditionStatement();
                        getIfNotNull.Condition = new CodeBinaryOperatorExpression(propertyRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                        var isRightIndex = new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.ValueEquality, indexRef);
                        getIfNotNull.TrueStatements.Add(new CodeConditionStatement(isRightIndex, new CodeMethodReturnStatement(propertyRef)));
                        getIfNotNull.TrueStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                        indexer.GetStatements.Add(getIfNotNull);

                        var setIfNotNull = new CodeConditionStatement();
                        setIfNotNull.Condition = getIfNotNull.Condition;
                        setIfNotNull.TrueStatements.Add(new CodeConditionStatement(isRightIndex, new CodeAssignStatement(propertyRef, setValue)));
                        setIfNotNull.TrueStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
                        indexer.SetStatements.Add(setIfNotNull);
                    }
                    else
                    {
                        var getIfInRange = new CodeConditionStatement();
                        var indexDiff = new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.Subtract, runningIndexRef);
                        getIfInRange.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(propertyRef, "Count"), CodeBinaryOperatorType.LessThanOrEqual, indexDiff);
                        getIfInRange.TrueStatements.Add(new CodeMethodReturnStatement(new CodeIndexerExpression(propertyRef, indexDiff)));
                        getIfInRange.FalseStatements.Add(new CodeAssignStatement(runningIndexRef, new CodeBinaryOperatorExpression(runningIndexRef, CodeBinaryOperatorType.Add, new CodePropertyReferenceExpression(propertyRef, "Count"))));
                        indexer.GetStatements.Add(getIfInRange);

                        var setIfInRange = new CodeConditionStatement();
                        setIfInRange.Condition = getIfInRange.Condition;
                        setIfInRange.TrueStatements.Add(new CodeAssignStatement(new CodeIndexerExpression(propertyRef, indexDiff), setValue));
                        setIfInRange.FalseStatements.AddRange(getIfInRange.FalseStatements);
                        indexer.SetStatements.Add(setIfInRange);
                    }
                }
                var throwOutOfRange = new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(IndexOutOfRangeException)));
                indexer.GetStatements.Add(throwOutOfRange);
                indexer.SetStatements.Add(throwOutOfRange);
                indexer.WriteDocumentation("Gets or sets the item at the given position");
                return indexer;
            }

            private CodeMemberProperty GenerateIsAttached()
            {
                var isAttachedProperty = new CodeMemberProperty()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Type = new CodeTypeReference(typeof(bool)),
                    Name = "IsAttached"
                };
                isAttachedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                isAttachedProperty.WriteDocumentation("Returns that this composed collection is always attached.");
                return isAttachedProperty;
            }

            private CodeMemberMethod GenerateCollectionAsNotifiableMethod(CodeTypeReference elementType)
            {
                var asNotifiable = new CodeMemberMethod()
                {
                    Name = "AsNotifiable",
                    Attributes = MemberAttributes.Private,
                    ReturnType = new CodeTypeReference(typeof(INotifyCollection<>).Name, elementType),
                    PrivateImplementationType = new CodeTypeReference(typeof(ICollectionExpression<>).Name, elementType)
                };
                asNotifiable.Statements.Add(new CodeMethodReturnStatement(new CodeThisReferenceExpression()));
                asNotifiable.WriteDocumentation("Gets an observable version of this collection");
                return asNotifiable;
            }

            private CodeMemberMethod GenerateEnumerableAsNotifiableMethod(CodeTypeReference elementType)
            {
                var asNotifiable = new CodeMemberMethod()
                {
                    Name = "AsNotifiable",
                    Attributes = MemberAttributes.Private,
                    ReturnType = new CodeTypeReference(typeof(INotifyEnumerable<>).Name, elementType),
                    PrivateImplementationType = typeof(IEnumerableExpression).ToTypeReference(elementType)
                };
                asNotifiable.Statements.Add(new CodeMethodReturnStatement(new CodeThisReferenceExpression()));
                asNotifiable.WriteDocumentation("Gets an observable version of this collection");
                return asNotifiable;
            }

            private CodeMemberMethod GenerateObjectAsNotifiableMethod()
            {
                var asNotifiable = new CodeMemberMethod()
                {
                    Name = "AsNotifiable",
                    Attributes = MemberAttributes.Private,
                    ReturnType = typeof(INotifyEnumerable).ToTypeReference(),
                    PrivateImplementationType = typeof(IEnumerableExpression).ToTypeReference()
                };
                asNotifiable.Statements.Add(new CodeMethodReturnStatement(new CodeThisReferenceExpression()));
                asNotifiable.WriteDocumentation("Gets an observable version of this collection");
                return asNotifiable;
            }

            private CodeMemberMethod GenerateAttachMethod()
            {
                var attach = new CodeMemberMethod()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = "Attach"
                };
                attach.WriteDocumentation("Attaches this collection class");
                return attach;
            }

            private CodeMemberMethod GenerateDetachMethod()
            {
                var detach = new CodeMemberMethod()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = "Detach"
                };
                detach.WriteDocumentation("Detaches this collection class");
                return detach;
            }
        }
    }
}

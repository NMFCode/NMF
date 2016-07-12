using NMF.CodeGen;
using NMF.Expressions;
using NMF.Models.Collections;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule that generates the collection class for the children reference
        /// </summary>
        public class Class2Children : TransformationRule<IClass, CodeTypeDeclaration>
        {
            /// <summary>
            /// Creates the uninitialized output type declaration
            /// </summary>
            /// <param name="scope">The scope in which the reference is refined</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The newly created code type declaration</returns>
            public override CodeTypeDeclaration CreateOutput(IClass scope, ITransformationContext context)
            {
                if (!scope.References.Any(r => r.IsContainment)) return null;
                return CodeDomHelper.CreateTypeDeclarationWithReference(scope.Name.ToPascalCase() + "ChildrenCollection");
            }

            protected virtual List<IReference> GetImplementingReferences(IClass scope, ITransformationContext context)
            {
                var generatedType = context.Trace.ResolveIn(Rule<Class2Type>(), scope);
                var r2p = Rule<Reference2Property>();
                return (from c in scope.Closure(c => c.BaseTypes)
                        from r in c.References
                        where r.IsContainment && generatedType.Members.Contains(context.Trace.ResolveIn(r2p, r))
                        select r).ToList();
            }

            private CodeFieldReferenceExpression parentRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_parent");

            /// <summary>
            /// Initializes the created type declaration
            /// </summary>
            /// <param name="scope">The scope in which the reference is refined</param>
            /// <param name="generatedType">The generated type declaration</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IClass scope, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                if (generatedType == null) return;
                generatedType.Attributes = MemberAttributes.Family;
                var scopeDeclaration = context.Trace.ResolveIn(Rule<Class2Type>(), scope);
                var elementType = CreateReference(null, true, context);
                var parent = new CodeMemberField(new CodeTypeReference(scopeDeclaration.Name), "_parent");
                parent.Attributes = MemberAttributes.Private;
                generatedType.Members.Add(parent);
                generatedType.BaseTypes.Add(typeof(ReferenceCollection).ToTypeReference());

                generatedType.WriteDocumentation(string.Format("The collection class to to represent the children of the {0} class", scope.Name));

                var constructor = new CodeConstructor();
                constructor.Attributes = MemberAttributes.Public;
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(parent.Type, "parent"));
                constructor.Statements.Add(new CodeAssignStatement(parentRef, new CodeArgumentReferenceExpression("parent")));

                constructor.WriteDocumentation("Creates a new instance");

                var implementingReferences = GetImplementingReferences(scope, context);

                CodeExpression standardValuesRef = null;

                generatedType.Members.Add(constructor);
                ImplementCollection(generatedType, elementType, standardValuesRef, implementingReferences, context);
            }

            /// <summary>
            /// Implements the ICollection interface
            /// </summary>
            /// <param name="generatedType">The generated code type declaration</param>
            /// <param name="elementType">The element type reference</param>
            /// <param name="implementingReferences">The attributes implementing the collection</param>
            protected virtual void ImplementCollection(CodeTypeDeclaration generatedType, CodeTypeReference elementType, CodeExpression standardValuesRef, List<IReference> implementingReferences, ITransformationContext context)
            {
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(ICollectionExpression<>).Name, elementType));
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(ICollection<>).Name, elementType));
                generatedType.Members.Add(GenerateAttachCore(implementingReferences));
                generatedType.Members.Add(GenerateDetachCore(implementingReferences));
                generatedType.Members.Add(GenerateAdd(implementingReferences, elementType, context));
                generatedType.Members.Add(GenerateClear(implementingReferences));
                generatedType.Members.Add(GenerateContains(implementingReferences, elementType, standardValuesRef));
                generatedType.Members.Add(GenerateCopyTo(implementingReferences, elementType, standardValuesRef));
                generatedType.Members.Add(GenerateCount(implementingReferences));
                generatedType.Members.Add(GenerateRemove(implementingReferences, elementType, context));
                generatedType.Members.Add(GenerateGenericGetEnumerator(implementingReferences, elementType, standardValuesRef));
            }

            private CodeMemberMethod GenerateAdd(IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, ITransformationContext context)
            {
                var add = new CodeMemberMethod()
                {
                    Name = "Add",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
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
                        if (reference.Type != null)
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
                        if (reference.Type != null)
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
                                ifNotFull.TrueStatements.Add(new CodeMethodInvokeExpression(propertyRef, "Add", varRef));
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
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
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

            private CodeMemberMethod GenerateContains(IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var contains = new CodeMemberMethod()
                {
                    Name = "Contains",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(bool))
                };
                contains.Parameters.Add(new CodeParameterDeclarationExpression(elementType, "item"));
                var itemRef = new CodeArgumentReferenceExpression("item");
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

            private CodeMemberMethod GenerateCopyTo(IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var copyTo = new CodeMemberMethod()
                {
                    Name = "CopyTo",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = null
                };
                copyTo.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(elementType, 1), "array"));
                copyTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "arrayIndex"));
                var arrayRef = new CodeArgumentReferenceExpression("array");
                var arrayIndexRef = new CodeArgumentReferenceExpression("arrayIndex");
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
                        if (reference.Type == null)
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

            private CodeMemberProperty GenerateCount(IEnumerable<IReference> implementingReferences)
            {
                var count = new CodeMemberProperty()
                {
                    Name = "Count",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Type = new CodeTypeReference(typeof(int)),
                    HasGet = true,
                    HasSet = false
                };
                count.GetStatements.Add(new CodeVariableDeclarationStatement(typeof(int), "count", new CodePrimitiveExpression(0)));
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

            private CodeMemberMethod GenerateRemove(IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, ITransformationContext context)
            {
                var remove = new CodeMemberMethod()
                {
                    Name = "Remove",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
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
                        if (reference.Type == null)
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

            private CodeMemberMethod GenerateGenericGetEnumerator(IEnumerable<IReference> implementingReferences, CodeTypeReference elementType, CodeExpression standardValuesRef)
            {
                var getEnumerator = new CodeMemberMethod()
                {
                    Name = "GetEnumerator",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(IEnumerator<>).Name, elementType)
                };
                CodeExpression currentExpression;
                currentExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(Enumerable).ToTypeReference()), "Empty", elementType));
                foreach (var reference in implementingReferences)
                {
                    currentExpression = new CodeMethodInvokeExpression(currentExpression, "Concat", GetPropertyReference(reference));
                }
                currentExpression = new CodeMethodInvokeExpression(currentExpression, "GetEnumerator");
                getEnumerator.Statements.Add(new CodeMethodReturnStatement(currentExpression));
                getEnumerator.WriteDocumentation("Gets an enumerator that enumerates the collection", "A generic enumerator");
                return getEnumerator;
            }

            private CodeMemberMethod GenerateAttachCore(IEnumerable<IReference> implementingReferences)
            {
                var attachCore = new CodeMemberMethod()
                {
                    Name = "AttachCore",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                var thisRef = new CodeThisReferenceExpression();
                foreach (var reference in implementingReferences)
                {
                    if (reference.UpperBound == 1)
                    {
                        attachCore.Statements.Add(new CodeAttachEventStatement(parentRef, reference.Name.ToPascalCase() + "Changed",
                            new CodeMethodReferenceExpression(thisRef, "PropagateValueChanges")));
                    }
                    else
                    {
                        attachCore.Statements.Add(new CodeAttachEventStatement(new CodeMethodInvokeExpression(
                            new CodePropertyReferenceExpression(parentRef, reference.Name.ToPascalCase()), "AsNotifiable"), "CollectionChanged",
                            new CodeMethodReferenceExpression(thisRef, "PropagateCollectionChanges")));
                    }
                }
                return attachCore;
            }

            private CodeMemberMethod GenerateDetachCore(IEnumerable<IReference> implementingReferences)
            {
                var detachCore = new CodeMemberMethod()
                {
                    Name = "DetachCore",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override
                };
                var thisRef = new CodeThisReferenceExpression();
                foreach (var reference in implementingReferences)
                {
                    if (reference.UpperBound == 1)
                    {
                        detachCore.Statements.Add(new CodeRemoveEventStatement(parentRef, reference.Name.ToPascalCase() + "Changed",
                            new CodeMethodReferenceExpression(thisRef, "PropagateValueChanges")));
                    }
                    else
                    {
                        detachCore.Statements.Add(new CodeRemoveEventStatement(new CodeMethodInvokeExpression(
                            new CodePropertyReferenceExpression(parentRef, reference.Name.ToPascalCase()), "AsNotifiable"), "CollectionChanged",
                            new CodeMethodReferenceExpression(thisRef, "PropagateCollectionChanges")));
                    }
                }
                return detachCore;
            }
        }
    }
}

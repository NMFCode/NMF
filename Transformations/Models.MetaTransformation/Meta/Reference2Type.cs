﻿using NMF.CodeGen;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations

namespace NMF.Models.Meta
{
    partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to generate an implementation class for a reference with an opposite
        /// </summary>
        public class Reference2Type : TransformationRule<IReference, CodeTypeDeclaration>
        {
            /// <summary>
            /// Creates the transformation rule output, i.e. an empty code declaration
            /// </summary>
            /// <param name="input">The NMeta reference</param>
            /// <param name="context">The transformation context</param>
            /// <returns>A new code type declaration</returns>
            public override CodeTypeDeclaration CreateOutput(IReference input, ITransformationContext context)
            {
                return CodeDomHelper.CreateTypeDeclarationWithReference(input.DeclaringType.Name.ToPascalCase() + input.Name.ToPascalCase() + "Collection", false);
            }

            /// <summary>
            /// Initializes the generated code type declaration
            /// </summary>
            /// <param name="input">The input NMeta reference</param>
            /// <param name="output">The generated code type declaration</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IReference input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.IsClass = true;
                output.Attributes = MemberAttributes.Assembly;

                var baseTypeReference = CreateReference(input.DeclaringType, true, context);
                var elementTypeReference = CreateReference(input.Type, true, context);

                output.BaseTypes.Add(GetBaseClass(input, baseTypeReference, elementTypeReference));

                CreateConstructor(output, baseTypeReference);

                CreateSetParentMethod(input, output, baseTypeReference, elementTypeReference, context);
            }

            private CodeTypeReference GetBaseClass(IReference input, CodeTypeReference baseTypeReference, CodeTypeReference elementTypeReference)
            {
                CodeTypeReference collectionType;

                if (input.IsUnique)
                {
                    if (input.IsOrdered)
                    {
                        collectionType = CreateOrderedSet(baseTypeReference, elementTypeReference, input.IsContainment);
                    }
                    else
                    {
                        collectionType = CreateSet(baseTypeReference, elementTypeReference, input.IsContainment);
                    }
                }
                else
                {
                    if (input.IsOrdered)
                    {
                        collectionType = CreateList(baseTypeReference, elementTypeReference, input.IsContainment);
                    }
                    else
                    {
                        collectionType = CreateBag(baseTypeReference, elementTypeReference, input.IsContainment);
                    }
                }
                return collectionType;
            }

            private void CreateSetParentMethod(IReference input, CodeTypeDeclaration output, CodeTypeReference baseTypeReference, CodeTypeReference elementTypeReference, ITransformationContext context)
            {
                var method = new CodeMemberMethod();
                method.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                method.Name = "SetOpposite";
                method.Parameters.Add(new CodeParameterDeclarationExpression(elementTypeReference, "item"));
                method.Parameters.Add(new CodeParameterDeclarationExpression(baseTypeReference, "parent"));

                ImplementSetParentMethod(input, output, elementTypeReference, method, context);
                output.Members.Add(method);
            }

            private void ImplementSetParentMethod(IReference input, CodeTypeDeclaration output, CodeTypeReference elementTypeReference, CodeMemberMethod method, ITransformationContext context)
            {
                var opposite = input.Opposite;

                var item = new CodeArgumentReferenceExpression("item");
                var parent = new CodeArgumentReferenceExpression("parent");

                var thisRef = new CodeThisReferenceExpression();
                var nullRef = new CodePrimitiveExpression(null);
                var item_opp = new CodePropertyReferenceExpression(item, context.Trace.ResolveIn(Rule<Reference2Property>(), opposite).Name);
                var ifNotNull = new CodeConditionStatement(new CodeBinaryOperatorExpression(parent, CodeBinaryOperatorType.IdentityInequality, nullRef));

                var targetClass = input.Type;

                var eventName = input.IsContainment ? "ParentChanged" : "Deleted";
                var eventType = input.IsContainment ? typeof(ValueChangedEventArgs).ToTypeReference() : typeof(EventArgs).ToTypeReference();

                var onItemDeleted = new CodeMemberMethod()
                {
                    Name = "OnItem" + eventName,
                    Attributes = MemberAttributes.Private,
                    ReturnType = new CodeTypeReference(typeof(void))
                };

                onItemDeleted.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
                onItemDeleted.Parameters.Add(new CodeParameterDeclarationExpression(eventType, "e"));
                var actualRemove = new CodeMethodInvokeExpression(
                    thisRef, "Remove", new CodeCastExpression(elementTypeReference, new CodeArgumentReferenceExpression("sender")));
                if (input.IsContainment)
                {
                    var newValue = new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("e"), "NewValue");
                    var parentRef = new CodePropertyReferenceExpression(thisRef, "Parent");
                    onItemDeleted.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(newValue, CodeBinaryOperatorType.IdentityInequality, parentRef),
                        new CodeExpressionStatement(actualRemove)));
                }
                else
                {
                    onItemDeleted.Statements.Add(actualRemove);
                }
                output.Members.Add(onItemDeleted);

                ifNotNull.TrueStatements.Add(new CodeAttachEventStatement(item, eventName, new CodeMethodReferenceExpression(thisRef, onItemDeleted.Name)));
                ifNotNull.FalseStatements.Add(new CodeRemoveEventStatement(item, eventName, new CodeMethodReferenceExpression(thisRef, onItemDeleted.Name)));


                if (opposite.UpperBound == 1)
                {
                    var assignStatement = new CodeAssignStatement(item_opp, parent);
                    ifNotNull.TrueStatements.Add(assignStatement);
                    ifNotNull.FalseStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(
                        item_opp, CodeBinaryOperatorType.IdentityEquality, new CodePropertyReferenceExpression(thisRef, "Parent")),
                        assignStatement));
                }
                else
                {
                    ifNotNull.FalseStatements.Add(new CodeMethodInvokeExpression(item_opp, "Remove", new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Parent")));
                    ifNotNull.TrueStatements.Add(new CodeMethodInvokeExpression(item_opp, "Add", parent));
                }

                method.Statements.Add(ifNotNull);
            }

            private void CreateConstructor(CodeTypeDeclaration output, CodeTypeReference baseTypeReference)
            {
                var constructor = new CodeConstructor();
                constructor.Attributes = MemberAttributes.Public;
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(baseTypeReference, "parent"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("parent"));
                output.Members.Add(constructor);
            }

            /// <summary>
            /// Creates the type reference to the base class implementing an opposite list
            /// </summary>
            /// <param name="baseTypeReference">The parent element type</param>
            /// <param name="elementTypeReference">The referenced element type</param>
            /// <param name="isContainment">True, if the collection is a containment collection, otherwise False</param>
            /// <returns>The type reference to the opposite list</returns>
            protected virtual CodeTypeReference CreateList(CodeTypeReference baseTypeReference, CodeTypeReference elementTypeReference, bool isContainment)
            {
                return new CodeTypeReference(typeof(ObservableOppositeList<,>).Name, baseTypeReference, elementTypeReference);
            }

            /// <summary>
            /// Creates the type reference to the base class implementing an opposite bag
            /// </summary>
            /// <param name="baseTypeReference">The parent element type</param>
            /// <param name="elementTypeReference">The referenced element type</param>
            /// <param name="isContainment">True, if the collection is a containment collection, otherwise False</param>
            /// <returns>The type reference to the opposite bag</returns>
            protected virtual CodeTypeReference CreateBag(CodeTypeReference baseTypeReference, CodeTypeReference elementTypeReference, bool isContainment)
            {
                return new CodeTypeReference(typeof(ObservableOppositeList<,>).Name, baseTypeReference, elementTypeReference);
            }

            /// <summary>
            /// Creates the type reference to the base class implementing an opposite set
            /// </summary>
            /// <param name="baseTypeReference">The parent element type</param>
            /// <param name="elementTypeReference">The referenced element type</param>
            /// <param name="isContainment">True, if the collection is a containment collection, otherwise False</param>
            /// <returns>The type reference to the opposite set</returns>
            protected virtual CodeTypeReference CreateSet(CodeTypeReference baseTypeReference, CodeTypeReference elementTypeReference, bool isContainment)
            {
                return new CodeTypeReference(typeof(ObservableOppositeSet<,>).Name, baseTypeReference, elementTypeReference);
            }

            /// <summary>
            /// Creates the type reference to the base class implementing an opposite ordered set
            /// </summary>
            /// <param name="baseTypeReference">The parent element type</param>
            /// <param name="elementTypeReference">The referenced element type</param>
            /// <param name="isContainment">True, if the collection is a containment collection, otherwise False</param>
            /// <returns>The type reference to the opposite ordered set</returns>
            protected virtual CodeTypeReference CreateOrderedSet(CodeTypeReference baseTypeReference, CodeTypeReference elementTypeReference, bool isContainment)
            {
                return new CodeTypeReference(typeof(ObservableOppositeOrderedSet<,>).Name, baseTypeReference, elementTypeReference);
            }
        }
    }
}


#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations
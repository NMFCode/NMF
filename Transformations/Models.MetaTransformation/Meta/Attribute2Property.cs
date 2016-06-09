using NMF.CodeGen;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Models.Meta;
using NMF.Serialization;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform attributes of a class or extension to a code property
        /// </summary>
        public class Attribute2Property : TransformationRule<IAttribute, CodeMemberProperty>
        {
            /// <summary>
            /// Registers the dependency to generate refibed attributes
            /// </summary>
            public override void RegisterDependencies()
            {
                Require(Rule<RefinedAttributeGenerator>(), att => att.DeclaringType as IClass, att => att.Refines, att => att.Refines != null);
            }

            /// <summary>
            /// Initializes the generated property
            /// </summary>
            /// <param name="input">The input NMeta attribute</param>
            /// <param name="generatedProperty">The generated property</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IAttribute input, CodeMemberProperty generatedProperty, ITransformationContext context)
            {
                generatedProperty.Name = input.Name.ToPascalCase();
                generatedProperty.Attributes = MemberAttributes.Public;
                var summary = input.Summary;
                if (string.IsNullOrEmpty(summary)) summary = string.Format("The {0} property", input.Name);
                generatedProperty.WriteDocumentation(summary, input.Remarks);

                var fieldType = CreateTypeReference(input, att => generatedProperty.CustomAttributes.Add(att),
                    CreateCollectionImplementationType, context);
                var t = Transformation as Meta2ClassesTransformation;
                if ((t == null || t.IsValueType(input.Type)) && input.LowerBound == 0 && input.UpperBound == 1)
                {
                    fieldType = new CodeTypeReference(typeof(System.Nullable<>).Name, fieldType);
                }

                var fieldRef = generatedProperty.CreateBackingField(fieldType, CreateDefaultValue(input, fieldType, generatedProperty));

                generatedProperty.ImplementGetter(fieldRef);

                if (input.UpperBound == 1)
                {
                    generatedProperty.Type = fieldType;

                    var callOnPropertyChanging = new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(), "OnPropertyChanging",
                        new CodePrimitiveExpression(generatedProperty.Name));

                    var oldDef = new CodeVariableDeclarationStatement(fieldType, "old", fieldRef);
                    var oldRef = new CodeVariableReferenceExpression("old");
                    var value = new CodePropertySetValueReferenceExpression();

                    var valueChangeTypeRef = typeof(ValueChangedEventArgs).ToTypeReference();
                    var valueChangeDef = new CodeVariableDeclarationStatement(valueChangeTypeRef, "e",
                        new CodeObjectCreateExpression(typeof(ValueChangedEventArgs).ToTypeReference(), oldRef, value));
                    var valueChangeRef = new CodeVariableReferenceExpression(valueChangeDef.Name);

                    var callOnPropertyChanged = new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(), "OnPropertyChanged", 
                        new CodePrimitiveExpression(generatedProperty.Name), valueChangeRef);

                    generatedProperty.SetStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(fieldRef, CodeBinaryOperatorType.IdentityInequality, value),
                        generatedProperty.CreateOnChangingEventPattern(),
                        new CodeExpressionStatement(callOnPropertyChanging),
                        oldDef,
                        new CodeAssignStatement(fieldRef, value),
                        valueChangeDef,
                        generatedProperty.CreateOnChangedEventPattern(valueChangeTypeRef, valueChangeRef),
                        new CodeExpressionStatement(callOnPropertyChanged)));
                }
                else
                {
                    generatedProperty.Type = CreateTypeReference(input, null, CreateCollectionInterfaceType, context);
                    generatedProperty.MarkCollectionProperty();

                    var createEmptyCollection = new CodeAssignStatement(fieldRef, new CodeObjectCreateExpression(fieldType));
                    var constructorStmts = generatedProperty.ImpliedConstructorStatements(true);
                    constructorStmts.Add(createEmptyCollection);
                    constructorStmts.Add(new CodeAttachEventStatement(fieldRef, "CollectionChanging",
                        GenerateCollectionBubbleHandler(generatedProperty, "CollectionChanging", typeof(NotifyCollectionChangingEventArgs))));
                    constructorStmts.Add(new CodeAttachEventStatement(fieldRef, "CollectionChanged",
                        GenerateCollectionBubbleHandler(generatedProperty, "CollectionChanged", typeof(NotifyCollectionChangedEventArgs))));
                }

                GenerateSerializationAttributes(input, generatedProperty, context);
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
            /// Creates the default value for the property
            /// </summary>
            /// <param name="input">The input NMeta attribute</param>
            /// <param name="fieldType">The code type reference for the field type</param>
            /// <param name="generatedProperty">The generated property</param>
            /// <returns>A code expression that represents the default value</returns>
            protected virtual CodeExpression CreateDefaultValue(IAttribute input, CodeTypeReference fieldType, CodeMemberProperty generatedProperty)
            {
                if (input.DefaultValue == null) return null;

                var expression = CodeDomHelper.CreatePrimitiveExpression(input.DefaultValue, fieldType);
                if (expression != null)
                {
                    generatedProperty.AddAttribute(typeof(DefaultValueAttribute), expression);
                }
                return expression;
            }

            private void GenerateSerializationAttributes(IAttribute input, CodeMemberProperty output, ITransformationContext context)
            {
                if (input.Name != output.Name)
                {
                    output.AddAttribute(typeof(XmlElementNameAttribute), input.Name);
                }

                IClass ownedClass = input.DeclaringType as IClass;
                if (ownedClass != null && input == ownedClass.Identifier)
                {
                    output.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(IdAttribute).ToTypeReference()));
                    var declaration = context.Trace.ResolveIn(Rule<Class2Type>(), input.DeclaringType as IClass);
                    if (!declaration.IsInterface)
                    {
                        declaration.AddAttribute(typeof(XmlIdentifierAttribute), input.Name);
                    }
                }
                output.AddAttribute(typeof(XmlAttributeAttribute), true);
                if (input.UpperBound != 1)
                {
                    output.AddAttribute(typeof(ConstantAttribute));
                }
            }

            private CodeTypeReference CreateCollectionImplementationType(ITypedElement arg, CodeTypeReference elementType, ITransformationContext context)
            {
                var feature = arg as IAttribute;
                if (feature.IsUnique)
                {
                    if (feature.IsOrdered)
                    {
                        return CreateOrderedSetImplementationType(elementType);
                    }
                    else
                    {
                        return CreateSetImplementationType(elementType);
                    }
                }
                else
                {
                    if (feature.IsOrdered)
                    {
                        return CreateListImplementationType(elementType);
                    }
                    else
                    {
                        return CreateBagImplementationType(elementType);
                    }
                }
            }

            private CodeTypeReference CreateCollectionInterfaceType(ITypedElement arg, CodeTypeReference elementType, ITransformationContext context)
            {
                var feature = arg as IAttribute;
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
            /// Creates the bag implementation of the given element type reference
            /// </summary>
            /// <param name="elementTypeReference">The type reference of the bags elements</param>
            /// <returns>A type reference for the bag implementation for the given element type</returns>
            protected virtual CodeTypeReference CreateBagImplementationType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ObservableList<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the list implementation of the given element type reference
            /// </summary>
            /// <param name="elementTypeReference">The type reference of the lists elements</param>
            /// <returns>A type reference for the list implementation for the given element type</returns>
            protected virtual CodeTypeReference CreateListImplementationType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ObservableList<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the unordered set implementation of the given element type reference
            /// </summary>
            /// <param name="elementTypeReference">The elements type reference</param>
            /// <returns>A type reference for the unordered set implementation for the given element type</returns>
            protected virtual CodeTypeReference CreateSetImplementationType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ObservableSet<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the ordered set implementation of the given element type reference
            /// </summary>
            /// <param name="elementTypeReference">The type reference of the elements</param>
            /// <returns>A type reference for the ordered set implementation for the given element type</returns>
            protected virtual CodeTypeReference CreateOrderedSetImplementationType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ObservableOrderedSet<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the list interface type for the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type</param>
            /// <returns>The list interface type reference, by default the generic IList</returns>
            protected virtual CodeTypeReference CreateListInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(IListExpression<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the bag interface type for the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type</param>
            /// <returns>The bag interface type reference, by default the generic ICollection</returns>
            protected virtual CodeTypeReference CreateBagInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ICollectionExpression<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the unordered set interface type for the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <returns>The unordered set interface type, by default the generic ISet</returns>
            protected virtual CodeTypeReference CreateSetInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(ISetExpression<>).Name, elementTypeReference);
            }

            /// <summary>
            /// Creates the ordered set interface type for the given element type
            /// </summary>
            /// <param name="elementTypeReference">The element type reference</param>
            /// <returns>The ordered set interface type, by default the generic IOrderedSet interface from NMF.Collections.Generic</returns>
            protected virtual CodeTypeReference CreateOrderedSetInterfaceType(CodeTypeReference elementTypeReference)
            {
                return new CodeTypeReference(typeof(IOrderedSetExpression<>).Name, elementTypeReference);
            }
        }
    }
}

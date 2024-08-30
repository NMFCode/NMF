using NMF.CodeGen;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to generate enumerations from NMeta enumerations
        /// </summary>
        public class Enumeration2Type : EnumGenerator<IEnumeration>
        {
            /// <summary>
            /// Gets the enumeration members that should be generated based on the given NMeta enumeration
            /// </summary>
            /// <param name="input">The NMeta enumeration</param>
            /// <param name="context">The context in which the enumeration is generated</param>
            /// <param name="generatedType">The generated type</param>
            /// <returns>A collection of enumeration members</returns>
            protected override IEnumerable<EnumGenerator<IEnumeration>.EnumMember> GetMembers(IEnumeration input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var names = new HashSet<string>();
                var dict = new Dictionary<ILiteral, string>();

                foreach (var literal in input.Literals)
                {
                    var fieldName = literal.Name.ToPascalCase();
                    if (!names.Add(fieldName))
                    {
                        var counter = 1;
                        while (!names.Add(fieldName + counter.ToString()))
                        {
                            counter++;
                        }
                        fieldName += counter.ToString();
                    }
                    dict.Add(literal, fieldName);
                    yield return new EnumMember()
                    {
                        Name = fieldName,
                        Summary = literal.Summary,
                        Remarks = literal.Remarks,
                        Value = literal.Value
                    };
                }

                Enumeration2Type.GenerateTypeConverter(generatedType, dict);
            }

            private static void GenerateTypeConverter(CodeTypeDeclaration generatedType, Dictionary<ILiteral, string> fieldNames)
            {
                var dependents = generatedType.DependentTypes(true);
                if (dependents.Any(c => c.Name == generatedType.Name + "Converter") ||
                    fieldNames.All(field => field.Value == field.Key.Name)) return;

                var typeConverter = new CodeTypeDeclaration(generatedType.Name + "Converter");
                typeConverter.BaseTypes.Add(typeof(TypeConverter).ToTypeReference());
                var stringTypeRef = new CodeTypeOfExpression(typeof(string));

                var canConvertFromMethod = new CodeMemberMethod
                {
                    Name = "CanConvertFrom",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(bool))
                };
                canConvertFromMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext).ToTypeReference(), "context"));
                canConvertFromMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Type).ToTypeReference(), "sourceType"));
                var sourceTypeRef = new CodeArgumentReferenceExpression("sourceType");
                canConvertFromMethod.Statements.Add(new CodeMethodReturnStatement(new CodeBinaryOperatorExpression(sourceTypeRef, CodeBinaryOperatorType.IdentityEquality, stringTypeRef)));
                typeConverter.Members.Add(canConvertFromMethod);

                var canConvertTo = new CodeMemberMethod
                {
                    Name = "CanConvertTo",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(bool))
                };
                canConvertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext).ToTypeReference(), "context"));
                canConvertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Type), "destinationType"));
                var destinationTypeRef = new CodeArgumentReferenceExpression("destinationType");
                canConvertTo.Statements.Add(new CodeMethodReturnStatement(new CodeBinaryOperatorExpression(destinationTypeRef, CodeBinaryOperatorType.IdentityEquality, stringTypeRef)));
                typeConverter.Members.Add(canConvertTo);
                
                var typeRef = generatedType.GetReferenceForType();
                var typeExpression = new CodeTypeReferenceExpression(typeRef);
                var convertFrom = new CodeMemberMethod
                {
                    Name = "ConvertFrom",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(object))
                };
                convertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext).ToTypeReference(), "context"));
                convertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(CultureInfo).ToTypeReference(), "culture"));
                convertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "value"));
                var valueRef = new CodeArgumentReferenceExpression("value");
                var nullRef = new CodePrimitiveExpression();
                convertFrom.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(valueRef, CodeBinaryOperatorType.IdentityEquality, nullRef),
                    new CodeMethodReturnStatement(new CodeDefaultValueExpression(typeRef))));
                var valueString = new CodeVariableDeclarationStatement(typeof(string), "valueString", new CodeMethodInvokeExpression(valueRef, "ToString"));
                convertFrom.Statements.Add(valueString);
                var valueStringRef = new CodeVariableReferenceExpression(valueString.Name);
                foreach (var field in fieldNames)
                {
                    convertFrom.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(valueStringRef, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(field.Key.Name)),
                        new CodeMethodReturnStatement(new CodeFieldReferenceExpression(typeExpression, field.Value))));
                }
                convertFrom.Statements.Add(new CodeMethodReturnStatement(new CodeDefaultValueExpression(typeRef)));
                typeConverter.Members.Add(convertFrom);
                
                var convertTo = new CodeMemberMethod
                {
                    Name = "ConvertTo",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(object))
                };
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext).ToTypeReference(), "context"));
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(CultureInfo).ToTypeReference(), "culture"));
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "value"));
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Type).ToTypeReference(), "destinationType"));
                convertTo.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(valueRef, CodeBinaryOperatorType.IdentityEquality, nullRef),
                    new CodeMethodReturnStatement(nullRef)));
                var valueCasted = new CodeVariableDeclarationStatement(typeRef, "valueCasted", new CodeCastExpression(typeRef, valueRef));
                convertTo.Statements.Add(valueCasted);
                var valueCastedRef = new CodeVariableReferenceExpression(valueCasted.Name);
                foreach (var field in fieldNames)
                {
                    convertTo.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(valueCastedRef, CodeBinaryOperatorType.ValueEquality, new CodeFieldReferenceExpression(typeExpression, field.Value)),
                        new CodeMethodReturnStatement(new CodePrimitiveExpression(field.Key.Name))));
                }
                convertTo.ThrowException<ArgumentOutOfRangeException>("value");
                typeConverter.Members.Add(convertTo);

                generatedType.AddAttribute(typeof(TypeConverterAttribute), new CodeTypeOfExpression(typeConverter.Name));
                dependents.Add(typeConverter);
            }

            /// <summary>
            /// Initializes the created enumeration
            /// </summary>
            /// <param name="input">The input NMeta enumeration</param>
            /// <param name="output">The generated code declaration</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IEnumeration input, CodeTypeDeclaration output, ITransformationContext context)
            {
                SetTypeReferenceForMappedType(input, CodeDomHelper.GetReferenceForType(output));
                base.Transform(input, output, context);
                if (input.AbsoluteUri != null)
                {
                    output.AddAttribute(typeof(ModelRepresentationClassAttribute), input.AbsoluteUri.AbsoluteUri);
                }
                output.WriteDocumentation(input.Summary, input.Remarks);
            }

            /// <summary>
            /// Gets the name of the enumeration
            /// </summary>
            /// <param name="input">The NMeta enumeration</param>
            /// <returns>The name of the enumeration to be generated</returns>
            protected override string GetName(IEnumeration input)
            {
                return input.Name.ToPascalCase();
            }

            /// <summary>
            /// Gets a value indicating whether the generated enumeration is flagged
            /// </summary>
            /// <param name="input">The NMeta enumeration</param>
            /// <returns>True, if the enumeration is flagged, otherwise false</returns>
            protected override bool GetIsFlagged(IEnumeration input)
            {
                return input.IsFlagged;
            }

            /// <summary>
            /// Marks the transformation rule instantiating for Type2Type
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());
            }
        }
    }
}

#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations
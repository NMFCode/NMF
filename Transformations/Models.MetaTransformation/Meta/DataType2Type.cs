using NMF.CodeGen;
using NMF.Serialization;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;


#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to generate a struct from an NMeta DataType
        /// </summary>
        public class DataType2Type : TransformationRule<IDataType, CodeTypeDeclaration>
        {
            /// <summary>
            /// Creates the output of this transformation rule
            /// </summary>
            /// <param name="input">The input NMeta DataType</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The generated code declaration</returns>
            public override CodeTypeDeclaration CreateOutput(IDataType input, ITransformationContext context)
            {
                return CodeDomHelper.CreateTypeDeclarationWithReference(input.Name.ToPascalCase(), true);
            }

            /// <summary>
            /// Initializes the generated code declaration
            /// </summary>
            /// <param name="input">The NMeta DataType for which to generate the struct type declaration</param>
            /// <param name="generatedType">The type declaration for the struct</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IDataType input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                generatedType.IsStruct = true;
                generatedType.IsEnum = false;
                generatedType.IsPartial = true;
                generatedType.IsClass = false;
                generatedType.IsInterface = false;

                generatedType.WriteDocumentation(input.Summary ?? string.Format("The {0} type", input.Name), input.Remarks);

                generatedType.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(SerializableAttribute).ToTypeReference()));

                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(IEquatable<>).Name, new CodeTypeReference(generatedType.Name)));

                var constructor = new CodeConstructor() { Attributes = MemberAttributes.Public };

                var attr2Property = Rule<DataTypeAttribute2Property>();
                foreach (var attr in input.Attributes)
                {
                    var property = context.Trace.ResolveIn(attr2Property, attr);
                    var fieldRef = property.GetBackingField();

                    var cArg = new CodeParameterDeclarationExpression(property.Type, attr.Name.ToCamelCase());
                    var cStatement = new CodeAssignStatement(fieldRef, new CodeArgumentReferenceExpression(cArg.Name));

                    constructor.Parameters.Add(cArg);
                    constructor.Statements.Add(cStatement);

                    generatedType.Members.Add(property);
                    var dependent = CodeDomHelper.DependentMembers(property, false);
                    if (dependent != null)
                    {
                        foreach (var item in dependent)
                        {
                            generatedType.Members.Add(item);
                        }
                    }
                }

                generatedType.Members.Add(constructor);
                generatedType.Members.Add(CreateGenericEquals(input, generatedType, context));
                generatedType.Members.Add(CreateObjectEquals(input, generatedType, context));
                generatedType.Members.Add(CreateGetHashCode(input, context));
                generatedType.Members.Add(CreateEqualsOperator(input, generatedType, context));
                generatedType.Members.Add(CreateNotEqualsOperator(input, generatedType, context));
                generatedType.Members.Add(CreateSerializeToJson(input, context));
                generatedType.Members.Add(CreateTryParseJsonMethod(input, generatedType, context));
                var converter = CreateTypeConverter(input, generatedType, context);
                generatedType.Members.Add(converter);
                generatedType.AddAttribute(typeof(TypeConverterAttribute), new CodeTypeOfExpression(converter.GetReferenceForType()));
            }

            /// <summary>
            /// Generates the Object Equals overriding method
            /// </summary>
            /// <param name="input">The NMeta DataType for which to generate the Equals method</param>
            /// <param name="output">The generated type declaration</param>
            /// <param name="context">The transformation context</param>
            protected virtual CodeMemberMethod CreateObjectEquals(IDataType input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var thisTypeRef = new CodeTypeReference(output.Name);
                var equals = new CodeMemberMethod()
                {
                    Name = "Equals",
                    ReturnType = new CodeTypeReference(typeof(bool)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                equals.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "obj"));
                var argRef = new CodeArgumentReferenceExpression("obj");
                var nullRef = new CodePrimitiveExpression(null);
                var body = new CodeConditionStatement(new CodeBinaryOperatorExpression(
                    new CodeBinaryOperatorExpression(argRef, CodeBinaryOperatorType.IdentityInequality, nullRef),
                    CodeBinaryOperatorType.BooleanAnd,
                    new CodeBinaryOperatorExpression(new CodeMethodInvokeExpression(argRef, "GetType"), CodeBinaryOperatorType.IdentityEquality, new CodeTypeOfExpression(thisTypeRef))));

                equals.WriteDocumentation("Determines whether this structure and the given struct should be treated as equivalent",
                    "True, if this structure and the given other object are equivalent", new Dictionary<string, string>() { { "obj", "The other object" } });

                body.TrueStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "Equals", new CodeCastExpression(thisTypeRef, argRef))));
                body.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
                equals.Statements.Add(body);
                return equals;
            }

            /// <summary>
            /// Generates the generic Equals method implementing Equals
            /// </summary>
            /// <param name="input">The NMeta data type</param>
            /// <param name="generatedType">The type generated for the given data type</param>
            /// <param name="context">The transformation context</param>
            protected virtual CodeMemberMethod CreateGenericEquals(IDataType input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var thisTypeRef = generatedType.GetReferenceForType();
                var equals = new CodeMemberMethod()
                {
                    Name = "Equals",
                    ReturnType = new CodeTypeReference(typeof(bool)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Final
                };
                equals.Parameters.Add(new CodeParameterDeclarationExpression(thisTypeRef, "other"));

                equals.WriteDocumentation("Determines whether this structure and the given struct should be treated as equivalent",
                    "True, if this structure and the given other object are equivalent", new Dictionary<string, string>() { { "other", "The other object" } });

                var argRef = new CodeArgumentReferenceExpression("other");
                var expressions = input.Attributes.Select(prop =>
                {
                    var propName = prop.Name.ToPascalCase();
                    var thisRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), propName);
                    var otherRef = new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("other"), propName);
                    return new CodeBinaryOperatorExpression(thisRef, CodeBinaryOperatorType.IdentityEquality, otherRef);
                }).ToArray();

                var exp = expressions[0];
                for (int i = 0; i < expressions.Length; i++)
                {
                    exp = new CodeBinaryOperatorExpression(exp, CodeBinaryOperatorType.BooleanAnd, expressions[i]);
                }
                equals.Statements.Add(new CodeMethodReturnStatement(exp));
                return equals;
            }

            /// <summary>
            /// Generates a GetHashCode method
            /// </summary>
            /// <param name="input">The data type for which the GetHashCode method should be generated</param>
            /// <param name="context">The context in which the transformation is made</param>
            /// <returns>A definition of the GetHashCode method</returns>
            protected virtual CodeMemberMethod CreateGetHashCode(IDataType input, ITransformationContext context)
            {
                var getHashCode = new CodeMemberMethod()
                {
                    Name = "GetHashCode",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = typeof(int).ToTypeReference()
                };

                getHashCode.WriteDocumentation("Gets a has representation of the current value");

                var hash = new CodeVariableReferenceExpression("hash");
                getHashCode.Statements.Add(new CodeVariableDeclarationStatement(getHashCode.ReturnType, hash.VariableName, new CodePrimitiveExpression(0)));
                
                var attr2Property = Rule<DataTypeAttribute2Property>();
                var thisRef = new CodeThisReferenceExpression();
                var counter = 1;
                foreach (var att in input.Attributes)
                {
                    var prop = context.Trace.ResolveIn(attr2Property, att);
                    var propValue = new CodePropertyReferenceExpression(thisRef, prop.Name);

                    var reassignHash = new CodeAssignStatement(hash,
                        new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(NMF.Utilities.Extensions).ToTypeReference()), "CombineHash",
                        hash,
                        new CodeMethodInvokeExpression(propValue, "GetHashCode"),
                        new CodePrimitiveExpression(counter)));

                    if (!IsString(att.Type))
                    {
                        getHashCode.Statements.Add(reassignHash);
                    }
                    else
                    {
                        getHashCode.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(propValue, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression()),
                            reassignHash));
                    }
                    counter++;
                }

                getHashCode.Statements.Add(new CodeMethodReturnStatement(hash));
                return getHashCode;
            }

            /// <summary>
            /// Creates an equals operator
            /// </summary>
            /// <param name="dataType">The data type for which the operator should be generated</param>
            /// <param name="generatedType">the generated type definition</param>
            /// <param name="context">The context in which the request is made</param>
            /// <returns>An equals operator</returns>
            public virtual CodeTypeMember CreateEqualsOperator(IDataType dataType, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                return new CodeSnippetTypeMember(string.Format(@"
        public static bool operator==({0} {1}1, {0} {1}2)
        {{
            return {1}1.Equals({1}2);
        }}", generatedType.Name, dataType.Name.ToCamelCase()));
            }

            /// <summary>
            /// Creates a not-equals operator
            /// </summary>
            /// <param name="dataType">The data type for which the operator should be generated</param>
            /// <param name="generatedType">The generated tye definition</param>
            /// <param name="context">The context in which the request is made</param>
            /// <returns>An unequals operator</returns>
            public virtual CodeTypeMember CreateNotEqualsOperator(IDataType dataType, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                return new CodeSnippetTypeMember(string.Format(@"
        public static bool operator!=({0} {1}1, {0} {1}2)
        {{
            return !{1}1.Equals({1}2);
        }}", generatedType.Name, dataType.Name.ToCamelCase()));
            }

            /// <summary>
            /// Creates a method that exports the data type to Json
            /// </summary>
            /// <param name="dataType">The data type</param>
            /// <param name="context">The context in which the request is made</param>
            /// <returns>A method declaration</returns>
            public virtual CodeMemberMethod CreateSerializeToJson(IDataType dataType, ITransformationContext context)
            {
                var serializeToJson = new CodeMemberMethod()
                {
                    Name = "SerializeToJson",
                    ReturnType = typeof(string).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Final
                };

                serializeToJson.WriteDocumentation("Serializes this value to a JSON string");

                var builder = new CodeVariableReferenceExpression("json");
                serializeToJson.Statements.Add(new CodeVariableDeclarationStatement(typeof(StringBuilder).ToTypeReference(), builder.VariableName,
                    new CodeObjectCreateExpression(typeof(StringBuilder).ToTypeReference())));

                var thisRef = new CodeThisReferenceExpression();
                var format = "{{{0}=";
                var att2Prop = Rule<DataTypeAttribute2Property>();
                var types = new Dictionary<IType, CodeVariableReferenceExpression>();
                foreach (var att in dataType.Attributes)
                {
                    serializeToJson.Statements.Add(new CodeMethodInvokeExpression(builder, "Append", new CodePrimitiveExpression(string.Format(format, att.Name))));
                    var prop = context.Trace.ResolveIn(att2Prop, att);
                    CodeVariableReferenceExpression converter;
                    if (!types.TryGetValue(att.Type, out converter))
                    {
                        var converterName = att.Type.Name.ToCamelCase() + "Converter";
                        serializeToJson.Statements.Add(new CodeVariableDeclarationStatement(
                            typeof(TypeConverter).ToTypeReference(),
                            converterName,
                            new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TypeConversion).ToTypeReference()), "GetConverter", new CodeTypeOfExpression(prop.Type))));
                        converter = new CodeVariableReferenceExpression(converterName);
                        types.Add(att.Type, converter);
                    }
                    serializeToJson.Statements.Add(new CodeMethodInvokeExpression(builder, "Append",
                        new CodeMethodInvokeExpression(converter, "ConvertToInvariantString", new CodePropertyReferenceExpression(thisRef, prop.Name))));
                    format = ", {0}=";
                }
                serializeToJson.Statements.Add(new CodeMethodInvokeExpression(builder, "Append", new CodePrimitiveExpression("}")));
                serializeToJson.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(builder, "ToString")));
                return serializeToJson;
            }

            /// <summary>
            /// Creates a TryParse method targeted for Json
            /// </summary>
            /// <param name="dataType">The data type</param>
            /// <param name="generatedType">The generated type definition</param>
            /// <param name="context">The context in which the call is made</param>
            /// <returns>A method declaration</returns>
            public virtual CodeMemberMethod CreateTryParseJsonMethod(IDataType dataType, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var tryParseJson = new CodeMemberMethod()
                {
                    Name = "TryParseJson",
                    ReturnType = typeof(bool).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static
                };
                tryParseJson.WriteDocumentation($"Deserializes the given JSON string into a {dataType.Name}", "True, if the conversion was successful, otherwise false",
                    new Dictionary<string, string>()
                    {
                        { "json", "The JSON string that is used as input to the conversion" },
                        { "result", $"The resulting {dataType.Name} value" }
                    });

                tryParseJson.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string).ToTypeReference(), "json"));
                var resultP = new CodeParameterDeclarationExpression(generatedType.GetReferenceForType(), "result")
                {
                    Direction = FieldDirection.Out
                };
                tryParseJson.Parameters.Add(resultP);
                var result = new CodeArgumentReferenceExpression(resultP.Name);

                var parsed = new CodeVariableReferenceExpression("parsed");
                tryParseJson.Statements.Add(new CodeVariableDeclarationStatement(typeof(IDictionary<string, string>).ToTypeReference(), parsed.VariableName,
                    new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TypeConversion).ToTypeReference()), "ParseJson", new CodeArgumentReferenceExpression("json"))));

                var ifParsed = new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression(parsed, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null))
                };
                ifParsed.FalseStatements.Add(new CodeAssignStatement(result, new CodeDefaultValueExpression(resultP.Type)));
                ifParsed.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
                ifParsed.TrueStatements.Add(new CodeAssignStatement(result, new CodeObjectCreateExpression(resultP.Type)));
                var value = new CodeVariableReferenceExpression("value");
                ifParsed.TrueStatements.Add(new CodeVariableDeclarationStatement(typeof(string).ToTypeReference(), value.VariableName));

                var att2Prop = Rule<DataTypeAttribute2Property>();
                var types = new Dictionary<IType, CodeVariableReferenceExpression>();
                foreach (var att in dataType.Attributes)
                {
                    var prop = context.Trace.ResolveIn(att2Prop, att);
                    CodeVariableReferenceExpression converter;
                    if (!types.TryGetValue(att.Type, out converter))
                    {
                        var converterName = att.Type.Name.ToCamelCase() + "Converter";
                        ifParsed.TrueStatements.Add(new CodeVariableDeclarationStatement(
                            typeof(TypeConverter).ToTypeReference(),
                            converterName,
                            new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TypeConversion).ToTypeReference()), "GetConverter", new CodeTypeOfExpression(prop.Type))));
                        converter = new CodeVariableReferenceExpression(converterName);
                        types.Add(att.Type, converter);
                    }
                    var field = prop.GetBackingField();

                    var ifPresent = new CodeConditionStatement
                    {
                        Condition = new CodeMethodInvokeExpression(parsed, "TryGetValue", new CodePrimitiveExpression(att.Name),
                        new CodeDirectionExpression(FieldDirection.Out, value))
                    };
                    var resultField = new CodeFieldReferenceExpression(result, field.FieldName);
                    var resultValue = new CodeCastExpression(prop.Type, new CodeMethodInvokeExpression(converter, "ConvertFromInvariantString", value));
                    ifPresent.TrueStatements.Add(new CodeAssignStatement(resultField, resultValue));
                    ifParsed.TrueStatements.Add(ifPresent);
                }

                ifParsed.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                tryParseJson.Statements.Add(ifParsed);
                return tryParseJson;
            }

            /// <summary>
            /// Creates a type converter for the given data type
            /// </summary>
            /// <param name="dataType">The data type</param>
            /// <param name="generatedType">The generated type</param>
            /// <param name="context">The context in which the request is made</param>
            /// <returns>A type declaration of the type converter</returns>
            protected virtual CodeTypeDeclaration CreateTypeConverter(IDataType dataType, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var converter = CodeDomHelper.CreateTypeDeclarationWithReference($"{generatedType.Name}Converter", true);
                converter.Attributes = MemberAttributes.Public;
                converter.BaseTypes.Add(typeof(TypeConverter).ToTypeReference());
                converter.WriteDocumentation($"A converter class to convert {generatedType.Name} instances from and to strings");

                var canConvertFrom = new CodeMemberMethod
                {
                    Name = "CanConvertFrom",
                    ReturnType = typeof(bool).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                canConvertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext), "context"));
                canConvertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Type), "sourceType"));
                canConvertFrom.Statements.Add(new CodeMethodReturnStatement(new CodeBinaryOperatorExpression(
                    new CodeArgumentReferenceExpression("sourceType"),
                    CodeBinaryOperatorType.IdentityEquality,
                    new CodeTypeOfExpression(typeof(string)))));
                canConvertFrom.WriteDocumentation("Determines whether the converter is able to convert from the given type");
                converter.Members.Add(canConvertFrom);

                var canConvertTo = new CodeMemberMethod
                {
                    Name = "CanConvertTo",
                    ReturnType = typeof(bool).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                canConvertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext), "context"));
                canConvertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Type), "destinationType"));
                canConvertTo.Statements.Add(new CodeMethodReturnStatement(new CodeBinaryOperatorExpression(
                    new CodeArgumentReferenceExpression("destinationType"),
                    CodeBinaryOperatorType.IdentityEquality,
                    new CodeTypeOfExpression(typeof(string)))));
                canConvertTo.WriteDocumentation("Determines whether the converter is able to convert to the given type");
                converter.Members.Add(canConvertTo);

                var convertFrom = new CodeMemberMethod
                {
                    Name = "ConvertFrom",
                    ReturnType = typeof(object).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                convertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext), "context"));
                convertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(CultureInfo), "culture"));
                convertFrom.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "value"));
                var result = new CodeVariableReferenceExpression("result");
                convertFrom.Statements.Add(new CodeVariableDeclarationStatement(generatedType.GetReferenceForType(), result.VariableName));
                var value = new CodeArgumentReferenceExpression("value");
                var ifParsed = new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression(
                    new CodeBinaryOperatorExpression(value, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression()),
                    CodeBinaryOperatorType.BooleanAnd,
                    new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(generatedType.GetReferenceForType()), "TryParseJson",
                        new CodeMethodInvokeExpression(value, "ToString"),
                        new CodeDirectionExpression(FieldDirection.Out, result)))
                };
                ifParsed.TrueStatements.Add(new CodeMethodReturnStatement(result));
                ifParsed.FalseStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression()));
                convertFrom.Statements.Add(ifParsed);
                converter.Members.Add(convertFrom);

                var convertTo = new CodeMemberMethod
                {
                    Name = "ConvertTo",
                    ReturnType = typeof(object).ToTypeReference(),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override
                };
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext), "context"));
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(CultureInfo), "culture"));
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "value"));
                convertTo.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Type), "destinationType"));
                convertTo.Statements.Add(new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(new CodeCastExpression(generatedType.GetReferenceForType(), new CodeArgumentReferenceExpression("value")),
                                                   "SerializeToJson")));
                converter.Members.Add(convertTo);

                return converter;
            }

            /// <summary>
            /// Registers the dependencies, i.e. marks the rule instantiating for Type2Type and requires the transformation of attributes
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());

                RequireMany(Rule<DataTypeAttribute2Property>(), dataType => dataType.Attributes);
            }
        }
    }
}

#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations

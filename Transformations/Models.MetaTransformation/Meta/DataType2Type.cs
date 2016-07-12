using NMF.CodeGen;
using NMF.Models.Meta;
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
                return new CodeTypeDeclaration()
                {
                    Name = input.Name.ToPascalCase(),
                    IsStruct = true
                };
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

                var constructor = new CodeConstructor();

                var attr2Property = Rule<DataTypeAttribute2Property>();
                foreach (var attr in input.Attributes)
                {
                    var fieldName = attr.Name.ToCamelCase();
                    var property = context.Trace.ResolveIn(attr2Property, attr);
                    var field = new CodeMemberField(property.Type, fieldName);
                    var fieldRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);

                    var cArg = new CodeParameterDeclarationExpression(field.Type, fieldName);
                    var cStatement = new CodeAssignStatement(fieldRef, new CodeArgumentReferenceExpression(fieldName));

                    constructor.Parameters.Add(cArg);
                    constructor.Statements.Add(cStatement);

                    generatedType.Members.Add(property);
                    generatedType.Members.Add(field);
                }

                CreateGenericEquals(input, generatedType, context);
                CreateObjectEquals(input, generatedType, context);
            }

            /// <summary>
            /// Generates the Object Equals overriding method
            /// </summary>
            /// <param name="input">The NMeta DataType for which to generate the Equals method</param>
            /// <param name="output">The generated type declaration</param>
            /// <param name="context">The transformation context</param>
            protected virtual void CreateObjectEquals(IDataType input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var thisTypeRef = new CodeTypeReference(output.Name);
                var equals = new CodeMemberMethod()
                {
                    Name = "Equals",
                    ReturnType = new CodeTypeReference(typeof(bool)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override | MemberAttributes.Final
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
                output.Members.Add(equals);
            }

            /// <summary>
            /// Generates the generic Equals method implementing Equals
            /// </summary>
            /// <param name="input">The NMeta data type</param>
            /// <param name="output">The generated type declaration</param>
            /// <param name="context">The transformation context</param>
            protected virtual void CreateGenericEquals(IDataType input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var thisTypeRef = new CodeTypeReference(output.Name);
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
                output.Members.Add(equals);
                var expressions = input.Attributes.Select(prop =>
                {
                    var propName = prop.Name.ToPascalCase();
                    var thisRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), propName);
                    var otherRef = new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("other"), propName);
                    return new CodeBinaryOperatorExpression(thisRef, CodeBinaryOperatorType.IdentityEquality, otherRef);
                }).ToArray();

                if (expressions != null && expressions.Length > 0)
                {
                    var exp = expressions[0];
                    for (int i = 0; i < expressions.Length; i++)
                    {
                        exp = new CodeBinaryOperatorExpression(exp, CodeBinaryOperatorType.BooleanAnd, expressions[i]);
                    }
                    equals.Statements.Add(new CodeMethodReturnStatement(exp));
                }
                else
                {
                    equals.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                }
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

using NMF.CodeGen;
using NMF.Collections.Generic;
using NMF.Expressions;
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
    partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform operations
        /// </summary>
        public class Operation2Method : TransformationRule<IOperation, CodeMemberMethod>
        {
            /// <summary>
            /// Initialize the generated operation
            /// </summary>
            /// <param name="input">The input NMeta operation</param>
            /// <param name="output">The generated code method</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IOperation input, CodeMemberMethod output, ITransformationContext context)
            {
                output.Name = input.Name.ToPascalCase();
                output.Attributes = MemberAttributes.Final | MemberAttributes.Public;
                if (input.Type != null)
                {
                    output.ReturnType = CreateTypeReference(input, null, CreateCollectionInterfaceType, context);
                }

                var parameterDocDict = new Dictionary<string, string>();
                foreach (var par in input.Parameters)
                {
                    parameterDocDict.Add(par.Name.ToCamelCase(), par.Summary);
                }
                output.WriteDocumentation(input.Summary, null, parameterDocDict, input.Remarks);

                CodeMemberEvent callingEvent, calledEvent;
                CodeMemberMethod onCalling, onCalled;
                CreateEvents(input, out callingEvent, out calledEvent, out onCalling, out onCalled);

                CodeMemberMethod retrieveOperation;
                CodeMemberField operationField;
                CreateReflectionOperationField(input, out retrieveOperation, out operationField);

                GenerateMethodBody(input, output, context, onCalling, onCalled, operationField);

                var dependent = output.DependentMembers(true);
                dependent.Add(callingEvent);
                dependent.Add(calledEvent);
                dependent.Add(retrieveOperation);
                dependent.Add(operationField);
                dependent.Add(onCalling);
                dependent.Add(onCalled);
            }

            private static void GenerateMethodBody(IOperation input, CodeMemberMethod output, ITransformationContext context, CodeMemberMethod onCalling, CodeMemberMethod onCalled, CodeMemberField operationField)
            {
                CodeTypeReference handlerType = CreateHandlerType(input, output, context);

                output.Statements.Add(new CodeVariableDeclarationStatement
                {
                    Name = "handler",
                    Type = handlerType,
                    InitExpression = new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(OperationBroker).ToTypeReference()), "Instance"),
                            "GetRegisteredDelegate",
                            handlerType),
                        new CodeFieldReferenceExpression(null, operationField.Name))
                });
                var handlerRef = new CodeVariableReferenceExpression("handler");
                var ifNotNull = new CodeConditionStatement(new CodeBinaryOperatorExpression(
                    handlerRef,
                    CodeBinaryOperatorType.IdentityInequality,
                    new CodePrimitiveExpression()));
                output.Statements.Add(ifNotNull);
                ifNotNull.FalseStatements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(InvalidOperationException).ToTypeReference(),
                    new CodePrimitiveExpression($"There is no implementation for method {input.Name} registered. Use the method broker to register a method implementation."))));
                var operationVal = new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(null, operationField.Name), "Value");
                var inputArgs = output.Parameters.Cast<CodeParameterDeclarationExpression>().Select(p => new CodeArgumentReferenceExpression(p.Name)).ToArray();
                var callEventInputs = new CodeExpression[input.Parameters.Count + 2];
                callEventInputs[0] = new CodeThisReferenceExpression();
                callEventInputs[1] = operationVal;
                Array.Copy(inputArgs, 0, callEventInputs, 2, inputArgs.Length);
                output.Statements.Add(new CodeVariableDeclarationStatement
                {
                    Name = "e",
                    Type = typeof(OperationCallEventArgs).ToTypeReference(),
                    InitExpression = new CodeObjectCreateExpression(typeof(OperationCallEventArgs).ToTypeReference(), callEventInputs)
                });
                var e = new CodeVariableReferenceExpression("e");
                var thisRef = new CodeThisReferenceExpression();
                output.Statements.Add(new CodeMethodInvokeExpression(
                    thisRef,
                    onCalling.Name,
                    e));
                output.Statements.Add(new CodeMethodInvokeExpression(
                    thisRef,
                    "OnBubbledChange",
                    new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(BubbledChangeEventArgs).ToTypeReference()),
                        "OperationCalling",
                        thisRef, operationVal, e)));

                CodeExpression result = null;
                var methodCallArgs = new CodeExpression[input.Parameters.Count + 1];
                methodCallArgs[0] = thisRef;
                Array.Copy(inputArgs, 0, methodCallArgs, 1, inputArgs.Length);
                var methodCall = new CodeMethodInvokeExpression(handlerRef, "Invoke", methodCallArgs);
                if (input.Type != null)
                {
                    output.Statements.Add(new CodeVariableDeclarationStatement
                    {
                        Name = "result",
                        Type = output.ReturnType,
                        InitExpression = methodCall
                    });
                    result = new CodeVariableReferenceExpression("result");
                    output.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(e, "Result"), result));
                }
                else
                {
                    output.Statements.Add(methodCall);
                }

                output.Statements.Add(new CodeMethodInvokeExpression(
                    thisRef,
                    onCalled.Name,
                    e));
                output.Statements.Add(new CodeMethodInvokeExpression(
                    thisRef,
                    "OnBubbledChange",
                    new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(BubbledChangeEventArgs).ToTypeReference()),
                        "OperationCalled",
                        thisRef, operationVal, e)));

                if (result != null)
                {
                    output.Statements.Add(new CodeMethodReturnStatement(result));
                }
            }

            private static CodeTypeReference CreateHandlerType(IOperation input, CodeMemberMethod output, ITransformationContext context)
            {
                CodeTypeReference handlerType;
                var handlerTypeArgs = new List<CodeTypeReference>();
                string baseType;
                handlerTypeArgs.Add(CreateReference(input.DeclaringType, true, context));
                foreach (CodeParameterDeclarationExpression par in output.Parameters)
                {
                    handlerTypeArgs.Add(par.Type);
                }
                if (input.Type != null)
                {
                    baseType = $"System.Func`{input.Parameters.Count + 2}";
                    handlerTypeArgs.Add(output.ReturnType);
                }
                else
                {
                    baseType = $"System.Action`{input.Parameters.Count + 1}";
                }
                handlerType = new CodeTypeReference(baseType, handlerTypeArgs.ToArray());
                return handlerType;
            }

            private static void CreateReflectionOperationField(IOperation input, out CodeMemberMethod retrieveOperation, out CodeMemberField operationField)
            {
                retrieveOperation = new CodeMemberMethod
                {
                    Name = "Retrieve" + input.Name.ToPascalCase() + "Operation",
                    Attributes = MemberAttributes.Private | MemberAttributes.Static,
                    ReturnType = typeof(IOperation).ToTypeReference()
                };
                retrieveOperation.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(
                    new CodePropertyReferenceExpression(null, "ClassInstance"),
                    "LookupOperation",
                    new CodePrimitiveExpression(input.Name))));

                var lazyType = typeof(Lazy<IOperation>).ToTypeReference();
                operationField = new CodeMemberField
                {
                    Name = "_" + input.Name.ToCamelCase() + "Operation",
                    Type = lazyType,
                    Attributes = MemberAttributes.Private | MemberAttributes.Static,
                    InitExpression = new CodeObjectCreateExpression(lazyType, new CodeMethodReferenceExpression(null, retrieveOperation.Name))
                };
            }

            private static void CreateEvents(IOperation input, out CodeMemberEvent callingEvent, out CodeMemberEvent calledEvent, out CodeMemberMethod onCalling, out CodeMemberMethod onCalled)
            {
                var callEventArgs = typeof(OperationCallEventArgs).ToTypeReference();
                var callEventDelegate = typeof(EventHandler<OperationCallEventArgs>).ToTypeReference();

                callingEvent = new CodeMemberEvent
                {
                    Name = input.Name.ToPascalCase() + "Calling",
                    Attributes = MemberAttributes.Final | MemberAttributes.Public,
                    Type = callEventDelegate
                };
                callingEvent.WriteDocumentation($"Gets fired before the operation {input.Name} gets called");
                calledEvent = new CodeMemberEvent
                {
                    Name = input.Name.ToPascalCase() + "Called",
                    Attributes = MemberAttributes.Final | MemberAttributes.Public,
                    Type = callEventDelegate
                };
                calledEvent.WriteDocumentation($"Gets fired after the operation {input.Name} got called");

                onCalling = callingEvent.CreateRaiseMethod(callEventArgs);
                onCalled = calledEvent.CreateRaiseMethod(callEventArgs);
            }

            private FieldDirection ConvertDirection(Direction direction)
            {
                switch (direction)
                {
                    case Direction.In:
                        return FieldDirection.In;
                    case Direction.Out:
                        return FieldDirection.Out;
                    case Direction.InOut:
                        return FieldDirection.Ref;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }
            }

            private CodeTypeReference CreateCollectionInterfaceType(ITypedElement feature, CodeTypeReference elementType, ITransformationContext context)
            {
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


            /// <summary>
            /// Registers the dependencies, i.e. transform the parameters
            /// </summary>
            public override void RegisterDependencies()
            {
                RequireMany(Rule<Parameter2Parameter>(), op => op.Parameters, (op, pars) => op.Parameters.AddRange(pars.ToArray()));
            }
        }

    }
}

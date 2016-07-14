using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to generate the class to represent an extension
        /// </summary>
        public class Extension2Type : ClassGenerator<IExtension>
        {
            /// <summary>
            /// Gets the name of the generated extension
            /// </summary>
            /// <param name="input">The input NMeta extension</param>
            /// <returns>The name of the generated extension</returns>
            protected override string GetName(IExtension input)
            {
                return input.Name.ToPascalCase();
            }

            /// <summary>
            /// Initializes the generated type for the NMeta extension
            /// </summary>
            /// <param name="input">The NMeta extension</param>
            /// <param name="generatedType">The generated type for the extension</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IExtension input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                SetTypeReferenceForMappedType(input, generatedType.GetReferenceForType());
                base.Transform(input, generatedType, context);
                generatedType.IsClass = true;
                generatedType.BaseTypes.Add(new CodeTypeReference(typeof(ModelElementExtension<>).Name, CreateReference(input.AdornedClass, true, context)));
                generatedType.WriteDocumentation(input.Summary ?? string.Format("The {0} extension", input.Name), input.Remarks);

                CreateConstructor(input, generatedType, context);
                CreateFromMethod(input, generatedType, context);
                CreateGetExtension(input, generatedType, context);
            }

            /// <summary>
            /// Creates the constructor for the extension
            /// </summary>
            /// <param name="input">The NMeta extension</param>
            /// <param name="generatedType">The generated type declaration for the extension</param>
            /// <param name="context">The transformation context</param>
            protected virtual void CreateConstructor(IExtension input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var classRef = CreateReference(input.AdornedClass, true, context);
                var constructor = CodeDomHelper.GetOrCreateDefaultConstructor(generatedType, () => new CodeConstructor());
                constructor.Attributes = MemberAttributes.Public;
                constructor.ReturnType = generatedType.GetReferenceForType();
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(classRef, "parent"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("parent"));
                constructor.WriteDocumentation("Creates a new extension instance for the given parent", null, new Dictionary<string, string>() { { "parent", "The parent model element" } });
                if (!generatedType.Members.Contains(constructor))
                {
                    generatedType.Members.Add(constructor);
                }
            }

            /// <summary>
            /// Generates the static From method, extracting the extension from a given model element
            /// </summary>
            /// <param name="input">The NMeta extension</param>
            /// <param name="generatedType">The generated type declaration for the given extension</param>
            /// <param name="context">The transformation context</param>
            protected virtual void CreateFromMethod(IExtension input, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var classRef = CreateReference(input.AdornedClass, true, context);
                var thisClassRef = new CodeTypeReference(generatedType.Name);
                var fromMethod = new CodeMemberMethod()
                {
                    Name = "From" + input.AdornedClass.Name.ToPascalCase(),
                    Attributes = MemberAttributes.Static | MemberAttributes.Public,
                    ReturnType = thisClassRef
                };
                fromMethod.Parameters.Add(new CodeParameterDeclarationExpression(classRef, "parent"));
                var parentRef = new CodeArgumentReferenceExpression("parent");
                var nullRef = new CodePrimitiveExpression(null);

                fromMethod.Statements.Add(
                    new CodeConditionStatement(new CodeBinaryOperatorExpression(parentRef, CodeBinaryOperatorType.IdentityEquality, nullRef), new CodeMethodReturnStatement(nullRef)));

                fromMethod.Statements.Add(new CodeVariableDeclarationStatement(thisClassRef, "extension",
                    new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(parentRef, "GetExtension", thisClassRef))));

                var extensionRef = new CodeVariableReferenceExpression("extension");

                fromMethod.Statements.Add(
                    new CodeConditionStatement(new CodeBinaryOperatorExpression(extensionRef, CodeBinaryOperatorType.IdentityInequality, nullRef), new CodeMethodReturnStatement(extensionRef)));

                fromMethod.Statements.Add(new CodeAssignStatement(extensionRef, new CodeObjectCreateExpression(thisClassRef, parentRef)));
                fromMethod.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(parentRef, "Extensions"), "Add", extensionRef));
                fromMethod.Statements.Add(new CodeMethodReturnStatement(extensionRef));
                fromMethod.WriteDocumentation(string.Format("Gets the {0} extension from the given model element", input.Name), "The extension object or null, if the model element does not have this extension",
                    new Dictionary<string, string>() {
                        {"parent", "The parent model element that may hold the extension"}
                    });
                generatedType.Members.Add(fromMethod);
            }

            /// <summary>
            /// Creates a method to return the extension type of the current instance
            /// </summary>
            /// <param name="extension">The extension</param>
            /// <param name="generatedType">The type generated for the given extension</param>
            /// <param name="context">The transformation context in which the code is generated</param>
            protected virtual void CreateGetExtension(IExtension extension, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                var getExtension = new CodeMemberMethod()
                {
                    Name = "GetExtension",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = typeof(IExtension).ToTypeReference()
                };

                var uri = extension.AbsoluteUri;
                if (uri != null && uri.IsAbsoluteUri)
                {
                    var extensionType = new CodeMemberField(getExtension.ReturnType, "_extensionType");
                    extensionType.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                    var extensionTypeRef = new CodeFieldReferenceExpression(null, extensionType.Name);
                    generatedType.Members.Add(extensionType);
                    var nullRef = new CodePrimitiveExpression(null);
                    getExtension.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(extensionTypeRef, CodeBinaryOperatorType.IdentityEquality, nullRef),
                        new CodeAssignStatement(extensionTypeRef, new CodeCastExpression(extensionType.Type, new CodeMethodInvokeExpression(
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(MetaRepository).ToTypeReference()), "Instance"),
                        "Resolve",
                        new CodePrimitiveExpression(uri.AbsoluteUri))))));
                    getExtension.Statements.Add(new CodeMethodReturnStatement(extensionTypeRef));
                }
                else
                {
                    getExtension.ThrowException<NotSupportedException>(new CodePrimitiveExpression(string.Format("The extension {0} has no absolute Uri and can therefore not be reflected.", extension)));
                }
                getExtension.WriteDocumentation("Gets the extension model element for the given model extension class");
                generatedType.Members.Add(getExtension);
            }

            /// <summary>
            /// Registers the dependencies, i.e. marks the transformation rule instantiating for Type2Type and requires the transformation of the members
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());

                var t = Transformation as Meta2ClassesTransformation;

                if (t == null || t.CreateOperations)
                {
                    RequireGenerateMethods(Rule<Operation2Method>(), cl => cl.Operations);
                }

                RequireGenerateProperties(Rule<Reference2Property>(), cl => cl.References);
                RequireGenerateProperties(Rule<Attribute2Property>(), cl => cl.Attributes);
                RequireEvents(Rule<Event2Event>(), cl => cl.Events);
            }
        }
    }
}

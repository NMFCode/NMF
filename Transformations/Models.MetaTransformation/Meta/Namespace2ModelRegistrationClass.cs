using NMF.CodeGen;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        public class Namespace2ModelRegistrationClass : TransformationRule<INamespace, CodeTypeDeclaration>
        {

            public override CodeTypeDeclaration CreateOutput(INamespace input, ITransformationContext context)
            {
                return CodeDomHelper.CreateTypeDeclarationWithReference("ClassRegistration");
            }

            public override void Transform(INamespace input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Attributes = MemberAttributes.Assembly | MemberAttributes.Static;
                var thisTypeReference = output.GetReferenceForType();
                output.Members.Add(new CodeMemberField(typeof(Type[]), "_containedTypes"));
                output.Members.Add(CreateContainedTypes(thisTypeReference));
                output.Members.Add(CreateTypeConstructor(thisTypeReference, context));
            }

            protected virtual CodeTypeConstructor CreateTypeConstructor(CodeTypeReference thisType, ITransformationContext context)
            {
                var typeConstructor = new CodeTypeConstructor();
                var models = context.GetModels(false);
                var t = Transformation as Meta2ClassesTransformation;
                if (models != null && t != null)
                {
                    var metaRepository = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(MetaRepository)), "Instance");
                    typeConstructor.Statements.Add(new CodeVariableDeclarationStatement(typeof(Assembly), "assembly", new CodePropertyReferenceExpression(new CodeTypeOfExpression(thisType), "Assembly")));
                    var assembly = new CodeVariableReferenceExpression("assembly");
                    foreach (var model in models)
                    {
                        if (model.ModelUri != null)
                        {
                            typeConstructor.Statements.Add(new CodeMethodInvokeExpression(metaRepository, "Register", new CodePrimitiveExpression(model.ModelUri.AbsoluteUri), assembly,
                                new CodePrimitiveExpression(t.GetResourceKey(model))));
                        }
                    }
                }
                var rootClasses = context.GetRootClasses(false);
                if (rootClasses != null)
                {
                    var class2Type = Rule<Class2Type>();
                    var allTypes = rootClasses.Select(c => new CodeTypeOfExpression(context.Trace.ResolveIn(class2Type, c).GetReferenceForType())).ToArray();
                    var containedTypesRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(thisType), "_containedType");
                    typeConstructor.Statements.Add(new CodeAssignStatement(containedTypesRef, new CodeArrayCreateExpression(typeof(Type[]), allTypes)));
                }
                return typeConstructor;
            }

            protected virtual CodeMemberProperty CreateContainedTypes(CodeTypeReference thisType)
            {
                var containedTypes = new CodeMemberProperty()
                {
                    Name = "ContainedTypes",
                    Type = new CodeTypeReference(typeof(IEnumerable<Type>).Name),
                    Attributes = MemberAttributes.Public | MemberAttributes.Static | MemberAttributes.Final
                };
                containedTypes.WriteDocumentation("Gets the root types contained in this assembly");
                containedTypes.GetStatements.Add(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(thisType), "_containedTypes"));
                return containedTypes;
            }
        }
    }
}

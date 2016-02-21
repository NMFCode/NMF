using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;
using NMF.Expressions;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        public class Feature2Proxy : TransformationRule<ITypedElement, CodeTypeDeclaration>
        {
            public override void Transform(ITypedElement feature, CodeTypeDeclaration generatedType, ITransformationContext context)
            {
                generatedType.Name = feature.Name.ToPascalCase() + "Proxy";
                generatedType.Attributes = MemberAttributes.Private | MemberAttributes.Final;

                var type = context.Trace.ResolveIn(Rule<Type2Type>(), feature.Type).GetReferenceForType();
                var declaringType = context.Trace.ResolveIn(Rule<Type2Type>(), feature.Parent as IType).GetReferenceForType();
                generatedType.BaseTypes.Add(new CodeTypeReference("ModelPropertyChange", declaringType, type));

                var modelElementRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "ModelElement");
                var propertyRef = new CodePropertyReferenceExpression(modelElementRef, feature.Name.ToPascalCase());
                var propertyChanged = new CodeEventReferenceExpression(modelElementRef, feature.Name.ToPascalCase() + "Changed");

                var value = new CodeMemberProperty()
                {
                    Name = "Value",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Type = type
                };
                value.GetStatements.Add(new CodeMethodReturnStatement(propertyRef));
                value.SetStatements.Add(new CodeAssignStatement(propertyRef, new CodePropertySetValueReferenceExpression()));
                generatedType.Members.Add(value);

                var handlerParameter = new CodeParameterDeclarationExpression(typeof(EventHandler<ValueChangedEventArgs>), "handler");
                var handlerParameterRef = new CodeArgumentReferenceExpression("handler");

                var registerChangeEventHandler = new CodeMemberMethod()
                {
                    Name = "RegisterChangeEventHandler",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = null
                };
                registerChangeEventHandler.Parameters.Add(handlerParameter);
                registerChangeEventHandler.Statements.Add(new CodeAttachEventStatement(propertyChanged, handlerParameterRef));
                generatedType.Members.Add(registerChangeEventHandler);

                var unregisterChangeEventHandler = new CodeMemberMethod()
                {
                    Name = "UnregisterChangeEventHandler",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = null
                };
                unregisterChangeEventHandler.Parameters.Add(handlerParameter);
                unregisterChangeEventHandler.Statements.Add(new CodeRemoveEventStatement(propertyChanged, handlerParameterRef));
                generatedType.Members.Add(unregisterChangeEventHandler);

                var constructor = new CodeConstructor()
                {
                    Attributes = MemberAttributes.Public
                };
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(declaringType, "modelElement"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("modelElement"));
                generatedType.Members.Add(constructor);
            }
        }
    }
}

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
                generatedType.TypeAttributes = System.Reflection.TypeAttributes.NestedPrivate | System.Reflection.TypeAttributes.Sealed;
                generatedType.WriteDocumentation(string.Format("Represents a proxy to represent an incremental access to the {0} property", feature.Name));

                var type = CreateReference(feature.Type, feature is IReference, context);
                var t = Transformation as Meta2ClassesTransformation;
                if ((t == null || t.IsValueType(feature.Type)) && feature.LowerBound == 0 && feature.UpperBound == 1)
                {
                    type = new CodeTypeReference(typeof(System.Nullable<>).Name, type);
                }

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
                value.WriteDocumentation("Gets or sets the value of this expression");
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
                registerChangeEventHandler.WriteDocumentation("Registers an event handler to subscribe specifically on the changed event for this property", null, new Dictionary<string, string>()
                {
                    { "handler", "The handler that should be subscribed to the property change event" }
                });
                registerChangeEventHandler.Parameters.Add(handlerParameter);
                registerChangeEventHandler.Statements.Add(new CodeAttachEventStatement(propertyChanged, handlerParameterRef));
                generatedType.Members.Add(registerChangeEventHandler);

                var unregisterChangeEventHandler = new CodeMemberMethod()
                {
                    Name = "UnregisterChangeEventHandler",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = null
                };
                unregisterChangeEventHandler.WriteDocumentation("Registers an event handler to subscribe specifically on the changed event for this property", null, new Dictionary<string, string>()
                {
                    { "handler", "The handler that should be unsubscribed from the property change event" }
                });
                unregisterChangeEventHandler.Parameters.Add(handlerParameter);
                unregisterChangeEventHandler.Statements.Add(new CodeRemoveEventStatement(propertyChanged, handlerParameterRef));
                generatedType.Members.Add(unregisterChangeEventHandler);

                var constructor = new CodeConstructor()
                {
                    Attributes = MemberAttributes.Public
                };
                constructor.WriteDocumentation("Creates a new observable property access proxy", null, new Dictionary<string, string>()
                {
                    { "modelElement", "The model instance element for which to create the property access proxy" }
                });
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(declaringType, "modelElement"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("modelElement"));
                generatedType.Members.Add(constructor);
            }
        }
    }
}

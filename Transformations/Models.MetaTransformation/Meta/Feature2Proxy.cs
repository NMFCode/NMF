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
        /// <summary>
        /// Denotes the transformation rule from a feature to a proxy type
        /// </summary>
        public class Feature2Proxy : TransformationRule<ITypedElement, CodeTypeDeclaration>
        {
            /// <inheritdoc />
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

                var property = context.Trace.ResolveIn(Rule<Feature2Property>(), feature);

                var propertyRef = new CodePropertyReferenceExpression(modelElementRef, property.Name);
                var propertyChanged = new CodeEventReferenceExpression(modelElementRef, property.Name + "Changed");

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
                constructor.BaseConstructorArgs.Add(new CodePrimitiveExpression(feature.Name));
                generatedType.Members.Add(constructor);
            }
        }
    }
}

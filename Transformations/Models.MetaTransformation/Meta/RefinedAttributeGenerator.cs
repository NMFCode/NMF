using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;
using NMF.Models.Meta;
using System.CodeDom;
using NMF.CodeGen;
using NMF.Utilities;
using NMF.Collections.Generic;
using NMF.Transformations.Core;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to generate a refined attribute
        /// </summary>
        public class RefinedAttributeGenerator : TransformationRule<IClass, IAttribute, CodeMemberProperty>
        {
            /// <summary>
            /// Initializes the generated code property for the refined attribute
            /// </summary>
            /// <param name="scope">The scope in which the attribute is refined</param>
            /// <param name="attribute">The NMeta attribute that is refined</param>
            /// <param name="property">The generated code property</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IClass scope, IAttribute attribute, CodeMemberProperty property, ITransformationContext context)
            {
                if (!scope.Closure(c => c.BaseTypes).Contains((IClass)attribute.DeclaringType))
                {
                    throw new System.InvalidOperationException(string.Format("The attribute {0} cannot be refined in the scope of class {1} because {1} does not inherit from its declaring class.", attribute.Name, scope.Name));
                }

                var classDeclaration = context.Trace.ResolveIn(Rule<Type2Type>(), scope);
                var originalAttribute = context.Trace.ResolveIn(Rule<Attribute2Property>(), attribute);

                property.Attributes = MemberAttributes.Private;
                property.Name = attribute.Name.ToPascalCase();
                property.PrivateImplementationType = CreateReference(attribute.DeclaringType, false, context);

                lock (classDeclaration)
                {
                    classDeclaration.Shadows(true).Add(originalAttribute);
                    classDeclaration.DependentMembers(true).Add(property);
                }

                var implementations = scope.Attributes.Where(att => att.Refines == attribute).ToList();
                var constraint = scope.AttributeConstraints.Where(c => c.Constrains == attribute).FirstOrDefault();

                if (implementations.Count == 0 && constraint == null) throw new System.InvalidOperationException("The RefinedAttributeGenerator rule was called irregularily as the attribute in question was not refined!");

                var attributeType = CreateReference(attribute.Type, false, context);

                if (attribute.UpperBound == 1)
                {
                    property.Type = attributeType;

                    if (implementations.Count > 1)
                    {
                        throw new System.InvalidOperationException("A single value typed attribute may only be refined once!");
                    }
                    else if (implementations.Count == 1)
                    {
                        if (constraint != null) throw new System.InvalidOperationException("A single values attribute must not be constrained and implemented at the same time!");
                        if (implementations[0].Type != attribute.Type) throw new System.InvalidOperationException("The refining attribute has a different type than the original attribute. Covariance is not supported for attributes!");

                        var castedThisVariable = new CodeVariableDeclarationStatement(classDeclaration.GetReferenceForType(), "_this", new CodeThisReferenceExpression());
                        var castedThisVariableRef = new CodeVariableReferenceExpression("_this");
                        property.GetStatements.Add(castedThisVariable);
                        property.SetStatements.Add(castedThisVariable);
                        CodeExpression implementationRef = new CodePropertyReferenceExpression(castedThisVariableRef, implementations[0].Name.ToPascalCase());

                        property.GetStatements.Add(new CodeMethodReturnStatement(implementationRef));
                        property.SetStatements.Add(new CodeAssignStatement(implementationRef, new CodePropertySetValueReferenceExpression()));
                    }
                    else
                    {
                        var ifNotDefault = new CodeConditionStatement();
                        ifNotDefault.TrueStatements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(System.NotSupportedException))));
                        CodeExpression value;
                        if (constraint.Values.Count == 0)
                        {
                            value = new CodeDefaultValueExpression(attributeType);
                        }
                        else
                        {
                            value = CodeDomHelper.CreatePrimitiveExpression(constraint.Values[0], attributeType);
                        }
                        property.GetStatements.Add(new CodeMethodReturnStatement(value));
                        ifNotDefault.Condition = new CodeBinaryOperatorExpression(new CodePropertySetValueReferenceExpression(), CodeBinaryOperatorType.IdentityInequality, value);
                        property.SetStatements.Add(ifNotDefault);
                    }

                    CreateChangeEvent(property, implementations, context, "EventHandler<ValueChangedEventArgs>", "Changed");
                    CreateChangeEvent(property, implementations, context, "EventHandler", "Changing");
                }
                else
                {
                    if (attribute.IsUnique) throw new System.InvalidOperationException("Unique attributes must not be refined.");

                    if (implementations.Count > 0 || (constraint != null && constraint.Values.Count > 0))
                    {
                        var collectionType = context.Trace.ResolveIn(Rule<RefinedAttributeCollectionClassGenerator>(), scope, attribute);
                        property.GetStatements.Add(new CodeMethodReturnStatement(new CodeObjectCreateExpression(collectionType.GetReferenceForType(), new CodeThisReferenceExpression())));
                        property.DependentTypes(true).Add(collectionType);
                    }
                    else
                    {
                        property.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(new CodeTypeReference(typeof(EmptyList<>).Name, attributeType)), "Instance")));
                    }
                }
            }

            private static void CreateChangeEvent(CodeMemberProperty property, List<IAttribute> implementations, ITransformationContext context, string eventHandler, string eventNameSuffix)
            {
                var eventSnippet = @"        event {4} {0}.{1}
        {{
            add
            {{{2}
            }}
            remove
            {{{3}
            }}
        }}";
                string addEvents = string.Empty;
                string removeEvents = string.Empty;
                var casts = new HashSet<string>();
                foreach (var impl in implementations)
                {
                    var implTypeRef = CreateReference(impl.DeclaringType, true, context);
                    var name = impl.Name.ToPascalCase() + eventNameSuffix;
                    if (casts.Add(implTypeRef.BaseType))
                    {
                        var thisDeclaration = string.Format("\r\n                I{0} _this_{0} = this;", implTypeRef.BaseType);
                        addEvents += thisDeclaration;
                        removeEvents += thisDeclaration;
                    }
                    addEvents += string.Format("\r\n                _this_{1}.{0} += value;", name, implTypeRef.BaseType);
                    removeEvents += string.Format("\r\n                _this_{1}.{0} -= value;", name, implTypeRef.BaseType);
                }
                eventSnippet = string.Format(eventSnippet, property.PrivateImplementationType.BaseType, property.Name + eventNameSuffix, addEvents, removeEvents, eventHandler);
                property.DependentMembers(true).Add(new CodeSnippetTypeMember(eventSnippet));
            }

            public override void RegisterDependencies()
            {
                Require(Rule<RefinedAttributeCollectionClassGenerator>(), (scope, att) => att.UpperBound != 1 && !att.IsUnique);
            }
        }
    }
}

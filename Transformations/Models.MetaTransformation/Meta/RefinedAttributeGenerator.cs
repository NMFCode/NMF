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
using System;
using NMF.Analyses;

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
                var baseTypes = Layering<IClass>.CreateLayers(scope, c => c.BaseTypes).Select(c => c.Single()).ToList();

                if (!baseTypes.Contains((IClass)attribute.DeclaringType))
                {
                    throw new InvalidOperationException(string.Format("The attribute {0} cannot be refined in the scope of class {1} because {1} does not inherit from its declaring class.", attribute.Name, scope.Name));
                }

                var classDeclaration = context.Trace.ResolveIn(Rule<Type2Type>(), scope);
                var originalAttribute = context.Trace.ResolveIn(Rule<Attribute2Property>(), attribute);

                property.Attributes = MemberAttributes.Private;
                property.Name = originalAttribute.Name;
                property.PrivateImplementationType = CreateReference(attribute.DeclaringType, false, context);

                lock (classDeclaration)
                {
                    classDeclaration.Shadows(true).Add(originalAttribute);
                    classDeclaration.DependentMembers(true).Add(property);
                }

                var implementations = baseTypes.SelectMany(s => s.Attributes).Where(att => att.Refines == attribute).ToList();
                var constraints = baseTypes.SelectMany(s => s.AttributeConstraints).Where(rc => rc.Constrains == attribute);

                foreach (var declClass in implementations.Select(a => a.DeclaringType).OfType<IClass>().Concat(constraints.Select(c => c.DeclaringType)).Distinct())
                {
                    if (declClass != scope)
                    {
                        var refinedAttribute = context.Trace.ResolveIn(this, declClass, attribute);
                        if (refinedAttribute != null)
                        {
                            property.Shadows(true).Add(refinedAttribute);
                        }
                    }
                }

                if (implementations.Count == 0 && !constraints.Any())
                {
                    throw new InvalidOperationException(
                        string.Format("The attribute {0} can not be refined in the scope of class {1} because no reference refines it. ", attribute, scope)
                    );
                }

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
                        if (constraints.Any()) throw new System.InvalidOperationException("A single values attribute must not be constrained and implemented at the same time!");
                        if (implementations[0].Type != attribute.Type) throw new System.InvalidOperationException("The refining attribute has a different type than the original attribute. Covariance is not supported for attributes!");

                        var castedThisVariable = new CodeVariableDeclarationStatement(classDeclaration.GetReferenceForType(), "_this", new CodeThisReferenceExpression());
                        var castedThisVariableRef = new CodeVariableReferenceExpression("_this");
                        property.GetStatements.Add(castedThisVariable);
                        property.SetStatements.Add(castedThisVariable);

                        var implProperty = context.Trace.ResolveIn(Rule<Attribute2Property>(), implementations[0]);
                        CodeExpression implementationRef = new CodePropertyReferenceExpression(castedThisVariableRef, implProperty.Name);

                        property.GetStatements.Add(new CodeMethodReturnStatement(implementationRef));
                        property.SetStatements.Add(new CodeAssignStatement(implementationRef, new CodePropertySetValueReferenceExpression()));
                    }
                    else
                    {
                        var constraint = constraints.Last();
                        var ifNotDefault = new CodeConditionStatement();
                        ifNotDefault.TrueStatements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(System.NotSupportedException))));
                        CodeExpression value;
                        if (constraints.Sum(c => c.Values.Count) == 0)
                        {
                            value = new CodeDefaultValueExpression(attributeType);
                        }
                        else
                        {
                            value = CodeDomHelper.CreatePrimitiveExpression(constraint.Values[0], attributeType, attribute.Type is IEnumeration);
                            if (value == null)
                            {
                                throw new InvalidOperationException(string.Format("The value {0} could not be serialized as a value for {1}", constraint.Values[0], attribute));
                            }
                        }
                        property.GetStatements.Add(new CodeMethodReturnStatement(value));
                        ifNotDefault.Condition = new CodeBinaryOperatorExpression(new CodePropertySetValueReferenceExpression(), CodeBinaryOperatorType.IdentityInequality, value);
                        property.SetStatements.Add(ifNotDefault);
                    }

                    CreateChangeEvent(property, implementations, context, "Changed");
                    CreateChangeEvent(property, implementations, context, "Changing");
                }
                else
                {
                    if (attribute.IsUnique) throw new System.InvalidOperationException("Unique attributes must not be refined.");

                    if (implementations.Count > 0 || constraints.Any(c => c.Values.Any()))
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

            private static void CreateChangeEvent(CodeMemberProperty property, List<IAttribute> implementations, ITransformationContext context, string eventNameSuffix)
            {
                var eventSnippet = @"        event EventHandler<ValueChangedEventArgs> {0}.{1}
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
                eventSnippet = string.Format(eventSnippet, property.PrivateImplementationType.BaseType, property.Name + eventNameSuffix, addEvents, removeEvents);
                property.DependentMembers(true).Add(new CodeSnippetTypeMember(eventSnippet));
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                Require(Rule<RefinedAttributeCollectionClassGenerator>(), (scope, att) => att.UpperBound != 1 && !att.IsUnique);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;
using NMF.Models.Meta;
using System.CodeDom;
using NMF.Utilities;
using NMF.CodeGen;
using NMF.Models.Repository;
using NMF.Collections.Generic;
using NMF.Transformations.Core;
using System;
using NMF.Analyses;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to generate a refined reference
        /// </summary>
        public class RefinedReferenceGenerator : TransformationRule<IClass, IReference, CodeMemberProperty>
        {
            /// <inheritdoc />
            public override CodeMemberProperty CreateOutput(IClass input1, IReference input2, ITransformationContext context)
            {
                return base.CreateOutput(input1, input2, context);
            }

            /// <inheritdoc />
            public override void Transform(IClass scope, IReference reference, CodeMemberProperty property, Transformations.Core.ITransformationContext context)
            {
                var baseTypes = Layering<IClass>.CreateLayers(scope, c => c.BaseTypes).Select(c => c.Single()).ToList();

                if (!baseTypes.Contains((IClass)reference.DeclaringType))
                {
                    throw new InvalidOperationException(string.Format("The reference {0} cannot be refined in the scope of class {1} because {1} does not inherit from its declaring class.", reference.Name, scope.Name));
                }

                var classDeclaration = context.Trace.ResolveIn(Rule<Type2Type>(), scope);
                var originalReference = context.Trace.ResolveIn(Rule<Reference2Property>(), reference);

                property.Attributes = MemberAttributes.Private;
                property.Name = originalReference.Name;
                property.PrivateImplementationType = CreateReference(reference.DeclaringType, false, context);
                property.Type = originalReference.Type;

                lock (classDeclaration)
                {
                    classDeclaration.Shadows(true).Add(originalReference);
                    classDeclaration.DependentMembers(true).Add(property);
                }

                var implementations = baseTypes.SelectMany(s => s.References).Where(r => r.Refines == reference).ToList();
                var constraints = baseTypes.SelectMany(s => s.ReferenceConstraints).Where(rc => rc.Constrains == reference);

                foreach (var declClass in implementations.Select(a => a.DeclaringType).OfType<IClass>().Concat(constraints.Select(c => c.DeclaringType)).Distinct())
                {
                    if (declClass != scope)
                    {
                        var refinedReference = context.Trace.ResolveIn(this, declClass, reference);
                        if (refinedReference != null)
                        {
                            property.Shadows(true).Add(refinedReference);
                        }
                    }
                }

                if (implementations.Count == 0 && !constraints.Any())
                {
                    throw new InvalidOperationException(
                        string.Format("The reference {0} can not be refined in the scope of class {1} because no reference refines it. ", reference, scope)
                    );
                }

                var referenceType = CreateReference(reference.Type, true, context);

                if (reference.UpperBound == 1)
                {
                    var nullRef = new CodePrimitiveExpression(null);
                    if (!constraints.Any())
                    {
                        var castedThisVariable = new CodeVariableDeclarationStatement(classDeclaration.GetReferenceForType(), "_this", new CodeThisReferenceExpression());
                        var castedThisVariableRef = new CodeVariableReferenceExpression("_this");
                        property.GetStatements.Add(castedThisVariable);
                        property.SetStatements.Add(castedThisVariable);
                        var setRef = new CodePropertySetValueReferenceExpression();
                        var ifNull = new CodeConditionStatement();
                        ifNull.Condition = new CodeBinaryOperatorExpression(setRef, CodeBinaryOperatorType.IdentityInequality, nullRef);
                        var foundMatch = false;

                        foreach (var implementation in implementations)
                        {
                            var implementationRef = new CodePropertyReferenceExpression(castedThisVariableRef, context.Trace.ResolveIn(Rule<Reference2Property>(), implementation).Name);

                            if (implementation.Type == reference.Type)
                            {
                                property.GetStatements.Add(new CodeMethodReturnStatement(implementationRef));
                                property.SetStatements.Add(new CodeAssignStatement(implementationRef, setRef));
                                foundMatch = true;
                                break;
                            }
                            else
                            {
                                var getIfStmt = new CodeConditionStatement();
                                getIfStmt.Condition = new CodeBinaryOperatorExpression(implementationRef, CodeBinaryOperatorType.IdentityInequality, nullRef);
                                getIfStmt.TrueStatements.Add(new CodeMethodReturnStatement(implementationRef));
                                property.GetStatements.Add(getIfStmt);

                                var implementationType = CreateReference(implementation.Type, true, context);
                                var asRef = new CodeMethodReferenceExpression(setRef, "As", implementationType);
                                var localVar = new CodeVariableDeclarationStatement(implementationType, "__" + implementation.Name, new CodeMethodInvokeExpression(asRef));
                                var localVarRef = new CodeVariableReferenceExpression(localVar.Name);
                                var setIfStmt = new CodeConditionStatement();
                                setIfStmt.Condition = new CodeBinaryOperatorExpression(localVarRef, CodeBinaryOperatorType.IdentityInequality, nullRef);
                                setIfStmt.TrueStatements.Add(new CodeAssignStatement(implementationRef, localVarRef));
                                setIfStmt.TrueStatements.Add(new CodeMethodReturnStatement());
                                ifNull.TrueStatements.Add(localVar);
                                ifNull.TrueStatements.Add(setIfStmt);
                                ifNull.FalseStatements.Add(new CodeAssignStatement(implementationRef, nullRef));
                            }
                        }
                        ifNull.FalseStatements.Add(new CodeMethodReturnStatement());

                        if (ifNull.TrueStatements.Count > 0)
                        {
                            property.SetStatements.Add(ifNull);
                        }

                        if (!foundMatch)
                        {
                            property.GetStatements.Add(new CodeMethodReturnStatement(nullRef));
                            property.SetStatements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(System.ArgumentException), new CodePrimitiveExpression("There was no suitable refining reference found for this object"))));
                        }
                    }
                    else
                    {
                        var constraint = constraints.Last();
                        var ifNotDefault = new CodeConditionStatement();
                        ifNotDefault.TrueStatements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(System.NotSupportedException))));
                        CodeExpression value;
                        if (constraint.References.Count == 0)
                        {
                            value = nullRef;
                        }
                        else
                        {
                            var refEl = constraint.References[0];
                            var uri = refEl.AbsoluteUri;
                            if (uri == null) throw new System.InvalidOperationException();
                            var metaRepositoryInstance = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(MetaRepository)), "Instance");
                            var refElExpression = new CodeMethodInvokeExpression(metaRepositoryInstance, "Resolve", new CodePrimitiveExpression(uri.AbsoluteUri));
                            refElExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(refElExpression, "As", property.Type));

                            var retrieveValueMethod = new CodeMemberMethod
                            {
                                Name = "Retrieve" + reference.Name.ToPascalCase(),
                                Attributes = MemberAttributes.Private | MemberAttributes.Static,
                                ReturnType = property.Type
                            };
                            retrieveValueMethod.Statements.Add(new CodeMethodReturnStatement(refElExpression));
                            property.DependentMembers(true).Add(retrieveValueMethod);

                            var staticField = new CodeMemberField(new CodeTypeReference("Lazy", property.Type), "_" + reference.Name.ToPascalCase());
                            staticField.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                            staticField.InitExpression = new CodeObjectCreateExpression(staticField.Type, new CodeMethodReferenceExpression(null, retrieveValueMethod.Name));
                            property.DependentMembers(true).Add(staticField);
                            value = new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(null, staticField.Name), "Value");
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
                    if (reference.IsUnique) throw new System.InvalidOperationException("Unique references must not be refined!");

                    if (implementations.Count > 0 || constraints.Any(c => c.References.Any()))
                    {
                        var collectionType = context.Trace.ResolveIn(Rule<RefinedReferenceCollectionClassGenerator>(), scope, reference);
                        property.GetStatements.Add(new CodeMethodReturnStatement(new CodeObjectCreateExpression(collectionType.GetReferenceForType(), new CodeThisReferenceExpression())));
                        property.DependentTypes(true).Add(collectionType);
                    }
                    else
                    {
                        property.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(new CodeTypeReference(typeof(EmptyList<>).Name, referenceType)), "Instance")));
                    }
                }
            }

            private void CreateChangeEvent(CodeMemberProperty property, List<IReference> implementations, ITransformationContext context, string eventNameSuffix)
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
                    var implTypeRefString = implTypeRef.BaseType;
                    if (impl.DeclaringType.GetExtension<MappedType>() == null)
                    {
                        implTypeRefString = "I" + implTypeRefString;
                    }
                    if (casts.Add(implTypeRef.BaseType))
                    {
                        var thisDeclaration = string.Format("\r\n                {0} _this_{0} = this;", implTypeRefString);
                        addEvents += thisDeclaration;
                        removeEvents += thisDeclaration;
                    }
                    addEvents += string.Format("\r\n                _this_{1}.{0} += value;", name, implTypeRefString);
                    removeEvents += string.Format("\r\n                _this_{1}.{0} -= value;", name, implTypeRefString);
                }
                eventSnippet = string.Format(eventSnippet, property.PrivateImplementationType.BaseType, property.Name + eventNameSuffix, addEvents, removeEvents);
                property.DependentMembers(true).Add(new CodeSnippetTypeMember(eventSnippet));
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                Require(Rule<RefinedReferenceCollectionClassGenerator>(), (scope, reference) => 
                reference.UpperBound != 1 && !reference.IsUnique);
            }
        }
    }
}

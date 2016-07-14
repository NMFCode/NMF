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

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to 
        /// </summary>
        public class RefinedReferenceGenerator : TransformationRule<IClass, IReference, CodeMemberProperty>
        {
            public override CodeMemberProperty CreateOutput(IClass input1, IReference input2, ITransformationContext context)
            {
                return base.CreateOutput(input1, input2, context);
            }

            public override void Transform(IClass scope, IReference reference, CodeMemberProperty property, Transformations.Core.ITransformationContext context)
            {
                if (!scope.Closure(c => c.BaseTypes).Contains((IClass)reference.DeclaringType))
                {
                    throw new System.InvalidOperationException(string.Format("The reference {0} cannot be refined in the scope of class {1} because {1} does not inherit from its declaring class.", reference.Name, scope.Name));
                }

                var classDeclaration = context.Trace.ResolveIn(Rule<Type2Type>(), scope);
                var originalReference = context.Trace.ResolveIn(Rule<Reference2Property>(), reference);

                property.Attributes = MemberAttributes.Private;
                property.Name = reference.Name.ToPascalCase();
                property.PrivateImplementationType = CreateReference(reference.DeclaringType, false, context);
                property.Type = originalReference.Type;

                lock (classDeclaration)
                {
                    classDeclaration.Shadows(true).Add(originalReference);
                    classDeclaration.DependentMembers(true).Add(property);
                }

                var implementations = scope.References.Where(r => r.Refines == reference).ToList();
                var constraint = scope.ReferenceConstraints.FirstOrDefault(r => r.Constrains == reference);

                if (implementations.Count == 0 && constraint == null) throw new System.InvalidOperationException();

                var referenceType = CreateReference(reference.Type, true, context);

                if (reference.UpperBound == 1)
                {
                    var nullRef = new CodePrimitiveExpression(null);
                    if (constraint == null)
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
                            var implementationRef = new CodePropertyReferenceExpression(castedThisVariableRef, implementation.Name.ToPascalCase());

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
                            var staticField = new CodeMemberField(property.Type, "_" + reference.Name.ToPascalCase());
                            staticField.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                            staticField.InitExpression = refElExpression;
                            property.DependentMembers(true).Add(staticField);
                            value = new CodeFieldReferenceExpression(null, staticField.Name);
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
                    if (reference.IsUnique) throw new System.InvalidOperationException("Unique references must not be refined!");

                    if (implementations.Count > 0 || (constraint != null && constraint.References.Count > 0))
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

            private void CreateChangeEvent(CodeMemberProperty property, List<IReference> implementations, ITransformationContext context, string eventHandler, string eventNameSuffix)
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
                Require(Rule<RefinedReferenceCollectionClassGenerator>(), (scope, reference) => 
                reference.UpperBound != 1 && !reference.IsUnique);
            }
        }
    }
}

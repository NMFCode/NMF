using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.AnyText.Transformation
{
    internal class AnytextCodeGenerator : ReflectiveTransformation
    {
        private static CodeTypeReference CreateReference(IRule rule, ITransformationContext context)
        {
            if (rule is IParanthesisRule paranthesisRule)
            {
                return CreateReference(paranthesisRule.InnerRule, context);
            }
            if (rule.TypeName != null)
            {
                return new CodeTypeReference(rule.TypeName);
            }
            if (rule is IDataRule)
            {
                return new CodeTypeReference(typeof(string));
            }
            return new CodeTypeReference(rule.Name);
        }
        private static CodeTypeReference CreateRuleReference(IRule rule, RuleToClass ruleTransformatio, ITransformationContext context)
        {
            var generatedClass = context.Trace.ResolveIn(ruleTransformatio, rule);
            return new CodeTypeReference(generatedClass.Name);
        }

        private static CodeTypeReference CreateAssignmentReference(IFeatureExpression assignExpression, AssignmentToClass assignTransformation, ITransformationContext context)
        {
            var generatedClass = context.Trace.ResolveIn(assignTransformation, assignExpression);
            var lookupResult = CodeGenerator._trace.LookupFeature(assignExpression);
            if (generatedClass == null)
            {
                generatedClass = context.Trace.ResolveInWhere(assignTransformation, feat => CodeGenerator._trace.LookupFeature(feat) == lookupResult).First();
            }
            return new CodeTypeReference(generatedClass.Name);
        }

        private static CodeMemberMethod CreateInitializeMethod()
        {
            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Name = nameof(AnyText.Rules.Rule.Initialize),
                Parameters =
                {
                    new CodeParameterDeclarationExpression
                    {
                        Name = "context",
                        Type = new CodeTypeReference(nameof(GrammarContext))
                    }
                }
            };
        }

        private static CodeExpression _contextRef = new CodeArgumentReferenceExpression("context");

        private static CodeExpression CreateResolveKeyword(string keyword)
        {
            var unescaped = keyword?.Replace(@"\'", "'");
            return new CodeMethodInvokeExpression(_contextRef, nameof(GrammarContext.ResolveKeyword), new CodePrimitiveExpression(unescaped));
        }

        private static CodeExpression CreateResolveRule(CodeTypeReference ruleType)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(_contextRef, nameof(GrammarContext.ResolveRule), ruleType));
        }

        private static IFeatureExpression GetIdAssignment(IClassRule rule)
        {
            if (rule is IModelRule modelRule)
            {
                return modelRule.Descendants().OfType<IFeatureExpression>()
                    .FirstOrDefault(ft => string.Equals("Name", ft.Feature, StringComparison.OrdinalIgnoreCase));
            }
            else if (rule is IInheritanceRule inheritanceRule)
            {
                return GetIdAssignment(inheritanceRule.Subtypes.First());
            }
            throw new NotSupportedException();
        }

        private static CodeExpression CreateParserExpression(IParserExpression parserExpression, RuleToClass ruleTransformation, AssignmentToClass assignTransformation, ITransformationContext context)
        {
            switch (parserExpression)
            {
                case IKeywordExpression keywordExpression:
                    return CreateResolveKeyword(keywordExpression.Keyword);
                case IReferenceExpression referenceExpression:
                    var assignment = GetIdAssignment(referenceExpression.ReferencedRule);
                    return CreateParserExpression(assignment.Assigned, ruleTransformation, assignTransformation, context);
                case IRuleExpression ruleExpression:
                    return CreateResolveRule(CreateRuleReference(ruleExpression.Rule, ruleTransformation, context));
                case IFeatureExpression assignExpression:
                    return CreateResolveRule(CreateAssignmentReference(assignExpression, assignTransformation, context));
                case IMaybeExpression maybe:
                    return new CodeObjectCreateExpression(nameof(ZeroOrOneRule), CreateParserExpression(maybe.Inner, ruleTransformation, assignTransformation, context));
                case IPlusExpression plus:
                    return new CodeObjectCreateExpression(nameof(OneOrMoreRule), CreateParserExpression(plus.Inner, ruleTransformation, assignTransformation, context));
                case IStarExpression star:
                    return new CodeObjectCreateExpression(nameof(ZeroOrMoreRule), CreateParserExpression(star.Inner, ruleTransformation, assignTransformation, context));
                case ISequenceExpression sequence:
                    var sequenceExpression = new CodeObjectCreateExpression(nameof(SequenceRule));
                    foreach (var item in sequence.InnerExpressions)
                    {
                        sequenceExpression.Parameters.Add(CreateParserExpression(item, ruleTransformation, assignTransformation, context));
                    }
                    return sequenceExpression;
                case IChoiceExpression choice:
                    var choiceExpression = new CodeObjectCreateExpression(nameof(ChoiceRule));
                    foreach (var item in choice.Alternatives)
                    {
                        choiceExpression.Parameters.Add(CreateParserExpression(item, ruleTransformation, assignTransformation, context));
                    }
                    return choiceExpression;
                case INegativeLookaheadExpression negative:
                    return new CodeObjectCreateExpression(nameof(NegativeLookaheadRule), CreateParserExpression(negative.Inner, ruleTransformation, assignTransformation, context));
            }
            throw new NotSupportedException();
        }

        private static CodeTypeReference SynthesizeType(IParserExpression expression, RuleToClass ruleToClass, ITransformationContext context)
        {
            switch (expression)
            {
                case IRuleExpression ruleExpression:
                    return CreateReference(ruleExpression.Rule, context);
                case IUnaryParserExpression unary:
                    return SynthesizeType(unary.Inner, ruleToClass, context);
                case IChoiceExpression choice:
                    return SynthesizeType(choice.Alternatives[0], ruleToClass, context);
                case ISequenceExpression sequence:
                    return SynthesizeType(sequence.InnerExpressions[0], ruleToClass, context);
                case IFeatureExpression feature:
                    return SynthesizeType(feature.Assigned, ruleToClass, context);
                case IKeywordExpression:
                    return new CodeTypeReference(typeof(string));

            }
            return new CodeTypeReference(typeof(string));
        }

        private static (CodeTypeReference semanticType, CodeTypeReference propertyType) GetSemanticTypeForFeature(IFeatureExpression input, ITransformationContext context)
        {
            var feature = CodeGenerator._trace.LookupFeature(input);
            var cl = feature?.Parent as IType;
            if (cl != null)
            {
                var semanticType = new CodeTypeReference(cl.Name);
                if (feature.Type == null)
                {
                    if (feature is IReference)
                    {
                        return (semanticType, new CodeTypeReference(nameof(IModelElement)));
                    }
                    else
                    {
                        return (semanticType, new CodeTypeReference(typeof(string)));
                    }
                }
                if (feature.Type is IPrimitiveType primitiveType)
                {
                    return (semanticType, new CodeTypeReference(primitiveType.SystemType));
                }
                var mappedType = feature.Type.GetExtension<MappedType>();
                if (mappedType != null)
                {
                    return (semanticType, new CodeTypeReference(mappedType.SystemType));
                }
                return (semanticType, new CodeTypeReference(feature.Type.Name.ToPascalCase()));
            }
            var modelRule = input.Ancestors().OfType<IModelRule>().FirstOrDefault();
            if (modelRule != null)
            {
                var semanticType = CreateReference(modelRule, context);
                return (semanticType, SynthesizeType(input.Assigned, null, context));
            }
            var fragment = input.Ancestors().OfType<IFragmentRule>().FirstOrDefault();
            if (fragment != null)
            {
                var semanticType = CreateReference(fragment, context);
                return (semanticType, SynthesizeType(input.Assigned, null, context));
            }
            throw new NotSupportedException();
        }

        public class GrammarToClass : TransformationRule<IGrammar, CodeTypeDeclaration>
        {
            public override void Transform(IGrammar input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Name = input.Name.ToPascalCase() + "Grammar";
                output.BaseTypes.Add(nameof(ReflectiveGrammar));
            }

            public override void RegisterDependencies()
            {
                CallMany(Rule<RuleToClass>(), g => g.Rules, AddChildClasses);
                CallMany(Rule<AssignmentToClass>(),
                    g => GetAssignments(g),
                    AddChildClasses);
            }

            private static IEnumerable<IFeatureExpression> GetAssignments(IGrammar grammar)
            {
                return grammar.Rules
                   .OfType<IModelRule>()
                   .SelectMany(r => r.Descendants().OfType<IFeatureExpression>())
                   .DistinctBy(f => CodeGenerator._trace.LookupFeature(f));
            }

            private static void AddChildClasses(CodeTypeDeclaration cl, IEnumerable<CodeTypeDeclaration> rules)
            {
                cl.Members.AddRange(rules.ToArray());
            }
        }

        public class RuleToClass : AbstractTransformationRule<IRule,  CodeTypeDeclaration>
        {
            public override void Transform(IRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Attributes = MemberAttributes.Public;
                output.IsClass = true;
                output.IsPartial = true;
            }
        }

        public class ModelRuleToClass : TransformationRule<IModelRule, CodeTypeDeclaration>
        {
            private RuleToClass _ruleTransformation;

            public override CodeTypeDeclaration CreateOutput(IModelRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                _ruleTransformation = Rule<RuleToClass>();
                MarkInstantiatingFor(_ruleTransformation);
            }

            public override void Transform(IModelRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(new CodeTypeReference(nameof(ModelElementRule<object>), CreateReference(input, context)));

                var initialize = CreateInitializeMethod();
                IEnumerable<IParserExpression> innerExpressions;
                if (input.Expression is ISequenceExpression sequence)
                {
                    innerExpressions = sequence.InnerExpressions;
                }
                else
                {
                    innerExpressions = Enumerable.Repeat(input.Expression, 1);
                }
                var rules = new CodeArrayCreateExpression("Rule");
                var assignTransformation = Rule<AssignmentToClass>();
                foreach (var exp in innerExpressions)
                {
                    rules.Initializers.Add(CreateParserExpression(exp, _ruleTransformation, assignTransformation, context));
                }
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(SequenceRule.Rules)), rules));
                output.Members.Add(initialize);
            }
        }

        public class ParanthesisRuleToClass : TransformationRule<IParanthesisRule, CodeTypeDeclaration>
        {
            private RuleToClass _ruleTransformation;

            public override CodeTypeDeclaration CreateOutput(IParanthesisRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                _ruleTransformation = Rule<RuleToClass>();
                MarkInstantiatingFor(_ruleTransformation);
            }

            public override void Transform(IParanthesisRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(nameof(ParanthesesRule));

                var initialize = CreateInitializeMethod();
                var rules = new CodeArrayCreateExpression("Rule");
                var assignTransformation = Rule<AssignmentToClass>();
                rules.Initializers.Add(CreateParserExpression(input.OpeningParanthesis, _ruleTransformation, assignTransformation, context));
                rules.Initializers.Add(CreateResolveRule(CreateRuleReference(input.InnerRule, _ruleTransformation, context)));
                rules.Initializers.Add(CreateParserExpression(input.ClosingParanthesis, _ruleTransformation, assignTransformation, context));
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(SequenceRule.Rules)), rules));
                output.Members.Add(initialize);
            }

        }

        public class DataRuleToClass : TransformationRule<IDataRule, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IDataRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<RuleToClass>());
            }

            public override void Transform(IDataRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(nameof(RegexRule));

                var innerRegex = "^" + input.Regex[1..^1];

                var initialize = CreateInitializeMethod();
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(RegexRule.Regex)),
                    new CodeObjectCreateExpression(nameof(Regex), new CodePrimitiveExpression(innerRegex), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(nameof(RegexOptions)), nameof(RegexOptions.Compiled)))));
                output.Members.Add(initialize);
            }
        }

        public class FragmentRuleToClass : TransformationRule<IFragmentRule, CodeTypeDeclaration>
        {

            private RuleToClass _ruleTransformation;

            public override CodeTypeDeclaration CreateOutput(IFragmentRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                _ruleTransformation = Rule<RuleToClass>();
                MarkInstantiatingFor(_ruleTransformation);
            }

            public override void Transform(IFragmentRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(nameof(QuoteRule));

                var initialize = CreateInitializeMethod();
                IEnumerable<IParserExpression> innerExpressions;
                if (input.Expression is ISequenceExpression sequence)
                {
                    innerExpressions = sequence.InnerExpressions;
                }
                else
                {
                    innerExpressions = Enumerable.Repeat(input.Expression, 1);
                }
                var rules = new CodeArrayCreateExpression("Rule");
                var assignTransformation = Rule<AssignmentToClass>();
                foreach (var exp in innerExpressions)
                {
                    rules.Initializers.Add(CreateParserExpression(exp, _ruleTransformation, assignTransformation, context));
                }
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(SequenceRule.Rules)), rules));
                output.Members.Add(initialize);
            }
        }

        public class InheritanceRuleToClass : TransformationRule<IInheritanceRule, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IInheritanceRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<RuleToClass>());
            }

            public override void Transform(IInheritanceRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(nameof(ChoiceRule));

                var initialize = CreateInitializeMethod();
                var rules = new CodeArrayCreateExpression("Rule");
                var ruleToClass = Rule<RuleToClass>();
                foreach (var exp in input.Subtypes)
                {
                    rules.Initializers.Add(CreateResolveRule(CreateRuleReference(exp, ruleToClass, context)));
                }
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(ChoiceRule.Alternatives)), rules));
                output.Members.Add(initialize);
            }
        }

        public class AssignmentToClass : AbstractTransformationRule<IFeatureExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IFeatureExpression input, ITransformationContext context)
            {
                var modelRule = input.Ancestors().OfType<IModelRule>().FirstOrDefault();
                var typeReference = CreateReference(modelRule, context);
                return new CodeTypeDeclaration { Name = typeReference.BaseType + input.Feature.ToPascalCase() + "Rule" };
            }

            public override void Transform(IFeatureExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var initialize = CreateInitializeMethod();
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(QuoteRule.Inner)),
                    CreateParserExpression(input.Assigned, Rule<RuleToClass>(), Rule<AssignmentToClass>(), context)));
                output.Members.Add(initialize);
            }
        }

        public class AssignRuleToClass : TransformationRule<IAssignExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IAssignExpression input, ITransformationContext context)
            {
                var semanticType = GetSemanticTypeForFeature(input, context);
                return new CodeTypeDeclaration
                {
                    Name = $"{semanticType.semanticType.BaseType}{input.Feature.ToPascalCase()}Rule"
                };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<AssignmentToClass>());
            }

            public override void Transform(IAssignExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var (semanticType, propertyType) = GetSemanticTypeForFeature(input, context);

                output.BaseTypes.Add(new CodeTypeReference(nameof(AssignRule<object, object>), semanticType, propertyType));

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");
                var propertyValueRef = new CodeArgumentReferenceExpression("propertyValue");

                var onChangeValue = new CodeMemberMethod
                {
                    Name = "OnChangeValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(propertyType, propertyValueRef.ParameterName),
                        new CodeParameterDeclarationExpression(nameof(GrammarContext), "context")
                    }
                };
                onChangeValue.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase()),
                    propertyValueRef));
                output.Members.Add(onChangeValue);
            }
        }

        public class AddAssignRuleToClass : TransformationRule<IAddAssignExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IAddAssignExpression input, ITransformationContext context)
            {
                var semanticType = GetSemanticTypeForFeature(input, context);
                return new CodeTypeDeclaration
                {
                    Name = $"{semanticType.semanticType.BaseType}{input.Feature.ToPascalCase()}Rule"
                };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<AssignmentToClass>());
            }

            public override void Transform(IAddAssignExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var (semanticType, propertyType) = GetSemanticTypeForFeature(input, context);
                
                output.BaseTypes.Add(new CodeTypeReference(nameof(AddAssignRule<object, object>), semanticType, propertyType));

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");

                var getCollection = new CodeMemberMethod
                {
                    Name = "GetCollection",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(nameof(GrammarContext), "context")
                    },
                    ReturnType = new CodeTypeReference(typeof(ICollection<>).Name, propertyType)
                };
                getCollection.Statements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase())));
                output.Members.Add(getCollection);
            }

        }
    }
}

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
using NMF.CodeGen;
using System.Reflection;

namespace NMF.AnyText.Transformation
{
    internal class AnytextCodeGenerator : ReflectiveTransformation
    {
        private static CodeTypeReference CreateReference(IRule rule, bool interfaceType, ITransformationContext context)
        {
            var type = CodeGenerator._trace.LookupType(rule);
            if (type != null)
            {
                var mappedType = type.GetExtension<MappedType>();
                if (mappedType != null)
                {
                    var typeReference = mappedType.SystemType.ToTypeReference();
                    if (!interfaceType && mappedType.SystemType.IsInterface)
                    {
                        typeReference.BaseType = typeReference.BaseType.Substring(1);
                    }
                    return typeReference;
                }
                return new CodeTypeReference((interfaceType ? "I" : string.Empty) + type.Name.ToPascalCase());
            }
            if (rule is IParanthesisRule paranthesisRule)
            {
                return CreateReference(paranthesisRule.InnerRule, interfaceType, context);
            }
            if (rule.TypeName != null)
            {
                return new CodeTypeReference((interfaceType ? "I" : string.Empty) + rule.TypeName);
            }
            if (rule is IDataRule)
            {
                return new CodeTypeReference(typeof(string));
            }
            return new CodeTypeReference((interfaceType ? "I" : string.Empty) + rule.Name);
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
                        Type = typeof(GrammarContext).ToTypeReference()
                    }
                }
            };
        }

        private static CodeExpression _contextRef = new CodeArgumentReferenceExpression("context");

        private static CodeExpression CreateResolveKeyword(string keyword, IParserExpression parserExpression)
        {
            var unescaped = keyword?.Replace(@"\'", "'");
            var res = new CodeMethodInvokeExpression(_contextRef, nameof(GrammarContext.ResolveKeyword), new CodePrimitiveExpression(unescaped));
            if (parserExpression != null)
            {
                foreach (var instruction in parserExpression.FormattingInstructions)
                {
                    res.Parameters.Add(CreateFormattingInstruction(instruction));
                }
            }
            return res;
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
                    return CreateResolveKeyword(keywordExpression.Keyword, keywordExpression);
                case IReferenceExpression referenceExpression:
                    var assignment = GetIdAssignment(referenceExpression.ReferencedRule);
                    return CreateParserExpression(assignment.Assigned, ruleTransformation, assignTransformation, context);
                case IRuleExpression ruleExpression:
                    return CreateResolveRule(CreateRuleReference(ruleExpression.Rule, ruleTransformation, context));
                case IFeatureExpression assignExpression:
                    return CreateResolveRule(CreateAssignmentReference(assignExpression, assignTransformation, context));
                case IMaybeExpression maybe:
                    return AddFormattingInstructions(new CodeObjectCreateExpression(typeof(ZeroOrOneRule).ToTypeReference(), CreateParserExpression(maybe.Inner, ruleTransformation, assignTransformation, context)), parserExpression);
                case IPlusExpression plus:
                    return AddFormattingInstructions(new CodeObjectCreateExpression(typeof(OneOrMoreRule).ToTypeReference(), CreateParserExpression(plus.Inner, ruleTransformation, assignTransformation, context)), parserExpression);
                case IStarExpression star:
                    return AddFormattingInstructions(new CodeObjectCreateExpression(typeof(ZeroOrMoreRule).ToTypeReference(), CreateParserExpression(star.Inner, ruleTransformation, assignTransformation, context)), parserExpression);
                case ISequenceExpression sequence:
                    var sequenceExpression = new CodeObjectCreateExpression(typeof(SequenceRule).ToTypeReference());
                    foreach (var item in sequence.InnerExpressions)
                    {
                        sequenceExpression.Parameters.Add(CreateParserExpression(item, ruleTransformation, assignTransformation, context));
                    }
                    return sequenceExpression;
                case IChoiceExpression choice:
                    var choiceExpression = new CodeObjectCreateExpression(typeof(ChoiceRule).ToTypeReference());
                    foreach (var item in choice.Alternatives)
                    {
                        choiceExpression.Parameters.Add(CreateParserExpression(item, ruleTransformation, assignTransformation, context));
                    }
                    return choiceExpression;
                case INegativeLookaheadExpression negative:
                    return new CodeObjectCreateExpression(typeof(NegativeLookaheadRule).ToTypeReference(), CreateParserExpression(negative.Inner, ruleTransformation, assignTransformation, context));
            }
            throw new NotSupportedException();
        }

        private static CodeObjectCreateExpression AddFormattingInstructions(CodeObjectCreateExpression createRuleExpression, IParserExpression parserExpression)
        {
            foreach (var instruction in parserExpression.FormattingInstructions)
            {
                createRuleExpression.Parameters.Add(CreateFormattingInstruction(instruction));
            }
            return createRuleExpression;
        }

        private static void AddFormattingInstructions(CodeMemberMethod initializeMethod, IParserExpression parserExpression)
        {
            if (parserExpression.FormattingInstructions.Count > 0)
            {
                var formattingInstructions = new CodeArrayCreateExpression(typeof(FormattingInstruction).ToTypeReference());
                initializeMethod.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(AnyText.Rules.Rule.FormattingInstructions)),
                    formattingInstructions
                    ));

                foreach (var instruction in parserExpression.FormattingInstructions)
                {
                    formattingInstructions.Initializers.Add(CreateFormattingInstruction(instruction));
                }
            }
        }

        private static CodeExpression CreateFormattingInstruction(FormattingInstruction formattingInstruction)
        {
            switch (formattingInstruction)
            {
                case FormattingInstruction.Indent:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.Indent));
                case FormattingInstruction.Unindent:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.Unindent));
                case FormattingInstruction.Newline:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.Newline));
                default:
                    throw new ArgumentOutOfRangeException(nameof(formattingInstruction));
            }
        }

        private static CodeTypeReference SynthesizeType(IParserExpression expression, RuleToClass ruleToClass, ITransformationContext context)
        {
            switch (expression)
            {
                case IRuleExpression ruleExpression:
                    return CreateReference(ruleExpression.Rule, true, context);
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
                        return (semanticType, typeof(IModelElement).ToTypeReference());
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
                var semanticType = CreateReference(modelRule, true, context);
                return (semanticType, SynthesizeType(input.Assigned, null, context));
            }
            var fragment = input.Ancestors().OfType<IFragmentRule>().FirstOrDefault();
            if (fragment != null)
            {
                var semanticType = CreateReference(fragment, true, context);
                return (semanticType, SynthesizeType(input.Assigned, null, context));
            }
            throw new NotSupportedException();
        }

        public class GrammarToNamespace : NamespaceGenerator<IGrammar>
        {
            public override IEnumerable<string> DefaultImports
            {
                get
                {
                    yield return "NMF.AnyText.Model";
                    yield return "NMF.AnyText.Rules";
                    yield return "System";
                    yield return "System.Text.RegularExpressions";
                }
            }

            protected override string GetName(IGrammar input)
            {
                return "Generated";
            }

            protected override IEnumerable<Assembly> AssembliesToCheck
            {
                get
                {
                    yield return typeof(ReflectiveGrammar).Assembly;
                    yield return typeof(Parser).Assembly;
                }
            }

            public override void RegisterDependencies()
            {
                RequireType(Rule<GrammarToClass>(), g => g);
            }
        }

        public class GrammarToClass : TransformationRule<IGrammar, CodeTypeDeclaration>
        {
            public override void Transform(IGrammar input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Name = input.Name.ToPascalCase() + "Grammar";
                output.BaseTypes.Add(typeof(ReflectiveGrammar).ToTypeReference());
            }

            public override void RegisterDependencies()
            {
                CallMany(Rule<RuleToClass>(), g => g.Rules, AddChildClasses);
                CallMany(Rule<AssignmentToClass>(),
                    GetAssignments,
                    AddChildClasses);
            }

            private static IEnumerable<IFeatureExpression> GetAssignments(IGrammar grammar)
            {
                return grammar.Rules
                   .OfType<IModelRule>().Concat<IRule>(grammar.Rules.OfType<IFragmentRule>())
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
                output.BaseTypes.Add(new CodeTypeReference(typeof(ModelElementRule<>).Name, CreateReference(input, false, context)));

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
                var rules = new CodeArrayCreateExpression(typeof(Rules.Rule).ToTypeReference());
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
                output.BaseTypes.Add(typeof(ParanthesesRule).ToTypeReference());

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
                if (input.TypeName == null)
                {
                    output.BaseTypes.Add(typeof(RegexRule).ToTypeReference());
                }
                else
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(ConvertRule<>).Name, CreateReference(input, false, context)));
                }

                var innerRegex = "^" + input.Regex[1..^1];

                var initialize = CreateInitializeMethod();
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(RegexRule.Regex)),
                    new CodeObjectCreateExpression(typeof(Regex).ToTypeReference(), new CodePrimitiveExpression(innerRegex), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(nameof(RegexOptions)), nameof(RegexOptions.Compiled)))));
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
                output.BaseTypes.Add(typeof(QuoteRule).ToTypeReference());

                var initialize = CreateInitializeMethod();
                var assignTransformation = Rule<AssignmentToClass>();
                var rule = CreateParserExpression(input.Expression, _ruleTransformation, assignTransformation, context);
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(QuoteRule.Inner)), rule));
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
                output.BaseTypes.Add(typeof(ChoiceRule).ToTypeReference());

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

        public class EnumRuleToClass : TransformationRule<IEnumRule, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IEnumRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<RuleToClass>());
            }

            public override void Transform(IEnumRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var enumType = CreateReference(input, false, context);
                output.BaseTypes.Add(new CodeTypeReference(typeof(EnumRule<>).Name, enumType));

                var initialize = CreateInitializeMethod();
                var literals = new CodeArrayCreateExpression(typeof(Rules.Rule).ToTypeReference());
                var values = new CodeArrayCreateExpression(enumType);
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(ChoiceRule.Alternatives)),
                    literals));
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, "Values"),
                    values));

                foreach (var lit in input.Literals)
                {
                    literals.Initializers.Add(CreateResolveKeyword(lit.Keyword, null));
                    values.Initializers.Add(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(enumType), lit.Literal.ToPascalCase()));
                }

                output.Members.Add(initialize);
            }
        }

        public class AssignmentToClass : AbstractTransformationRule<IFeatureExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IFeatureExpression input, ITransformationContext context)
            {
                var modelRule = input.Ancestors().OfType<IModelRule>().FirstOrDefault();
                var typeReference = CreateReference(modelRule, true, context);
                return new CodeTypeDeclaration { Name = typeReference.BaseType + input.Feature.ToPascalCase() + "Rule" };
            }

            public override void Transform(IFeatureExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var initialize = CreateInitializeMethod();
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(QuoteRule.Inner)),
                    CreateParserExpression(input.Assigned, Rule<RuleToClass>(), Rule<AssignmentToClass>(), context)));
                AddFormattingInstructions(initialize, input);
                output.Members.Add(initialize);

                var feature = new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Name = "Feature",
                    Type = typeof(string).ToTypeReference(),
                    HasGet = true,
                    HasSet = false
                };
                feature.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(input.Feature)));
                output.Members.Add(feature);
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


                if (input.Assigned is IReferenceExpression)
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AssignModelReferenceRule<,>).Name, semanticType, propertyType));
                }
                else
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AssignRule<,>).Name, semanticType, propertyType));
                }

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");
                var propertyValueRef = new CodeArgumentReferenceExpression("propertyValue");
                var property = new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase());

                var getValue = new CodeMemberMethod
                {
                    Name = "GetValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    },
                    ReturnType = propertyType
                };
                getValue.Statements.Add(new CodeMethodReturnStatement(property));
                output.Members.Add(getValue);
                var setValue = new CodeMemberMethod
                {
                    Name = "SetValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(propertyType, propertyValueRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    }
                };
                setValue.Statements.Add(new CodeAssignStatement(
                    property,
                    propertyValueRef));
                output.Members.Add(setValue);
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

                if (input.Assigned is IReferenceExpression)
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AddAssignModelReferenceRule<,>).Name, semanticType, propertyType));
                }
                else
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AddAssignRule<,>).Name, semanticType, propertyType));
                }

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");

                var getCollection = new CodeMemberMethod
                {
                    Name = "GetCollection",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    },
                    ReturnType = new CodeTypeReference(typeof(ICollection<>).Name, propertyType)
                };
                getCollection.Statements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase())));
                output.Members.Add(getCollection);
            }

        }
    }
}

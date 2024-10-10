using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NMF.AnyText.Grammars
{
    public class AnyTextGrammar : ReflectiveGrammar
    {
        public override string LanguageId => "anytext";

        protected override Rules.Rule GetRootRule(GrammarContext context)
        {
            return context.ResolveRule<GrammarRule>();
        }

        public class GrammarRule : ModelElementRule<Metamodel.Grammar>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("grammar"),
                    context.ResolveRule<GrammarNameRule>(),
                    new ZeroOrMoreRule(context.ResolveRule<GrammarImportsRule>()),
                    new OneOrMoreRule(context.ResolveRule<GrammarRulesRule>()),
                };
            }
        }

        public class IdRule : RegexRule
        {
            public override void Initialize(GrammarContext context)
            {
                Regex = new Regex("^[a-zA-Z]\\w*", RegexOptions.Compiled);
            }
        }

        public class KeywordRule : RegexRule
        {
            public override void Initialize(GrammarContext context)
            {
                Regex = new Regex(@"^(\\'|[^'])+", RegexOptions.Compiled);
            }
        }

        public class RegexTerminalRule : RegexRule
        {
            public override void Initialize(GrammarContext context)
            {
                Regex = new Regex(@"^/(\\/|[^/])*/", RegexOptions.Compiled);
            }
        }

        public class GrammarNameRule : AssignRule<IGrammar, string>
        {
            protected override void OnChangeValue(IGrammar semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Name = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class GrammarImportsRule : AddAssignRule<IGrammar, IMetamodelImport>
        {
            public override ICollection<IMetamodelImport> GetCollection(IGrammar semanticElement, ParseContext context)
            {
                return semanticElement.Imports;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<MetamodelImportRule>();
            }
        }

        public class GrammarRulesRule : AddAssignRule<IGrammar, IRule>
        {
            public override ICollection<IRule> GetCollection(IGrammar semanticElement, ParseContext context)
            {
                return semanticElement.Rules;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<RuleRule>();
            }
        }

        public class MetamodelImportRule : ModelElementRule<MetamodelImport>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("imports"),
                    new ZeroOrOneRule(new SequenceRule(
                        context.ResolveRule<MetamodelImportPrefixRule>(),
                        context.ResolveKeyword("from"))),
                    context.ResolveRule<MetamodelImportFileRule>()
                };
            }
        }

        public class MetamodelImportFileRule : AssignRule<IMetamodelImport, string>
        {
            protected override void OnChangeValue(IMetamodelImport semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.File = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class MetamodelImportPrefixRule : AssignRule<IMetamodelImport, string>
        {
            protected override void OnChangeValue(IMetamodelImport semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Prefix = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class RuleRule : ChoiceRule
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveRule<ClassRuleRule>(),
                    context.ResolveRule<DataRuleRule>(),
                    context.ResolveRule<FragmentRuleRule>(),
                    context.ResolveRule<ParanthesisRuleRule>()
                };
            }
        }

        public class ClassRuleRule : ChoiceRule
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveRule<InheritanceRuleRule>(),
                    context.ResolveRule<ModelRuleRule>()
                };
            }
        }

        public class RuleNameRule : AssignRule<IRule, string>
        {
            protected override void OnChangeValue(IRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Name = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class RuleTypeNameRule : AssignRule<IRule, string>
        {
            protected override void OnChangeValue(IRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.TypeName = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class RulePrefixRule : AssignRule<IRule, string>
        {
            protected override void OnChangeValue(IRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Prefix = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class ModelRuleRule : ModelElementRule<ModelRule>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<RuleNameRule>(),
                    context.ResolveRule<RuleTypeFragmentRule>(),
                    context.ResolveKeyword(":"),
                    context.ResolveRule<ModelRuleExpressionRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class ParanthesisRuleRule : ModelElementRule<ParanthesisRule>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("parantheses"),
                    context.ResolveRule<RuleNameRule>(),
                    context.ResolveKeyword(":"),
                    context.ResolveRule<ParanthesisRuleOpeningParanthesisRule>(),
                    context.ResolveRule<ParanthesisRuleInnerRuleRule>(),
                    context.ResolveRule<ParanthesisRuleClosingParanthesisRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class FragmentRuleRule : ModelElementRule<FragmentRule>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("fragment"),
                    context.ResolveRule<RuleNameRule>(),
                        context.ResolveKeyword("processes"),
                        new ZeroOrOneRule(new SequenceRule(
                            context.ResolveRule<RulePrefixRule>(),
                            context.ResolveKeyword(".")
                        )),
                        context.ResolveRule<RuleTypeNameRule>(),
                    context.ResolveKeyword(":"),
                    context.ResolveRule<FragmentRuleExpressionRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class FragmentRuleExpressionRule : AssignRule<IFragmentRule, IParserExpression>
        {
            protected override void OnChangeValue(IFragmentRule semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Expression = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
            }
        }

        public class ModelRuleExpressionRule : AssignRule<IModelRule, IParserExpression>
        {
            protected override void OnChangeValue(IModelRule semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Expression = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
            }
        }

        public class ParanthesisRuleInnerRuleRule : AssignReferenceRule<IParanthesisRule, IClassRule>
        {
            protected override void OnChangeValue(IParanthesisRule semanticElement, IClassRule propertyValue, ParseContext context)
            {
                semanticElement.InnerRule = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class ParanthesisRuleOpeningParanthesisRule : AssignRule<IParanthesisRule, IKeywordExpression>
        {
            protected override void OnChangeValue(IParanthesisRule semanticElement, IKeywordExpression propertyValue, ParseContext context)
            {
                semanticElement.OpeningParanthesis = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordExpressionRule>();
            }
        }

        public class ParanthesisRuleClosingParanthesisRule : AssignRule<IParanthesisRule, IKeywordExpression>
        {
            protected override void OnChangeValue(IParanthesisRule semanticElement, IKeywordExpression propertyValue, ParseContext context)
            {
                semanticElement.ClosingParanthesis = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordExpressionRule>();
            }
        }

        public class DataRuleRule : ModelElementRule<DataRule>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("terminal"),
                    context.ResolveRule<RuleNameRule>(),
                    context.ResolveRule<RuleTypeFragmentRule>(),
                    context.ResolveKeyword(":"),
                    context.ResolveRule<DataRuleRegexRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class RuleTypeFragmentRule : QuoteRule
        {
            public override void Initialize(GrammarContext context)
            {
                Inner = new ZeroOrOneRule(new SequenceRule(
                        context.ResolveKeyword("returns"),
                        new ZeroOrOneRule(new SequenceRule(
                            context.ResolveRule<RulePrefixRule>(),
                            context.ResolveKeyword(".")
                        )),
                        context.ResolveRule<RuleTypeNameRule>()
                    ));
            }
        }

        public class DataRuleRegexRule : AssignRule<IDataRule, string>
        {
            protected override void OnChangeValue(IDataRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Regex = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<RegexTerminalRule>();
            }
        }

        public class InheritanceRuleRule : ModelElementRule<InheritanceRule>
        {

            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<RuleNameRule>(),
                    context.ResolveRule<RuleTypeFragmentRule>(),
                    context.ResolveKeyword(":"),
                    context.ResolveRule<InheritanceSubtypeRule>(),
                    new OneOrMoreRule(new SequenceRule(
                        context.ResolveKeyword("|"),
                        context.ResolveRule<InheritanceSubtypeRule>()
                    )),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class InheritanceSubtypeRule : AddAssignReferenceRule<IInheritanceRule, IClassRule>
        {
            public override ICollection<IClassRule> GetCollection(IInheritanceRule semanticElement, ParseContext context)
            {
                return semanticElement.Subtypes;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class ParserExpressionRule : ChoiceRule
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveRule<ChoiceExpressionRule>(),
                    context.ResolveRule<SequenceExpressionRule>(),
                    context.ResolveRule<ConjunctiveParserExpressionRule>(),
                };
            }
        }

        public class ConjunctiveParserExpressionRule : ChoiceRule
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveRule<PlusExpressionRule>(),
                    context.ResolveRule<StarExpressionRule>(),
                    context.ResolveRule<MaybeExpressionRule>(),
                    context.ResolveRule<BasicParserExpressionRule>(),
                };
            }
        }

        public class BasicParserExpressionRule : ChoiceRule
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveRule<ParanthesisExpressionRule>(),
                    context.ResolveRule<NegativeLookaheadExpressionRule>(),
                    context.ResolveRule<KeywordExpressionRule>(),
                    context.ResolveRule<ReferenceExpressionRule>(),
                    context.ResolveRule<AssignExpressionRule>(),
                    context.ResolveRule<AddAssignExpressionRule>(),
                    context.ResolveRule<ExistsAssignExpressionRule>(),
                    context.ResolveRule<RuleExpressionRule>(),
                };
            }
        }

        public class ParanthesisExpressionRule : ParanthesesRule
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("("),
                    context.ResolveRule<ParserExpressionRule>(),
                    context.ResolveKeyword(")"),
                };
            }
        }

        public class SequenceExpressionRule : ModelElementRule<SequenceExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<SequenceExpressionInnerExpressionsRule>(),
                    new OneOrMoreRule(context.ResolveRule<SequenceExpressionInnerExpressionsRule>())
                };
            }
        }

        public class SequenceExpressionInnerExpressionsRule : AddAssignRule<ISequenceExpression, IParserExpression>
        {
            public override ICollection<IParserExpression> GetCollection(ISequenceExpression semanticElement, ParseContext context)
            {
                return semanticElement.InnerExpressions;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ConjunctiveParserExpressionRule>();
            }
        }

        public class UnaryParserExpressionInnerRule : AssignRule<IUnaryParserExpression, IParserExpression>
        {
            protected override void OnChangeValue(IUnaryParserExpression semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Inner = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<BasicParserExpressionRule>();
            }
        }

        public class PlusExpressionRule : ModelElementRule<PlusExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<UnaryParserExpressionInnerRule>(),
                    context.ResolveKeyword("+")
                };
            }
        }

        public class StarExpressionRule : ModelElementRule<StarExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<UnaryParserExpressionInnerRule>(),
                    context.ResolveKeyword("*")
                };
            }
        }

        public class MaybeExpressionRule : ModelElementRule<MaybeExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<UnaryParserExpressionInnerRule>(),
                    context.ResolveKeyword("?")
                };
            }
        }

        public class KeywordExpressionRule : ModelElementRule<KeywordExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("'"),
                    context.ResolveRule<KeywordExpressionKeywordRule>(),
                    context.ResolveKeyword("'")
                };
            }
        }

        public class NegativeLookaheadExpressionRule : ModelElementRule<NegativeLookaheadExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("!"),
                    context.ResolveRule<UnaryParserExpressionInnerRule>()
                };
            }
        }

        public class KeywordExpressionKeywordRule : AssignRule<IKeywordExpression, string>
        {
            protected override void OnChangeValue(IKeywordExpression semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Keyword = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordRule>();
            }
        }

        public class ChoiceExpressionRule : ModelElementRule<ChoiceExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    new OneOrMoreRule(new SequenceRule(context.ResolveRule<ChoiceExpressionAlternativesRule>(), context.ResolveKeyword("|"))),
                    context.ResolveRule<ChoiceExpressionAlternativesRule>()
                };
            }
        }

        public class ChoiceExpressionAlternativesRule : AddAssignRule<IChoiceExpression, IParserExpression>
        {
            public override ICollection<IParserExpression> GetCollection(IChoiceExpression semanticElement, ParseContext context)
            {
                return semanticElement.Alternatives;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ConjunctiveParserExpressionRule>();
            }
        }

        public class FeatureExpressionFeatureRule : AssignRule<IFeatureExpression, string>
        {
            protected override void OnChangeValue(IFeatureExpression semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Feature = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class FeatureExpressionAssignedRule : AssignRule<IFeatureExpression, IParserExpression>
        {
            protected override void OnChangeValue(IFeatureExpression semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Assigned = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<BasicParserExpressionRule>();
            }
        }

        public class AssignExpressionRule : ModelElementRule<AssignExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<FeatureExpressionFeatureRule>(),
                    context.ResolveKeyword("="),
                    context.ResolveRule<FeatureExpressionAssignedRule>()
                };
            }
        }

        public class AddAssignExpressionRule : ModelElementRule<AddAssignExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<FeatureExpressionFeatureRule>(),
                    context.ResolveKeyword("+="),
                    context.ResolveRule<FeatureExpressionAssignedRule>()
                };
            }
        }

        public class ExistsAssignExpressionRule : ModelElementRule<ExistsAssignExpression>
        {

            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<FeatureExpressionFeatureRule>(),
                    context.ResolveKeyword("?="),
                    context.ResolveRule<FeatureExpressionAssignedRule>()
                };
            }
        }

        public class RuleExpressionRule : ModelElementRule<RuleExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<RuleExpressionReferencedRule>(),
                    new NegativeLookaheadRule(context.ResolveKeyword("=")),
                    new NegativeLookaheadRule(context.ResolveKeyword("+=")),
                    new NegativeLookaheadRule(context.ResolveKeyword("?="))
                };
            }
        }

        public class RuleExpressionReferencedRule : AssignReferenceRule<IRuleExpression, IRule>
        {
            protected override void OnChangeValue(IRuleExpression semanticElement, IRule propertyValue, ParseContext context)
            {
                semanticElement.Rule = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class ReferenceExpressionRule : ModelElementRule<ReferenceExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("["),
                    context.ResolveRule<ReferenceExpressionReferencedRuleRule>(),
                    context.ResolveKeyword("]")
                };
            }
        }

        public class ReferenceExpressionReferencedRuleRule : AssignReferenceRule<IReferenceExpression, IClassRule>
        {
            protected override void OnChangeValue(IReferenceExpression semanticElement, IClassRule propertyValue, ParseContext context)
            {
                semanticElement.ReferencedRule = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }
    }
}

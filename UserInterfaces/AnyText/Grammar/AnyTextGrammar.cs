using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammar
{
    public class AnyTextGrammar : ReflectiveGrammar
    {
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
                Regex = new Regex("[^']+", RegexOptions.Compiled);
            }
        }

        public class RegexTerminalRule : RegexRule
        {
            public override void Initialize(GrammarContext context)
            {
                Regex = new Regex("/[^/]*/", RegexOptions.Compiled);
            }
        }

        public class GrammarNameRule : AssignRule<Metamodel.Grammar, string>
        {
            protected override void OnChangeValue(Metamodel.Grammar semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Name = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class GrammarImportsRule : AddAssignRule<Metamodel.Grammar, MetamodelImport>
        {
            public override ICollection<MetamodelImport> GetCollection(Metamodel.Grammar semanticElement, ParseContext context)
            {
                return semanticElement.Imports;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<MetamodelImportRule>();
            }
        }

        public class GrammarRulesRule : AddAssignRule<Metamodel.Grammar, Metamodel.Rule>
        {
            public override ICollection<Metamodel.Rule> GetCollection(Metamodel.Grammar semanticElement, ParseContext context)
            {
                return semanticElement.Rules;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<RuleRule>();
            }
        }

        public class MetamodelImportRule : ModelElementRule<Metamodel.MetamodelImport>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveKeyword("imports"),
                    context.ResolveRule<MetamodelImportPrefixRule>(),
                    context.ResolveKeyword("from"),
                    context.ResolveRule<MetamodelImportFileRule>()
                };
            }
        }

        public class MetamodelImportFileRule : AssignRule<MetamodelImport, string>
        {
            protected override void OnChangeValue(MetamodelImport semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.File = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class MetamodelImportPrefixRule : AssignRule<MetamodelImport, string>
        {
            protected override void OnChangeValue(MetamodelImport semanticElement, string propertyValue, ParseContext context)
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
                    context.ResolveRule<ModelRuleRule>(),
                    context.ResolveRule<DataRuleRule>(),
                };
            }
        }

        public class RuleNameRule : AssignRule<Metamodel.Rule, string>
        {
            protected override void OnChangeValue(Metamodel.Rule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Name = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class ModelRuleRule : ModelElementRule<Metamodel.ModelRule>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<RuleNameRule>(),
                    context.ResolveKeyword(":"),
                    context.ResolveRule<ModelRuleExpressionRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class ModelRuleExpressionRule : AssignRule<ModelRule, ParserExpression>
        {
            protected override void OnChangeValue(ModelRule semanticElement, ParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Expression = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
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
                    context.ResolveKeyword(":"),
                    context.ResolveRule<DataRuleRegexRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class DataRuleRegexRule : AssignRule<DataRule, string>
        {
            protected override void OnChangeValue(DataRule semanticElement, string propertyValue, ParseContext context)
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
                    context.ResolveKeyword(":"),
                    context.ResolveRule<ModelRuleExpressionRule>(),
                    context.ResolveKeyword(";")
                };
            }
        }

        public class ParserExpressionRule : ChoiceRule
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveRule<ChoiceExpressionRule>(),
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
                    context.ResolveRule<SequenceExpressionRule>(),
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
                    context.ResolveRule<KeywordExpressionRule>(),
                    context.ResolveRule<ReferenceExpressionRule>(),
                    context.ResolveRule<RuleExpressionRule>(),
                    context.ResolveRule<AssignExpressionRule>(),
                    context.ResolveRule<AddAssignExpressionRule>(),
                    context.ResolveRule<ExistsAssignExpressionRule>()
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

        public class SequenceExpressionInnerExpressionsRule : AddAssignRule<SequenceExpression, ParserExpression>
        {
            public override ICollection<ParserExpression> GetCollection(SequenceExpression semanticElement, ParseContext context)
            {
                return semanticElement.InnerExpressions;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<BasicParserExpressionRule>();
            }
        }

        public class UnaryParserExpressionInnerRule : AssignRule<UnaryParserExpression, ParserExpression>
        {
            protected override void OnChangeValue(UnaryParserExpression semanticElement, ParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Inner = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
            }
        }

        public class PlusExpressionRule : ModelElementRule<PlusExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<BasicParserExpressionRule>(),
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
                    context.ResolveRule<BasicParserExpressionRule>(),
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
                    context.ResolveRule<BasicParserExpressionRule>(),
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

        public class KeywordExpressionKeywordRule : AssignRule<KeywordExpression, string>
        {
            protected override void OnChangeValue(KeywordExpression semanticElement, string propertyValue, ParseContext context)
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

        public class ChoiceExpressionAlternativesRule : AddAssignRule<ChoiceExpression, ParserExpression>
        {
            public override ICollection<ParserExpression> GetCollection(ChoiceExpression semanticElement, ParseContext context)
            {
                return semanticElement.Alternatives;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ConjunctiveParserExpressionRule>();
            }
        }

        public class FeatureExpressionFeatureRule : AssignRule<FeatureExpression, string>
        {
            protected override void OnChangeValue(FeatureExpression semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Feature = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }
        }

        public class FeatureExpressionAssignedRule : AssignRule<FeatureExpression, ParserExpression>
        {
            protected override void OnChangeValue(FeatureExpression semanticElement, ParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Assigned = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
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

        public class RuleExpressionReferencedRule : AssignReferenceRule<RuleExpression, Metamodel.Rule>
        {
            protected override void OnChangeValue(RuleExpression semanticElement, Metamodel.Rule propertyValue, ParseContext context)
            {
                semanticElement.Referenced = propertyValue;
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

        public class ReferenceExpressionReferencedRuleRule : AssignReferenceRule<ReferenceExpression, ModelRule>
        {
            protected override void OnChangeValue(ReferenceExpression semanticElement, ModelRule propertyValue, ParseContext context)
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

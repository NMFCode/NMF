﻿using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
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

        public partial class GrammarRule : ModelElementRule<Metamodel.Grammar>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[] {
                        context.ResolveKeyword("grammar"),
                        context.ResolveRule<GrammarNameRule>(),
                        new ZeroOrOneRule(new SequenceRule(context.ResolveKeyword("("), context.ResolveRule<GrammarLanguageIdRule>(), context.ResolveKeyword(")")), PrettyPrinting.FormattingInstruction.Newline),
                        context.ResolveKeyword("root"),
                        context.ResolveRule<GrammarStartRuleRule>(),
                        new ZeroOrMoreRule(context.ResolveRule<GrammarImportsRule>(), PrettyPrinting.FormattingInstruction.Newline),
                        new OneOrMoreRule(context.ResolveRule<GrammarRulesRule>())};
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

        public partial class UriRule : RegexRule
        {

            public override void Initialize(GrammarContext context)
            {
                Regex = new Regex("^.+", RegexOptions.Compiled);
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
            protected override string Feature => "Name";

            protected override void SetValue(IGrammar semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Name = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IGrammar semanticElement, ParseContext context)
            {
                return semanticElement.Name;
            }
        }

        public class GrammarLanguageIdRule : AssignRule<IGrammar, string>
        {

            protected override String Feature
            {
                get
                {
                    return "LanguageID";
                }
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IGrammar semanticElement, ParseContext context)
            {
                return semanticElement.LanguageId;
            }

            protected override void SetValue(IGrammar semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.LanguageId = propertyValue;
            }
        }


        public class GrammarStartRuleRule : AssignModelReferenceRule<IGrammar, IClassRule>
        {

            protected override String Feature
            {
                get
                {
                    return "Root";
                }
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
                FormattingInstructions = new PrettyPrinting.FormattingInstruction[] {
                        PrettyPrinting.FormattingInstruction.Newline};
            }

            protected override IClassRule GetValue(IGrammar semanticElement, ParseContext context)
            {
                return semanticElement.StartRule;
            }

            protected override void SetValue(IGrammar semanticElement, IClassRule propertyValue, ParseContext context)
            {
                semanticElement.StartRule = propertyValue;
            }
        }

        public class GrammarImportsRule : AddAssignRule<IGrammar, IMetamodelImport>
        {
            protected override string Feature => "Imports";

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
            protected override string Feature => "Rules";

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
            protected override string Feature => "File";

            protected override void SetValue(IMetamodelImport semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.File = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<UriRule>();
            }

            protected override string GetValue(IMetamodelImport semanticElement, ParseContext context)
            {
                return semanticElement.File;
            }
        }

        public class MetamodelImportPrefixRule : AssignRule<IMetamodelImport, string>
        {
            protected override string Feature => "Prefix";

            protected override void SetValue(IMetamodelImport semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Prefix = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IMetamodelImport semanticElement, ParseContext context)
            {
                return semanticElement.Prefix;
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
                    context.ResolveRule<ParanthesisRuleRule>(),
                    context.ResolveRule<EnumRuleRule>()
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
            protected override string Feature => "Name";

            protected override void SetValue(IRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Name = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IRule semanticElement, ParseContext context)
            {
                return semanticElement.Name;
            }
        }

        public class RuleTypeNameRule : AssignRule<IRule, string>
        {
            protected override string Feature => "TypeName";

            protected override void SetValue(IRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.TypeName = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IRule semanticElement, ParseContext context)
            {
                return semanticElement.TypeName;
            }
        }

        public class RulePrefixRule : AssignRule<IRule, string>
        {
            protected override string Feature => "Prefix";

            protected override void SetValue(IRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Prefix = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IRule semanticElement, ParseContext context)
            {
                return semanticElement.Prefix;
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
            protected override string Feature => "Expression";

            protected override void SetValue(IFragmentRule semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Expression = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
            }

            protected override IParserExpression GetValue(IFragmentRule semanticElement, ParseContext context)
            {
                return semanticElement.Expression;
            }
        }

        public class ModelRuleExpressionRule : AssignRule<IModelRule, IParserExpression>
        {
            protected override string Feature => "Expression";

            protected override void SetValue(IModelRule semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Expression = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<ParserExpressionRule>();
            }

            protected override IParserExpression GetValue(IModelRule semanticElement, ParseContext context)
            {
                return semanticElement.Expression;
            }
        }

        public class ParanthesisRuleInnerRuleRule : AssignReferenceRule<IParanthesisRule, IClassRule>
        {
            protected override string Feature => "InnerRule";

            protected override void SetValue(IParanthesisRule semanticElement, IClassRule propertyValue, ParseContext context)
            {
                semanticElement.InnerRule = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override IClassRule GetValue(IParanthesisRule semanticElement, ParseContext context)
            {
                return semanticElement.InnerRule;
            }

            protected override string GetReferenceString(IClassRule reference, ParseContext context)
            {
                return reference.ToIdentifierString();
            }
        }

        public class ParanthesisRuleOpeningParanthesisRule : AssignRule<IParanthesisRule, IKeywordExpression>
        {
            protected override string Feature => "OpeningParanthesis";

            protected override void SetValue(IParanthesisRule semanticElement, IKeywordExpression propertyValue, ParseContext context)
            {
                semanticElement.OpeningParanthesis = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordExpressionRule>();
            }

            protected override IKeywordExpression GetValue(IParanthesisRule semanticElement, ParseContext context)
            {
                return semanticElement.OpeningParanthesis;
            }
        }

        public class ParanthesisRuleClosingParanthesisRule : AssignRule<IParanthesisRule, IKeywordExpression>
        {
            protected override string Feature => "ClosingParanthesis";

            protected override void SetValue(IParanthesisRule semanticElement, IKeywordExpression propertyValue, ParseContext context)
            {
                semanticElement.ClosingParanthesis = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordExpressionRule>();
            }

            protected override IKeywordExpression GetValue(IParanthesisRule semanticElement, ParseContext context)
            {
                return semanticElement.ClosingParanthesis;
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
            protected override string Feature => "Regex";

            protected override void SetValue(IDataRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Regex = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<RegexTerminalRule>();
            }

            protected override string GetValue(IDataRule semanticElement, ParseContext context)
            {
                return semanticElement.Regex;
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


        public partial class EnumRuleRule : ModelElementRule<EnumRule>
        {

            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[] {
                        context.ResolveKeyword("enum"),
                        context.ResolveRule<RuleNameRule>(),
                        context.ResolveRule<RuleTypeFragmentRule>(),
                        context.ResolveKeyword(":"),
                        new OneOrMoreRule(context.ResolveRule<EnumRuleLiteralsRule>()),
                        context.ResolveKeyword(";")};
            }
        }

        public partial class LiteralRuleRule : ModelElementRule<Metamodel.LiteralRule>
        {

            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[] {
                        context.ResolveRule<LiteralRuleLiteralRule>(),
                        context.ResolveKeyword("=>"),
                        context.ResolveKeyword("\'"),
                        context.ResolveRule<LiteralRuleKeywordRule>(),
                        context.ResolveKeyword("\'")};
            }
        }

        public class EnumRuleLiteralsRule : AddAssignRule<IEnumRule, ILiteralRule>
        {

            protected override String Feature
            {
                get
                {
                    return "Literals";
                }
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<LiteralRuleRule>();
            }

            public override ICollection<ILiteralRule> GetCollection(IEnumRule semanticElement, ParseContext context)
            {
                return semanticElement.Literals;
            }
        }

        public class LiteralRuleKeywordRule : AssignRule<ILiteralRule, string>
        {

            protected override String Feature
            {
                get
                {
                    return "Keyword";
                }
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordRule>();
            }

            protected override string GetValue(ILiteralRule semanticElement, ParseContext context)
            {
                return semanticElement.Keyword;
            }

            protected override void SetValue(ILiteralRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Keyword = propertyValue;
            }
        }

        public class LiteralRuleLiteralRule : AssignRule<ILiteralRule, string>
        {

            protected override String Feature
            {
                get
                {
                    return "Literal";
                }
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(ILiteralRule semanticElement, ParseContext context)
            {
                return semanticElement.Literal;
            }

            protected override void SetValue(ILiteralRule semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Literal = propertyValue;
            }
        }

        public class InheritanceSubtypeRule : AddAssignReferenceRule<IInheritanceRule, IClassRule>
        {
            protected override string Feature => "Subtypes";

            public override ICollection<IClassRule> GetCollection(IInheritanceRule semanticElement, ParseContext context)
            {
                return semanticElement.Subtypes;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetReferenceString(IClassRule reference, ParseContext context)
            {
                return reference.ToIdentifierString();
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
                    context.ResolveRule<NegativeLookaheadExpressionRule>(),
                    context.ResolveRule<KeywordExpressionRule>(),
                    context.ResolveRule<ReferenceExpressionRule>(),
                    context.ResolveRule<AssignExpressionRule>(),
                    context.ResolveRule<AddAssignExpressionRule>(),
                    context.ResolveRule<ExistsAssignExpressionRule>(),
                    context.ResolveRule<RuleExpressionRule>(),
                    context.ResolveRule<ParanthesisExpressionRule>(),
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
            protected override string Feature => "InnerExpressions";

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
            protected override string Feature => "Inner";

            protected override void SetValue(IUnaryParserExpression semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Inner = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<BasicParserExpressionRule>();
            }

            protected override IParserExpression GetValue(IUnaryParserExpression semanticElement, ParseContext context)
            {
                return semanticElement.Inner;
            }
        }

        public class PlusExpressionRule : ModelElementRule<PlusExpression>
        {
            public override void Initialize(GrammarContext context)
            {
                Rules = new Rules.Rule[]
                {
                    context.ResolveRule<UnaryParserExpressionInnerRule>(),
                    context.ResolveKeyword("+"),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
                    context.ResolveKeyword("*"),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
                    context.ResolveKeyword("?"),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
                    context.ResolveKeyword("'"),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
            protected override string Feature => "Keyword";

            protected override void SetValue(IKeywordExpression semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Keyword = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<KeywordRule>();
            }

            protected override string GetValue(IKeywordExpression semanticElement, ParseContext context)
            {
                return semanticElement.Keyword;
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
            protected override string Feature => "Alternatives";

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
            protected override string Feature => "Feature";

            protected override void SetValue(IFeatureExpression semanticElement, string propertyValue, ParseContext context)
            {
                semanticElement.Feature = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override string GetValue(IFeatureExpression semanticElement, ParseContext context)
            {
                return semanticElement.Feature;
            }
        }

        public class FeatureExpressionAssignedRule : AssignRule<IFeatureExpression, IParserExpression>
        {
            protected override string Feature => "Assigned";

            protected override void SetValue(IFeatureExpression semanticElement, IParserExpression propertyValue, ParseContext context)
            {
                semanticElement.Assigned = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<BasicParserExpressionRule>();
            }

            protected override IParserExpression GetValue(IFeatureExpression semanticElement, ParseContext context)
            {
                return semanticElement.Assigned;
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
                    context.ResolveRule<FeatureExpressionAssignedRule>(),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
                    context.ResolveRule<FeatureExpressionAssignedRule>(),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
                    context.ResolveRule<FeatureExpressionAssignedRule>(),
                    context.ResolveRule<FormattingInstructionFragmentRule>()
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
            protected override string Feature => "Rule";

            protected override void SetValue(IRuleExpression semanticElement, IRule propertyValue, ParseContext context)
            {
                semanticElement.Rule = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override IRule GetValue(IRuleExpression semanticElement, ParseContext context)
            {
                return semanticElement.Rule;
            }

            protected override string GetReferenceString(IRule reference, ParseContext context)
            {
                return reference.ToIdentifierString();
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
            protected override string Feature => "ReferencedRule";

            protected override void SetValue(IReferenceExpression semanticElement, IClassRule propertyValue, ParseContext context)
            {
                semanticElement.ReferencedRule = propertyValue;
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<IdRule>();
            }

            protected override IClassRule GetValue(IReferenceExpression semanticElement, ParseContext context)
            {
                return semanticElement.ReferencedRule;
            }

            protected override string GetReferenceString(IClassRule reference, ParseContext context)
            {
                return reference.ToIdentifierString();
            }
        }


        public class ParserExpressionFormattingInstructionsRule : AddAssignRule<IParserExpression, FormattingInstruction>
        {

            protected override String Feature
            {
                get
                {
                    return "FormattingInstructions";
                }
            }

            public override void Initialize(GrammarContext context)
            {
                Inner = context.ResolveRule<FormattingInstructionRule>();
            }

            public override ICollection<FormattingInstruction> GetCollection(IParserExpression semanticElement, ParseContext context)
            {
                return semanticElement.FormattingInstructions;
            }
        }

        public partial class FormattingInstructionRule : EnumRule<FormattingInstruction>
        {
            public override void Initialize(GrammarContext context)
            {
                Alternatives = new Rules.Rule[]
                {
                    context.ResolveKeyword("<nl>"),
                    context.ResolveKeyword("<ind>"),
                    context.ResolveKeyword("<unind>")
                };
                Values = new FormattingInstruction[]
                {
                    FormattingInstruction.Newline,
                    FormattingInstruction.Indent,
                    FormattingInstruction.Unindent
                };
            }
        }


        public partial class FormattingInstructionFragmentRule : QuoteRule
        {

            public override void Initialize(GrammarContext context)
            {
                Inner = new ZeroOrMoreRule(context.ResolveRule<ParserExpressionFormattingInstructionsRule>());
            }
        }
    }
}
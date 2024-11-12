using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        public partial class AddAssignExpressionRule
        {
            public override string TokenType => "keyword";
        }
        public partial class GrammarStartRuleRule : AssignModelReferenceRule<IGrammar, IClassRule>
        {
            public override string TokenType => "class";
        }



        public partial class GrammarLanguageIdRule : AssignRule<IGrammar, string>
        {
            public override string TokenType => "namespace";
        }

        public partial class GrammarNameRule : AssignRule<IGrammar, string>
        {
            public override string TokenType => "namespace";
        }

        public partial class MetamodelImportFileRule : AssignRule<IMetamodelImport, string>
        {
            public override string TokenType => "string";
        }

        public partial class MetamodelImportPrefixRule : AssignRule<IMetamodelImport, string>
        {
            public override string TokenType => "namespace";
        }

        public partial class InheritanceRuleSubtypesRule : AddAssignModelReferenceRule<IInheritanceRule, IClassRule>
        {
            public override string TokenType => "variable";
        }

        public partial class RuleNameRule : AssignRule<IRule, string>
        {
            public override string TokenType => "class";
        }
        public partial class DataRuleSurroundCharacterRule : AssignRule<IDataRule, string>
        {
            public override string TokenType => "keyword";
        }

        public partial class DataRuleRegexRule : AssignRule<IDataRule, string>
        {
            public override string TokenType => "regexp";
        }

        public partial class EscapeRuleEscapeRule : AssignRule<IEscapeRule, string>
        {
            public override string TokenType => "keyword";
        }

        public partial class EscapeRuleCharacterRule : AssignRule<IEscapeRule, string>
        {
            public override string TokenType => "keyword";
        }


        public partial class RuleTypeNameRule : AssignRule<IRule, string>
        {
            public override string TokenType => "parameter";
        }

        public partial class RulePrefixRule : AssignRule<IRule, string>
        {
            public override string TokenType => "keyword";
        }


        public partial class ParanthesisRuleInnerRuleRule : AssignModelReferenceRule<IParanthesisRule, IClassRule>
        {
            public override string TokenType => "variable";
        }

        public partial class LiteralRuleKeywordRule : AssignRule<ILiteralRule, string>
        {
            public override string TokenType => "keyword";
        }

        public partial class LiteralRuleLiteralRule : AssignRule<ILiteralRule, string>
        {
            public override string TokenType => "keyword";
        }


        public partial class KeywordExpressionKeywordRule : AssignRule<IKeywordExpression, string>
        {
            public override string TokenType => "keyword";
        }

        public partial class FeatureExpressionAssignedRule : AssignRule<IFeatureExpression, IParserExpression>
        {
            public override string TokenType => "variable";
        }

        public partial class FeatureExpressionFeatureRule : AssignRule<IFeatureExpression, string>
        {
            public override string TokenType => "property";
        }

        public partial class RuleExpressionRuleRule : AssignModelReferenceRule<IRuleExpression, IRule>
        {
            public virtual string[] TokenModifiers => new string[] { "readonly", "static" };
            public override string TokenType => "enum";
        }

        public partial class ReferenceExpressionReferencedRuleRule : AssignModelReferenceRule<IReferenceExpression, IClassRule>
        {
            public override string TokenType => "type";
        }
    }
}

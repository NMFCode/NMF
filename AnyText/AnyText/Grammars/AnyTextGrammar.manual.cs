using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System.Runtime.CompilerServices;
using LiteralRule = NMF.AnyText.Rules.LiteralRule;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        protected override LiteralRule CreateKeywordRule(string keyword)
        {
            var rule = new LiteralRule(keyword);
            if (!char.IsLetterOrDigit(keyword[0]))
            {
                rule.TokenType = "operator";
            }

            return rule;
        }
        public partial class RuleRule
        {
            public override string[] TokenModifiers => new [] { "definition" };

        }
        public partial class InheritanceRuleRule
        {
            public override string[] TokenModifiers => new [] { "declaration" };

        }
        public partial class GrammarStartRuleRule
        {
            public override string TokenType => "variable";


        }
        public partial class GrammarLanguageIdRule
        {
            public override string TokenType => "string";
            

        }
        public partial class GrammarNameRule
        {
            public override string TokenType => "namespace";

        }
        public partial class MetamodelImportFileRule
        {
            public override string TokenType => "string";

        }
        public partial class MetamodelImportPrefixRule
        {
            public override string TokenType => "variable";

        }
        public partial class InheritanceRuleSubtypesRule
        {
            public override string TokenType => "type";
        }
        public partial class FragmentRuleRule
        {
            public override string TokenType => "function";

        }
        public partial class RuleTypeNameRule
        { 
            public override string TokenType => "variable";
        }
        public partial class DataRuleRegexRule
        {
            public override string TokenType => "regexp";
            
        }
        public partial class DataRuleRule
        {
            public override string TokenType => "regexp";
        }
        public partial class ParanthesisRuleRule
        {
            public override string TokenType => "interface";
        }
        public partial class ParanthesisRuleInnerRuleRule
        {
            public override string TokenType => "parameter";

        }
        public partial class ClassRuleRule
        {
            public override string TokenType => "class";
        }
        public partial class EnumRuleRule
        {
            public override string TokenType => "enum";
            public override string[] TokenModifiers => new [] { "declaration" };

        }
        public partial class EnumRuleLiteralsRule
        {
            public override string TokenType => "enumMember";
            public override string[] TokenModifiers => new [] { "definition" };


        }
        public partial class KeywordExpressionKeywordRule
        {
            public override string TokenType => "keyword";
            public override string[] TokenModifiers => new [] { "declaration" };

        }
        public partial class FeatureExpressionFeatureRule
        {
            public override string TokenType => "property";
            public override string[] TokenModifiers => new [] { "declaration" };


        }
        public partial class RulePrefixRule
        {
            public override string TokenType => "string";
        }

        public partial class RuleExpressionRuleRule
        {
          public override string TokenType => "type";

        }

        public partial class ReferenceExpressionReferencedRuleRule
        {
            public override string TokenType => "class";

        }
        public partial class KeywordRule
        {
            public override string TokenType => "keyword";
        }
    }
}

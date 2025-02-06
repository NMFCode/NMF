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
        /// <inheritdoc/>
        public override string[] CompletionTriggerCharacters() => new[] {"." };


        public partial class GrammarNameIDRule
        {
            public override string TokenType => "type";
        }

        public partial class GrammarStartRuleClassRuleRule
        {
            public override string TokenType => "function";
        }

        public partial class InheritanceRuleRule
        {
            public override bool IsFoldable() => true;

        }
        public partial class MetamodelImportPrefixIDRule
        {
            public override string TokenType => "variable";

        }
        public partial class InheritanceRuleSubtypesClassRuleRule
        {
            public override string TokenType => "type";
        }
        public partial class FragmentRuleRule
        {
            public override bool IsFoldable() => true;

        }

        public partial class RuleNameIDRule
        {
            public override string TokenType => "function";

            public override string[] TokenModifiers => new[] { "declaration" }; 
        }

        public partial class RuleTypeNameIDRule
        {
            public override string TokenType => "type";
        }
        public partial class DataRuleRegexRegexRule
        {
            public override string TokenType => "regexp";
            
        }
        public partial class DataRuleRule
        {
            public override bool IsFoldable() => true;
        }
        public partial class ParanthesisRuleInnerRuleClassRuleRule
        {
            public override string TokenType => "parameter";

        }
        public partial class EnumRuleRule
        {
            public override string TokenType => "enum";
            public override string[] TokenModifiers => new [] { "declaration" };
            public override bool IsFoldable() => true;
        }
        public partial class EnumRuleLiteralsLiteralRuleRule
        {
            public override string TokenType => "enumMember";
            public override string[] TokenModifiers => new [] { "definition" };


        }
        public partial class KeywordExpressionKeywordKeywordRule
        {
            public override string TokenType => "keyword";
            public override string[] TokenModifiers => new [] { "declaration" };

        }
        public partial class FeatureExpressionFeatureIdOrContextRefRule
        {
            public override string TokenType => "property";
            public override string[] TokenModifiers => new [] { "declaration" };


        }

        public partial class ReferenceExpressionReferencedRuleRuleRule
        {
            public override string TokenType => "type";
        }

        public partial class RuleExpressionRule
        {
            public override string TokenType => "function";
        }

        public partial class KeywordRule
        {
            public override string TokenType => "keyword";
            public override string[] TokenModifiers => new [] { "definition" };

        }

        public partial class GrammarImportsMetamodelImportRule
        {
            public override bool IsImports() => true;
        }

        public partial class ModelRuleRule
        {
            public override bool IsFoldable() => true;
        }
    }
}

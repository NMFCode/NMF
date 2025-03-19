using NMF.AnyText.Rules;
using System.Runtime.CompilerServices;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        public override string[] CompletionTriggerCharacters() => new[] { "." };

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
            public override SymbolKind SymbolKind => SymbolKind.Class;

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
            public override SymbolKind SymbolKind => SymbolKind.Function;
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
            public override SymbolKind SymbolKind => SymbolKind.Constant;
        }

        public partial class ParanthesisRuleRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Interface;

            public override SymbolTag[] SymbolTags(RuleApplication ruleApplication) => new SymbolTag[] { SymbolTag.Deprecated };
        }

        public partial class ParanthesisRuleInnerRuleClassRuleRule
        {
            public override string TokenType => "parameter";
        }

        public partial class EnumRuleRule
        {
            public override string TokenType => "enum";
            public override string[] TokenModifiers => new[] { "definition" };
            public override bool IsFoldable() => true;
            public override SymbolKind SymbolKind => SymbolKind.Enum;

            public override string GetHoverText(RuleApplication ruleApplication, Parser document, ParsePosition position) => "Special Enum rule";
        }

        public partial class EnumRuleLiteralsLiteralRuleRule
        {
            public override string TokenType => "enumMember";
            public override string[] TokenModifiers => new[] { "definition" };
            public override SymbolKind SymbolKind => SymbolKind.EnumMember;
        }

        public partial class KeywordExpressionKeywordKeywordRule
        {
            public override string TokenType => "keyword";
            public override string[] TokenModifiers => new[] { "definition" };

        }

        public partial class FeatureExpressionFeatureIdOrContextRefRule
        {
            public override string TokenType => "property";
            public override string[] TokenModifiers => new[] { "declaration" };
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
            public override string[] TokenModifiers => new[] { "definition" };

        }

        public partial class IDRule
        {
            public override bool IsIdentifier => true;
        }

        public partial class GrammarImportsMetamodelImportRule
        {
            public override bool IsImports() => true;
        }

        public partial class GrammarRule
        {
            public override SymbolKind SymbolKind => SymbolKind.File;
        }

        public partial class ModelRuleRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Class;
            public override bool IsFoldable() => true;
        }
    }
}
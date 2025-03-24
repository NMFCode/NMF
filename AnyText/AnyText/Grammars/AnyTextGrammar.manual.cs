using NMF.AnyText.Rules;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace NMF.AnyText.Grammars
{
    public partial class AnyTextGrammar
    {
        public partial class GrammarNameIDRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";
        }
        public partial class GrammarStartRuleClassRuleRule
        {
            /// <inheritdoc />
            public override string TokenType => "function";
        }
        public partial class InheritanceRuleRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Class;
        }
        public partial class MetamodelImportPrefixIDRule
        {
            /// <inheritdoc />
            public override string TokenType => "variable";
        }
        public partial class InheritanceRuleSubtypesClassRuleRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";
        }
        public partial class FragmentRuleRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Function;
        }
        public partial class RuleNameIDRule
        {
            /// <inheritdoc />
            public override string TokenType => "function";

            /// <inheritdoc />
            public override string[] TokenModifiers => new[] { "declaration" };
        }
        public partial class RuleTypeNameIDRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";
        }
        public partial class DataRuleRegexRegexRule
        {
            /// <inheritdoc />
            public override string TokenType => "regexp";
        }
        public partial class DataRuleRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Constant;
        }
        public partial class ParanthesisRuleRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Interface;
        }
        public partial class ParanthesisRuleInnerRuleClassRuleRule
        {
            /// <inheritdoc />
            public override string TokenType => "parameter";
        }
        public partial class EnumRuleRule
        {
            /// <inheritdoc />
            public override string TokenType => "enum";
            /// <inheritdoc />
            public override string[] TokenModifiers => new[] { "definition" };
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Enum;
        }
        public partial class EnumRuleLiteralsLiteralRuleRule
        {
            /// <inheritdoc />
            public override string TokenType => "enumMember";
            /// <inheritdoc />
            public override string[] TokenModifiers => new[] { "definition" };
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.EnumMember;
        }
        public partial class KeywordExpressionKeywordKeywordRule
        {
            /// <inheritdoc />
            public override string TokenType => "keyword";
            /// <inheritdoc />
            public override string[] TokenModifiers => new[] { "definition" };
        }
        public partial class FeatureExpressionFeatureIdOrContextRefRule
        {
            /// <inheritdoc />
            public override string TokenType => "property";
            /// <inheritdoc />
            public override string[] TokenModifiers => new[] { "declaration" };
        }
        public partial class ReferenceExpressionReferencedRuleRuleRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";
        }
        public partial class RuleExpressionRule
        {
            /// <inheritdoc />
            public override string TokenType => "function";
        }
        public partial class KeywordRule
        {
            /// <inheritdoc />
            public override string TokenType => "keyword";
            /// <inheritdoc />
            public override string[] TokenModifiers => new[] { "definition" };

        }
        public partial class IDRule
        {
            /// <inheritdoc />
            public override bool IsIdentifier => true;
        }
        public partial class GrammarImportsMetamodelImportRule
        {
            /// <inheritdoc />
            public override bool IsImports() => true;
        }
        public partial class GrammarRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.File;
        }
        public partial class ModelRuleRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Class;
        }
    }
}
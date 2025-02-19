using NMF.AnyText.Grammars;
using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;
using NMF.Models;
using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace NMF.AnyText.AnyMeta
{
    public partial class AnyMetaGrammar
    {
        /// <inheritdoc />
        protected override ParseContext CreateParseContext()
        {
            return new AnyMetaParseContext(this);
        }

        private static string GetQualifiedType(IType reference, object contextElement)
        {
            if (contextElement is IModelElement modelElement)
            {
                var namespaceOfReference = reference.Namespace;
                if (namespaceOfReference != null && !modelElement.Ancestors().Contains(namespaceOfReference))
                {
                    return $"{namespaceOfReference.Prefix}.{reference.Name}";
                }
            }
            return reference.Name;
        }

        public partial class ClassRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Class;
        }

        public partial class NamespaceRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Package;
        }

        public partial class AttributeRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Property;
        }

        public partial class ReferenceRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Property;
        }

        public partial class OperationRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Method;
        }

        public partial class ParameterRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Variable;
        }

        public partial class EnumerationRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Enum;
        }

        public partial class LiteralRule
        {
            public override SymbolKind SymbolKind => SymbolKind.EnumMember;
        }

        public partial class ExtensionRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Interface;
        }

        public partial class DataTypeRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Struct;
        }

        public partial class PrimitiveTypeRule
        {
            public override SymbolKind SymbolKind => SymbolKind.Struct;
        }

        public partial class MetaElementNameIdentifierRule
        {
            public override string TokenType => "type";
        }

        public partial class TypedElementTypeTypeRule
        {
            public override string TokenType => "type";

            protected override string GetReferenceString(IType reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ReferenceReferenceTypeReferenceTypeRule
        {
            public override string TokenType => "type";

            protected override string GetReferenceString(IReferenceType reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ClassBaseTypesClassRule
        {
            public override string TokenType => "class";

            protected override string GetReferenceString(IClass reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ClassInstanceOfClassRule
        {
            public override string TokenType => "class";

            protected override string GetReferenceString(IClass reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ReferenceOppositeReferenceRule
        {
            public override string TokenType => "property";

            protected override bool TryResolveReference(IReference contextElement, string input, ParseContext context, out IReference resolved)
            {
                if (contextElement.ReferenceType is IClass referencedClass)
                {
                    resolved = referencedClass.LookupReference(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }

            protected override byte ResolveDelayLevel => 1;
        }

        public partial class ReferenceRefinesReferenceRule
        {
            public override string TokenType => "property";

            protected override bool TryResolveReference(IReference contextElement, string input, ParseContext context, out IReference resolved)
            {
                if (contextElement.DeclaringType is IClass declaringClass)
                {
                    resolved = declaringClass.LookupReference(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }

            protected override byte ResolveDelayLevel => 1;
        }

        public partial class AttributeRefinesAttributeRule
        {
            public override string TokenType => "property";

            protected override bool TryResolveReference(IAttribute contextElement, string input, ParseContext context, out IAttribute resolved)
            {
                if (contextElement.DeclaringType is IClass declaringClass)
                {
                    resolved = declaringClass.LookupAttribute(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }

            protected override byte ResolveDelayLevel => 1;

        }

        public partial class OperationRefinesOperationRule
        {
            public override string TokenType => "property";

            protected override bool TryResolveReference(IOperation contextElement, string input, ParseContext context, out IOperation resolved)
            {
                if (contextElement.DeclaringType is IClass declaringClass)
                {
                    resolved = declaringClass.LookupOperation(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }

            protected override byte ResolveDelayLevel => 1;
        }

        public partial class TypedElementLowerBoundIntegerRule
        {
            protected override bool CanSynthesize(ITypedElement semanticElement, int propertyValue)
            {
                return semanticElement.UpperBound != propertyValue;
            }

            public override string TokenType => "operator";
        }

        public class TypedElementLowerAndUpperBoundRule : TypedElementUpperBoundBoundRule
        {
            protected override void SetValue(ITypedElement semanticElement, int propertyValue, ParseContext context)
            {
                base.SetValue(semanticElement, propertyValue, context);
                semanticElement.LowerBound = propertyValue;
            }

            public override string TokenType => "operator";
        }

        public partial class TypedElementUpperBoundBoundRule
        {
            public override string TokenType => "operator";
        }

        public partial class BoundsRule
        {
            protected override void PostInitialize(GrammarContext context)
            {
                Inner = RuleFormatter.ZeroOrOne(
                    new SequenceRule(context.ResolveKeyword("[", FormattingInstruction.SupressSpace), 
                        new ChoiceRule(
                            new SequenceRule(context.ResolveFormattedRule<TypedElementLowerBoundIntegerRule>(FormattingInstruction.SupressSpace), context.ResolveKeyword("..", FormattingInstruction.SupressSpace), context.ResolveFormattedRule<TypedElementUpperBoundBoundRule>(FormattingInstruction.SupressSpace)), 
                            context.ResolveFormattedRule<TypedElementLowerAndUpperBoundRule>(FormattingInstruction.SupressSpace)), 
                        context.ResolveKeyword("]")));
            }
        }
    }
}

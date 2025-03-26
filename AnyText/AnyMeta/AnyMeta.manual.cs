using NMF.AnyText.Grammars;
using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Meta;
using System;
using System.Linq;

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

        private static string ValidateChildNameAlreadyTaken(IMetaElement element)
        {
            if (element == null)
            {
                return null;
            }
            var other = element.Parent?.Children.OfType<IMetaElement>()
                .FirstOrDefault(e => e != element && e.Name == element.Name);
            if (other != null)
            {
                return $"There is already {other} with the same name.";
            }
            return null;
        }

        public partial class ClassRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Class;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class NamespaceRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Package;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class AttributeRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Property;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class ReferenceRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Property;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class OperationRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Method;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class ParameterRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Variable;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class EnumerationRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Enum;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class LiteralRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.EnumMember;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class ExtensionRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Interface;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class DataTypeRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Struct;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class PrimitiveTypeRule
        {
            /// <inheritdoc />
            public override SymbolKind SymbolKind => SymbolKind.Struct;

            /// <inheritdoc />
            protected override void PostInitialize(GrammarContext context)
            {
                Validate(ValidateChildNameAlreadyTaken, DiagnosticSeverity.Error);
            }
        }

        public partial class MetaElementNameIdentifierRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";
        }

        public partial class TypedElementTypeTypeRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";

            /// <inheritdoc />
            protected override string GetReferenceString(IType reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ReferenceReferenceTypeReferenceTypeRule
        {
            /// <inheritdoc />
            public override string TokenType => "type";

            /// <inheritdoc />
            protected override string GetReferenceString(IReferenceType reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ClassBaseTypesClassRule
        {
            /// <inheritdoc />
            public override string TokenType => "class";

            /// <inheritdoc />
            protected override string GetReferenceString(IClass reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ClassInstanceOfClassRule
        {
            /// <inheritdoc />
            public override string TokenType => "class";

            /// <inheritdoc />
            protected override string GetReferenceString(IClass reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ReferenceOppositeReferenceRule
        {
            /// <inheritdoc />
            public override string TokenType => "property";

            /// <inheritdoc />
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

            /// <inheritdoc />
            protected override byte ResolveDelayLevel => 1;
        }

        public partial class ReferenceRefinesReferenceRule
        {
            /// <inheritdoc />
            public override string TokenType => "property";

            /// <inheritdoc />
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

            /// <inheritdoc />
            protected override byte ResolveDelayLevel => 1;
        }

        public partial class AttributeRefinesAttributeRule
        {
            /// <inheritdoc />
            public override string TokenType => "property";

            /// <inheritdoc />
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

            /// <inheritdoc />
            protected override byte ResolveDelayLevel => 1;

        }

        public partial class OperationRefinesOperationRule
        {
            /// <inheritdoc />
            public override string TokenType => "property";

            /// <inheritdoc />
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

            /// <inheritdoc />
            protected override byte ResolveDelayLevel => 1;
        }

        public partial class TypedElementLowerBoundIntegerRule
        {
            /// <inheritdoc />
            protected override bool CanSynthesize(ITypedElement semanticElement, int propertyValue)
            {
                return semanticElement.UpperBound != propertyValue;
            }

            /// <inheritdoc />
            public override string TokenType => "operator";
        }

        /// <summary>
        /// A custom rule that sets both upper and lower bound
        /// </summary>
        public class TypedElementLowerAndUpperBoundRule : TypedElementUpperBoundBoundRule
        {
            /// <inheritdoc />
            protected override void SetValue(ITypedElement semanticElement, int propertyValue, ParseContext context)
            {
                base.SetValue(semanticElement, propertyValue, context);
                semanticElement.LowerBound = propertyValue;
            }

            /// <inheritdoc />
            public override string TokenType => "operator";
        }

        public partial class TypedElementUpperBoundBoundRule
        {
            /// <inheritdoc />
            public override string TokenType => "operator";
        }

        public partial class BoundsRule
        {
            /// <inheritdoc />
            /// <remarks>The contents of the rule are overridden here</remarks>
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

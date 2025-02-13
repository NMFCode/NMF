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

        public partial class TypedElementTypeTypeRule
        {
            protected override string GetReferenceString(IType reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ReferenceReferenceTypeReferenceTypeRule
        {
            protected override string GetReferenceString(IReferenceType reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ClassBaseTypesClassRule
        {
            protected override string GetReferenceString(IClass reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ClassInstanceOfClassRule
        {
            protected override string GetReferenceString(IClass reference, object contextElement, ParseContext context)
            {
                return GetQualifiedType(reference, contextElement);
            }
        }

        public partial class ReferenceOppositeReferenceRule
        {
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
        }

        public class TypedElementLowerAndUpperBoundRule : TypedElementUpperBoundBoundRule
        {
            protected override void SetValue(ITypedElement semanticElement, int propertyValue, ParseContext context)
            {
                base.SetValue(semanticElement, propertyValue, context);
                semanticElement.LowerBound = propertyValue;
            }
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

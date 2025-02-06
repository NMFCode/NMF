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

        public partial class ReferenceOppositeReferenceRule
        {
            protected override bool TryResolveReference(object contextElement, string input, ParseContext context, out IReference resolved)
            {
                if (contextElement is IReference reference && reference.ReferenceType is IClass referencedClass)
                {
                    resolved = referencedClass.LookupReference(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }
        }

        public partial class ReferenceRefinesReferenceRule
        {
            protected override bool TryResolveReference(object contextElement, string input, ParseContext context, out IReference resolved)
            {
                if (contextElement is IReference reference && reference.DeclaringType is IClass declaringClass)
                {
                    resolved = declaringClass.LookupReference(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }
        }

        public partial class AttributeRefinesAttributeRule
        {
            protected override bool TryResolveReference(object contextElement, string input, ParseContext context, out IAttribute resolved)
            {
                if (contextElement is IReference reference && reference.DeclaringType is IClass declaringClass)
                {
                    resolved = declaringClass.LookupAttribute(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }

        }

        public partial class OperationRefinesOperationRule
        {

            protected override bool TryResolveReference(object contextElement, string input, ParseContext context, out IOperation resolved)
            {
                if (contextElement is IReference reference && reference.DeclaringType is IClass declaringClass)
                {
                    resolved = declaringClass.LookupOperation(input);
                    return resolved != null;
                }
                resolved = null;
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public static class ClassExtensions
    {
        public static bool IsRefined(this IClass @class, IReference reference)
        {
            var refine = @class.ReferenceConstraints.Any(c => c.Constrains == reference) || @class.References.Any(r => r.Refines == reference);
            if (refine) return true;
            foreach (var baseType in @class.BaseTypes)
            {
                if (baseType == reference.DeclaringType) return false;
                if (IsRefined(baseType, reference)) return true;
            }
            return false;
        }

        public static bool IsRefined(this IClass @class, IAttribute attribute)
        {
            var refine = @class.AttributeConstraints.Any(c => c.Constrains == attribute) || @class.Attributes.Any(r => r.Refines == attribute);
            if (refine) return true;
            foreach (var baseType in @class.BaseTypes)
            {
                if (baseType == attribute.DeclaringType) return false;
                if (IsRefined(baseType, attribute)) return true;
            }
            return false;
        }

        public static IReference LookupReference(this IClass @class, string name)
        {
            var reference = @class.References.FirstOrDefault(r => r.Name == name);
            if (reference != null) return reference;
            foreach (var baseType in @class.BaseTypes)
            {
                reference = LookupReference(baseType, name);
                if (reference != null) return reference;
            }
            return null;
        }

        public static IAttribute LookupAttribute(this IClass @class, string name)
        {
            var attribute = @class.Attributes.FirstOrDefault(r => r.Name == name);
            if (attribute != null) return attribute;
            foreach (var baseType in @class.BaseTypes)
            {
                attribute = LookupAttribute(baseType, name);
                if (attribute != null) return attribute;
            }
            return null;
        }

        public static bool IsContainerReference(this IReference reference)
        {
            return reference.Opposite != null && reference.Opposite.IsContainment;
        }

        public static IAttribute RetrieveIdentifier(this IClass @class)
        {
            if (@class.Identifier != null) return @class.Identifier;
            foreach (var baseType in @class.BaseTypes.Where(b => !b.IsInterface))
            {
                var ident = baseType.RetrieveIdentifier();
                if (ident != null) return ident;
            }
            foreach (var baseType in @class.BaseTypes.Where(b => b.IsInterface))
            {
                var ident = baseType.RetrieveIdentifier();
                if (ident != null) return ident;
            }
            return null;
        }

        public static bool IsAssignableFrom(this IClass @class, IClass specificType)
        {
            if (specificType == null) throw new ArgumentNullException("specificType");
            return specificType == @class || specificType.BaseTypes.Any(t => t.IsAssignableFrom(specificType));
        }
    }
}

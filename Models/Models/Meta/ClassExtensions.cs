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

        public static IOperation LookupOperation(this IClass @class, string name)
        {
            var operation = @class.Operations.FirstOrDefault(op => op.Name == name);
            if (operation != null) return operation;
            foreach (var baseType in @class.BaseTypes)
            {
                operation = LookupOperation(baseType, name);
                if (operation != null) return operation;
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

        public static ScopedIdentifier RetrieveIdentifier(this IClass @class)
        {
            if (@class.Identifier != null)
            {
                return new ScopedIdentifier(@class.Identifier, @class.IdentifierScope.GetActual(IdentifierScope.Local));
            }
            else
            {
                foreach (var baseType in @class.BaseTypes)
                {
                    var id = baseType.RetrieveIdentifier();
                    if (id.Identifier != null)
                    {
                        return id;
                    }
                }
            }
            return new ScopedIdentifier();
        }

        public static bool IsAssignableFrom(this IClass @class, IClass specificType)
        {
            if (specificType == null) throw new ArgumentNullException("specificType");
            return specificType == @class || specificType.BaseTypes.Any(t => @class.IsAssignableFrom(t));
        }
    }
}

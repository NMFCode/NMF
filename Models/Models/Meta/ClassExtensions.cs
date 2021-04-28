using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    /// <summary>
    /// Denotes extension methods for Class instances
    /// </summary>
    public static class ClassExtensions
    {
        /// <summary>
        /// Determines whether the given reference is refined in the scope of this class
        /// </summary>
        /// <param name="class">the context class</param>
        /// <param name="reference">the reference</param>
        /// <returns>True, if the reference is refined otherwise False</returns>
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

        /// <summary>
        /// Determines whether the given attribute is refined in the scope of this class
        /// </summary>
        /// <param name="class">the context class</param>
        /// <param name="attribute">the attribute</param>
        /// <returns>True, if the attribute is refined, otherwise False</returns>
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

        /// <summary>
        /// Looks up the reference with the given name
        /// </summary>
        /// <param name="class">the context class</param>
        /// <param name="name">the name of the reference</param>
        /// <returns>The reference or null, if no such reference could be found</returns>
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

        /// <summary>
        /// Looks up the operation with the given name
        /// </summary>
        /// <param name="class">the context class</param>
        /// <param name="name">the name of the operation</param>
        /// <returns>The operation or null, if no such operation was found</returns>
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

        /// <summary>
        /// Looks up the attribute with the given name
        /// </summary>
        /// <param name="class">the context class</param>
        /// <param name="name">the name of the attribute</param>
        /// <returns>The attribute or null, if no such attribute could be found</returns>
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

        /// <summary>
        /// Determines whethe the given reference is a reference to the container
        /// </summary>
        /// <param name="reference">the reference</param>
        /// <returns>True, if the reference has an opposite containment reference, otherwise False</returns>
        public static bool IsContainerReference(this IReference reference)
        {
            return reference.Opposite != null && reference.Opposite.IsContainment;
        }

        /// <summary>
        /// Retrieves the identifier of the class
        /// </summary>
        /// <param name="class">the class</param>
        /// <returns>A scoped identifier</returns>
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

        /// <summary>
        /// Determines whether an instance of the provided class can be assigned to this class
        /// </summary>
        /// <param name="class">the more abstract class</param>
        /// <param name="specificType">the more concrete class</param>
        /// <returns>True, if the provided class is a derived class, otherwise False</returns>
        public static bool IsAssignableFrom(this IClass @class, IClass specificType)
        {
            if (specificType == null) throw new ArgumentNullException("specificType");
            return specificType == @class || specificType.BaseTypes.Any(t => @class.IsAssignableFrom(t));
        }
    }
}

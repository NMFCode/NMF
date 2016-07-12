using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Analysis;
using NMF.Models.Repository;

namespace NMF.Models.Meta
{
    internal static class ModelExtensions
    {
        public static ICollection<IAttribute> Decompose(this IClass scope, IAttribute attribute)
        {
            var layering = Layering<IClass>.CreateLayers(scope, cl => cl.BaseTypes);
            var attributes = new HashSet<IAttribute>();
            attributes.Add(attribute);
            foreach (var layer in layering)
            {
                if (layer.Count > 1)
                {
                    throw new InvalidOperationException("There is a cycle in the inheritance hierarchy.");
                }
                foreach (var cl in layer)
                {
                    foreach (var att in cl.Attributes)
                    {
                        if (att.Refines != null && attributes.Contains(att.Refines))
                        {
                            attributes.Add(att);
                        }
                    }
                }
            }
            return attributes;
        }

        public static ICollection<IAttribute> MostSpecificRefinement(this IClass scope, IAttribute attribute)
        {
            var decompositions = scope.Decompose(attribute);
            var result = new List<IAttribute>();
            foreach (var att in decompositions)
            {
                if (!decompositions.Any(a => a.Refines == att))
                {
                    result.Add(att);
                }
            }
            return result;
        }

        public static ICollection<IReference> Decompose(this IClass scope, IReference reference)
        {
            var layering = Layering<IClass>.CreateLayers(scope, cl => cl.BaseTypes);
            var references = new HashSet<IReference>();
            references.Add(reference);
            foreach (var layer in layering)
            {
                if (layer.Count > 1)
                {
                    throw new InvalidOperationException("There is a cycle in the inheritance hierarchy.");
                }
                foreach (var cl in layer)
                {
                    foreach (var r in cl.References)
                    {
                        if (r.Refines != null && references.Contains(r.Refines))
                        {
                            references.Add(r);
                        }
                    }
                }
            }
            return references;
        }

        public static ICollection<IReference> MostSpecificRefinement(this IClass scope, IReference reference)
        {
            var decompositions = scope.Decompose(reference);
            var result = new List<IReference>();
            foreach (var att in decompositions)
            {
                if (!decompositions.Any(a => a.Refines == att))
                {
                    result.Add(att);
                }
            }
            return result;
        }

        public static ICollection<string> GetAttributeConstraintValue(this IClass referenceClass, IAttribute attribute)
        {
            var attRefinement = referenceClass.AttributeConstraints.FirstOrDefault(c => c.Constrains == attribute);
            if (attRefinement != null)
            {
                return attRefinement.Values;
            }
            foreach (var baseClass in referenceClass.BaseTypes)
            {
                var value = baseClass.GetAttributeConstraintValue(attribute);
                if (value != null)
                {
                    return value;
                }
            }
            return null;
        }

        public static ICollection<IModelElement> GetReferenceConstraintValue(this IClass referenceClass, IReference reference)
        {
            var refRefinement = referenceClass.ReferenceConstraints.FirstOrDefault(c => c.Constrains == reference);
            if (refRefinement != null)
            {
                return refRefinement.References;
            }
            foreach (var baseClass in referenceClass.BaseTypes)
            {
                var value = baseClass.GetReferenceConstraintValue(reference);
                if (value != null)
                {
                    return value;
                }
            }
            return null;
        }

        public static int? GetUpperBoundConstraintValue(this IClass scope)
        {
            var upperBound = scope.GetAttributeConstraintValue(TypedElementUpperBoundAttribute);
            if (upperBound != null && upperBound.Count == 1)
            {
                return int.Parse(upperBound.First(), CultureInfo.InvariantCulture);
            }
            else
            {
                return null;
            }
        }

        public static int? GetLowerBoundConstraintValue(this IClass scope)
        {
            var lowerBound = scope.GetAttributeConstraintValue(TypedElementLoweroundAttribute);
            if (lowerBound != null && lowerBound.Count == 1)
            {
                return int.Parse(lowerBound.First(), CultureInfo.InvariantCulture);
            }
            else
            {
                return null;
            }
        }

        public static bool? GetIsUniqueConstraintValue(this IClass scope)
        {
            var isUnique = scope.GetAttributeConstraintValue(TypedElementIsUniqueAttribute);
            if (isUnique != null && isUnique.Count == 1)
            {
                return bool.Parse(isUnique.First());
            }
            else
            {
                return null;
            }
        }

        public static bool? GetIsOrderedConstraintValue(this IClass scope)
        {
            var isOrdered = scope.GetAttributeConstraintValue(TypedElementIsOrderedAttribute);
            if (isOrdered != null && isOrdered.Count == 1)
            {
                return bool.Parse(isOrdered.First());
            }
            else
            {
                return null;
            }
        }

        public static IReferenceType GetReferenceTypeConstraintValue(this IClass scope)
        {
            var types = scope.GetReferenceConstraintValue(ReferenceReferenceTypeReference);
            if (types != null)
            {
                return (IReferenceType)types.Single();
            }
            else
            {
                return null;
            }
        }

        public static readonly IAttribute TypedElementUpperBoundAttribute = (IAttribute)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//TypedElement/UpperBound");
        public static readonly IAttribute TypedElementLoweroundAttribute = (IAttribute)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//TypedElement/LowerBound");
        public static readonly IAttribute TypedElementIsOrderedAttribute = (IAttribute)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//TypedElement/IsOrdered");
        public static readonly IAttribute TypedElementIsUniqueAttribute = (IAttribute)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//TypedElement/IsUnique");
        public static readonly IReference ReferenceReferenceTypeReference = (IReference)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Reference/ReferenceType");
        public static readonly IReference ReferenceTypeReferencesReference = (IReference)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//ReferenceType/References");
        public static readonly IReference StructuredTypeAttributesReference = (IReference)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//StructuredType/Attributes");
        public static readonly IReference ClassBaseTypesReference = (IReference)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Class/BaseTypes");
        public static readonly IClass ClassModelElement = (IClass)MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//ModelElement/");
    }
}

using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace NMF.Models.Security
{
    /// <summary>
    /// A class that computes model hashes
    /// </summary>
    public static class ModelHasher
    {
        /// <summary>
        /// Creates a SHA512 hash for the given model
        /// </summary>
        /// <param name="modelElement">The model element for which a hash should be provided</param>
        /// <returns>A hash code for the given model element</returns>
        public static byte[] CreateHash(IModelElement modelElement)
        {
            return CreateHash(modelElement, modelElement?.Model, new SHA512Managed());
        }

        /// <summary>
        /// Creates a hash for the given model
        /// </summary>
        /// <param name="modelElement">The model element for which a hash should be provided</param>
        /// <param name="hashAlgorithm">Tha hashing algorithm that should be used or null to use the default SHA512</param>
        /// <returns>A hash code for the given model element</returns>
        public static byte[] CreateHash(IModelElement modelElement, HashAlgorithm hashAlgorithm)
        {
            return CreateHash(modelElement, modelElement?.Model, hashAlgorithm);
        }

        private static byte[] CreateHash(IModelElement modelElement, Model containingModel, HashAlgorithm hashAlgorithm)
        {
            if (modelElement == null) throw new ArgumentNullException(nameof(modelElement));
            if (containingModel == null) throw new ArgumentNullException(nameof(containingModel));
            
            return (hashAlgorithm ?? new SHA512Managed()).ComputeHash(CreateHashInternal(modelElement, containingModel).ToByteArray());
        }

        private static BigInteger CreateHashInternal(IModelElement modelElement, Model containingModel)
        {
            var hashCode = new BigInteger(GetDeterministicHashCode(modelElement.GetClass().AbsoluteUri.AbsoluteUri));
            ApplyClass(modelElement, containingModel, modelElement.GetClass(), new HashSet<IClass>() { ModelElement.ClassInstance }, ref hashCode);
            return hashCode;
        }

        private static void ApplyClass(IModelElement modelElement, Model containingModel, IClass @class, HashSet<IClass> visitedClasses, ref BigInteger hash)
        {
            foreach (var attribute in @class.Attributes)
            {
                hash = 23 * hash + HashAttribute(modelElement, attribute);
            }

            foreach (var reference in @class.References)
            {
                if (reference.IsContainment)
                {
                    hash = 29 * hash + HashReference(modelElement, reference, containingModel, HashContainedElement);
                }
                else
                {
                    hash = 31 * hash + HashReference(modelElement, reference, containingModel, HashCrossReferencedElement);
                }
            }

            foreach (var baseType in @class.BaseTypes)
            {
                if (visitedClasses.Add(baseType))
                {
                    ApplyClass(modelElement, containingModel, baseType, visitedClasses, ref hash);
                }
            }
        }

        private static BigInteger HashAttribute(IModelElement modelElement, IAttribute attribute)
        {
            var hash = new BigInteger(GetDeterministicHashCode(attribute.Name));
            if (attribute.UpperBound == 1)
            {
                var value = modelElement.GetAttributeValue(attribute);
                if (value != null)
                {
                    hash += GetDeterministicHashCode(value.ToString());
                }
            }
            else
            {
                var collection = modelElement.GetAttributeValues(attribute);
                if (collection != null)
                {
                    var index = 0;
                    foreach (var item in collection)
                    {
                        hash += index + GetDeterministicHashCode(item.ToString());
                        if (attribute.IsOrdered)
                        {
                            index++;
                        }
                    }
                }
            }
            return hash;
        }

        private static BigInteger HashCrossReferencedElement(IModelElement modelElement, Model containingModel)
        {
            if (modelElement == null)
            {
                return new BigInteger(37);
            }
            if (modelElement.Model == containingModel)
            {
                return new BigInteger(GetDeterministicHashCode(modelElement.RelativeUri.ToString().ToLowerInvariant()));
            }
            else
            {
                return new BigInteger(GetDeterministicHashCode(modelElement.AbsoluteUri.AbsoluteUri.ToLowerInvariant()));
            }
        }

        private static BigInteger HashContainedElement(IModelElement modelElement, Model containingModel)
        {
            if (modelElement == null)
            {
                return new BigInteger(41);
            }
            else
            {
                return CreateHashInternal(modelElement, containingModel);
            }
        }

        private static BigInteger HashReference(IModelElement modelElement, IReference reference, Model containingModel, Func<IModelElement, Model, BigInteger> computeElementHash)
        {
            var hash = new BigInteger(GetDeterministicHashCode(reference.Name));
            if (reference.UpperBound == 1)
            {
                var value = modelElement.GetReferencedElement(reference);
                if (value != null)
                {
                    hash += computeElementHash(value, containingModel);
                }
            }
            else
            {
                var collection = modelElement.GetReferencedElements(reference);
                if (collection != null)
                {
                    var index = 0;
                    foreach (IModelElement item in collection)
                    {
                        hash += index + computeElementHash(item, containingModel);
                        if (reference.IsOrdered)
                        {
                            index++;
                        }
                    }
                }
            }
            return hash;
        }

        private static int GetDeterministicHashCode(string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

    }
}

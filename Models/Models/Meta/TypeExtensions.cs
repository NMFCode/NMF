using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Meta
{
    /// <summary>
    /// Denotes common extensions to types
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Parses the given textual representation into an object
        /// </summary>
        /// <param name="type">The type of the data</param>
        /// <param name="input">The actual inputs</param>
        /// <returns>A deserialized object</returns>
        public static object Parse(this IType type, string input)
        {
            var systemType = type.GetExtension<MappedType>();
            if (systemType == null)
            {
                throw new ArgumentException("The given type is not mapped to a system type.", "type");
            }
            else
            {
                var typeConverter = TypeDescriptor.GetConverter(systemType.SystemType);
                if (typeConverter == null)
                {
                    throw new ArgumentException("The mapped system type does not have a type converter", "type");
                }
                return typeConverter.ConvertFromInvariantString(input);
            }
        }

        /// <summary>
        /// Serializes the given value of the provided type into a string
        /// </summary>
        /// <param name="type">The model type of the object</param>
        /// <param name="value">The value</param>
        /// <returns>A string representation</returns>
        public static string Serialize(this IType type, object value)
        {
            var systemType = type.GetExtension<MappedType>();
            if (systemType == null)
            {
                throw new ArgumentException("The given type is not mapped to a system type.", "type");
            }
            else
            {
                var typeConverter = TypeDescriptor.GetConverter(systemType.SystemType);
                if (typeConverter == null)
                {
                    throw new ArgumentException("The mapped system type does not have a type converter", "type");
                }
                return typeConverter.ConvertToInvariantString(value);
            }
        }
    }
}

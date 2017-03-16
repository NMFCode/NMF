using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Meta
{
    public static class TypeExtensions
    {
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

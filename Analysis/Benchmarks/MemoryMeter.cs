using System;
using System.Collections.Generic;
using System.Reflection;

namespace NMF.Benchmarks
{
    /// <summary>
    /// This class measures the memory consumption of objects including all references
    /// </summary>
    public class MemoryMeter
    {
        /// <summary>
        /// Retrieves the memory consumption of the given objects 
        /// </summary>
        /// <param name="args">Objects for which to measure the memory consumption</param>
        /// <returns>The consumed memory for a minimal representation in bytes</returns>
        public static long ComputeMemoryConsumption(params object[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (args.Length == 0) throw new ArgumentOutOfRangeException("args");

            var set = new HashSet<object>();
            var memory = 0L;
            for (int i = 0; i < args.Length; i++)
            {
                memory += ComputeMemoryConsumption(args[i], set);
            }
            GC.Collect();
            return memory;
        }

        /// <summary>
        /// The memory allocated for a pointer. 4 Bytes for a 32bit system, 8 bytes for a 64bit system
        /// </summary>
        public static long MemoryForPointer = sizeof(int);

        /// <summary>
        /// The memory allocated for an object. We need a pointer to the objects type and some memory for GC flags
        /// </summary>
        public static long MemoryForObject = MemoryForPointer + sizeof(int);

        /// <summary>
        /// Gets the size of the given object when the given list of objects already have been considered
        /// </summary>
        /// <param name="obj">The object to analyze</param>
        /// <param name="visited">The list of already visited objects</param>
        /// <returns>The memory consumption in bytes</returns>
        private static long ComputeMemoryConsumption(object obj, HashSet<object> visited)
        {
            if (obj == null) return 0;
            // Each object only requires memory a single time
            if (visited.Add(obj))
            {
                var type = obj.GetType();

                // We consider elements from the System.Reflection namespace as given, as these are also used by the runtime.
                // As a consequence, we do not count extra memory for them
                if (type.FullName.StartsWith("System.Reflection"))
                {
                    return 0;
                }

                long size = type.IsValueType ? 0 : MemoryForPointer;
                // loop over all base classes of the current object
                while (type != null)
                {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        bool checkInnerStructure;
                        size += GetTypeSize(fieldInfo.FieldType, out checkInnerStructure);
                        if (checkInnerStructure)
                        {
                            // field is struct or object
                            if (fieldInfo.FieldType.IsArray)
                            {
                                if (fieldInfo.GetValue(obj) is Array subObj)
                                {
                                    //overhead for array
                                    size += MemoryForObject;
                                    size += sizeof(int);
                                    //memory used for elements
                                    size += subObj.LongLength * GetTypeSize(fieldInfo.FieldType.GetElementType(), out checkInnerStructure);
                                    if (checkInnerStructure)
                                    {
                                        //analyze array elements
                                        foreach (object item in subObj)
                                        {
                                            size += ComputeMemoryConsumption(item, visited);
                                        }
                                    }
                                }
                            }
                            // special treatment for strings to reflect special treatment from runtime
                            else if (fieldInfo.FieldType == typeof(string))
                            {
                                var subObj = fieldInfo.GetValue(obj);
                                if (subObj != null)
                                {
                                    // we ignore interning of strings
                                    size += subObj.ToString().Length * sizeof(char) + MemoryForPointer;
                                }
                            }
                            else
                            {
                                var subObj = fieldInfo.GetValue(obj);
                                if (subObj != null)
                                {
                                    size += ComputeMemoryConsumption(subObj, visited);
                                }
                            }
                        }
                    }
                    type = type.BaseType;
                }

                return size;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the memory used for the given type
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="plusInnerStructure">Determines whether the inner structure of the type has to be considered</param>
        /// <returns>The memory consumed by an instance of the given type (in bytes)</returns>
        private static long GetTypeSize(Type type, out bool plusInnerStructure)
        {
            plusInnerStructure = false;
            if (type.IsEnum)
            {
                return GetTypeSize(type.GetEnumUnderlyingType(), out plusInnerStructure);
            }
            else if (type == typeof(double))
            {
                return sizeof(double);
            }
            else if (type == typeof(float))
            {
                return sizeof(float);
            }
            else if (type == typeof(char))
            {
                return sizeof(char);
            }
            else if (type == typeof(short))
            {
                return sizeof(short);
            }
            else if (type == typeof(int))
            {
                return sizeof(int);
            }
            else if (type == typeof(long))
            {
                return sizeof(long);
            }
            else if (type == typeof(ushort))
            {
                return sizeof(ushort);
            }
            else if (type == typeof(uint))
            {
                return sizeof(uint);
            }
            else if (type == typeof(ulong))
            {
                return sizeof(ulong);
            }
            else if (type == typeof(decimal))
            {
                return sizeof(decimal);
            }
            else if (type == typeof(byte))
            {
                return sizeof(byte);
            }
            else if (type == typeof(sbyte))
            {
                return sizeof(sbyte);
            }
            else if (type == typeof(bool))
            {
                return sizeof(bool);
            }
            else
            {
                plusInnerStructure = true;
                return type.IsValueType ? 0 : MemoryForObject;
            }
        }
    }
}

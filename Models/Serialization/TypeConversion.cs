using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace NMF.Serialization
{
    public static class TypeConversion
    {
        private static readonly Dictionary<Type, TypeConverter> standardTypes = new Dictionary<Type, TypeConverter>();

        static TypeConversion()
        {
            standardTypes.Add(typeof(string), new StringConverter());
            standardTypes.Add(typeof(Int32), new Int32Converter());
            standardTypes.Add(typeof(Int64), new Int64Converter());
            standardTypes.Add(typeof(Int16), new Int16Converter());
            standardTypes.Add(typeof(Boolean), new BooleanConverter());
            standardTypes.Add(typeof(UInt16), new UInt16Converter());
            standardTypes.Add(typeof(UInt32), new UInt32Converter());
            standardTypes.Add(typeof(UInt64), new UInt64Converter());
            standardTypes.Add(typeof(Byte), new ByteConverter());
            standardTypes.Add(typeof(SByte), new SByteConverter());
            standardTypes.Add(typeof(Double), new DoubleConverter());
            standardTypes.Add(typeof(Single), new SingleConverter());
            standardTypes.Add(typeof(Decimal), new DecimalConverter());
            standardTypes.Add(typeof(DateTime), new DateTimeConverter());
            standardTypes.Add(typeof(DateTimeOffset), new DateTimeOffsetConverter());
            standardTypes.Add(typeof(TimeSpan), new TimeSpanConverter());
        }


        public static TypeConverter GetTypeConverter(Type type)
        {
            var converterType = XmlSerializer.Fetch(XmlSerializer.FetchAttribute<XmlTypeConverterAttribute>(type, true), att => att.Type);
            if (converterType != null)
            {
                try
                {
                    return Activator.CreateInstance(converterType) as TypeConverter;
                }
                catch (Exception)
                { }
            }
            var converterTypeString = XmlSerializer.Fetch(XmlSerializer.FetchAttribute<TypeConverterAttribute>(type, true), att => att.ConverterTypeName);
            if (converterTypeString != null)
            {
                try
                {
                    return Activator.CreateInstance(Type.GetType(converterTypeString)) as TypeConverter;
                }
                catch (Exception) { }
            }
            TypeConverter converter;
            if (!standardTypes.TryGetValue(type, out converter))
            {
                if (type.IsEnum)
                {
                    converter = new EnumConverter(type);
                }
                else
                {
                    converterTypeString = XmlSerializer.Fetch(XmlSerializer.FetchAttribute<TypeConverterAttribute>(type, true), att => att.ConverterTypeName);
                    if (converterTypeString != null)
                    {
                        try
                        {
                            converter = Activator.CreateInstance(Type.GetType(converterTypeString)) as TypeConverter;
                        }
                        catch (Exception) { }
                    }
                }
                standardTypes.Add(type, converter);
            }
            return converter;
        }


        private static readonly Regex jsonParser = new Regex(@"^{(?<key>\w+)=(?<value>((?<brace>{)|[^{,}]|(?<-brace>})|(?(brace),))*)(?(brace)(?!))(,\s*(?<key>\w+)=(?<value>((?<brace>{)|[^{,}]|(?<-brace>})|(?(brace),))*)(?(brace)(?!)))*}$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Parses the given string as JSON
        /// </summary>
        /// <param name="json">A JSON string</param>
        /// <returns>If the provided string is a valid JSON object, then the result is a dictionary of the properties and provided values (which might be JSON strings themselves). Otherwise, null is returned</returns>
        public static IDictionary<string, string> ParseJson(string json)
        {
            if (json == null) return null;
            var match = jsonParser.Match(json);
            if (match.Success)
            {
                var result = new Dictionary<string, string>();
                var keys = match.Groups["key"];
                var values = match.Groups["value"];

                for (int i = 0; i < keys.Captures.Count; i++)
                {
                    result.Add(keys.Captures[i].Value, values.Captures[i].Value);
                }

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}

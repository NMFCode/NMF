//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;


namespace NMF.Interop.Cmof
{
    
    
    /// <summary>
    /// Implements a type converter for the enumeration VisibilityKind
    /// </summary>
    public class VisibilityKindConverter : TypeConverter
    {
        
        /// <summary>
        /// Determines whether the converter can convert from the provided source type into VisibilityKind
        /// </summary>
        /// <returns>true, if the converter can convert from the source type, otherwise false</returns>
        /// <param name="sourceType">the source type</param>
        /// <param name="context">the context in which the value should be transformed</param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return (sourceType == typeof(string));
        }
        
        /// <summary>
        /// Determines whether the converter can convert to the destination type from VisibilityKind
        /// </summary>
        /// <returns>true, if the converter can convert from the source type, otherwise false</returns>
        /// <param name="destinationType">the destination type</param>
        /// <param name="context">the context in which the value should be transformed</param>
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return (destinationType == typeof(string));
        }
        
        /// <summary>
        /// Convert the provided value into a VisibilityKind
        /// </summary>
        /// <returns>the converted value as a VisibilityKind</returns>
        /// <param name="value">the value to convert</param>
        /// <param name="context">the context in which the value should be transformed</param>
        /// <param name="culture">the culture in which the value should be converted</param>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if ((value == null))
            {
                return default(VisibilityKind);
            }
            string valueString = value.ToString();
            if ((valueString == "public"))
            {
                return VisibilityKind.Public;
            }
            if ((valueString == "private"))
            {
                return VisibilityKind.Private;
            }
            if ((valueString == "protected"))
            {
                return VisibilityKind.Protected;
            }
            if ((valueString == "package"))
            {
                return VisibilityKind.Package;
            }
            return default(VisibilityKind);
        }
        
        /// <summary>
        /// Convert the provided value into a VisibilityKind
        /// </summary>
        /// <returns>the converted value</returns>
        /// <param name="destinationType">the destination type</param>
        /// <param name="value">the value to convert</param>
        /// <param name="context">the context in which the value should be transformed</param>
        /// <param name="culture">the culture in which the value should be converted</param>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if ((value == null))
            {
                return null;
            }
            VisibilityKind valueCasted = ((VisibilityKind)(value));
            if ((valueCasted == VisibilityKind.Public))
            {
                return "public";
            }
            if ((valueCasted == VisibilityKind.Private))
            {
                return "private";
            }
            if ((valueCasted == VisibilityKind.Protected))
            {
                return "protected";
            }
            if ((valueCasted == VisibilityKind.Package))
            {
                return "package";
            }
            throw new ArgumentOutOfRangeException("value");
        }
    }
}
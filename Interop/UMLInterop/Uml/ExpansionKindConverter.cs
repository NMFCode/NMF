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
using NMF.Interop.Ecore;
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


namespace NMF.Interop.Uml
{
    
    
    /// <summary>
    /// Implements a type converter for the enumeration ExpansionKind
    /// </summary>
    public class ExpansionKindConverter : TypeConverter
    {
        
        /// <summary>
        /// Determines whether the converter can convert from the provided source type into ExpansionKind
        /// </summary>
        /// <returns>true, if the converter can convert from the source type, otherwise false</returns>
        /// <param name="sourceType">the source type</param>
        /// <param name="context">the context in which the value should be transformed</param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return (sourceType == typeof(string));
        }
        
        /// <summary>
        /// Determines whether the converter can convert to the destination type from ExpansionKind
        /// </summary>
        /// <returns>true, if the converter can convert from the source type, otherwise false</returns>
        /// <param name="destinationType">the destination type</param>
        /// <param name="context">the context in which the value should be transformed</param>
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return (destinationType == typeof(string));
        }
        
        /// <summary>
        /// Convert the provided value into a ExpansionKind
        /// </summary>
        /// <returns>the converted value as a ExpansionKind</returns>
        /// <param name="value">the value to convert</param>
        /// <param name="context">the context in which the value should be transformed</param>
        /// <param name="culture">the culture in which the value should be converted</param>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if ((value == null))
            {
                return default(ExpansionKind);
            }
            string valueString = value.ToString();
            if ((valueString == "parallel"))
            {
                return ExpansionKind.Parallel;
            }
            if ((valueString == "iterative"))
            {
                return ExpansionKind.Iterative;
            }
            if ((valueString == "stream"))
            {
                return ExpansionKind.Stream;
            }
            return default(ExpansionKind);
        }
        
        /// <summary>
        /// Convert the provided value into a ExpansionKind
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
            ExpansionKind valueCasted = ((ExpansionKind)(value));
            if ((valueCasted == ExpansionKind.Parallel))
            {
                return "parallel";
            }
            if ((valueCasted == ExpansionKind.Iterative))
            {
                return "iterative";
            }
            if ((valueCasted == ExpansionKind.Stream))
            {
                return "stream";
            }
            throw new ArgumentOutOfRangeException("value");
        }
    }
}

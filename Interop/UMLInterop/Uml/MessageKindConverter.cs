//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.25
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
    
    
    public class MessageKindConverter : TypeConverter
    {
        
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return (sourceType == typeof(string));
        }
        
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return (destinationType == typeof(string));
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if ((value == null))
            {
                return default(MessageKind);
            }
            string valueString = value.ToString();
            if ((valueString == "complete"))
            {
                return MessageKind.Complete;
            }
            if ((valueString == "lost"))
            {
                return MessageKind.Lost;
            }
            if ((valueString == "found"))
            {
                return MessageKind.Found;
            }
            if ((valueString == "unknown"))
            {
                return MessageKind.Unknown;
            }
            return default(MessageKind);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if ((value == null))
            {
                return null;
            }
            MessageKind valueCasted = ((MessageKind)(value));
            if ((valueCasted == MessageKind.Complete))
            {
                return "complete";
            }
            if ((valueCasted == MessageKind.Lost))
            {
                return "lost";
            }
            if ((valueCasted == MessageKind.Found))
            {
                return "found";
            }
            if ((valueCasted == MessageKind.Unknown))
            {
                return "unknown";
            }
            throw new ArgumentOutOfRangeException("value");
        }
    }
}

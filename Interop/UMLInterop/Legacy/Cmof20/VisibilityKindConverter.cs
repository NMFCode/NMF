//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMF.Interop.Legacy.Cmof
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    using NMF.Models.Repository;
    using System.Globalization;
    
    
    public class VisibilityKindConverter : TypeConverter
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
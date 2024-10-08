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
    /// The public interface for LiteralBoolean
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(LiteralBoolean))]
    [XmlDefaultImplementationTypeAttribute(typeof(LiteralBoolean))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//LiteralBoolean")]
    public interface ILiteralBoolean : IModelElement, ILiteralSpecification
    {
        
        /// <summary>
        /// The specified Boolean value.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("value")]
        [DescriptionAttribute("The specified Boolean value.\n<p>From package UML::Values.</p>")]
        [CategoryAttribute("LiteralBoolean")]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(true)]
        bool Value
        {
            get;
            set;
        }
    }
}

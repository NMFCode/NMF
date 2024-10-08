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
    /// The public interface for StructuralFeature
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(StructuralFeature))]
    [XmlDefaultImplementationTypeAttribute(typeof(StructuralFeature))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//StructuralFeature")]
    public interface IStructuralFeature : IModelElement, IMultiplicityElement, NMF.Interop.Uml.ITypedElement, IFeature
    {
        
        /// <summary>
        /// If isReadOnly is true, the StructuralFeature may not be written to after initialization.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isReadOnly")]
        [DescriptionAttribute("If isReadOnly is true, the StructuralFeature may not be written to after initiali" +
            "zation.\n<p>From package UML::Classification.</p>")]
        [CategoryAttribute("StructuralFeature")]
        [XmlElementNameAttribute("isReadOnly")]
        [XmlAttributeAttribute(true)]
        bool IsReadOnly
        {
            get;
            set;
        }
    }
}

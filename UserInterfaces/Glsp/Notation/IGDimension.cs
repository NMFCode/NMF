//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.21
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
using System.Linq;

namespace NMF.Glsp.Notation
{
    
    
    /// <summary>
    /// The public interface for GDimension
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(GDimension))]
    [XmlDefaultImplementationTypeAttribute(typeof(GDimension))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/glsp/notation#//GDimension")]
    public interface IGDimension : IModelElement
    {
        
        /// <summary>
        /// The width property
        /// </summary>
        [DefaultValueAttribute(0D)]
        [DisplayNameAttribute("width")]
        [CategoryAttribute("GDimension")]
        [XmlElementNameAttribute("width")]
        [XmlAttributeAttribute(true)]
        double Width
        {
            get;
            set;
        }
        
        /// <summary>
        /// The height property
        /// </summary>
        [DefaultValueAttribute(0D)]
        [DisplayNameAttribute("height")]
        [CategoryAttribute("GDimension")]
        [XmlElementNameAttribute("height")]
        [XmlAttributeAttribute(true)]
        double Height
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets fired before the Width property changes its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> WidthChanging;
        
        /// <summary>
        /// Gets fired when the Width property changed its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> WidthChanged;
        
        /// <summary>
        /// Gets fired before the Height property changes its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> HeightChanging;
        
        /// <summary>
        /// Gets fired when the Height property changed its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> HeightChanged;
    }
}

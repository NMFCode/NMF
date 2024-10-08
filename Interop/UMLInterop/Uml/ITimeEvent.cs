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
    /// The public interface for TimeEvent
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(TimeEvent))]
    [XmlDefaultImplementationTypeAttribute(typeof(TimeEvent))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//TimeEvent")]
    public interface ITimeEvent : IModelElement, NMF.Interop.Uml.IEvent
    {
        
        /// <summary>
        /// Specifies whether the TimeEvent is specified as an absolute or relative time.
        ///&lt;p&gt;From package UML::CommonBehavior.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isRelative")]
        [DescriptionAttribute("Specifies whether the TimeEvent is specified as an absolute or relative time.\n<p>" +
            "From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("TimeEvent")]
        [XmlElementNameAttribute("isRelative")]
        [XmlAttributeAttribute(true)]
        bool IsRelative
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies the time of the TimeEvent.
        ///&lt;p&gt;From package UML::CommonBehavior.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("when")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        ITimeExpression When
        {
            get;
            set;
        }
        
        /// <summary>
        /// The ValueSpecification when must return a non-negative Integer.
        ///when.integerValue() &gt;= 0
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool When_non_negative(object diagnostics, object context);
    }
}

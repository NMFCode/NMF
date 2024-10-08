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
    /// The public interface for LinkEndDestructionData
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(LinkEndDestructionData))]
    [XmlDefaultImplementationTypeAttribute(typeof(LinkEndDestructionData))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//LinkEndDestructionData")]
    public interface ILinkEndDestructionData : IModelElement, ILinkEndData
    {
        
        /// <summary>
        /// Specifies whether to destroy duplicates of the value in nonunique Association ends.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isDestroyDuplicates")]
        [DescriptionAttribute("Specifies whether to destroy duplicates of the value in nonunique Association end" +
            "s.\n<p>From package UML::Actions.</p>")]
        [CategoryAttribute("LinkEndDestructionData")]
        [XmlElementNameAttribute("isDestroyDuplicates")]
        [XmlAttributeAttribute(true)]
        bool IsDestroyDuplicates
        {
            get;
            set;
        }
        
        /// <summary>
        /// The InputPin that provides the position of an existing link to be destroyed in an ordered, nonunique Association end. The type of the destroyAt InputPin is UnlimitedNatural, but the value cannot be zero or unlimited.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("destroyAt")]
        [DescriptionAttribute("The InputPin that provides the position of an existing link to be destroyed in an" +
            " ordered, nonunique Association end. The type of the destroyAt InputPin is Unlim" +
            "itedNatural, but the value cannot be zero or unlimited.\n<p>From package UML::Act" +
            "ions.</p>")]
        [CategoryAttribute("LinkEndDestructionData")]
        [XmlElementNameAttribute("destroyAt")]
        [XmlAttributeAttribute(true)]
        IInputPin DestroyAt
        {
            get;
            set;
        }
        
        /// <summary>
        /// LinkEndDestructionData for ordered, nonunique Association ends must have a single destroyAt InputPin if isDestroyDuplicates is false, which must be of type UnlimitedNatural and have a multiplicity of 1..1. Otherwise, the action has no destroyAt input pin.
        ///if  not end.isOrdered or end.isUnique or isDestroyDuplicates
        ///then destroyAt = null
        ///else
        ///	destroyAt &lt;&gt; null and 
        ///	destroyAt-&gt;forAll(type=UnlimitedNatural and is(1,1))
        ///endif
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool DestroyAt_pin(object diagnostics, object context);
    }
}

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
    /// The public interface for LinkEndCreationData
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(LinkEndCreationData))]
    [XmlDefaultImplementationTypeAttribute(typeof(LinkEndCreationData))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//LinkEndCreationData")]
    public interface ILinkEndCreationData : IModelElement, ILinkEndData
    {
        
        /// <summary>
        /// Specifies whether the existing links emanating from the object on this end should be destroyed before creating a new link.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isReplaceAll")]
        [DescriptionAttribute("Specifies whether the existing links emanating from the object on this end should" +
            " be destroyed before creating a new link.\n<p>From package UML::Actions.</p>")]
        [CategoryAttribute("LinkEndCreationData")]
        [XmlElementNameAttribute("isReplaceAll")]
        [XmlAttributeAttribute(true)]
        bool IsReplaceAll
        {
            get;
            set;
        }
        
        /// <summary>
        /// For ordered Association ends, the InputPin that provides the position where the new link should be inserted or where an existing link should be moved to. The type of the insertAt InputPin is UnlimitedNatural, but the input cannot be zero. It is omitted for Association ends that are not ordered.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("insertAt")]
        [DescriptionAttribute(@"For ordered Association ends, the InputPin that provides the position where the new link should be inserted or where an existing link should be moved to. The type of the insertAt InputPin is UnlimitedNatural, but the input cannot be zero. It is omitted for Association ends that are not ordered.
<p>From package UML::Actions.</p>")]
        [CategoryAttribute("LinkEndCreationData")]
        [XmlElementNameAttribute("insertAt")]
        [XmlAttributeAttribute(true)]
        IInputPin InsertAt
        {
            get;
            set;
        }
        
        /// <summary>
        /// LinkEndCreationData for ordered Association ends must have a single insertAt InputPin for the insertion point with type UnlimitedNatural and multiplicity of 1..1, if isReplaceAll=false, and must have no InputPin for the insertion point when the association ends are unordered.
        ///if  not end.isOrdered
        ///then insertAt = null
        ///else
        ///	not isReplaceAll=false implies
        ///	insertAt &lt;&gt; null and insertAt-&gt;forAll(type=UnlimitedNatural and is(1,1))
        ///endif
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool InsertAt_pin(object diagnostics, object context);
    }
}

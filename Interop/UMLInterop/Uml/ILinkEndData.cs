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
    /// The public interface for LinkEndData
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(LinkEndData))]
    [XmlDefaultImplementationTypeAttribute(typeof(LinkEndData))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//LinkEndData")]
    public interface ILinkEndData : IModelElement, IElement
    {
        
        /// <summary>
        /// The Association end for which this LinkEndData specifies values.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("end")]
        [DescriptionAttribute("The Association end for which this LinkEndData specifies values.\n<p>From package " +
            "UML::Actions.</p>")]
        [CategoryAttribute("LinkEndData")]
        [XmlElementNameAttribute("end")]
        [XmlAttributeAttribute(true)]
        IProperty End
        {
            get;
            set;
        }
        
        /// <summary>
        /// A set of QualifierValues used to provide values for the qualifiers of the end.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("qualifier")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IQualifierValue> Qualifier
        {
            get;
        }
        
        /// <summary>
        /// The InputPin that provides the specified value for the given end. This InputPin is omitted if the LinkEndData specifies the &quot;open&quot; end for a ReadLinkAction.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("value")]
        [DescriptionAttribute("The InputPin that provides the specified value for the given end. This InputPin i" +
            "s omitted if the LinkEndData specifies the \"open\" end for a ReadLinkAction.\n<p>F" +
            "rom package UML::Actions.</p>")]
        [CategoryAttribute("LinkEndData")]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(true)]
        IInputPin Value
        {
            get;
            set;
        }
        
        /// <summary>
        /// The type of the value InputPin conforms to the type of the Association end.
        ///value&lt;&gt;null implies value.type.conformsTo(end.type)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Same_type(object diagnostics, object context);
        
        /// <summary>
        /// The multiplicity of the value InputPin must be 1..1.
        ///value&lt;&gt;null implies value.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity(object diagnostics, object context);
        
        /// <summary>
        /// The value InputPin is not also the qualifier value InputPin.
        ///value-&gt;excludesAll(qualifier.value)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool End_object_input_pin(object diagnostics, object context);
        
        /// <summary>
        /// The Property must be an Association memberEnd.
        ///end.association &lt;&gt; null
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Property_is_association_end(object diagnostics, object context);
        
        /// <summary>
        /// The qualifiers must be qualifiers of the Association end.
        ///end.qualifier-&gt;includesAll(qualifier.qualifier)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Qualifiers(object diagnostics, object context);
        
        /// <summary>
        /// Returns all the InputPins referenced by this LinkEndData. By default this includes the value and qualifier InputPins, but subclasses may override the operation to add other InputPins.
        ///result = (value-&gt;asBag()-&gt;union(qualifier.value))
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        ICollectionExpression<IInputPin> AllPins();
    }
}

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
    /// The public interface for Slot
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Slot))]
    [XmlDefaultImplementationTypeAttribute(typeof(Slot))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Slot")]
    public interface ISlot : IModelElement, IElement
    {
        
        /// <summary>
        /// The StructuralFeature that specifies the values that may be held by the Slot.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("definingFeature")]
        [DescriptionAttribute("The StructuralFeature that specifies the values that may be held by the Slot.\n<p>" +
            "From package UML::Classification.</p>")]
        [CategoryAttribute("Slot")]
        [XmlElementNameAttribute("definingFeature")]
        [XmlAttributeAttribute(true)]
        IStructuralFeature DefiningFeature
        {
            get;
            set;
        }
        
        /// <summary>
        /// The value or values held by the Slot.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IValueSpecification> Value
        {
            get;
        }
        
        /// <summary>
        /// The InstanceSpecification that owns this Slot.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("owningInstance")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("slot")]
        IInstanceSpecification OwningInstance
        {
            get;
            set;
        }
    }
}

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
    /// The public interface for InteractionFragment
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(InteractionFragment))]
    [XmlDefaultImplementationTypeAttribute(typeof(InteractionFragment))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//InteractionFragment")]
    public interface IInteractionFragment : IModelElement, INamedElement
    {
        
        /// <summary>
        /// References the Lifelines that the InteractionFragment involves.
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("covered")]
        [DescriptionAttribute("References the Lifelines that the InteractionFragment involves.\n<p>From package U" +
            "ML::Interactions.</p>")]
        [CategoryAttribute("InteractionFragment")]
        [XmlElementNameAttribute("covered")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("coveredBy")]
        [ConstantAttribute()]
        ISetExpression<ILifeline> Covered
        {
            get;
        }
        
        /// <summary>
        /// The operand enclosing this InteractionFragment (they may nest recursively).
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("enclosingOperand")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("fragment")]
        IInteractionOperand EnclosingOperand
        {
            get;
            set;
        }
        
        /// <summary>
        /// The Interaction enclosing this InteractionFragment.
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("enclosingInteraction")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("fragment")]
        IInteraction EnclosingInteraction
        {
            get;
            set;
        }
        
        /// <summary>
        /// The general ordering relationships contained in this fragment.
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("generalOrdering")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IGeneralOrdering> GeneralOrdering
        {
            get;
        }
    }
}

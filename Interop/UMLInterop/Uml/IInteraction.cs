//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.25
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
    /// The public interface for Interaction
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Interaction))]
    [XmlDefaultImplementationTypeAttribute(typeof(Interaction))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Interaction")]
    public interface IInteraction : IModelElement, IInteractionFragment, IBehavior
    {
        
        /// <summary>
        /// Specifies the participants in this Interaction.
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("lifeline")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("interaction")]
        [ConstantAttribute()]
        IOrderedSetExpression<ILifeline> Lifeline
        {
            get;
        }
        
        /// <summary>
        /// The ordered set of fragments in the Interaction.
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("fragment")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("enclosingInteraction")]
        [ConstantAttribute()]
        IOrderedSetExpression<IInteractionFragment> Fragment
        {
            get;
        }
        
        /// <summary>
        /// Actions owned by the Interaction.
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("action")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IAction> Action
        {
            get;
        }
        
        /// <summary>
        /// Specifies the gates that form the message interface between this Interaction and any InteractionUses which reference it.
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("formalGate")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IGate> FormalGate
        {
            get;
        }
        
        /// <summary>
        /// The Messages contained in this Interaction.
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("message")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("interaction")]
        [ConstantAttribute()]
        IOrderedSetExpression<IMessage> Message
        {
            get;
        }
        
        /// <summary>
        /// An Interaction instance must not be contained within another Interaction instance.
        ///enclosingInteraction->isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Not_contained(object diagnostics, object context);
    }
}

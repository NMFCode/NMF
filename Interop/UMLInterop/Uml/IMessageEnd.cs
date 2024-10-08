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
    /// The public interface for MessageEnd
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(MessageEnd))]
    [XmlDefaultImplementationTypeAttribute(typeof(MessageEnd))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//MessageEnd")]
    public interface IMessageEnd : IModelElement, INamedElement
    {
        
        /// <summary>
        /// References a Message.
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("message")]
        [DescriptionAttribute("References a Message.\n<p>From package UML::Interactions.</p>")]
        [CategoryAttribute("MessageEnd")]
        [XmlElementNameAttribute("message")]
        [XmlAttributeAttribute(true)]
        IMessage Message
        {
            get;
            set;
        }
        
        /// <summary>
        /// This query returns a set including the MessageEnd (if exists) at the opposite end of the Message for this MessageEnd.
        ///message-&gt;notEmpty()
        ///result = (message-&gt;asSet().messageEnd-&gt;asSet()-&gt;excluding(self))
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        ISetExpression<IMessageEnd> OppositeEnd();
        
        /// <summary>
        /// This query returns value true if this MessageEnd is a sendEvent.
        ///message-&gt;notEmpty()
        ///result = (message.sendEvent-&gt;asSet()-&gt;includes(self))
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        bool IsSend();
        
        /// <summary>
        /// This query returns value true if this MessageEnd is a receiveEvent.
        ///message-&gt;notEmpty()
        ///result = (message.receiveEvent-&gt;asSet()-&gt;includes(self))
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        bool IsReceive();
        
        /// <summary>
        /// This query returns a set including the enclosing InteractionFragment this MessageEnd is enclosed within.
        ///result = (if self-&gt;select(oclIsKindOf(Gate))-&gt;notEmpty() 
        ///then -- it is a Gate
        ///let endGate : Gate = 
        ///  self-&gt;select(oclIsKindOf(Gate)).oclAsType(Gate)-&gt;asOrderedSet()-&gt;first()
        ///  in
        ///  if endGate.isOutsideCF() 
        ///  then endGate.combinedFragment.enclosingInteraction.oclAsType(InteractionFragment)-&gt;asSet()-&gt;
        ///     union(endGate.combinedFragment.enclosingOperand.oclAsType(InteractionFragment)-&gt;asSet())
        ///  else if endGate.isInsideCF() 
        ///    then endGate.combinedFragment.oclAsType(InteractionFragment)-&gt;asSet()
        ///    else if endGate.isFormal() 
        ///      then endGate.interaction.oclAsType(InteractionFragment)-&gt;asSet()
        ///      else if endGate.isActual() 
        ///        then endGate.interactionUse.enclosingInteraction.oclAsType(InteractionFragment)-&gt;asSet()-&gt;
        ///     union(endGate.interactionUse.enclosingOperand.oclAsType(InteractionFragment)-&gt;asSet())
        ///        else null
        ///        endif
        ///      endif
        ///    endif
        ///  endif
        ///else -- it is a MessageOccurrenceSpecification
        ///let endMOS : MessageOccurrenceSpecification  = 
        ///  self-&gt;select(oclIsKindOf(MessageOccurrenceSpecification)).oclAsType(MessageOccurrenceSpecification)-&gt;asOrderedSet()-&gt;first() 
        ///  in
        ///  if endMOS.enclosingInteraction-&gt;notEmpty() 
        ///  then endMOS.enclosingInteraction.oclAsType(InteractionFragment)-&gt;asSet()
        ///  else endMOS.enclosingOperand.oclAsType(InteractionFragment)-&gt;asSet()
        ///  endif
        ///endif)
        ///&lt;p&gt;From package UML::Interactions.&lt;/p&gt;
        /// </summary>
        ISetExpression<IInteractionFragment> EnclosingFragment();
    }
}

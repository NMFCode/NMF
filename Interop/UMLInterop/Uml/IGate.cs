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
    /// The public interface for Gate
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Gate))]
    [XmlDefaultImplementationTypeAttribute(typeof(Gate))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Gate")]
    public interface IGate : IModelElement, IMessageEnd
    {
        
        /// <summary>
        /// If this Gate is an actualGate, it must have exactly one matching formalGate within the referred Interaction.
        ///interactionUse->notEmpty() implies interactionUse.refersTo.formalGate->select(matches(self))->size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Actual_gate_matched(object diagnostics, object context);
        
        /// <summary>
        /// If this Gate is inside a CombinedFragment, it must have exactly one matching Gate which is outside of that CombinedFragment.
        ///isInsideCF() implies combinedFragment.cfragmentGate->select(isOutsideCF() and matches(self))->size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Inside_cf_matched(object diagnostics, object context);
        
        /// <summary>
        /// If this Gate is outside an 'alt' CombinedFragment,  for every InteractionOperator inside that CombinedFragment there must be exactly one matching Gate inside the CombindedFragment with its opposing end enclosed by that InteractionOperator. If this Gate is outside CombinedFragment with operator other than 'alt',   there must be exactly one matching Gate inside that CombinedFragment.
        ///isOutsideCF() implies
        /// if self.combinedFragment.interactionOperator->asOrderedSet()->first() = InteractionOperatorKind::alt
        /// then self.combinedFragment.operand->forAll(op : InteractionOperand |
        /// self.combinedFragment.cfragmentGate->select(isInsideCF() and 
        /// oppositeEnd().enclosingFragment()->includes(self.combinedFragment) and matches(self))->size()=1)
        /// else  self.combinedFragment.cfragmentGate->select(isInsideCF() and matches(self))->size()=1
        /// endif
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Outside_cf_matched(object diagnostics, object context);
        
        /// <summary>
        /// isFormal() implies that no other formalGate of the parent Interaction returns the same getName() as returned for self
        ///isFormal() implies interaction.formalGate->select(getName() = self.getName())->size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Formal_gate_distinguishable(object diagnostics, object context);
        
        /// <summary>
        /// isActual() implies that no other actualGate of the parent InteractionUse returns the same getName() as returned for self
        ///isActual() implies interactionUse.actualGate->select(getName() = self.getName())->size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Actual_gate_distinguishable(object diagnostics, object context);
        
        /// <summary>
        /// isOutsideCF() implies that no other outside cfragmentGate of the parent CombinedFragment returns the same getName() as returned for self
        ///isOutsideCF() implies combinedFragment.cfragmentGate->select(getName() = self.getName())->size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Outside_cf_gate_distinguishable(object diagnostics, object context);
        
        /// <summary>
        /// isInsideCF() implies that no other inside cfragmentGate attached to a message with its other end in the same InteractionOperator as self, returns the same getName() as returned for self
        ///isInsideCF() implies
        ///let selfOperand : InteractionOperand = self.getOperand() in
        ///  combinedFragment.cfragmentGate->select(isInsideCF() and getName() = self.getName())->select(getOperand() = selfOperand)->size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Inside_cf_gate_distinguishable(object diagnostics, object context);
        
        /// <summary>
        /// This query returns true if this Gate is attached to the boundary of a CombinedFragment, and its other end (if present)  is outside of the same CombinedFragment.
        ///result = (self.oppositeEnd()-> notEmpty() and combinedFragment->notEmpty() implies
        ///let oppEnd : MessageEnd = self.oppositeEnd()->asOrderedSet()->first() in
        ///if oppEnd.oclIsKindOf(MessageOccurrenceSpecification) 
        ///then let oppMOS : MessageOccurrenceSpecification = oppEnd.oclAsType(MessageOccurrenceSpecification)
        ///in  self.combinedFragment.enclosingInteraction.oclAsType(InteractionFragment)->asSet()->
        ///     union(self.combinedFragment.enclosingOperand.oclAsType(InteractionFragment)->asSet()) =
        ///     oppMOS.enclosingInteraction.oclAsType(InteractionFragment)->asSet()->
        ///     union(oppMOS.enclosingOperand.oclAsType(InteractionFragment)->asSet())
        ///else let oppGate : Gate = oppEnd.oclAsType(Gate) 
        ///in self.combinedFragment.enclosingInteraction.oclAsType(InteractionFragment)->asSet()->
        ///     union(self.combinedFragment.enclosingOperand.oclAsType(InteractionFragment)->asSet()) =
        ///     oppGate.combinedFragment.enclosingInteraction.oclAsType(InteractionFragment)->asSet()->
        ///     union(oppGate.combinedFragment.enclosingOperand.oclAsType(InteractionFragment)->asSet())
        ///endif)
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        bool IsOutsideCF();
        
        /// <summary>
        /// This query returns true if this Gate is attached to the boundary of a CombinedFragment, and its other end (if present) is inside of an InteractionOperator of the same CombinedFragment.
        ///result = (self.oppositeEnd()-> notEmpty() and combinedFragment->notEmpty() implies
        ///let oppEnd : MessageEnd = self.oppositeEnd()->asOrderedSet()->first() in
        ///if oppEnd.oclIsKindOf(MessageOccurrenceSpecification)
        ///then let oppMOS : MessageOccurrenceSpecification
        ///= oppEnd.oclAsType(MessageOccurrenceSpecification)
        ///in combinedFragment = oppMOS.enclosingOperand.combinedFragment
        ///else let oppGate : Gate = oppEnd.oclAsType(Gate)
        ///in combinedFragment = oppGate.combinedFragment.enclosingOperand.combinedFragment
        ///endif)
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        bool IsInsideCF();
        
        /// <summary>
        /// This query returns true value if this Gate is an actualGate of an InteractionUse.
        ///result = (interactionUse->notEmpty())
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        bool IsActual();
        
        /// <summary>
        /// This query returns true if this Gate is a formalGate of an Interaction.
        ///result = (interaction->notEmpty())
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        bool IsFormal();
        
        /// <summary>
        /// This query returns the name of the gate, either the explicit name (.name) or the constructed name ('out_" or 'in_' concatenated in front of .message.name) if the explicit name is not present.
        ///result = (if name->notEmpty() then name->asOrderedSet()->first()
        ///else  if isActual() or isOutsideCF() 
        ///  then if isSend() 
        ///    then 'out_'.concat(self.message.name->asOrderedSet()->first())
        ///    else 'in_'.concat(self.message.name->asOrderedSet()->first())
        ///    endif
        ///  else if isSend()
        ///    then 'in_'.concat(self.message.name->asOrderedSet()->first())
        ///    else 'out_'.concat(self.message.name->asOrderedSet()->first())
        ///    endif
        ///  endif
        ///endif)
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        string GetName();
        
        /// <summary>
        /// This query returns true if the name of this Gate matches the name of the in parameter Gate, and the messages for the two Gates correspond. The Message for one Gate (say A) corresponds to the Message for another Gate (say B) if (A and B have the same name value) and (if A is a sendEvent then B is a receiveEvent) and (if A is a receiveEvent then B is a sendEvent) and (A and B have the same messageSort value) and (A and B have the same signature value).
        ///result = (self.getName() = gateToMatch.getName() and 
        ///self.message.messageSort = gateToMatch.message.messageSort and
        ///self.message.name = gateToMatch.message.name and
        ///self.message.sendEvent->includes(self) implies gateToMatch.message.receiveEvent->includes(gateToMatch)  and
        ///self.message.receiveEvent->includes(self) implies gateToMatch.message.sendEvent->includes(gateToMatch) and
        ///self.message.signature = gateToMatch.message.signature)
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        /// <param name="gateToMatch"></param>
        bool Matches(IGate gateToMatch);
        
        /// <summary>
        /// If the Gate is an inside Combined Fragment Gate, this operation returns the InteractionOperand that the opposite end of this Gate is included within.
        ///result = (if isInsideCF() then
        ///  let oppEnd : MessageEnd = self.oppositeEnd()->asOrderedSet()->first() in
        ///    if oppEnd.oclIsKindOf(MessageOccurrenceSpecification)
        ///    then let oppMOS : MessageOccurrenceSpecification = oppEnd.oclAsType(MessageOccurrenceSpecification)
        ///        in oppMOS.enclosingOperand->asOrderedSet()->first()
        ///    else let oppGate : Gate = oppEnd.oclAsType(Gate)
        ///        in oppGate.combinedFragment.enclosingOperand->asOrderedSet()->first()
        ///    endif
        ///  else null
        ///endif)
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        IInteractionOperand GetOperand();
    }
}

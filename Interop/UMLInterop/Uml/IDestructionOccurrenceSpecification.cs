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
    /// The public interface for DestructionOccurrenceSpecification
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(DestructionOccurrenceSpecification))]
    [XmlDefaultImplementationTypeAttribute(typeof(DestructionOccurrenceSpecification))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//DestructionOccurrenceSpecification")]
    public interface IDestructionOccurrenceSpecification : IModelElement, IMessageOccurrenceSpecification
    {
        
        /// <summary>
        /// No other OccurrenceSpecifications on a given Lifeline in an InteractionOperand may appear below a DestructionOccurrenceSpecification.
        ///let o : InteractionOperand = enclosingOperand in o-&gt;notEmpty() and 
        ///let peerEvents : OrderedSet(OccurrenceSpecification) = covered.events-&gt;select(enclosingOperand = o)
        ///in peerEvents-&gt;last() = self
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_occurrence_specifications_below(object diagnostics, object context);
    }
}

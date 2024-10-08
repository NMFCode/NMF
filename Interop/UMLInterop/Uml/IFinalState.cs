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
    /// The public interface for FinalState
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(FinalState))]
    [XmlDefaultImplementationTypeAttribute(typeof(FinalState))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//FinalState")]
    public interface IFinalState : IModelElement, IState
    {
        
        /// <summary>
        /// A FinalState has no exit Behavior.
        ///exit-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_exit_behavior(object diagnostics, object context);
        
        /// <summary>
        /// A FinalState cannot have any outgoing Transitions.
        ///outgoing-&gt;size() = 0
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_outgoing_transitions(object diagnostics, object context);
        
        /// <summary>
        /// A FinalState cannot have Regions.
        ///region-&gt;size() = 0
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_regions(object diagnostics, object context);
        
        /// <summary>
        /// A FinalState cannot reference a submachine.
        ///submachine-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Cannot_reference_submachine(object diagnostics, object context);
        
        /// <summary>
        /// A FinalState has no entry Behavior.
        ///entry-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_entry_behavior(object diagnostics, object context);
        
        /// <summary>
        /// A FinalState has no state (doActivity) Behavior.
        ///doActivity-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_state_behavior(object diagnostics, object context);
    }
}

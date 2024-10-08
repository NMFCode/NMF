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
    /// TransitionKind is an Enumeration type used to differentiate the various kinds of Transitions.
    ///&lt;p&gt;From package UML::StateMachines.&lt;/p&gt;
    /// </summary>
    [TypeConverterAttribute(typeof(TransitionKindConverter))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//TransitionKind")]
    public enum TransitionKind
    {
        
        /// <summary>
        ///Implies that the Transition, if triggered, occurs without exiting or entering the source State (i.e., it does not cause a state change). This means that the entry or exit condition of the source State will not be invoked. An internal Transition can be taken even if the SateMachine is in one or more Regions nested within the associated State.
        ///</summary>
        Internal = 0,
        
        /// <summary>
        ///Implies that the Transition, if triggered, will not exit the composite (source) State, but it will exit and re-enter any state within the composite State that is in the current state configuration.
        ///</summary>
        Local = 1,
        
        /// <summary>
        ///Implies that the Transition, if triggered, will exit the composite (source) State.
        ///</summary>
        External = 2,
    }
}

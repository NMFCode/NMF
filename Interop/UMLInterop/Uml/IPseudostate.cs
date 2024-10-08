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
    /// The public interface for Pseudostate
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Pseudostate))]
    [XmlDefaultImplementationTypeAttribute(typeof(Pseudostate))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Pseudostate")]
    public interface IPseudostate : IModelElement, IVertex
    {
        
        /// <summary>
        /// Determines the precise type of the Pseudostate and can be one of: entryPoint, exitPoint, initial, deepHistory, shallowHistory, join, fork, junction, terminate or choice.
        ///&lt;p&gt;From package UML::StateMachines.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(PseudostateKind.Initial)]
        [DisplayNameAttribute("kind")]
        [DescriptionAttribute("Determines the precise type of the Pseudostate and can be one of: entryPoint, exi" +
            "tPoint, initial, deepHistory, shallowHistory, join, fork, junction, terminate or" +
            " choice.\n<p>From package UML::StateMachines.</p>")]
        [CategoryAttribute("Pseudostate")]
        [XmlElementNameAttribute("kind")]
        [XmlAttributeAttribute(true)]
        PseudostateKind Kind
        {
            get;
            set;
        }
        
        /// <summary>
        /// The State that owns this Pseudostate and in which it appears.
        ///&lt;p&gt;From package UML::StateMachines.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("state")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("connectionPoint")]
        IState State
        {
            get;
            set;
        }
        
        /// <summary>
        /// The StateMachine in which this Pseudostate is defined. This only applies to Pseudostates of the kind entryPoint or exitPoint.
        ///&lt;p&gt;From package UML::StateMachines.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("stateMachine")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("connectionPoint")]
        IStateMachine StateMachine
        {
            get;
            set;
        }
        
        /// <summary>
        /// All transitions outgoing a fork vertex must target states in different regions of an orthogonal state.
        ///(kind = PseudostateKind::fork) implies
        ///
        ///-- for any pair of outgoing transitions there exists an orthogonal state which contains the targets of these transitions 
        ///-- such that these targets belong to different regions of that orthogonal state 
        ///
        ///outgoing-&gt;forAll(t1:Transition, t2:Transition | let contState:State = containingStateMachine().LCAState(t1.target, t2.target) in
        ///	((contState &lt;&gt; null) and (contState.region
        ///		-&gt;exists(r1:Region, r2: Region | (r1 &lt;&gt; r2) and t1.target.isContainedInRegion(r1) and t2.target.isContainedInRegion(r2)))))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Transitions_outgoing(object diagnostics, object context);
        
        /// <summary>
        /// In a complete statemachine, a choice Vertex must have at least one incoming and one outgoing Transition.
        ///(kind = PseudostateKind::choice) implies (incoming-&gt;size() &gt;= 1 and outgoing-&gt;size() &gt;= 1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Choice_vertex(object diagnostics, object context);
        
        /// <summary>
        /// The outgoing Transition from an initial vertex may have a behavior, but not a trigger or a guard.
        ///(kind = PseudostateKind::initial) implies (outgoing.guard = null and outgoing.trigger-&gt;isEmpty())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Outgoing_from_initial(object diagnostics, object context);
        
        /// <summary>
        /// In a complete StateMachine, a join Vertex must have at least two incoming Transitions and exactly one outgoing Transition.
        ///(kind = PseudostateKind::join) implies (outgoing-&gt;size() = 1 and incoming-&gt;size() &gt;= 2)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Join_vertex(object diagnostics, object context);
        
        /// <summary>
        /// In a complete StateMachine, a junction Vertex must have at least one incoming and one outgoing Transition.
        ///(kind = PseudostateKind::junction) implies (incoming-&gt;size() &gt;= 1 and outgoing-&gt;size() &gt;= 1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Junction_vertex(object diagnostics, object context);
        
        /// <summary>
        /// History Vertices can have at most one outgoing Transition.
        ///((kind = PseudostateKind::deepHistory) or (kind = PseudostateKind::shallowHistory)) implies (outgoing-&gt;size() &lt;= 1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool History_vertices(object diagnostics, object context);
        
        /// <summary>
        /// An initial Vertex can have at most one outgoing Transition.
        ///(kind = PseudostateKind::initial) implies (outgoing-&gt;size() &lt;= 1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Initial_vertex(object diagnostics, object context);
        
        /// <summary>
        /// In a complete StateMachine, a fork Vertex must have at least two outgoing Transitions and exactly one incoming Transition.
        ///(kind = PseudostateKind::fork) implies (incoming-&gt;size() = 1 and outgoing-&gt;size() &gt;= 2)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Fork_vertex(object diagnostics, object context);
        
        /// <summary>
        /// All Transitions incoming a join Vertex must originate in different Regions of an orthogonal State.
        ///(kind = PseudostateKind::join) implies
        ///
        ///-- for any pair of incoming transitions there exists an orthogonal state which contains the source vetices of these transitions 
        ///-- such that these source vertices belong to different regions of that orthogonal state 
        ///
        ///incoming-&gt;forAll(t1:Transition, t2:Transition | let contState:State = containingStateMachine().LCAState(t1.source, t2.source) in
        ///	((contState &lt;&gt; null) and (contState.region
        ///		-&gt;exists(r1:Region, r2: Region | (r1 &lt;&gt; r2) and t1.source.isContainedInRegion(r1) and t2.source.isContainedInRegion(r2)))))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Transitions_incoming(object diagnostics, object context);
    }
}

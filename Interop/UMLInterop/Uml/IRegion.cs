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
    /// The public interface for Region
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Region))]
    [XmlDefaultImplementationTypeAttribute(typeof(Region))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Region")]
    public interface IRegion : IModelElement, IRedefinableElement, NMF.Interop.Uml.INamespace
    {
        
        /// <summary>
        /// The region of which this region is an extension.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [DisplayNameAttribute("extendedRegion")]
        [DescriptionAttribute("The region of which this region is an extension.\n<p>From package UML::StateMachin" +
            "es.</p>")]
        [CategoryAttribute("Region")]
        [XmlElementNameAttribute("extendedRegion")]
        [XmlAttributeAttribute(true)]
        IRegion ExtendedRegion
        {
            get;
            set;
        }
        
        /// <summary>
        /// The State that owns the Region. If a Region is owned by a State, then it cannot also be owned by a StateMachine.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("state")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("region")]
        IState State
        {
            get;
            set;
        }
        
        /// <summary>
        /// The StateMachine that owns the Region. If a Region is owned by a StateMachine, then it cannot also be owned by a State.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("stateMachine")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("region")]
        IStateMachine StateMachine
        {
            get;
            set;
        }
        
        /// <summary>
        /// The set of Transitions owned by the Region.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("transition")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("container")]
        [ConstantAttribute()]
        IOrderedSetExpression<ITransition> Transition
        {
            get;
        }
        
        /// <summary>
        /// The set of Vertices that are owned by this Region.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("subvertex")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("container")]
        [ConstantAttribute()]
        IOrderedSetExpression<IVertex> Subvertex
        {
            get;
        }
        
        /// <summary>
        /// A Region can have at most one deep history Vertex.
        ///self.subvertex->select (oclIsKindOf(Pseudostate))->collect(oclAsType(Pseudostate))->
        ///   select(kind = PseudostateKind::deepHistory)->size() <= 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Deep_history_vertex(object diagnostics, object context);
        
        /// <summary>
        /// A Region can have at most one shallow history Vertex.
        ///subvertex->select(oclIsKindOf(Pseudostate))->collect(oclAsType(Pseudostate))->
        ///  select(kind = PseudostateKind::shallowHistory)->size() <= 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Shallow_history_vertex(object diagnostics, object context);
        
        /// <summary>
        /// If a Region is owned by a StateMachine, then it cannot also be owned by a State and vice versa.
        ///(stateMachine <> null implies state = null) and (state <> null implies stateMachine = null)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Owned(object diagnostics, object context);
        
        /// <summary>
        /// A Region can have at most one initial Vertex.
        ///self.subvertex->select (oclIsKindOf(Pseudostate))->collect(oclAsType(Pseudostate))->
        ///  select(kind = PseudostateKind::initial)->size() <= 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Initial_vertex(object diagnostics, object context);
        
        /// <summary>
        /// The operation belongsToPSM () checks if the Region belongs to a ProtocolStateMachine.
        ///result = (if  stateMachine <> null 
        ///then
        ///  stateMachine.oclIsKindOf(ProtocolStateMachine)
        ///else 
        ///  state <> null  implies  state.container.belongsToPSM()
        ///endif )
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        bool BelongsToPSM();
        
        /// <summary>
        /// The operation containingStateMachine() returns the StateMachine in which this Region is defined.
        ///result = (if stateMachine = null 
        ///then
        ///  state.containingStateMachine()
        ///else
        ///  stateMachine
        ///endif)
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        IStateMachine ContainingStateMachine();
        
        /// <summary>
        /// The redefinition context of a Region is the nearest containing StateMachine.
        ///result = (let sm : StateMachine = containingStateMachine() in
        ///if sm._'context' = null or sm.general->notEmpty() then
        ///  sm
        ///else
        ///  sm._'context'
        ///endif)
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        IClassifier RedefinitionContext();
    }
}

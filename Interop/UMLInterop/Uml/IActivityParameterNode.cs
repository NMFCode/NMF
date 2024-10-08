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
    /// The public interface for ActivityParameterNode
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ActivityParameterNode))]
    [XmlDefaultImplementationTypeAttribute(typeof(ActivityParameterNode))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityParameterNode")]
    public interface IActivityParameterNode : IModelElement, IObjectNode
    {
        
        /// <summary>
        /// The Parameter for which the ActivityParameterNode will be accepting or providing values.
        ///&lt;p&gt;From package UML::Activities.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("parameter")]
        [DescriptionAttribute("The Parameter for which the ActivityParameterNode will be accepting or providing " +
            "values.\n<p>From package UML::Activities.</p>")]
        [CategoryAttribute("ActivityParameterNode")]
        [XmlElementNameAttribute("parameter")]
        [XmlAttributeAttribute(true)]
        NMF.Interop.Uml.IParameter Parameter
        {
            get;
            set;
        }
        
        /// <summary>
        /// An ActivityParameterNode with no outgoing ActivityEdges and one or more incoming ActivityEdges must have a parameter with direction out, inout, or return.
        ///(incoming-&gt;notEmpty() and outgoing-&gt;isEmpty()) implies 
        ///	(parameter.direction = ParameterDirectionKind::out or 
        ///	 parameter.direction = ParameterDirectionKind::inout or 
        ///	 parameter.direction = ParameterDirectionKind::return)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_outgoing_edges(object diagnostics, object context);
        
        /// <summary>
        /// The parameter of an ActivityParameterNode must be from the containing Activity.
        ///activity.ownedParameter-&gt;includes(parameter)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Has_parameters(object diagnostics, object context);
        
        /// <summary>
        /// The type of an ActivityParameterNode is the same as the type of its parameter.
        ///type = parameter.type
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Same_type(object diagnostics, object context);
        
        /// <summary>
        /// An ActivityParameterNode with no incoming ActivityEdges and one or more outgoing ActivityEdges must have a parameter with direction in or inout.
        ///(outgoing-&gt;notEmpty() and incoming-&gt;isEmpty()) implies 
        ///	(parameter.direction = ParameterDirectionKind::_&apos;in&apos; or 
        ///	 parameter.direction = ParameterDirectionKind::inout)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_incoming_edges(object diagnostics, object context);
        
        /// <summary>
        /// An ActivityParameterNode may have all incoming ActivityEdges or all outgoing ActivityEdges, but it must not have both incoming and outgoing ActivityEdges.
        ///incoming-&gt;isEmpty() or outgoing-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_edges(object diagnostics, object context);
    }
}

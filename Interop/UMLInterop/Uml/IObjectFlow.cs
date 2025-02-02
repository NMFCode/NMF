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
    /// The public interface for ObjectFlow
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ObjectFlow))]
    [XmlDefaultImplementationTypeAttribute(typeof(ObjectFlow))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ObjectFlow")]
    public interface IObjectFlow : IModelElement, IActivityEdge
    {
        
        /// <summary>
        /// Indicates whether the objects in the ObjectFlow are passed by multicasting.
        ///&lt;p&gt;From package UML::Activities.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isMulticast")]
        [DescriptionAttribute("Indicates whether the objects in the ObjectFlow are passed by multicasting.\n<p>Fr" +
            "om package UML::Activities.</p>")]
        [CategoryAttribute("ObjectFlow")]
        [XmlElementNameAttribute("isMulticast")]
        [XmlAttributeAttribute(true)]
        bool IsMulticast
        {
            get;
            set;
        }
        
        /// <summary>
        /// Indicates whether the objects in the ObjectFlow are gathered from respondents to multicasting.
        ///&lt;p&gt;From package UML::Activities.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isMultireceive")]
        [DescriptionAttribute("Indicates whether the objects in the ObjectFlow are gathered from respondents to " +
            "multicasting.\n<p>From package UML::Activities.</p>")]
        [CategoryAttribute("ObjectFlow")]
        [XmlElementNameAttribute("isMultireceive")]
        [XmlAttributeAttribute(true)]
        bool IsMultireceive
        {
            get;
            set;
        }
        
        /// <summary>
        /// A Behavior used to select tokens from a source ObjectNode.
        ///&lt;p&gt;From package UML::Activities.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("selection")]
        [DescriptionAttribute("A Behavior used to select tokens from a source ObjectNode.\n<p>From package UML::A" +
            "ctivities.</p>")]
        [CategoryAttribute("ObjectFlow")]
        [XmlElementNameAttribute("selection")]
        [XmlAttributeAttribute(true)]
        IBehavior Selection
        {
            get;
            set;
        }
        
        /// <summary>
        /// A Behavior used to change or replace object tokens flowing along the ObjectFlow.
        ///&lt;p&gt;From package UML::Activities.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("transformation")]
        [DescriptionAttribute("A Behavior used to change or replace object tokens flowing along the ObjectFlow.\n" +
            "<p>From package UML::Activities.</p>")]
        [CategoryAttribute("ObjectFlow")]
        [XmlElementNameAttribute("transformation")]
        [XmlAttributeAttribute(true)]
        IBehavior Transformation
        {
            get;
            set;
        }
        
        /// <summary>
        /// A selection Behavior has one input Parameter and one output Parameter. The input Parameter must have the same as or a supertype of the type of the source ObjectNode, be non-unique and have multiplicity 0..*. The output Parameter must be the same or a subtype of the type of source ObjectNode. The Behavior cannot have side effects.
        ///selection&lt;&gt;null implies
        ///	selection.inputParameters()-&gt;size()=1 and
        ///	selection.inputParameters()-&gt;forAll(not isUnique and is(0,*)) and
        ///	selection.outputParameters()-&gt;size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Input_and_output_parameter(object diagnostics, object context);
        
        /// <summary>
        /// ObjectFlows may not have ExecutableNodes at either end.
        ///not (source.oclIsKindOf(ExecutableNode) or target.oclIsKindOf(ExecutableNode))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_executable_nodes(object diagnostics, object context);
        
        /// <summary>
        /// A transformation Behavior has one input Parameter and one output Parameter. The input Parameter must be the same as or a supertype of the type of object token coming from the source end. The output Parameter must be the same or a subtype of the type of object token expected downstream. The Behavior cannot have side effects.
        ///transformation&lt;&gt;null implies
        ///	transformation.inputParameters()-&gt;size()=1 and
        ///	transformation.outputParameters()-&gt;size()=1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Transformation_behavior(object diagnostics, object context);
        
        /// <summary>
        /// An ObjectFlow may have a selection Behavior only if it has an ObjectNode as its source.
        ///selection&lt;&gt;null implies source.oclIsKindOf(ObjectNode)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Selection_behavior(object diagnostics, object context);
        
        /// <summary>
        /// ObjectNodes connected by an ObjectFlow, with optionally intervening ControlNodes, must have compatible types. In particular, the downstream ObjectNode type must be the same or a supertype of the upstream ObjectNode type.
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Compatible_types(object diagnostics, object context);
        
        /// <summary>
        /// ObjectNodes connected by an ObjectFlow, with optionally intervening ControlNodes, must have the same upperBounds.
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Same_upper_bounds(object diagnostics, object context);
        
        /// <summary>
        /// isMulticast and isMultireceive cannot both be true.
        ///not (isMulticast and isMultireceive)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Is_multicast_or_is_multireceive(object diagnostics, object context);
    }
}

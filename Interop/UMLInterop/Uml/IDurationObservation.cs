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
    /// The public interface for DurationObservation
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(DurationObservation))]
    [XmlDefaultImplementationTypeAttribute(typeof(DurationObservation))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//DurationObservation")]
    public interface IDurationObservation : IModelElement, IObservation
    {
        
        /// <summary>
        /// The value of firstEvent[i] is related to event[i] (where i is 1 or 2). If firstEvent[i] is true, then the corresponding observation event is the first time instant the execution enters event[i]. If firstEvent[i] is false, then the corresponding observation event is the time instant the execution exits event[i].
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [UpperBoundAttribute(2)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("firstEvent")]
        [DescriptionAttribute(@"The value of firstEvent[i] is related to event[i] (where i is 1 or 2). If firstEvent[i] is true, then the corresponding observation event is the first time instant the execution enters event[i]. If firstEvent[i] is false, then the corresponding observation event is the time instant the execution exits event[i].
<p>From package UML::Values.</p>")]
        [CategoryAttribute("DurationObservation")]
        [XmlElementNameAttribute("firstEvent")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<bool> FirstEvent
        {
            get;
        }
        
        /// <summary>
        /// The DurationObservation is determined as the duration between the entering or exiting of a single event Element during execution, or the entering/exiting of one event Element and the entering/exiting of a second.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [LowerBoundAttribute(1)]
        [UpperBoundAttribute(2)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("event")]
        [DescriptionAttribute("The DurationObservation is determined as the duration between the entering or exi" +
            "ting of a single event Element during execution, or the entering/exiting of one " +
            "event Element and the entering/exiting of a second.\n<p>From package UML::Values." +
            "</p>")]
        [CategoryAttribute("DurationObservation")]
        [XmlElementNameAttribute("event")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        IOrderedSetExpression<INamedElement> Event
        {
            get;
        }
        
        /// <summary>
        /// The multiplicity of firstEvent must be 2 if the multiplicity of event is 2. Otherwise the multiplicity of firstEvent is 0.
        ///if (event-&gt;size() = 2)
        ///  then (firstEvent-&gt;size() = 2) else (firstEvent-&gt;size() = 0)
        ///endif
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool First_event_multiplicity(object diagnostics, object context);
    }
}

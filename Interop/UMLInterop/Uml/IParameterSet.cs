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
    /// The public interface for ParameterSet
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ParameterSet))]
    [XmlDefaultImplementationTypeAttribute(typeof(ParameterSet))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ParameterSet")]
    public interface IParameterSet : IModelElement, INamedElement
    {
        
        /// <summary>
        /// A constraint that should be satisfied for the owner of the Parameters in an input ParameterSet to start execution using the values provided for those Parameters, or the owner of the Parameters in an output ParameterSet to end execution providing the values for those Parameters, if all preconditions and conditions on input ParameterSets were satisfied.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("condition")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IConstraint> Condition
        {
            get;
        }
        
        /// <summary>
        /// Parameters in the ParameterSet.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("parameter")]
        [DescriptionAttribute("Parameters in the ParameterSet.\n<p>From package UML::Classification.</p>")]
        [CategoryAttribute("ParameterSet")]
        [XmlElementNameAttribute("parameter")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("parameterSet")]
        [ConstantAttribute()]
        ISetExpression<NMF.Interop.Uml.IParameter> Parameter
        {
            get;
        }
        
        /// <summary>
        /// The Parameters in a ParameterSet must all be inputs or all be outputs of the same parameterized entity, and the ParameterSet is owned by that entity.
        ///parameter-&gt;forAll(p1, p2 | self.owner = p1.owner and self.owner = p2.owner and p1.direction = p2.direction)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Same_parameterized_entity(object diagnostics, object context);
        
        /// <summary>
        /// If a parameterized entity has input Parameters that are in a ParameterSet, then any inputs that are not in a ParameterSet must be streaming. Same for output Parameters.
        ///((parameter-&gt;exists(direction = ParameterDirectionKind::_&apos;in&apos;)) implies 
        ///    behavioralFeature.ownedParameter-&gt;select(p | p.direction = ParameterDirectionKind::_&apos;in&apos; and p.parameterSet-&gt;isEmpty())-&gt;forAll(isStream))
        ///    and
        ///((parameter-&gt;exists(direction = ParameterDirectionKind::out)) implies 
        ///    behavioralFeature.ownedParameter-&gt;select(p | p.direction = ParameterDirectionKind::out and p.parameterSet-&gt;isEmpty())-&gt;forAll(isStream))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Input(object diagnostics, object context);
        
        /// <summary>
        /// Two ParameterSets cannot have exactly the same set of Parameters.
        ///parameter-&gt;forAll(parameterSet-&gt;forAll(s1, s2 | s1-&gt;size() = s2-&gt;size() implies s1.parameter-&gt;exists(p | not s2.parameter-&gt;includes(p))))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Two_parameter_sets(object diagnostics, object context);
    }
}

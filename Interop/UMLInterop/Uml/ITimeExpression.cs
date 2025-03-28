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
    /// The public interface for TimeExpression
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(TimeExpression))]
    [XmlDefaultImplementationTypeAttribute(typeof(TimeExpression))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//TimeExpression")]
    public interface ITimeExpression : IModelElement, IValueSpecification
    {
        
        /// <summary>
        /// A ValueSpecification that evaluates to the value of the TimeExpression.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("expr")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IValueSpecification Expr
        {
            get;
            set;
        }
        
        /// <summary>
        /// Refers to the Observations that are involved in the computation of the TimeExpression value.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("observation")]
        [DescriptionAttribute("Refers to the Observations that are involved in the computation of the TimeExpres" +
            "sion value.\n<p>From package UML::Values.</p>")]
        [CategoryAttribute("TimeExpression")]
        [XmlElementNameAttribute("observation")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<IObservation> Observation
        {
            get;
        }
        
        /// <summary>
        /// If a TimeExpression has no expr, then it must have a single observation that is a TimeObservation.
        ///expr = null implies (observation-&gt;size() = 1 and observation-&gt;forAll(oclIsKindOf(TimeObservation)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_expr_requires_observation(object diagnostics, object context);
    }
}

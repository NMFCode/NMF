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
    /// The public interface for ValueSpecificationAction
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ValueSpecificationAction))]
    [XmlDefaultImplementationTypeAttribute(typeof(ValueSpecificationAction))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ValueSpecificationAction")]
    public interface IValueSpecificationAction : IModelElement, IAction
    {
        
        /// <summary>
        /// The OutputPin on which the result value is placed.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("result")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IOutputPin Result
        {
            get;
            set;
        }
        
        /// <summary>
        /// The ValueSpecification to be evaluated.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IValueSpecification Value
        {
            get;
            set;
        }
        
        /// <summary>
        /// The multiplicity of the result OutputPin is 1..1
        ///result.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity(object diagnostics, object context);
        
        /// <summary>
        /// The type of the value ValueSpecification must conform to the type of the result OutputPin.
        ///value.type.conformsTo(result.type)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Compatible_type(object diagnostics, object context);
    }
}

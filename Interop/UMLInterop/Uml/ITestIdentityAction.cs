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
    /// The public interface for TestIdentityAction
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(TestIdentityAction))]
    [XmlDefaultImplementationTypeAttribute(typeof(TestIdentityAction))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//TestIdentityAction")]
    public interface ITestIdentityAction : IModelElement, IAction
    {
        
        /// <summary>
        /// The InputPin on which the first input object is placed.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("first")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IInputPin First
        {
            get;
            set;
        }
        
        /// <summary>
        /// The OutputPin whose Boolean value indicates whether the two input objects are identical.
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
        /// The OutputPin on which the second input object is placed.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("second")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IInputPin Second
        {
            get;
            set;
        }
        
        /// <summary>
        /// The multiplicity of the InputPins is 1..1.
        ///first.is(1,1) and second.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity(object diagnostics, object context);
        
        /// <summary>
        /// The InputPins have no type.
        ///first.type= null and second.type = null
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_type(object diagnostics, object context);
        
        /// <summary>
        /// The type of the result OutputPin is Boolean.
        ///result.type=Boolean
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Result_is_boolean(object diagnostics, object context);
    }
}

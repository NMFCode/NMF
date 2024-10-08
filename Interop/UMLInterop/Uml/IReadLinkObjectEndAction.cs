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
    /// The public interface for ReadLinkObjectEndAction
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ReadLinkObjectEndAction))]
    [XmlDefaultImplementationTypeAttribute(typeof(ReadLinkObjectEndAction))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ReadLinkObjectEndAction")]
    public interface IReadLinkObjectEndAction : IModelElement, IAction
    {
        
        /// <summary>
        /// The Association end to be read.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("end")]
        [DescriptionAttribute("The Association end to be read.\n<p>From package UML::Actions.</p>")]
        [CategoryAttribute("ReadLinkObjectEndAction")]
        [XmlElementNameAttribute("end")]
        [XmlAttributeAttribute(true)]
        IProperty End
        {
            get;
            set;
        }
        
        /// <summary>
        /// The input pin from which the link object is obtained.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("object")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IInputPin Object
        {
            get;
            set;
        }
        
        /// <summary>
        /// The OutputPin where the result value is placed.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
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
        /// The end Property must be an Association memberEnd.
        ///end.association &lt;&gt; null
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Property(object diagnostics, object context);
        
        /// <summary>
        /// The multiplicity of the object InputPin is 1..1.
        ///object.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity_of_object(object diagnostics, object context);
        
        /// <summary>
        /// The ends of the association must not be static.
        ///end.association.memberEnd-&gt;forAll(e | not e.isStatic)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Ends_of_association(object diagnostics, object context);
        
        /// <summary>
        /// The type of the result OutputPin is the same as the type of the end Property.
        ///result.type = end.type
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Type_of_result(object diagnostics, object context);
        
        /// <summary>
        /// The multiplicity of the result OutputPin is 1..1.
        ///result.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity_of_result(object diagnostics, object context);
        
        /// <summary>
        /// The type of the object InputPin is the AssociationClass that owns the end Property.
        ///object.type = end.association
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Type_of_object(object diagnostics, object context);
        
        /// <summary>
        /// The association of the end must be an AssociationClass.
        ///end.association.oclIsKindOf(AssociationClass)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Association_of_association(object diagnostics, object context);
    }
}

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
    /// The public interface for ReadLinkAction
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ReadLinkAction))]
    [XmlDefaultImplementationTypeAttribute(typeof(ReadLinkAction))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ReadLinkAction")]
    public interface IReadLinkAction : IModelElement, ILinkAction
    {
        
        /// <summary>
        /// The OutputPin on which the objects retrieved from the "open" end of those links whose values on other ends are given by the endData.
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
        /// The type and ordering of the result OutputPin are same as the type and ordering of the open Association end.
        ///self.openEnd()->forAll(type=result.type and isOrdered=result.isOrdered)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Type_and_ordering(object diagnostics, object context);
        
        /// <summary>
        /// The multiplicity of the open Association end must be compatible with the multiplicity of the result OutputPin.
        ///self.openEnd()->first().compatibleWith(result)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Compatible_multiplicity(object diagnostics, object context);
        
        /// <summary>
        /// Visibility of the open end must allow access from the object performing the action.
        ///let openEnd : Property = self.openEnd()->first() in
        ///  openEnd.visibility = VisibilityKind::public or 
        ///  endData->exists(oed | 
        ///    oed.end<>openEnd and 
        ///    (_'context' = oed.end.type or 
        ///      (openEnd.visibility = VisibilityKind::protected and 
        ///        _'context'.conformsTo(oed.end.type.oclAsType(Classifier)))))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Visibility(object diagnostics, object context);
        
        /// <summary>
        /// Exactly one linkEndData specification (corresponding to the "open" end) must not have an value InputPin.
        ///self.openEnd()->size() = 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool One_open_end(object diagnostics, object context);
        
        /// <summary>
        /// The open end must be navigable.
        ///self.openEnd()->first().isNavigable()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Navigable_open_end(object diagnostics, object context);
        
        /// <summary>
        /// Returns the ends corresponding to endData with no value InputPin. (A well-formed ReadLinkAction is constrained to have only one of these.)
        ///result = (endData->select(value=null).end->asOrderedSet())
        ///<p>From package UML::Actions.</p>
        /// </summary>
        IOrderedSetExpression<IProperty> OpenEnd();
    }
}

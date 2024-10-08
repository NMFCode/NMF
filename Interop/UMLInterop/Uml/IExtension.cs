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
    /// The public interface for Extension
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Extension))]
    [XmlDefaultImplementationTypeAttribute(typeof(Extension))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Extension")]
    public interface IExtension : IModelElement, IAssociation
    {
        
        /// <summary>
        /// The non-owned end of an Extension is typed by a Class.
        ///metaclassEnd()-&gt;notEmpty() and metaclassEnd().type.oclIsKindOf(Class)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Non_owned_end(object diagnostics, object context);
        
        /// <summary>
        /// An Extension is binary, i.e., it has only two memberEnds.
        ///memberEnd-&gt;size() = 2
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Is_binary(object diagnostics, object context);
        
        /// <summary>
        /// Retrieves the stereotype that extends a metaclass through this extension.
        /// </summary>
        IStereotype GetStereotype();
        
        /// <summary>
        /// Retrieves the extension end that is typed by a stereotype (as opposed to a metaclass).
        /// </summary>
        IProperty GetStereotypeEnd();
        
        /// <summary>
        /// The query metaclass() returns the metaclass that is being extended (as opposed to the extending stereotype).
        ///result = (metaclassEnd().type.oclAsType(Class))
        ///&lt;p&gt;From package UML::Packages.&lt;/p&gt;
        /// </summary>
        NMF.Interop.Uml.IClass GetMetaclass();
        
        /// <summary>
        /// The query metaclassEnd() returns the Property that is typed by a metaclass (as opposed to a stereotype).
        ///result = (memberEnd-&gt;reject(p | ownedEnd-&gt;includes(p.oclAsType(ExtensionEnd)))-&gt;any(true))
        ///&lt;p&gt;From package UML::Packages.&lt;/p&gt;
        /// </summary>
        IProperty MetaclassEnd();
    }
}

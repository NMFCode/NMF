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
    /// VisibilityKind is an enumeration type that defines literals to determine the visibility of Elements in a model.
    ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
    /// </summary>
    [TypeConverterAttribute(typeof(VisibilityKindConverter))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//VisibilityKind")]
    public enum VisibilityKind
    {
        
        /// <summary>
        ///A Named Element with public visibility is visible to all elements that can access the contents of the Namespace that owns it.
        ///</summary>
        Public = 0,
        
        /// <summary>
        ///A NamedElement with private visibility is only visible inside the Namespace that owns it.
        ///</summary>
        Private = 1,
        
        /// <summary>
        ///A NamedElement with protected visibility is visible to Elements that have a generalization relationship to the Namespace that owns it.
        ///</summary>
        Protected = 2,
        
        /// <summary>
        ///A NamedElement with package visibility is visible to all Elements within the nearest enclosing Package (given that other owning Elements have proper visibility). Outside the nearest enclosing Package, a NamedElement marked as having package visibility is not visible.  Only NamedElements that are not owned by Packages can be marked as having package visibility.
        ///</summary>
        Package = 3,
    }
}

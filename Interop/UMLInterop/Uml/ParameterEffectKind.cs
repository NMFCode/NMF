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
    /// ParameterEffectKind is an Enumeration that indicates the effect of a Behavior on values passed in or out of its parameters.
    ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
    /// </summary>
    [TypeConverterAttribute(typeof(ParameterEffectKindConverter))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ParameterEffectKind")]
    public enum ParameterEffectKind
    {
        
        /// <summary>
        ///Indicates that the behavior creates values.
        ///</summary>
        Create = 0,
        
        /// <summary>
        ///Indicates objects that are values of the parameter have values of their properties, or links in which they participate, or their classifiers retrieved during executions of the behavior.
        ///</summary>
        Read = 1,
        
        /// <summary>
        ///Indicates objects that are values of the parameter have values of their properties, or links in which they participate, or their classification changed during executions of the behavior.
        ///</summary>
        Update = 2,
        
        /// <summary>
        ///Indicates objects that are values of the parameter do not exist after executions of the behavior are finished.
        ///</summary>
        Delete = 3,
    }
}

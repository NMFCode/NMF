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
    /// The public interface for WriteVariableAction
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(WriteVariableAction))]
    [XmlDefaultImplementationTypeAttribute(typeof(WriteVariableAction))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//WriteVariableAction")]
    public interface IWriteVariableAction : IModelElement, IVariableAction
    {
        
        /// <summary>
        /// The InputPin that gives the value to be added or removed from the Variable.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IInputPin Value
        {
            get;
            set;
        }
        
        /// <summary>
        /// The type of the value InputPin must conform to the type of the variable.
        ///value &lt;&gt; null implies value.type.conformsTo(variable.type)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Value_type(object diagnostics, object context);
        
        /// <summary>
        /// The multiplicity of the value InputPin is 1..1.
        ///value&lt;&gt;null implies value.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity(object diagnostics, object context);
    }
}

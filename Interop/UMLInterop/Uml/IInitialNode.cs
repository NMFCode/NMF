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
    /// The public interface for InitialNode
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(InitialNode))]
    [XmlDefaultImplementationTypeAttribute(typeof(InitialNode))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//InitialNode")]
    public interface IInitialNode : IModelElement, IControlNode
    {
        
        /// <summary>
        /// An InitialNode has no incoming ActivityEdges.
        ///incoming->isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool No_incoming_edges(object diagnostics, object context);
        
        /// <summary>
        /// All the outgoing ActivityEdges from an InitialNode must be ControlFlows.
        ///outgoing->forAll(oclIsKindOf(ControlFlow))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Control_edges(object diagnostics, object context);
    }
}

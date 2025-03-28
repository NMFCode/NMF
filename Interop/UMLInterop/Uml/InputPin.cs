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
    /// An InputPin is a Pin that holds input values to be consumed by an Action.
    ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//InputPin")]
    [DebuggerDisplayAttribute("InputPin {Name}")]
    public partial class InputPin : Pin, IInputPin, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _outgoing_edges_structured_onlyOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveOutgoing_edges_structured_onlyOperation);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//InputPin")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// An InputPin may have outgoing ActivityEdges only when it is owned by a StructuredActivityNode, and these edges must target a node contained (directly or indirectly) in the owning StructuredActivityNode.
        ///outgoing-&gt;notEmpty() implies
        ///	action&lt;&gt;null and
        ///	action.oclIsKindOf(StructuredActivityNode) and
        ///	action.oclAsType(StructuredActivityNode).allOwnedNodes()-&gt;includesAll(outgoing.target)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Outgoing_edges_structured_only(object diagnostics, object context)
        {
            System.Func<IInputPin, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IInputPin, object, object, bool>>(_outgoing_edges_structured_onlyOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method outgoing_edges_structured_only registered. " +
                        "Use the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _outgoing_edges_structured_onlyOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _outgoing_edges_structured_onlyOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _outgoing_edges_structured_onlyOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveOutgoing_edges_structured_onlyOperation()
        {
            return ClassInstance.LookupOperation("outgoing_edges_structured_only");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//InputPin")));
            }
            return _classInstance;
        }
    }
}

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
    /// A PartDecomposition is a description of the internal Interactions of one Lifeline relative to an Interaction.
    ///<p>From package UML::Interactions.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//PartDecomposition")]
    [DebuggerDisplayAttribute("PartDecomposition {Name}")]
    public partial class PartDecomposition : InteractionUse, IPartDecomposition, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _commutativity_of_decompositionOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveCommutativity_of_decompositionOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _assumeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveAssumeOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _parts_of_internal_structuresOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveParts_of_internal_structuresOperation);
        
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//PartDecomposition")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Assume that within Interaction X, Lifeline L is of class C and decomposed to D. Assume also that there is within X an InteractionUse (say) U that covers L. According to the constraint above U will have a counterpart CU within D. Within the Interaction referenced by U, L should also be decomposed, and the decomposition should reference CU. (This rule is called commutativity of decomposition.)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Commutativity_of_decomposition(object diagnostics, object context)
        {
            System.Func<IPartDecomposition, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IPartDecomposition, object, object, bool>>(_commutativity_of_decompositionOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method commutativity_of_decomposition registered. " +
                        "Use the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _commutativity_of_decompositionOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _commutativity_of_decompositionOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _commutativity_of_decompositionOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveCommutativity_of_decompositionOperation()
        {
            return ClassInstance.LookupOperation("commutativity_of_decomposition");
        }
        
        /// <summary>
        /// Assume that within Interaction X, Lifeline L is of class C and decomposed to D. Within X there is a sequence of constructs along L (such constructs are CombinedFragments, InteractionUse and (plain) OccurrenceSpecifications). Then a corresponding sequence of constructs must appear within D, matched one-to-one in the same order. i) CombinedFragment covering L are matched with an extra-global CombinedFragment in D. ii) An InteractionUse covering L is matched with a global (i.e., covering all Lifelines) InteractionUse in D. iii) A plain OccurrenceSpecification on L is considered an actualGate that must be matched by a formalGate of D.
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Assume(object diagnostics, object context)
        {
            System.Func<IPartDecomposition, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IPartDecomposition, object, object, bool>>(_assumeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method assume registered. Use the method broker to" +
                        " register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _assumeOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _assumeOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _assumeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveAssumeOperation()
        {
            return ClassInstance.LookupOperation("assume");
        }
        
        /// <summary>
        /// PartDecompositions apply only to Parts that are Parts of Internal Structures not to Parts of Collaborations.
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Parts_of_internal_structures(object diagnostics, object context)
        {
            System.Func<IPartDecomposition, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IPartDecomposition, object, object, bool>>(_parts_of_internal_structuresOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method parts_of_internal_structures registered. Us" +
                        "e the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _parts_of_internal_structuresOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _parts_of_internal_structuresOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _parts_of_internal_structuresOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveParts_of_internal_structuresOperation()
        {
            return ClassInstance.LookupOperation("parts_of_internal_structures");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//PartDecomposition")));
            }
            return _classInstance;
        }
    }
}

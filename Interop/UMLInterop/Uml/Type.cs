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
    /// A Type constrains the values represented by a TypedElement.
    ///<p>From package UML::CommonStructure.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Type")]
    [DebuggerDisplayAttribute("Type {Name}")]
    public abstract partial class Type : PackageableElement, NMF.Interop.Uml.IType, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _createAssociationOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveCreateAssociationOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getAssociationsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetAssociationsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _conformsToOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveConformsToOperation);
        
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Type")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Creates a(n) (binary) association between this type and the specified other type, with the specified navigabilities, aggregations, names, lower bounds, and upper bounds, and owned by this type's nearest package.
        /// </summary>
        /// <param name="end1IsNavigable">The navigability for the first end of the new association.</param>
        /// <param name="end1Aggregation">The aggregation for the first end of the new association.</param>
        /// <param name="end1Name">The name for the first end of the new association.</param>
        /// <param name="end1Lower">The lower bound for the first end of the new association.</param>
        /// <param name="end1Upper">The upper bound for the first end of the new association.</param>
        /// <param name="end1Type">The type for the first end of the new association.</param>
        /// <param name="end2IsNavigable">The navigability for the second end of the new association.</param>
        /// <param name="end2Aggregation">The aggregation for the second end of the new association.</param>
        /// <param name="end2Name">The name for the second end of the new association.</param>
        /// <param name="end2Lower">The lower bound for the second end of the new association.</param>
        /// <param name="end2Upper">The upper bound for the second end of the new association.</param>
        public IAssociation CreateAssociation(bool end1IsNavigable, AggregationKind end1Aggregation, string end1Name, int end1Lower, object end1Upper, NMF.Interop.Uml.IType end1Type, bool end2IsNavigable, AggregationKind end2Aggregation, string end2Name, int end2Lower, object end2Upper)
        {
            System.Func<NMF.Interop.Uml.IType, bool, AggregationKind, string, int, object, NMF.Interop.Uml.IType, bool, AggregationKind, string, int, object, IAssociation> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IType, bool, AggregationKind, string, int, object, NMF.Interop.Uml.IType, bool, AggregationKind, string, int, object, IAssociation>>(_createAssociationOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method createAssociation registered. Use the metho" +
                        "d broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _createAssociationOperation.Value, end1IsNavigable, end1Aggregation, end1Name, end1Lower, end1Upper, end1Type, end2IsNavigable, end2Aggregation, end2Name, end2Lower, end2Upper);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _createAssociationOperation.Value, e));
            IAssociation result = handler.Invoke(this, end1IsNavigable, end1Aggregation, end1Name, end1Lower, end1Upper, end1Type, end2IsNavigable, end2Aggregation, end2Name, end2Lower, end2Upper);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _createAssociationOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveCreateAssociationOperation()
        {
            return ClassInstance.LookupOperation("createAssociation");
        }
        
        /// <summary>
        /// Retrieves the associations in which this type is involved.
        /// </summary>
        public ISetExpression<IAssociation> GetAssociations()
        {
            System.Func<NMF.Interop.Uml.IType, ISetExpression<IAssociation>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IType, ISetExpression<IAssociation>>>(_getAssociationsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getAssociations registered. Use the method " +
                        "broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getAssociationsOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getAssociationsOperation.Value, e));
            ISetExpression<IAssociation> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getAssociationsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetAssociationsOperation()
        {
            return ClassInstance.LookupOperation("getAssociations");
        }
        
        /// <summary>
        /// The query conformsTo() gives true for a Type that conforms to another. By default, two Types do not conform to each other. This query is intended to be redefined for specific conformance situations.
        ///result = (false)
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        /// <param name="other"></param>
        public bool ConformsTo(NMF.Interop.Uml.IType other)
        {
            System.Func<NMF.Interop.Uml.IType, NMF.Interop.Uml.IType, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IType, NMF.Interop.Uml.IType, bool>>(_conformsToOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method conformsTo registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _conformsToOperation.Value, other);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _conformsToOperation.Value, e));
            bool result = handler.Invoke(this, other);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _conformsToOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveConformsToOperation()
        {
            return ClassInstance.LookupOperation("conformsTo");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Type")));
            }
            return _classInstance;
        }
    }
}

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
    /// An EncapsulatedClassifier may own Ports to specify typed interaction points.
    ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//EncapsulatedClassifier")]
    [DebuggerDisplayAttribute("EncapsulatedClassifier {Name}")]
    public abstract partial class EncapsulatedClassifier : Classifier, IEncapsulatedClassifier, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getOwnedPortsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetOwnedPortsOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedPortReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedPortReference);
        
        /// <summary>
        /// The backing field for the OwnedPort property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IPort> _ownedPort;
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _createOwnedAttributeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveCreateOwnedAttributeOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getPartsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetPartsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _allRolesOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveAllRolesOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedConnectorReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedConnectorReference);
        
        /// <summary>
        /// The backing field for the OwnedConnector property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IConnector> _ownedConnector;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public EncapsulatedClassifier()
        {
            this._ownedPort = new ObservableCompositionOrderedSet<IPort>(this);
            this._ownedPort.CollectionChanging += this.OwnedPortCollectionChanging;
            this._ownedPort.CollectionChanged += this.OwnedPortCollectionChanged;
            this._ownedConnector = new ObservableCompositionOrderedSet<IConnector>(this);
            this._ownedConnector.CollectionChanging += this.OwnedConnectorCollectionChanging;
            this._ownedConnector.CollectionChanged += this.OwnedConnectorCollectionChanged;
        }
        
        /// <summary>
        /// The Ports owned by the EncapsulatedClassifier.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedPort")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IPort> OwnedPort
        {
            get
            {
                return this._ownedPort;
            }
        }
        
        IListExpression<IProperty> IStructuredClassifier.OwnedAttribute
        {
            get
            {
                return new EncapsulatedClassifierOwnedAttributeCollection(this);
            }
        }
        
        /// <summary>
        /// The connectors owned by the StructuredClassifier.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedConnector")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IConnector> OwnedConnector
        {
            get
            {
                return this._ownedConnector;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new EncapsulatedClassifierChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new EncapsulatedClassifierReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//EncapsulatedClassifier")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Derivation for EncapsulatedClassifier::/ownedPort : Port
        ///result = (ownedAttribute-&gt;select(oclIsKindOf(Port))-&gt;collect(oclAsType(Port))-&gt;asOrderedSet())
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        public IOrderedSetExpression<IPort> GetOwnedPorts()
        {
            System.Func<IEncapsulatedClassifier, IOrderedSetExpression<IPort>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IEncapsulatedClassifier, IOrderedSetExpression<IPort>>>(_getOwnedPortsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getOwnedPorts registered. Use the method br" +
                        "oker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getOwnedPortsOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getOwnedPortsOperation.Value, e));
            IOrderedSetExpression<IPort> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getOwnedPortsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetOwnedPortsOperation()
        {
            return ClassInstance.LookupOperation("getOwnedPorts");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedPortReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.EncapsulatedClassifier.ClassInstance)).Resolve("ownedPort")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedPort property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedPortCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedPort", e, _ownedPortReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedPort property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedPortCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedPort", e, _ownedPortReference);
        }
        
        /// <summary>
        /// Creates a property with the specified name, type, lower bound, and upper bound as an owned attribute of this structured classifier.
        /// </summary>
        /// <param name="name">The name for the new attribute, or null.</param>
        /// <param name="type">The type for the new attribute, or null.</param>
        /// <param name="lower">The lower bound for the new attribute.</param>
        /// <param name="upper">The upper bound for the new attribute.</param>
        public IProperty CreateOwnedAttribute(string name, NMF.Interop.Uml.IType type, int lower, object upper)
        {
            System.Func<IStructuredClassifier, string, NMF.Interop.Uml.IType, int, object, IProperty> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IStructuredClassifier, string, NMF.Interop.Uml.IType, int, object, IProperty>>(_createOwnedAttributeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method createOwnedAttribute registered. Use the me" +
                        "thod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _createOwnedAttributeOperation.Value, name, type, lower, upper);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _createOwnedAttributeOperation.Value, e));
            IProperty result = handler.Invoke(this, name, type, lower, upper);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _createOwnedAttributeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveCreateOwnedAttributeOperation()
        {
            return ClassInstance.LookupOperation("createOwnedAttribute");
        }
        
        /// <summary>
        /// Derivation for StructuredClassifier::/part
        ///result = (ownedAttribute-&gt;select(isComposite)-&gt;asSet())
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        public ISetExpression<IProperty> GetParts()
        {
            System.Func<IStructuredClassifier, ISetExpression<IProperty>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IStructuredClassifier, ISetExpression<IProperty>>>(_getPartsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getParts registered. Use the method broker " +
                        "to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getPartsOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getPartsOperation.Value, e));
            ISetExpression<IProperty> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getPartsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetPartsOperation()
        {
            return ClassInstance.LookupOperation("getParts");
        }
        
        /// <summary>
        /// All features of type ConnectableElement, equivalent to all direct and inherited roles.
        ///result = (allFeatures()-&gt;select(oclIsKindOf(ConnectableElement))-&gt;collect(oclAsType(ConnectableElement))-&gt;asSet())
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        public ISetExpression<IConnectableElement> AllRoles()
        {
            System.Func<IStructuredClassifier, ISetExpression<IConnectableElement>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IStructuredClassifier, ISetExpression<IConnectableElement>>>(_allRolesOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method allRoles registered. Use the method broker " +
                        "to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _allRolesOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _allRolesOperation.Value, e));
            ISetExpression<IConnectableElement> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _allRolesOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveAllRolesOperation()
        {
            return ClassInstance.LookupOperation("allRoles");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedConnectorReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.StructuredClassifier.ClassInstance)).Resolve("ownedConnector")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedConnector property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedConnectorCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedConnector", e, _ownedConnectorReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedConnector property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedConnectorCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedConnector", e, _ownedConnectorReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int ownedPortIndex = ModelHelper.IndexOfReference(this.OwnedPort, element);
            if ((ownedPortIndex != -1))
            {
                return ModelHelper.CreatePath("ownedPort", ownedPortIndex);
            }
            int ownedConnectorIndex = ModelHelper.IndexOfReference(this.OwnedConnector, element);
            if ((ownedConnectorIndex != -1))
            {
                return ModelHelper.CreatePath("ownedConnector", ownedConnectorIndex);
            }
            return base.GetRelativePathForNonIdentifiedChild(element);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "OWNEDPORT"))
            {
                if ((index < this.OwnedPort.Count))
                {
                    return this.OwnedPort[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "OWNEDCONNECTOR"))
            {
                if ((index < this.OwnedConnector.Count))
                {
                    return this.OwnedConnector[index];
                }
                else
                {
                    return null;
                }
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "OWNEDPORT"))
            {
                return this._ownedPort;
            }
            if ((feature == "OWNEDCONNECTOR"))
            {
                return this._ownedConnector;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected override string GetCompositionName(object container)
        {
            if ((container == this._ownedPort))
            {
                return "ownedPort";
            }
            if ((container == this._ownedConnector))
            {
                return "ownedConnector";
            }
            return base.GetCompositionName(container);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//EncapsulatedClassifier")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the EncapsulatedClassifier class
        /// </summary>
        public class EncapsulatedClassifierChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private EncapsulatedClassifier _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public EncapsulatedClassifierChildrenCollection(EncapsulatedClassifier parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the EncapsulatedClassifier class
        /// </summary>
        public class EncapsulatedClassifierReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private EncapsulatedClassifier _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public EncapsulatedClassifierReferencedElementsCollection(EncapsulatedClassifier parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
    }
}

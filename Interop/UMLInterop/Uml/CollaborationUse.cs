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
    /// A CollaborationUse is used to specify the application of a pattern specified by a Collaboration to a specific situation.
    ///<p>From package UML::StructuredClassifiers.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//CollaborationUse")]
    [DebuggerDisplayAttribute("CollaborationUse {Name}")]
    public partial class CollaborationUse : NamedElement, ICollaborationUse, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _client_elementsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveClient_elementsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _every_roleOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveEvery_roleOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _connectorsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveConnectorsOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _roleBindingReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveRoleBindingReference);
        
        /// <summary>
        /// The backing field for the RoleBinding property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IDependency> _roleBinding;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _typeReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveTypeReference);
        
        /// <summary>
        /// The backing field for the Type property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ICollaboration _type;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        public CollaborationUse()
        {
            this._roleBinding = new ObservableCompositionOrderedSet<IDependency>(this);
            this._roleBinding.CollectionChanging += this.RoleBindingCollectionChanging;
            this._roleBinding.CollectionChanged += this.RoleBindingCollectionChanged;
        }
        
        /// <summary>
        /// A mapping between features of the Collaboration and features of the owning Classifier. This mapping indicates which ConnectableElement of the Classifier plays which role(s) in the Collaboration. A ConnectableElement may be bound to multiple roles in the same CollaborationUse (that is, it may play multiple roles).
        ///<p>From package UML::StructuredClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("roleBinding")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IDependency> RoleBinding
        {
            get
            {
                return this._roleBinding;
            }
        }
        
        /// <summary>
        /// The Collaboration which is used in this CollaborationUse. The Collaboration defines the cooperation between its roles which are mapped to ConnectableElements relating to the Classifier owning the CollaborationUse.
        ///<p>From package UML::StructuredClassifiers.</p>
        /// </summary>
        [DisplayNameAttribute("type")]
        [DescriptionAttribute(@"The Collaboration which is used in this CollaborationUse. The Collaboration defines the cooperation between its roles which are mapped to ConnectableElements relating to the Classifier owning the CollaborationUse.
<p>From package UML::StructuredClassifiers.</p>")]
        [CategoryAttribute("CollaborationUse")]
        [XmlElementNameAttribute("type")]
        [XmlAttributeAttribute(true)]
        public ICollaboration Type
        {
            get
            {
                return this._type;
            }
            set
            {
                if ((this._type != value))
                {
                    ICollaboration old = this._type;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Type", e, _typeReference);
                    this._type = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetType;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetType;
                    }
                    this.OnPropertyChanged("Type", e, _typeReference);
                }
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new CollaborationUseChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new CollaborationUseReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//CollaborationUse")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// All the client elements of a roleBinding are in one Classifier and all supplier elements of a roleBinding are in one Collaboration.
        ///roleBinding->collect(client)->forAll(ne1, ne2 |
        ///  ne1.oclIsKindOf(ConnectableElement) and ne2.oclIsKindOf(ConnectableElement) and
        ///    let ce1 : ConnectableElement = ne1.oclAsType(ConnectableElement), ce2 : ConnectableElement = ne2.oclAsType(ConnectableElement) in
        ///      ce1.structuredClassifier = ce2.structuredClassifier)
        ///and
        ///  roleBinding->collect(supplier)->forAll(ne1, ne2 |
        ///  ne1.oclIsKindOf(ConnectableElement) and ne2.oclIsKindOf(ConnectableElement) and
        ///    let ce1 : ConnectableElement = ne1.oclAsType(ConnectableElement), ce2 : ConnectableElement = ne2.oclAsType(ConnectableElement) in
        ///      ce1.collaboration = ce2.collaboration)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Client_elements(object diagnostics, object context)
        {
            System.Func<ICollaborationUse, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICollaborationUse, object, object, bool>>(_client_elementsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method client_elements registered. Use the method " +
                        "broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _client_elementsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _client_elementsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _client_elementsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveClient_elementsOperation()
        {
            return ClassInstance.LookupOperation("client_elements");
        }
        
        /// <summary>
        /// Every collaborationRole in the Collaboration is bound within the CollaborationUse.
        ///type.collaborationRole->forAll(role | roleBinding->exists(rb | rb.supplier->includes(role)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Every_role(object diagnostics, object context)
        {
            System.Func<ICollaborationUse, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICollaborationUse, object, object, bool>>(_every_roleOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method every_role registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _every_roleOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _every_roleOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _every_roleOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveEvery_roleOperation()
        {
            return ClassInstance.LookupOperation("every_role");
        }
        
        /// <summary>
        /// Connectors in a Collaboration typing a CollaborationUse must have corresponding Connectors between elements bound in the context Classifier, and these corresponding Connectors must have the same or more general type than the Collaboration Connectors.
        ///type.ownedConnector->forAll(connector |
        ///  let rolesConnectedInCollab : Set(ConnectableElement) = connector.end.role->asSet(),
        ///        relevantBindings : Set(Dependency) = roleBinding->select(rb | rb.supplier->intersection(rolesConnectedInCollab)->notEmpty()),
        ///        boundRoles : Set(ConnectableElement) = relevantBindings->collect(client.oclAsType(ConnectableElement))->asSet(),
        ///        contextClassifier : StructuredClassifier = boundRoles->any(true).structuredClassifier->any(true) in
        ///          contextClassifier.ownedConnector->exists( correspondingConnector | 
        ///              correspondingConnector.end.role->forAll( role | boundRoles->includes(role) )
        ///              and (connector.type->notEmpty() and correspondingConnector.type->notEmpty()) implies connector.type->forAll(conformsTo(correspondingConnector.type)) )
        ///)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Connectors(object diagnostics, object context)
        {
            System.Func<ICollaborationUse, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICollaborationUse, object, object, bool>>(_connectorsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method connectors registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _connectorsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _connectorsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _connectorsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveConnectorsOperation()
        {
            return ClassInstance.LookupOperation("connectors");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveRoleBindingReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.CollaborationUse.ClassInstance)).Resolve("roleBinding")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the RoleBinding property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void RoleBindingCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("RoleBinding", e, _roleBindingReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the RoleBinding property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void RoleBindingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("RoleBinding", e, _roleBindingReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveTypeReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.CollaborationUse.ClassInstance)).Resolve("type")));
        }
        
        /// <summary>
        /// Handles the event that the Type property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetType(object sender, System.EventArgs eventArgs)
        {
            this.Type = null;
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int roleBindingIndex = ModelHelper.IndexOfReference(this.RoleBinding, element);
            if ((roleBindingIndex != -1))
            {
                return ModelHelper.CreatePath("roleBinding", roleBindingIndex);
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
            if ((reference == "ROLEBINDING"))
            {
                if ((index < this.RoleBinding.Count))
                {
                    return this.RoleBinding[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "TYPE"))
            {
                return this.Type;
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
            if ((feature == "ROLEBINDING"))
            {
                return this._roleBinding;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "TYPE"))
            {
                this.Type = ((ICollaboration)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given reference
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="reference">The requested reference in upper case</param>
        protected override NMF.Expressions.INotifyExpression<NMF.Models.IModelElement> GetExpressionForReference(string reference)
        {
            if ((reference == "TYPE"))
            {
                return new TypeProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected override string GetCompositionName(object container)
        {
            if ((container == this._roleBinding))
            {
                return "roleBinding";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//CollaborationUse")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the CollaborationUse class
        /// </summary>
        public class CollaborationUseChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private CollaborationUse _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public CollaborationUseChildrenCollection(CollaborationUse parent)
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
                    count = (count + this._parent.RoleBinding.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.RoleBinding.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.RoleBinding.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IDependency roleBindingCasted = item.As<IDependency>();
                if ((roleBindingCasted != null))
                {
                    this._parent.RoleBinding.Add(roleBindingCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.RoleBinding.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.RoleBinding.Contains(item))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
                IEnumerator<IModelElement> roleBindingEnumerator = this._parent.RoleBinding.GetEnumerator();
                try
                {
                    for (
                    ; roleBindingEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = roleBindingEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    roleBindingEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IDependency dependencyItem = item.As<IDependency>();
                if (((dependencyItem != null) 
                            && this._parent.RoleBinding.Remove(dependencyItem)))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().Concat(this._parent.RoleBinding).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the CollaborationUse class
        /// </summary>
        public class CollaborationUseReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private CollaborationUse _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public CollaborationUseReferencedElementsCollection(CollaborationUse parent)
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
                    count = (count + this._parent.RoleBinding.Count);
                    if ((this._parent.Type != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.RoleBinding.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.RoleBinding.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IDependency roleBindingCasted = item.As<IDependency>();
                if ((roleBindingCasted != null))
                {
                    this._parent.RoleBinding.Add(roleBindingCasted);
                }
                if ((this._parent.Type == null))
                {
                    ICollaboration typeCasted = item.As<ICollaboration>();
                    if ((typeCasted != null))
                    {
                        this._parent.Type = typeCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.RoleBinding.Clear();
                this._parent.Type = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.RoleBinding.Contains(item))
                {
                    return true;
                }
                if ((item == this._parent.Type))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
                IEnumerator<IModelElement> roleBindingEnumerator = this._parent.RoleBinding.GetEnumerator();
                try
                {
                    for (
                    ; roleBindingEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = roleBindingEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    roleBindingEnumerator.Dispose();
                }
                if ((this._parent.Type != null))
                {
                    array[arrayIndex] = this._parent.Type;
                    arrayIndex = (arrayIndex + 1);
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IDependency dependencyItem = item.As<IDependency>();
                if (((dependencyItem != null) 
                            && this._parent.RoleBinding.Remove(dependencyItem)))
                {
                    return true;
                }
                if ((this._parent.Type == item))
                {
                    this._parent.Type = null;
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().Concat(this._parent.RoleBinding).Concat(this._parent.Type).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the type property
        /// </summary>
        private sealed class TypeProxy : ModelPropertyChange<ICollaborationUse, ICollaboration>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public TypeProxy(ICollaborationUse modelElement) : 
                    base(modelElement, "type")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override ICollaboration Value
            {
                get
                {
                    return this.ModelElement.Type;
                }
                set
                {
                    this.ModelElement.Type = value;
                }
            }
        }
    }
}

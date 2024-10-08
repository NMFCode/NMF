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
    /// A BehavioredClassifier may have InterfaceRealizations, and owns a set of Behaviors one of which may specify the behavior of the BehavioredClassifier itself.
    ///&lt;p&gt;From package UML::SimpleClassifiers.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//BehavioredClassifier")]
    [DebuggerDisplayAttribute("BehavioredClassifier {Name}")]
    public abstract partial class BehavioredClassifier : Classifier, IBehavioredClassifier, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _class_behaviorOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveClass_behaviorOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getAllImplementedInterfacesOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetAllImplementedInterfacesOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getImplementedInterfacesOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetImplementedInterfacesOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _classifierBehaviorReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveClassifierBehaviorReference);
        
        /// <summary>
        /// The backing field for the ClassifierBehavior property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IBehavior _classifierBehavior;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _interfaceRealizationReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveInterfaceRealizationReference);
        
        /// <summary>
        /// The backing field for the InterfaceRealization property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private BehavioredClassifierInterfaceRealizationCollection _interfaceRealization;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public BehavioredClassifier()
        {
            this._interfaceRealization = new BehavioredClassifierInterfaceRealizationCollection(this);
            this._interfaceRealization.CollectionChanging += this.InterfaceRealizationCollectionChanging;
            this._interfaceRealization.CollectionChanged += this.InterfaceRealizationCollectionChanged;
        }
        
        /// <summary>
        /// A Behavior that specifies the behavior of the BehavioredClassifier itself.
        ///&lt;p&gt;From package UML::SimpleClassifiers.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("classifierBehavior")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IBehavior ClassifierBehavior
        {
            get
            {
                return this._classifierBehavior;
            }
            set
            {
                if ((this._classifierBehavior != value))
                {
                    IBehavior old = this._classifierBehavior;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("ClassifierBehavior", e, _classifierBehaviorReference);
                    this._classifierBehavior = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetClassifierBehavior;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetClassifierBehavior;
                    }
                    this.OnPropertyChanged("ClassifierBehavior", e, _classifierBehaviorReference);
                }
            }
        }
        
        /// <summary>
        /// The set of InterfaceRealizations owned by the BehavioredClassifier. Interface realizations reference the Interfaces of which the BehavioredClassifier is an implementation.
        ///&lt;p&gt;From package UML::SimpleClassifiers.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("interfaceRealization")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("implementingClassifier")]
        [ConstantAttribute()]
        public IOrderedSetExpression<IInterfaceRealization> InterfaceRealization
        {
            get
            {
                return this._interfaceRealization;
            }
        }
        
        IListExpression<IBehavior> IBehavioredClassifier.OwnedBehavior
        {
            get
            {
                return new BehavioredClassifierOwnedBehaviorCollection(this);
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new BehavioredClassifierChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new BehavioredClassifierReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//BehavioredClassifier")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If a behavior is classifier behavior, it does not have a specification.
        ///classifierBehavior-&gt;notEmpty() implies classifierBehavior.specification-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Class_behavior(object diagnostics, object context)
        {
            System.Func<IBehavioredClassifier, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IBehavioredClassifier, object, object, bool>>(_class_behaviorOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method class_behavior registered. Use the method b" +
                        "roker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _class_behaviorOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _class_behaviorOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _class_behaviorOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveClass_behaviorOperation()
        {
            return ClassInstance.LookupOperation("class_behavior");
        }
        
        /// <summary>
        /// Retrieves all the interfaces on which this behaviored classifier or any of its parents has an interface realization dependency.
        /// </summary>
        public ISetExpression<IInterface> GetAllImplementedInterfaces()
        {
            System.Func<IBehavioredClassifier, ISetExpression<IInterface>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IBehavioredClassifier, ISetExpression<IInterface>>>(_getAllImplementedInterfacesOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getAllImplementedInterfaces registered. Use" +
                        " the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getAllImplementedInterfacesOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getAllImplementedInterfacesOperation.Value, e));
            ISetExpression<IInterface> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getAllImplementedInterfacesOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetAllImplementedInterfacesOperation()
        {
            return ClassInstance.LookupOperation("getAllImplementedInterfaces");
        }
        
        /// <summary>
        /// Retrieves the interfaces on which this behaviored classifier has an interface realization dependency.
        /// </summary>
        public ISetExpression<IInterface> GetImplementedInterfaces()
        {
            System.Func<IBehavioredClassifier, ISetExpression<IInterface>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IBehavioredClassifier, ISetExpression<IInterface>>>(_getImplementedInterfacesOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getImplementedInterfaces registered. Use th" +
                        "e method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getImplementedInterfacesOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getImplementedInterfacesOperation.Value, e));
            ISetExpression<IInterface> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getImplementedInterfacesOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetImplementedInterfacesOperation()
        {
            return ClassInstance.LookupOperation("getImplementedInterfaces");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveClassifierBehaviorReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.BehavioredClassifier.ClassInstance)).Resolve("classifierBehavior")));
        }
        
        /// <summary>
        /// Handles the event that the ClassifierBehavior property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetClassifierBehavior(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.ClassifierBehavior))
            {
                this.ClassifierBehavior = null;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveInterfaceRealizationReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.BehavioredClassifier.ClassInstance)).Resolve("interfaceRealization")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the InterfaceRealization property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void InterfaceRealizationCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("InterfaceRealization", e, _interfaceRealizationReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the InterfaceRealization property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void InterfaceRealizationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("InterfaceRealization", e, _interfaceRealizationReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.ClassifierBehavior))
            {
                return ModelHelper.CreatePath("classifierBehavior");
            }
            int interfaceRealizationIndex = ModelHelper.IndexOfReference(this.InterfaceRealization, element);
            if ((interfaceRealizationIndex != -1))
            {
                return ModelHelper.CreatePath("interfaceRealization", interfaceRealizationIndex);
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
            if ((reference == "CLASSIFIERBEHAVIOR"))
            {
                return this.ClassifierBehavior;
            }
            if ((reference == "INTERFACEREALIZATION"))
            {
                if ((index < this.InterfaceRealization.Count))
                {
                    return this.InterfaceRealization[index];
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
            if ((feature == "INTERFACEREALIZATION"))
            {
                return this._interfaceRealization;
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
            if ((feature == "CLASSIFIERBEHAVIOR"))
            {
                this.ClassifierBehavior = ((IBehavior)(value));
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
            if ((reference == "CLASSIFIERBEHAVIOR"))
            {
                return new ClassifierBehaviorProxy(this);
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
            if ((container == this._interfaceRealization))
            {
                return "interfaceRealization";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//BehavioredClassifier")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the BehavioredClassifier class
        /// </summary>
        public class BehavioredClassifierChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private BehavioredClassifier _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public BehavioredClassifierChildrenCollection(BehavioredClassifier parent)
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
        /// The collection class to to represent the children of the BehavioredClassifier class
        /// </summary>
        public class BehavioredClassifierReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private BehavioredClassifier _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public BehavioredClassifierReferencedElementsCollection(BehavioredClassifier parent)
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
        /// Represents a proxy to represent an incremental access to the classifierBehavior property
        /// </summary>
        private sealed class ClassifierBehaviorProxy : ModelPropertyChange<IBehavioredClassifier, IBehavior>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ClassifierBehaviorProxy(IBehavioredClassifier modelElement) : 
                    base(modelElement, "classifierBehavior")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IBehavior Value
            {
                get
                {
                    return this.ModelElement.ClassifierBehavior;
                }
                set
                {
                    this.ModelElement.ClassifierBehavior = value;
                }
            }
        }
    }
}

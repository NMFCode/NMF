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
    /// Realization is specialized to (optionally) define the Classifiers that realize the contract offered by a Component in terms of its provided and required Interfaces. The Component forms an abstraction from these various Classifiers.
    ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ComponentRealization")]
    [DebuggerDisplayAttribute("ComponentRealization {Name}")]
    public partial class ComponentRealization : PackageableElement, IComponentRealization, IModelElement
    {
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _realizingClassifierReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveRealizingClassifierReference);
        
        /// <summary>
        /// The backing field for the RealizingClassifier property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationSet<IClassifier> _realizingClassifier;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _abstractionReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveAbstractionReference);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _mappingReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveMappingReference);
        
        /// <summary>
        /// The backing field for the Mapping property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IOpaqueExpression _mapping;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ComponentRealization()
        {
            this._realizingClassifier = new ObservableAssociationSet<IClassifier>();
            this._realizingClassifier.CollectionChanging += this.RealizingClassifierCollectionChanging;
            this._realizingClassifier.CollectionChanged += this.RealizingClassifierCollectionChanged;
        }
        
        /// <summary>
        /// The Classifiers that are involved in the implementation of the Component that owns this Realization.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("realizingClassifier")]
        [DescriptionAttribute("The Classifiers that are involved in the implementation of the Component that own" +
            "s this Realization.\n<p>From package UML::StructuredClassifiers.</p>")]
        [CategoryAttribute("ComponentRealization")]
        [XmlElementNameAttribute("realizingClassifier")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public ISetExpression<IClassifier> RealizingClassifier
        {
            get
            {
                return this._realizingClassifier;
            }
        }
        
        /// <summary>
        /// The Component that owns this ComponentRealization and which is implemented by its realizing Classifiers.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("abstraction")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("realization")]
        public NMF.Interop.Uml.IComponent Abstraction
        {
            get
            {
                return ModelHelper.CastAs<NMF.Interop.Uml.IComponent>(this.Parent);
            }
            set
            {
                this.Parent = value;
            }
        }
        
        ICollectionExpression<INamedElement> IDependency.Client
        {
            get
            {
                return new ComponentRealizationClientCollection(this);
            }
        }
        
        ICollectionExpression<INamedElement> IDependency.Supplier
        {
            get
            {
                return new ComponentRealizationSupplierCollection(this);
            }
        }
        
        /// <summary>
        /// An OpaqueExpression that states the abstraction relationship between the supplier(s) and the client(s). In some cases, such as derivation, it is usually formal and unidirectional; in other cases, such as trace, it is usually informal and bidirectional. The mapping expression is optional and may be omitted if the precise relationship between the Elements is not specified.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("mapping")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IOpaqueExpression Mapping
        {
            get
            {
                return this._mapping;
            }
            set
            {
                if ((this._mapping != value))
                {
                    IOpaqueExpression old = this._mapping;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Mapping", e, _mappingReference);
                    this._mapping = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetMapping;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetMapping;
                    }
                    this.OnPropertyChanged("Mapping", e, _mappingReference);
                }
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ComponentRealizationReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ComponentRealization")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveRealizingClassifierReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ComponentRealization.ClassInstance)).Resolve("realizingClassifier")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the RealizingClassifier property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void RealizingClassifierCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("RealizingClassifier", e, _realizingClassifierReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the RealizingClassifier property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void RealizingClassifierCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("RealizingClassifier", e, _realizingClassifierReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveAbstractionReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ComponentRealization.ClassInstance)).Resolve("abstraction")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            NMF.Interop.Uml.IComponent oldAbstraction = ModelHelper.CastAs<NMF.Interop.Uml.IComponent>(oldParent);
            NMF.Interop.Uml.IComponent newAbstraction = ModelHelper.CastAs<NMF.Interop.Uml.IComponent>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldAbstraction, newAbstraction);
            this.OnPropertyChanging("Abstraction", e, _abstractionReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            NMF.Interop.Uml.IComponent oldAbstraction = ModelHelper.CastAs<NMF.Interop.Uml.IComponent>(oldParent);
            NMF.Interop.Uml.IComponent newAbstraction = ModelHelper.CastAs<NMF.Interop.Uml.IComponent>(newParent);
            if ((oldAbstraction != null))
            {
                oldAbstraction.Realization.Remove(this);
            }
            if ((newAbstraction != null))
            {
                newAbstraction.Realization.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldAbstraction, newAbstraction);
            this.OnPropertyChanged("Abstraction", e, _abstractionReference);
            base.OnParentChanged(newParent, oldParent);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveMappingReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Abstraction.ClassInstance)).Resolve("mapping")));
        }
        
        /// <summary>
        /// Handles the event that the Mapping property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetMapping(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Mapping))
            {
                this.Mapping = null;
            }
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Mapping))
            {
                return ModelHelper.CreatePath("mapping");
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
            if ((reference == "ABSTRACTION"))
            {
                return this.Abstraction;
            }
            if ((reference == "MAPPING"))
            {
                return this.Mapping;
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
            if ((feature == "REALIZINGCLASSIFIER"))
            {
                return this._realizingClassifier;
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
            if ((feature == "ABSTRACTION"))
            {
                this.Abstraction = ((NMF.Interop.Uml.IComponent)(value));
                return;
            }
            if ((feature == "MAPPING"))
            {
                this.Mapping = ((IOpaqueExpression)(value));
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
            if ((reference == "ABSTRACTION"))
            {
                return new AbstractionProxy(this);
            }
            if ((reference == "MAPPING"))
            {
                return new MappingProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ComponentRealization")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ComponentRealization class
        /// </summary>
        public class ComponentRealizationReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ComponentRealization _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ComponentRealizationReferencedElementsCollection(ComponentRealization parent)
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
        /// Represents a proxy to represent an incremental access to the abstraction property
        /// </summary>
        private sealed class AbstractionProxy : ModelPropertyChange<IComponentRealization, NMF.Interop.Uml.IComponent>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public AbstractionProxy(IComponentRealization modelElement) : 
                    base(modelElement, "abstraction")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override NMF.Interop.Uml.IComponent Value
            {
                get
                {
                    return this.ModelElement.Abstraction;
                }
                set
                {
                    this.ModelElement.Abstraction = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the mapping property
        /// </summary>
        private sealed class MappingProxy : ModelPropertyChange<IAbstraction, IOpaqueExpression>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public MappingProxy(IAbstraction modelElement) : 
                    base(modelElement, "mapping")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IOpaqueExpression Value
            {
                get
                {
                    return this.ModelElement.Mapping;
                }
                set
                {
                    this.ModelElement.Mapping = value;
                }
            }
        }
    }
}

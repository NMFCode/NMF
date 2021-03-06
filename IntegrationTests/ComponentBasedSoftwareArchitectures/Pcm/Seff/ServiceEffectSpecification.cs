//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.Identifier;
using NMFExamples.Pcm.Core;
using NMFExamples.Pcm.Core.Entity;
using NMFExamples.Pcm.Parameter;
using NMFExamples.Pcm.Reliability;
using NMFExamples.Pcm.Repository;
using NMFExamples.Pcm.Seff.Seff_performance;
using NMFExamples.Pcm.Seff.Seff_reliability;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using global::System.Collections;
using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using global::System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMFExamples.Pcm.Seff
{
    
    
    /// <summary>
    /// The default implementation of the ServiceEffectSpecification class
    /// </summary>
    [XmlNamespaceAttribute("http://sdq.ipd.uka.de/PalladioComponentModel/SEFF/5.0")]
    [XmlNamespacePrefixAttribute("seff")]
    [ModelRepresentationClassAttribute("http://sdq.ipd.uka.de/PalladioComponentModel/5.0#//seff/ServiceEffectSpecificatio" +
        "n")]
    public abstract partial class ServiceEffectSpecification : ModelElement, IServiceEffectSpecification, IModelElement
    {
        
        /// <summary>
        /// The backing field for the SeffTypeID property
        /// </summary>
        private string _seffTypeID = "1";
        
        private static Lazy<ITypedElement> _seffTypeIDAttribute = new Lazy<ITypedElement>(RetrieveSeffTypeIDAttribute);
        
        private static Lazy<ITypedElement> _describedService__SEFFReference = new Lazy<ITypedElement>(RetrieveDescribedService__SEFFReference);
        
        /// <summary>
        /// The backing field for the DescribedService__SEFF property
        /// </summary>
        private ISignature _describedService__SEFF;
        
        private static Lazy<ITypedElement> _basicComponent_ServiceEffectSpecificationReference = new Lazy<ITypedElement>(RetrieveBasicComponent_ServiceEffectSpecificationReference);
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The seffTypeID property
        /// </summary>
        [DefaultValueAttribute("1")]
        [XmlElementNameAttribute("seffTypeID")]
        [XmlAttributeAttribute(true)]
        public string SeffTypeID
        {
            get
            {
                return this._seffTypeID;
            }
            set
            {
                if ((this._seffTypeID != value))
                {
                    string old = this._seffTypeID;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnSeffTypeIDChanging(e);
                    this.OnPropertyChanging("SeffTypeID", e, _seffTypeIDAttribute);
                    this._seffTypeID = value;
                    this.OnSeffTypeIDChanged(e);
                    this.OnPropertyChanged("SeffTypeID", e, _seffTypeIDAttribute);
                }
            }
        }
        
        /// <summary>
        /// The describedService__SEFF property
        /// </summary>
        [XmlElementNameAttribute("describedService__SEFF")]
        [XmlAttributeAttribute(true)]
        public ISignature DescribedService__SEFF
        {
            get
            {
                return this._describedService__SEFF;
            }
            set
            {
                if ((this._describedService__SEFF != value))
                {
                    ISignature old = this._describedService__SEFF;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnDescribedService__SEFFChanging(e);
                    this.OnPropertyChanging("DescribedService__SEFF", e, _describedService__SEFFReference);
                    this._describedService__SEFF = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetDescribedService__SEFF;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetDescribedService__SEFF;
                    }
                    this.OnDescribedService__SEFFChanged(e);
                    this.OnPropertyChanged("DescribedService__SEFF", e, _describedService__SEFFReference);
                }
            }
        }
        
        /// <summary>
        /// The basicComponent_ServiceEffectSpecification property
        /// </summary>
        [XmlElementNameAttribute("basicComponent_ServiceEffectSpecification")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("serviceEffectSpecifications__BasicComponent")]
        public IBasicComponent BasicComponent_ServiceEffectSpecification
        {
            get
            {
                return ModelHelper.CastAs<IBasicComponent>(this.Parent);
            }
            set
            {
                this.Parent = value;
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ServiceEffectSpecificationReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.uka.de/PalladioComponentModel/5.0#//seff/ServiceEffectSpecificatio" +
                            "n")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the SeffTypeID property changes its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> SeffTypeIDChanging;
        
        /// <summary>
        /// Gets fired when the SeffTypeID property changed its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> SeffTypeIDChanged;
        
        /// <summary>
        /// Gets fired before the DescribedService__SEFF property changes its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> DescribedService__SEFFChanging;
        
        /// <summary>
        /// Gets fired when the DescribedService__SEFF property changed its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> DescribedService__SEFFChanged;
        
        /// <summary>
        /// Gets fired before the BasicComponent_ServiceEffectSpecification property changes its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> BasicComponent_ServiceEffectSpecificationChanging;
        
        /// <summary>
        /// Gets fired when the BasicComponent_ServiceEffectSpecification property changed its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> BasicComponent_ServiceEffectSpecificationChanged;
        
        private static ITypedElement RetrieveSeffTypeIDAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMFExamples.Pcm.Seff.ServiceEffectSpecification.ClassInstance)).Resolve("seffTypeID")));
        }
        
        /// <summary>
        /// Raises the SeffTypeIDChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSeffTypeIDChanging(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.SeffTypeIDChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the SeffTypeIDChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSeffTypeIDChanged(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.SeffTypeIDChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        private static ITypedElement RetrieveDescribedService__SEFFReference()
        {
            return ((ITypedElement)(((ModelElement)(NMFExamples.Pcm.Seff.ServiceEffectSpecification.ClassInstance)).Resolve("describedService__SEFF")));
        }
        
        /// <summary>
        /// Raises the DescribedService__SEFFChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnDescribedService__SEFFChanging(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.DescribedService__SEFFChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the DescribedService__SEFFChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnDescribedService__SEFFChanged(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.DescribedService__SEFFChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the DescribedService__SEFF property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetDescribedService__SEFF(object sender, global::System.EventArgs eventArgs)
        {
            this.DescribedService__SEFF = null;
        }
        
        private static ITypedElement RetrieveBasicComponent_ServiceEffectSpecificationReference()
        {
            return ((ITypedElement)(((ModelElement)(NMFExamples.Pcm.Seff.ServiceEffectSpecification.ClassInstance)).Resolve("basicComponent_ServiceEffectSpecification")));
        }
        
        /// <summary>
        /// Raises the BasicComponent_ServiceEffectSpecificationChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnBasicComponent_ServiceEffectSpecificationChanging(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.BasicComponent_ServiceEffectSpecificationChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            IBasicComponent oldBasicComponent_ServiceEffectSpecification = ModelHelper.CastAs<IBasicComponent>(oldParent);
            IBasicComponent newBasicComponent_ServiceEffectSpecification = ModelHelper.CastAs<IBasicComponent>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldBasicComponent_ServiceEffectSpecification, newBasicComponent_ServiceEffectSpecification);
            this.OnBasicComponent_ServiceEffectSpecificationChanging(e);
            this.OnPropertyChanging("BasicComponent_ServiceEffectSpecification", e, _basicComponent_ServiceEffectSpecificationReference);
        }
        
        /// <summary>
        /// Raises the BasicComponent_ServiceEffectSpecificationChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnBasicComponent_ServiceEffectSpecificationChanged(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.BasicComponent_ServiceEffectSpecificationChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            IBasicComponent oldBasicComponent_ServiceEffectSpecification = ModelHelper.CastAs<IBasicComponent>(oldParent);
            IBasicComponent newBasicComponent_ServiceEffectSpecification = ModelHelper.CastAs<IBasicComponent>(newParent);
            if ((oldBasicComponent_ServiceEffectSpecification != null))
            {
                oldBasicComponent_ServiceEffectSpecification.ServiceEffectSpecifications__BasicComponent.Remove(this);
            }
            if ((newBasicComponent_ServiceEffectSpecification != null))
            {
                newBasicComponent_ServiceEffectSpecification.ServiceEffectSpecifications__BasicComponent.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldBasicComponent_ServiceEffectSpecification, newBasicComponent_ServiceEffectSpecification);
            this.OnBasicComponent_ServiceEffectSpecificationChanged(e);
            this.OnPropertyChanged("BasicComponent_ServiceEffectSpecification", e, _basicComponent_ServiceEffectSpecificationReference);
            base.OnParentChanged(newParent, oldParent);
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "SEFFTYPEID"))
            {
                return this.SeffTypeID;
            }
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "DESCRIBEDSERVICE__SEFF"))
            {
                this.DescribedService__SEFF = ((ISignature)(value));
                return;
            }
            if ((feature == "BASICCOMPONENT_SERVICEEFFECTSPECIFICATION"))
            {
                this.BasicComponent_ServiceEffectSpecification = ((IBasicComponent)(value));
                return;
            }
            if ((feature == "SEFFTYPEID"))
            {
                this.SeffTypeID = ((string)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given attribute
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="attribute">The requested attribute in upper case</param>
        protected override NMF.Expressions.INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            if ((attribute == "DescribedService__SEFF"))
            {
                return new DescribedService__SEFFProxy(this);
            }
            if ((attribute == "BasicComponent_ServiceEffectSpecification"))
            {
                return new BasicComponent_ServiceEffectSpecificationProxy(this);
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the property expression for the given reference
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="reference">The requested reference in upper case</param>
        protected override NMF.Expressions.INotifyExpression<NMF.Models.IModelElement> GetExpressionForReference(string reference)
        {
            if ((reference == "DescribedService__SEFF"))
            {
                return new DescribedService__SEFFProxy(this);
            }
            if ((reference == "BasicComponent_ServiceEffectSpecification"))
            {
                return new BasicComponent_ServiceEffectSpecificationProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.uka.de/PalladioComponentModel/5.0#//seff/ServiceEffectSpecificatio" +
                        "n")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ServiceEffectSpecification class
        /// </summary>
        public class ServiceEffectSpecificationReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ServiceEffectSpecification _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ServiceEffectSpecificationReferencedElementsCollection(ServiceEffectSpecification parent)
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
                    if ((this._parent.DescribedService__SEFF != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.BasicComponent_ServiceEffectSpecification != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.DescribedService__SEFFChanged += this.PropagateValueChanges;
                this._parent.BasicComponent_ServiceEffectSpecificationChanged += this.PropagateValueChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.DescribedService__SEFFChanged -= this.PropagateValueChanges;
                this._parent.BasicComponent_ServiceEffectSpecificationChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.DescribedService__SEFF == null))
                {
                    ISignature describedService__SEFFCasted = item.As<ISignature>();
                    if ((describedService__SEFFCasted != null))
                    {
                        this._parent.DescribedService__SEFF = describedService__SEFFCasted;
                        return;
                    }
                }
                if ((this._parent.BasicComponent_ServiceEffectSpecification == null))
                {
                    IBasicComponent basicComponent_ServiceEffectSpecificationCasted = item.As<IBasicComponent>();
                    if ((basicComponent_ServiceEffectSpecificationCasted != null))
                    {
                        this._parent.BasicComponent_ServiceEffectSpecification = basicComponent_ServiceEffectSpecificationCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.DescribedService__SEFF = null;
                this._parent.BasicComponent_ServiceEffectSpecification = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.DescribedService__SEFF))
                {
                    return true;
                }
                if ((item == this._parent.BasicComponent_ServiceEffectSpecification))
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
                if ((this._parent.DescribedService__SEFF != null))
                {
                    array[arrayIndex] = this._parent.DescribedService__SEFF;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.BasicComponent_ServiceEffectSpecification != null))
                {
                    array[arrayIndex] = this._parent.BasicComponent_ServiceEffectSpecification;
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
                if ((this._parent.DescribedService__SEFF == item))
                {
                    this._parent.DescribedService__SEFF = null;
                    return true;
                }
                if ((this._parent.BasicComponent_ServiceEffectSpecification == item))
                {
                    this._parent.BasicComponent_ServiceEffectSpecification = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.DescribedService__SEFF).Concat(this._parent.BasicComponent_ServiceEffectSpecification).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the seffTypeID property
        /// </summary>
        private sealed class SeffTypeIDProxy : ModelPropertyChange<IServiceEffectSpecification, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SeffTypeIDProxy(IServiceEffectSpecification modelElement) : 
                    base(modelElement, "seffTypeID")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.SeffTypeID;
                }
                set
                {
                    this.ModelElement.SeffTypeID = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the describedService__SEFF property
        /// </summary>
        private sealed class DescribedService__SEFFProxy : ModelPropertyChange<IServiceEffectSpecification, ISignature>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public DescribedService__SEFFProxy(IServiceEffectSpecification modelElement) : 
                    base(modelElement, "describedService__SEFF")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override ISignature Value
            {
                get
                {
                    return this.ModelElement.DescribedService__SEFF;
                }
                set
                {
                    this.ModelElement.DescribedService__SEFF = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the basicComponent_ServiceEffectSpecification property
        /// </summary>
        private sealed class BasicComponent_ServiceEffectSpecificationProxy : ModelPropertyChange<IServiceEffectSpecification, IBasicComponent>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public BasicComponent_ServiceEffectSpecificationProxy(IServiceEffectSpecification modelElement) : 
                    base(modelElement, "basicComponent_ServiceEffectSpecification")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IBasicComponent Value
            {
                get
                {
                    return this.ModelElement.BasicComponent_ServiceEffectSpecification;
                }
                set
                {
                    this.ModelElement.BasicComponent_ServiceEffectSpecification = value;
                }
            }
        }
    }
}


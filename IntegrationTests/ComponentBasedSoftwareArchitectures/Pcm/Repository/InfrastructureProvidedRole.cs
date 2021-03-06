//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.Pcm.Core;
using NMFExamples.Pcm.Core.Entity;
using NMFExamples.Pcm.Parameter;
using NMFExamples.Pcm.Protocol;
using NMFExamples.Pcm.Reliability;
using NMFExamples.Pcm.Resourcetype;
using NMFExamples.Pcm.Seff;
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
using System.Globalization;
using System.Linq;

namespace NMFExamples.Pcm.Repository
{
    
    
    /// <summary>
    /// The default implementation of the InfrastructureProvidedRole class
    /// </summary>
    [XmlNamespaceAttribute("http://sdq.ipd.uka.de/PalladioComponentModel/Repository/5.0")]
    [XmlNamespacePrefixAttribute("repository")]
    [ModelRepresentationClassAttribute("http://sdq.ipd.uka.de/PalladioComponentModel/5.0#//repository/InfrastructureProvi" +
        "dedRole")]
    [DebuggerDisplayAttribute("InfrastructureProvidedRole {Id}")]
    public partial class InfrastructureProvidedRole : ProvidedRole, IInfrastructureProvidedRole, IModelElement
    {
        
        private static Lazy<ITypedElement> _providedInterface__InfrastructureProvidedRoleReference = new Lazy<ITypedElement>(RetrieveProvidedInterface__InfrastructureProvidedRoleReference);
        
        /// <summary>
        /// The backing field for the ProvidedInterface__InfrastructureProvidedRole property
        /// </summary>
        private IInfrastructureInterface _providedInterface__InfrastructureProvidedRole;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The providedInterface__InfrastructureProvidedRole property
        /// </summary>
        [XmlElementNameAttribute("providedInterface__InfrastructureProvidedRole")]
        [XmlAttributeAttribute(true)]
        public IInfrastructureInterface ProvidedInterface__InfrastructureProvidedRole
        {
            get
            {
                return this._providedInterface__InfrastructureProvidedRole;
            }
            set
            {
                if ((this._providedInterface__InfrastructureProvidedRole != value))
                {
                    IInfrastructureInterface old = this._providedInterface__InfrastructureProvidedRole;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnProvidedInterface__InfrastructureProvidedRoleChanging(e);
                    this.OnPropertyChanging("ProvidedInterface__InfrastructureProvidedRole", e, _providedInterface__InfrastructureProvidedRoleReference);
                    this._providedInterface__InfrastructureProvidedRole = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetProvidedInterface__InfrastructureProvidedRole;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetProvidedInterface__InfrastructureProvidedRole;
                    }
                    this.OnProvidedInterface__InfrastructureProvidedRoleChanged(e);
                    this.OnPropertyChanged("ProvidedInterface__InfrastructureProvidedRole", e, _providedInterface__InfrastructureProvidedRoleReference);
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
                return base.ReferencedElements.Concat(new InfrastructureProvidedRoleReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.uka.de/PalladioComponentModel/5.0#//repository/InfrastructureProvi" +
                            "dedRole")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the ProvidedInterface__InfrastructureProvidedRole property changes its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> ProvidedInterface__InfrastructureProvidedRoleChanging;
        
        /// <summary>
        /// Gets fired when the ProvidedInterface__InfrastructureProvidedRole property changed its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> ProvidedInterface__InfrastructureProvidedRoleChanged;
        
        private static ITypedElement RetrieveProvidedInterface__InfrastructureProvidedRoleReference()
        {
            return ((ITypedElement)(((ModelElement)(NMFExamples.Pcm.Repository.InfrastructureProvidedRole.ClassInstance)).Resolve("providedInterface__InfrastructureProvidedRole")));
        }
        
        /// <summary>
        /// Raises the ProvidedInterface__InfrastructureProvidedRoleChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnProvidedInterface__InfrastructureProvidedRoleChanging(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.ProvidedInterface__InfrastructureProvidedRoleChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the ProvidedInterface__InfrastructureProvidedRoleChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnProvidedInterface__InfrastructureProvidedRoleChanged(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.ProvidedInterface__InfrastructureProvidedRoleChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the ProvidedInterface__InfrastructureProvidedRole property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetProvidedInterface__InfrastructureProvidedRole(object sender, global::System.EventArgs eventArgs)
        {
            this.ProvidedInterface__InfrastructureProvidedRole = null;
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "PROVIDEDINTERFACE__INFRASTRUCTUREPROVIDEDROLE"))
            {
                this.ProvidedInterface__InfrastructureProvidedRole = ((IInfrastructureInterface)(value));
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
            if ((attribute == "ProvidedInterface__InfrastructureProvidedRole"))
            {
                return new ProvidedInterface__InfrastructureProvidedRoleProxy(this);
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
            if ((reference == "ProvidedInterface__InfrastructureProvidedRole"))
            {
                return new ProvidedInterface__InfrastructureProvidedRoleProxy(this);
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
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.uka.de/PalladioComponentModel/5.0#//repository/InfrastructureProvi" +
                        "dedRole")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the InfrastructureProvidedRole class
        /// </summary>
        public class InfrastructureProvidedRoleReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private InfrastructureProvidedRole _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public InfrastructureProvidedRoleReferencedElementsCollection(InfrastructureProvidedRole parent)
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
                    if ((this._parent.ProvidedInterface__InfrastructureProvidedRole != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.ProvidedInterface__InfrastructureProvidedRoleChanged += this.PropagateValueChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.ProvidedInterface__InfrastructureProvidedRoleChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.ProvidedInterface__InfrastructureProvidedRole == null))
                {
                    IInfrastructureInterface providedInterface__InfrastructureProvidedRoleCasted = item.As<IInfrastructureInterface>();
                    if ((providedInterface__InfrastructureProvidedRoleCasted != null))
                    {
                        this._parent.ProvidedInterface__InfrastructureProvidedRole = providedInterface__InfrastructureProvidedRoleCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.ProvidedInterface__InfrastructureProvidedRole = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.ProvidedInterface__InfrastructureProvidedRole))
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
                if ((this._parent.ProvidedInterface__InfrastructureProvidedRole != null))
                {
                    array[arrayIndex] = this._parent.ProvidedInterface__InfrastructureProvidedRole;
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
                if ((this._parent.ProvidedInterface__InfrastructureProvidedRole == item))
                {
                    this._parent.ProvidedInterface__InfrastructureProvidedRole = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.ProvidedInterface__InfrastructureProvidedRole).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the providedInterface__InfrastructureProvidedRole property
        /// </summary>
        private sealed class ProvidedInterface__InfrastructureProvidedRoleProxy : ModelPropertyChange<IInfrastructureProvidedRole, IInfrastructureInterface>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ProvidedInterface__InfrastructureProvidedRoleProxy(IInfrastructureProvidedRole modelElement) : 
                    base(modelElement, "providedInterface__InfrastructureProvidedRole")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IInfrastructureInterface Value
            {
                get
                {
                    return this.ModelElement.ProvidedInterface__InfrastructureProvidedRole;
                }
                set
                {
                    this.ModelElement.ProvidedInterface__InfrastructureProvidedRole = value;
                }
            }
        }
    }
}


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
    /// A substitution is a relationship between two classifiers signifying that the substituting classifier complies with the contract specified by the contract classifier. This implies that instances of the substituting classifier are runtime substitutable where instances of the contract classifier are expected.
    ///<p>From package UML::Classification.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Substitution")]
    [DebuggerDisplayAttribute("Substitution {Name}")]
    public partial class Substitution : Realization, ISubstitution, IModelElement
    {
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _contractReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveContractReference);
        
        /// <summary>
        /// The backing field for the Contract property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IClassifier _contract;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _substitutingClassifierReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveSubstitutingClassifierReference);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The contract with which the substituting classifier complies.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [DisplayNameAttribute("contract")]
        [DescriptionAttribute("The contract with which the substituting classifier complies.\n<p>From package UML" +
            "::Classification.</p>")]
        [CategoryAttribute("Substitution")]
        [XmlElementNameAttribute("contract")]
        [XmlAttributeAttribute(true)]
        public IClassifier Contract
        {
            get
            {
                return this._contract;
            }
            set
            {
                if ((this._contract != value))
                {
                    IClassifier old = this._contract;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Contract", e, _contractReference);
                    this._contract = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetContract;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetContract;
                    }
                    this.OnPropertyChanged("Contract", e, _contractReference);
                }
            }
        }
        
        /// <summary>
        /// Instances of the substituting classifier are runtime substitutable where instances of the contract classifier are expected.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("substitutingClassifier")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("substitution")]
        public IClassifier SubstitutingClassifier
        {
            get
            {
                return ModelHelper.CastAs<IClassifier>(this.Parent);
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
                return base.ReferencedElements.Concat(new SubstitutionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Substitution")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveContractReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Substitution.ClassInstance)).Resolve("contract")));
        }
        
        /// <summary>
        /// Handles the event that the Contract property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetContract(object sender, System.EventArgs eventArgs)
        {
            this.Contract = null;
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveSubstitutingClassifierReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Substitution.ClassInstance)).Resolve("substitutingClassifier")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            IClassifier oldSubstitutingClassifier = ModelHelper.CastAs<IClassifier>(oldParent);
            IClassifier newSubstitutingClassifier = ModelHelper.CastAs<IClassifier>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldSubstitutingClassifier, newSubstitutingClassifier);
            this.OnPropertyChanging("SubstitutingClassifier", e, _substitutingClassifierReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            IClassifier oldSubstitutingClassifier = ModelHelper.CastAs<IClassifier>(oldParent);
            IClassifier newSubstitutingClassifier = ModelHelper.CastAs<IClassifier>(newParent);
            if ((oldSubstitutingClassifier != null))
            {
                oldSubstitutingClassifier.Substitution.Remove(this);
            }
            if ((newSubstitutingClassifier != null))
            {
                newSubstitutingClassifier.Substitution.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldSubstitutingClassifier, newSubstitutingClassifier);
            this.OnPropertyChanged("SubstitutingClassifier", e, _substitutingClassifierReference);
            base.OnParentChanged(newParent, oldParent);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "CONTRACT"))
            {
                return this.Contract;
            }
            if ((reference == "SUBSTITUTINGCLASSIFIER"))
            {
                return this.SubstitutingClassifier;
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "CONTRACT"))
            {
                this.Contract = ((IClassifier)(value));
                return;
            }
            if ((feature == "SUBSTITUTINGCLASSIFIER"))
            {
                this.SubstitutingClassifier = ((IClassifier)(value));
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
            if ((reference == "CONTRACT"))
            {
                return new ContractProxy(this);
            }
            if ((reference == "SUBSTITUTINGCLASSIFIER"))
            {
                return new SubstitutingClassifierProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Substitution")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Substitution class
        /// </summary>
        public class SubstitutionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Substitution _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public SubstitutionReferencedElementsCollection(Substitution parent)
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
                    if ((this._parent.Contract != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.SubstitutingClassifier != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.Contract == null))
                {
                    IClassifier contractCasted = item.As<IClassifier>();
                    if ((contractCasted != null))
                    {
                        this._parent.Contract = contractCasted;
                        return;
                    }
                }
                if ((this._parent.SubstitutingClassifier == null))
                {
                    IClassifier substitutingClassifierCasted = item.As<IClassifier>();
                    if ((substitutingClassifierCasted != null))
                    {
                        this._parent.SubstitutingClassifier = substitutingClassifierCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Contract = null;
                this._parent.SubstitutingClassifier = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Contract))
                {
                    return true;
                }
                if ((item == this._parent.SubstitutingClassifier))
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
                if ((this._parent.Contract != null))
                {
                    array[arrayIndex] = this._parent.Contract;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.SubstitutingClassifier != null))
                {
                    array[arrayIndex] = this._parent.SubstitutingClassifier;
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
                if ((this._parent.Contract == item))
                {
                    this._parent.Contract = null;
                    return true;
                }
                if ((this._parent.SubstitutingClassifier == item))
                {
                    this._parent.SubstitutingClassifier = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Contract).Concat(this._parent.SubstitutingClassifier).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the contract property
        /// </summary>
        private sealed class ContractProxy : ModelPropertyChange<ISubstitution, IClassifier>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ContractProxy(ISubstitution modelElement) : 
                    base(modelElement, "contract")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IClassifier Value
            {
                get
                {
                    return this.ModelElement.Contract;
                }
                set
                {
                    this.ModelElement.Contract = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the substitutingClassifier property
        /// </summary>
        private sealed class SubstitutingClassifierProxy : ModelPropertyChange<ISubstitution, IClassifier>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SubstitutingClassifierProxy(ISubstitution modelElement) : 
                    base(modelElement, "substitutingClassifier")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IClassifier Value
            {
                get
                {
                    return this.ModelElement.SubstitutingClassifier;
                }
                set
                {
                    this.ModelElement.SubstitutingClassifier = value;
                }
            }
        }
    }
}

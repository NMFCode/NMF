//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;


namespace NMF.AnyText.Metamodel
{
    
    
    /// <summary>
    /// The default implementation of the InheritanceRule class
    /// </summary>
    [XmlNamespaceAttribute("https://github.com/NMFCode/NMF/AnyText")]
    [XmlNamespacePrefixAttribute("anytext")]
    [ModelRepresentationClassAttribute("https://github.com/NMFCode/NMF/AnyText#//InheritanceRule")]
    [DebuggerDisplayAttribute("InheritanceRule {Name}")]
    public partial class InheritanceRule : ClassRule, IInheritanceRule, IModelElement
    {
        
        private static Lazy<ITypedElement> _subtypesReference = new Lazy<ITypedElement>(RetrieveSubtypesReference);
        
        /// <summary>
        /// The backing field for the Subtypes property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationOrderedSet<IClassRule> _subtypes;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public InheritanceRule()
        {
            this._subtypes = new ObservableAssociationOrderedSet<IClassRule>();
            this._subtypes.CollectionChanging += this.SubtypesCollectionChanging;
            this._subtypes.CollectionChanged += this.SubtypesCollectionChanged;
        }
        
        /// <summary>
        /// The Subtypes property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [CategoryAttribute("InheritanceRule")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IOrderedSetExpression<IClassRule> Subtypes
        {
            get
            {
                return this._subtypes;
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new InheritanceRuleReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("https://github.com/NMFCode/NMF/AnyText#//InheritanceRule")));
                }
                return _classInstance;
            }
        }
        
        private static ITypedElement RetrieveSubtypesReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.AnyText.Metamodel.InheritanceRule.ClassInstance)).Resolve("Subtypes")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Subtypes property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void SubtypesCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Subtypes", e, _subtypesReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Subtypes property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void SubtypesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Subtypes", e, _subtypesReference);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "SUBTYPES"))
            {
                if ((index < this.Subtypes.Count))
                {
                    return this.Subtypes[index];
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
            if ((feature == "SUBTYPES"))
            {
                return this._subtypes;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("https://github.com/NMFCode/NMF/AnyText#//InheritanceRule")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the InheritanceRule class
        /// </summary>
        public class InheritanceRuleReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private InheritanceRule _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public InheritanceRuleReferencedElementsCollection(InheritanceRule parent)
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
                    count = (count + this._parent.Subtypes.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.Subtypes.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.Subtypes.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IClassRule subtypesCasted = item.As<IClassRule>();
                if ((subtypesCasted != null))
                {
                    this._parent.Subtypes.Add(subtypesCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Subtypes.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.Subtypes.Contains(item))
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
                IEnumerator<IModelElement> subtypesEnumerator = this._parent.Subtypes.GetEnumerator();
                try
                {
                    for (
                    ; subtypesEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = subtypesEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    subtypesEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IClassRule classRuleItem = item.As<IClassRule>();
                if (((classRuleItem != null) 
                            && this._parent.Subtypes.Remove(classRuleItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Subtypes).GetEnumerator();
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMF.Interop.Legacy.Cmof
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    using NMF.Models.Repository;
    using System.Globalization;
    
    
    /// <summary>
    /// A data type is a type whose instances are identified only by their value. A DataType may contain attributes to support the modeling of structured data types.
    ///
    ///
    ///
    ///A typical use of data types would be to represent programming language primitive types or CORBA basic types. For example, integer and string types are often treated as data types.
    ///DataType is an abstract class that acts as a common superclass for different kinds of data types.
    /// </summary>
    [XmlNamespaceAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//DataType")]
    [DebuggerDisplayAttribute("DataType {Name}")]
    public partial class DataType : Classifier, NMF.Interop.Legacy.Cmof.IDataType, IModelElement
    {
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedOperationReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedOperationReference);
        
        /// <summary>
        /// The backing field for the OwnedOperation property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private DataTypeOwnedOperationCollection _ownedOperation;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedAttributeReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedAttributeReference);
        
        /// <summary>
        /// The backing field for the OwnedAttribute property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private DataTypeOwnedAttributeCollection _ownedAttribute;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public DataType()
        {
            this._ownedOperation = new DataTypeOwnedOperationCollection(this);
            this._ownedOperation.CollectionChanging += this.OwnedOperationCollectionChanging;
            this._ownedOperation.CollectionChanged += this.OwnedOperationCollectionChanged;
            this._ownedAttribute = new DataTypeOwnedAttributeCollection(this);
            this._ownedAttribute.CollectionChanging += this.OwnedAttributeCollectionChanging;
            this._ownedAttribute.CollectionChanged += this.OwnedAttributeCollectionChanged;
        }
        
        /// <summary>
        /// The Operations owned by the DataType. Subsets Classifier::feature and Element::ownedMember.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedOperation")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("datatype")]
        [ConstantAttribute()]
        public IOrderedSetExpression<NMF.Interop.Legacy.Cmof.IOperation> OwnedOperation
        {
            get
            {
                return this._ownedOperation;
            }
        }
        
        /// <summary>
        /// The Attributes owned by the DataType. Subsets Classifier::attribute and Element::ownedMember.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedAttribute")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("datatype")]
        [ConstantAttribute()]
        public IOrderedSetExpression<IProperty> OwnedAttribute
        {
            get
            {
                return this._ownedAttribute;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new DataTypeChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new DataTypeReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//DataType")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedOperationReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.DataType.ClassInstance)).Resolve("ownedOperation")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedOperation property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedOperationCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedOperation", e, _ownedOperationReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedOperation property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedOperationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedOperation", e, _ownedOperationReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedAttributeReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.DataType.ClassInstance)).Resolve("ownedAttribute")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedAttribute property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedAttributeCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedAttribute", e, _ownedAttributeReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedAttribute property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedAttributeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedAttribute", e, _ownedAttributeReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int ownedOperationIndex = ModelHelper.IndexOfReference(this.OwnedOperation, element);
            if ((ownedOperationIndex != -1))
            {
                return ModelHelper.CreatePath("ownedOperation", ownedOperationIndex);
            }
            int ownedAttributeIndex = ModelHelper.IndexOfReference(this.OwnedAttribute, element);
            if ((ownedAttributeIndex != -1))
            {
                return ModelHelper.CreatePath("ownedAttribute", ownedAttributeIndex);
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
            if ((reference == "OWNEDOPERATION"))
            {
                if ((index < this.OwnedOperation.Count))
                {
                    return this.OwnedOperation[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "OWNEDATTRIBUTE"))
            {
                if ((index < this.OwnedAttribute.Count))
                {
                    return this.OwnedAttribute[index];
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
            if ((feature == "OWNEDOPERATION"))
            {
                return this._ownedOperation;
            }
            if ((feature == "OWNEDATTRIBUTE"))
            {
                return this._ownedAttribute;
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
            if ((container == this._ownedOperation))
            {
                return "ownedOperation";
            }
            if ((container == this._ownedAttribute))
            {
                return "ownedAttribute";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//DataType")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the DataType class
        /// </summary>
        public class DataTypeChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private DataType _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public DataTypeChildrenCollection(DataType parent)
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
                    count = (count + this._parent.OwnedOperation.Count);
                    count = (count + this._parent.OwnedAttribute.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                NMF.Interop.Legacy.Cmof.IOperation ownedOperationCasted = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if ((ownedOperationCasted != null))
                {
                    this._parent.OwnedOperation.Add(ownedOperationCasted);
                }
                IProperty ownedAttributeCasted = item.As<IProperty>();
                if ((ownedAttributeCasted != null))
                {
                    this._parent.OwnedAttribute.Add(ownedAttributeCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedOperation.Clear();
                this._parent.OwnedAttribute.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedOperation.Contains(item))
                {
                    return true;
                }
                if (this._parent.OwnedAttribute.Contains(item))
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
                IEnumerator<IModelElement> ownedOperationEnumerator = this._parent.OwnedOperation.GetEnumerator();
                try
                {
                    for (
                    ; ownedOperationEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedOperationEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedOperationEnumerator.Dispose();
                }
                IEnumerator<IModelElement> ownedAttributeEnumerator = this._parent.OwnedAttribute.GetEnumerator();
                try
                {
                    for (
                    ; ownedAttributeEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedAttributeEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedAttributeEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                NMF.Interop.Legacy.Cmof.IOperation operationItem = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if (((operationItem != null) 
                            && this._parent.OwnedOperation.Remove(operationItem)))
                {
                    return true;
                }
                IProperty propertyItem = item.As<IProperty>();
                if (((propertyItem != null) 
                            && this._parent.OwnedAttribute.Remove(propertyItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedOperation).Concat(this._parent.OwnedAttribute).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the DataType class
        /// </summary>
        public class DataTypeReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private DataType _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public DataTypeReferencedElementsCollection(DataType parent)
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
                    count = (count + this._parent.OwnedOperation.Count);
                    count = (count + this._parent.OwnedAttribute.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                NMF.Interop.Legacy.Cmof.IOperation ownedOperationCasted = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if ((ownedOperationCasted != null))
                {
                    this._parent.OwnedOperation.Add(ownedOperationCasted);
                }
                IProperty ownedAttributeCasted = item.As<IProperty>();
                if ((ownedAttributeCasted != null))
                {
                    this._parent.OwnedAttribute.Add(ownedAttributeCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedOperation.Clear();
                this._parent.OwnedAttribute.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedOperation.Contains(item))
                {
                    return true;
                }
                if (this._parent.OwnedAttribute.Contains(item))
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
                IEnumerator<IModelElement> ownedOperationEnumerator = this._parent.OwnedOperation.GetEnumerator();
                try
                {
                    for (
                    ; ownedOperationEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedOperationEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedOperationEnumerator.Dispose();
                }
                IEnumerator<IModelElement> ownedAttributeEnumerator = this._parent.OwnedAttribute.GetEnumerator();
                try
                {
                    for (
                    ; ownedAttributeEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedAttributeEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedAttributeEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                NMF.Interop.Legacy.Cmof.IOperation operationItem = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if (((operationItem != null) 
                            && this._parent.OwnedOperation.Remove(operationItem)))
                {
                    return true;
                }
                IProperty propertyItem = item.As<IProperty>();
                if (((propertyItem != null) 
                            && this._parent.OwnedAttribute.Remove(propertyItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedOperation).Concat(this._parent.OwnedAttribute).GetEnumerator();
            }
        }
    }
}
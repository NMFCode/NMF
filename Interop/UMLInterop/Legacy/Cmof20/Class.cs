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


namespace NMF.Interop.Legacy.Cmof
{
    
    
    /// <summary>
    /// Class is a kind of classifier whose features are attributes and operations. Attributes of a class are represented by instances of Property that are owned by the class. Some of these attributes may represent the navigable ends of binary associations.
    ///A class is a type that has objects as its instances.
    /// </summary>
    [XmlNamespaceAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Class")]
    [DebuggerDisplayAttribute("Class {Name}")]
    public partial class Class : Classifier, NMF.Interop.Legacy.Cmof.IClass, IModelElement
    {
        
        /// <summary>
        /// The backing field for the IsAbstract property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private bool _isAbstract = false;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _isAbstractAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveIsAbstractAttribute);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedAttributeReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedAttributeReference);
        
        /// <summary>
        /// The backing field for the OwnedAttribute property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ClassOwnedAttributeCollection _ownedAttribute;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedOperationReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedOperationReference);
        
        /// <summary>
        /// The backing field for the OwnedOperation property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ClassOwnedOperationCollection _ownedOperation;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _superClassReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveSuperClassReference);
        
        /// <summary>
        /// The backing field for the SuperClass property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationSet<NMF.Interop.Legacy.Cmof.IClass> _superClass;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Class()
        {
            this._ownedAttribute = new ClassOwnedAttributeCollection(this);
            this._ownedAttribute.CollectionChanging += this.OwnedAttributeCollectionChanging;
            this._ownedAttribute.CollectionChanged += this.OwnedAttributeCollectionChanged;
            this._ownedOperation = new ClassOwnedOperationCollection(this);
            this._ownedOperation.CollectionChanging += this.OwnedOperationCollectionChanging;
            this._ownedOperation.CollectionChanged += this.OwnedOperationCollectionChanged;
            this._superClass = new ObservableAssociationSet<NMF.Interop.Legacy.Cmof.IClass>();
            this._superClass.CollectionChanging += this.SuperClassCollectionChanging;
            this._superClass.CollectionChanged += this.SuperClassCollectionChanged;
        }
        
        /// <summary>
        /// True when a class is abstract. The default value is false.
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isAbstract")]
        [DescriptionAttribute("True when a class is abstract. The default value is false.")]
        [CategoryAttribute("Class")]
        [XmlElementNameAttribute("isAbstract")]
        [XmlAttributeAttribute(true)]
        public bool IsAbstract
        {
            get
            {
                return this._isAbstract;
            }
            set
            {
                if ((this._isAbstract != value))
                {
                    bool old = this._isAbstract;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("IsAbstract", e, _isAbstractAttribute);
                    this._isAbstract = value;
                    this.OnPropertyChanged("IsAbstract", e, _isAbstractAttribute);
                }
            }
        }
        
        /// <summary>
        /// The attributes (i.e. the properties) owned by the class. This is an ordered association. Subsets Classifier::attribute and Namespace::ownedMember.
        ///The attributes owned by a class. These do not include the inherited attributes. Attributes are represented by instances of Property.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedAttribute")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("class")]
        [ConstantAttribute()]
        public IOrderedSetExpression<IProperty> OwnedAttribute
        {
            get
            {
                return this._ownedAttribute;
            }
        }
        
        /// <summary>
        /// The operations owned by the class. This is an ordered association. Subsets Classifier::feature and Namespace::ownedMember.
        ///The operations owned by a class. These do not include the inherited operations.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedOperation")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("class")]
        [ConstantAttribute()]
        public IOrderedSetExpression<NMF.Interop.Legacy.Cmof.IOperation> OwnedOperation
        {
            get
            {
                return this._ownedOperation;
            }
        }
        
        /// <summary>
        /// This gives the superclasses of a class. It redefines Classifier::general.
        ///The immediate superclasses of a class, from which the class inherits.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("superClass")]
        [DescriptionAttribute("This gives the superclasses of a class. It redefines Classifier::general.\r\nThe im" +
            "mediate superclasses of a class, from which the class inherits.")]
        [CategoryAttribute("Class")]
        [XmlElementNameAttribute("superClass")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public ISetExpression<NMF.Interop.Legacy.Cmof.IClass> SuperClass
        {
            get
            {
                return this._superClass;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new ClassChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ClassReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Class")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveIsAbstractAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.Class.ClassInstance)).Resolve("isAbstract")));
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedAttributeReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.Class.ClassInstance)).Resolve("ownedAttribute")));
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
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedOperationReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.Class.ClassInstance)).Resolve("ownedOperation")));
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
        
        private static NMF.Models.Meta.ITypedElement RetrieveSuperClassReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.Class.ClassInstance)).Resolve("superClass")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the SuperClass property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void SuperClassCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("SuperClass", e, _superClassReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the SuperClass property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void SuperClassCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("SuperClass", e, _superClassReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int ownedAttributeIndex = ModelHelper.IndexOfReference(this.OwnedAttribute, element);
            if ((ownedAttributeIndex != -1))
            {
                return ModelHelper.CreatePath("ownedAttribute", ownedAttributeIndex);
            }
            int ownedOperationIndex = ModelHelper.IndexOfReference(this.OwnedOperation, element);
            if ((ownedOperationIndex != -1))
            {
                return ModelHelper.CreatePath("ownedOperation", ownedOperationIndex);
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
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "ISABSTRACT"))
            {
                return this.IsAbstract;
            }
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "OWNEDATTRIBUTE"))
            {
                return this._ownedAttribute;
            }
            if ((feature == "OWNEDOPERATION"))
            {
                return this._ownedOperation;
            }
            if ((feature == "SUPERCLASS"))
            {
                return this._superClass;
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
            if ((feature == "ISABSTRACT"))
            {
                this.IsAbstract = ((bool)(value));
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
            if ((attribute == "ISABSTRACT"))
            {
                return Observable.Box(new IsAbstractProxy(this));
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected override string GetCompositionName(object container)
        {
            if ((container == this._ownedAttribute))
            {
                return "ownedAttribute";
            }
            if ((container == this._ownedOperation))
            {
                return "ownedOperation";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Class")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Class class
        /// </summary>
        public class ClassChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Class _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ClassChildrenCollection(Class parent)
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
                    count = (count + this._parent.OwnedAttribute.Count);
                    count = (count + this._parent.OwnedOperation.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IProperty ownedAttributeCasted = item.As<IProperty>();
                if ((ownedAttributeCasted != null))
                {
                    this._parent.OwnedAttribute.Add(ownedAttributeCasted);
                }
                NMF.Interop.Legacy.Cmof.IOperation ownedOperationCasted = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if ((ownedOperationCasted != null))
                {
                    this._parent.OwnedOperation.Add(ownedOperationCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedAttribute.Clear();
                this._parent.OwnedOperation.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedAttribute.Contains(item))
                {
                    return true;
                }
                if (this._parent.OwnedOperation.Contains(item))
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
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IProperty propertyItem = item.As<IProperty>();
                if (((propertyItem != null) 
                            && this._parent.OwnedAttribute.Remove(propertyItem)))
                {
                    return true;
                }
                NMF.Interop.Legacy.Cmof.IOperation operationItem = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if (((operationItem != null) 
                            && this._parent.OwnedOperation.Remove(operationItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedAttribute).Concat(this._parent.OwnedOperation).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Class class
        /// </summary>
        public class ClassReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Class _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ClassReferencedElementsCollection(Class parent)
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
                    count = (count + this._parent.OwnedAttribute.Count);
                    count = (count + this._parent.OwnedOperation.Count);
                    count = (count + this._parent.SuperClass.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.SuperClass.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.OwnedOperation.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.SuperClass.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IProperty ownedAttributeCasted = item.As<IProperty>();
                if ((ownedAttributeCasted != null))
                {
                    this._parent.OwnedAttribute.Add(ownedAttributeCasted);
                }
                NMF.Interop.Legacy.Cmof.IOperation ownedOperationCasted = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if ((ownedOperationCasted != null))
                {
                    this._parent.OwnedOperation.Add(ownedOperationCasted);
                }
                NMF.Interop.Legacy.Cmof.IClass superClassCasted = item.As<NMF.Interop.Legacy.Cmof.IClass>();
                if ((superClassCasted != null))
                {
                    this._parent.SuperClass.Add(superClassCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedAttribute.Clear();
                this._parent.OwnedOperation.Clear();
                this._parent.SuperClass.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedAttribute.Contains(item))
                {
                    return true;
                }
                if (this._parent.OwnedOperation.Contains(item))
                {
                    return true;
                }
                if (this._parent.SuperClass.Contains(item))
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
                IEnumerator<IModelElement> superClassEnumerator = this._parent.SuperClass.GetEnumerator();
                try
                {
                    for (
                    ; superClassEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = superClassEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    superClassEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IProperty propertyItem = item.As<IProperty>();
                if (((propertyItem != null) 
                            && this._parent.OwnedAttribute.Remove(propertyItem)))
                {
                    return true;
                }
                NMF.Interop.Legacy.Cmof.IOperation operationItem = item.As<NMF.Interop.Legacy.Cmof.IOperation>();
                if (((operationItem != null) 
                            && this._parent.OwnedOperation.Remove(operationItem)))
                {
                    return true;
                }
                NMF.Interop.Legacy.Cmof.IClass classItem = item.As<NMF.Interop.Legacy.Cmof.IClass>();
                if (((classItem != null) 
                            && this._parent.SuperClass.Remove(classItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedAttribute).Concat(this._parent.OwnedOperation).Concat(this._parent.SuperClass).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the isAbstract property
        /// </summary>
        private sealed class IsAbstractProxy : ModelPropertyChange<NMF.Interop.Legacy.Cmof.IClass, bool>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public IsAbstractProxy(NMF.Interop.Legacy.Cmof.IClass modelElement) : 
                    base(modelElement, "isAbstract")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override bool Value
            {
                get
                {
                    return this.ModelElement.IsAbstract;
                }
                set
                {
                    this.ModelElement.IsAbstract = value;
                }
            }
        }
    }
}

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
    /// A type serves as a constraint on the range of values represented by a typed element. Type is an abstract metaclass.
    ///A type is a named element that is used as the type for a typed element. A type can be contained in a package.
    /// </summary>
    [XmlNamespaceAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Type")]
    [DebuggerDisplayAttribute("Type {Name}")]
    public abstract partial class Type : PackageableElement, NMF.Interop.Legacy.Cmof.IType, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _conformsToOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveConformsToOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isInstanceOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsInstanceOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _packageReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrievePackageReference);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// References the owning package of a package. Subsets NamedElement::namespace and redefines Basic::Package::nestingPackage.
        ///Specifies the owning package of this classifier, if any.
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("package")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("ownedType")]
        public IPackage Package
        {
            get
            {
                return ModelHelper.CastAs<IPackage>(this.Parent);
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
                return base.ReferencedElements.Concat(new TypeReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Type")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The query conformsTo() gives true for a type that conforms to another. By default, two types do not conform to each other. This query is intended to be redefined for specific conformance situations.
        ///result = false
        /// </summary>
        /// <param name="other"></param>
        public bool ConformsTo(NMF.Interop.Legacy.Cmof.IType other)
        {
            System.Func<NMF.Interop.Legacy.Cmof.IType, NMF.Interop.Legacy.Cmof.IType, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Legacy.Cmof.IType, NMF.Interop.Legacy.Cmof.IType, bool>>(_conformsToOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method conformsTo registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _conformsToOperation.Value, other);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _conformsToOperation.Value, e));
            bool result = handler.Invoke(this, other);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _conformsToOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveConformsToOperation()
        {
            return ClassInstance.LookupOperation("conformsTo");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="object"></param>
        public bool IsInstance(object @object)
        {
            System.Func<NMF.Interop.Legacy.Cmof.IType, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Legacy.Cmof.IType, object, bool>>(_isInstanceOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isInstance registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isInstanceOperation.Value, @object);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isInstanceOperation.Value, e));
            bool result = handler.Invoke(this, @object);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isInstanceOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsInstanceOperation()
        {
            return ClassInstance.LookupOperation("isInstance");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrievePackageReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Legacy.Cmof.Type.ClassInstance)).Resolve("package")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            IPackage oldPackage = ModelHelper.CastAs<IPackage>(oldParent);
            IPackage newPackage = ModelHelper.CastAs<IPackage>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldPackage, newPackage);
            this.OnPropertyChanging("Package", e, _packageReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            IPackage oldPackage = ModelHelper.CastAs<IPackage>(oldParent);
            IPackage newPackage = ModelHelper.CastAs<IPackage>(newParent);
            if ((oldPackage != null))
            {
                oldPackage.OwnedType.Remove(this);
            }
            if ((newPackage != null))
            {
                newPackage.OwnedType.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldPackage, newPackage);
            this.OnPropertyChanged("Package", e, _packageReference);
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
            if ((reference == "PACKAGE"))
            {
                return this.Package;
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
            if ((feature == "PACKAGE"))
            {
                this.Package = ((IPackage)(value));
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
            if ((reference == "PACKAGE"))
            {
                return new PackageProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Type")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Type class
        /// </summary>
        public class TypeReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Type _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public TypeReferencedElementsCollection(Type parent)
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
                    if ((this._parent.Package != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.Package == null))
                {
                    IPackage packageCasted = item.As<IPackage>();
                    if ((packageCasted != null))
                    {
                        this._parent.Package = packageCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Package = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Package))
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
                if ((this._parent.Package != null))
                {
                    array[arrayIndex] = this._parent.Package;
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
                if ((this._parent.Package == item))
                {
                    this._parent.Package = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Package).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the package property
        /// </summary>
        private sealed class PackageProxy : ModelPropertyChange<NMF.Interop.Legacy.Cmof.IType, IPackage>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public PackageProxy(NMF.Interop.Legacy.Cmof.IType modelElement) : 
                    base(modelElement, "package")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IPackage Value
            {
                get
                {
                    return this.ModelElement.Package;
                }
                set
                {
                    this.ModelElement.Package = value;
                }
            }
        }
    }
}

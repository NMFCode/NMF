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


namespace NMF.Interop.Cmof
{
    
    
    /// <summary>
    /// A package import is a relationship that allows the use of unqualified names to refer to package members from other namespaces.
    /// </summary>
    [XmlNamespaceAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//PackageImport")]
    public partial class PackageImport : DirectedRelationship, IPackageImport, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _public_or_privateOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrievePublic_or_privateOperation);
        
        /// <summary>
        /// The backing field for the Visibility property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private VisibilityKind _visibility = VisibilityKind.Public;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _visibilityAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveVisibilityAttribute);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _importedPackageReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveImportedPackageReference);
        
        /// <summary>
        /// The backing field for the ImportedPackage property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IPackage _importedPackage;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _importingNamespaceReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveImportingNamespaceReference);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Specifies the visibility of the imported PackageableElements within the importing Namespace, i.e., whether imported elements will in turn be visible to other packages that use that importingPackage as an importedPackage. If the PackageImport is public, the imported elements will be visible outside the package, while if it is private they will not.
        /// </summary>
        [DefaultValueAttribute(VisibilityKind.Public)]
        [DisplayNameAttribute("visibility")]
        [DescriptionAttribute(@"Specifies the visibility of the imported PackageableElements within the importing Namespace, i.e., whether imported elements will in turn be visible to other packages that use that importingPackage as an importedPackage. If the PackageImport is public, the imported elements will be visible outside the package, while if it is private they will not.")]
        [CategoryAttribute("PackageImport")]
        [XmlElementNameAttribute("visibility")]
        [XmlAttributeAttribute(true)]
        public VisibilityKind Visibility
        {
            get
            {
                return this._visibility;
            }
            set
            {
                if ((this._visibility != value))
                {
                    VisibilityKind old = this._visibility;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Visibility", e, _visibilityAttribute);
                    this._visibility = value;
                    this.OnPropertyChanged("Visibility", e, _visibilityAttribute);
                }
            }
        }
        
        /// <summary>
        /// Specifies the Package whose members are imported into a Namespace.
        /// </summary>
        [DisplayNameAttribute("importedPackage")]
        [DescriptionAttribute("Specifies the Package whose members are imported into a Namespace.")]
        [CategoryAttribute("PackageImport")]
        [XmlElementNameAttribute("importedPackage")]
        [XmlAttributeAttribute(true)]
        public IPackage ImportedPackage
        {
            get
            {
                return this._importedPackage;
            }
            set
            {
                if ((this._importedPackage != value))
                {
                    IPackage old = this._importedPackage;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("ImportedPackage", e, _importedPackageReference);
                    this._importedPackage = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetImportedPackage;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetImportedPackage;
                    }
                    this.OnPropertyChanged("ImportedPackage", e, _importedPackageReference);
                }
            }
        }
        
        /// <summary>
        /// Specifies the Namespace that imports the members from a Package.
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("importingNamespace")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("packageImport")]
        public NMF.Interop.Cmof.INamespace ImportingNamespace
        {
            get
            {
                return ModelHelper.CastAs<NMF.Interop.Cmof.INamespace>(this.Parent);
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
                return base.ReferencedElements.Concat(new PackageImportReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//PackageImport")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The visibility of a PackageImport is either public or private.
        ///self.visibility = #public or self.visibility = #private
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Public_or_private(object diagnostics, object context)
        {
            System.Func<IPackageImport, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IPackageImport, object, object, bool>>(_public_or_privateOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method public_or_private registered. Use the metho" +
                        "d broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _public_or_privateOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _public_or_privateOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _public_or_privateOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrievePublic_or_privateOperation()
        {
            return ClassInstance.LookupOperation("public_or_private");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveVisibilityAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.PackageImport.ClassInstance)).Resolve("visibility")));
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveImportedPackageReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.PackageImport.ClassInstance)).Resolve("importedPackage")));
        }
        
        /// <summary>
        /// Handles the event that the ImportedPackage property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetImportedPackage(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.ImportedPackage))
            {
                this.ImportedPackage = null;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveImportingNamespaceReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.PackageImport.ClassInstance)).Resolve("importingNamespace")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            NMF.Interop.Cmof.INamespace oldImportingNamespace = ModelHelper.CastAs<NMF.Interop.Cmof.INamespace>(oldParent);
            NMF.Interop.Cmof.INamespace newImportingNamespace = ModelHelper.CastAs<NMF.Interop.Cmof.INamespace>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldImportingNamespace, newImportingNamespace);
            this.OnPropertyChanging("ImportingNamespace", e, _importingNamespaceReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            NMF.Interop.Cmof.INamespace oldImportingNamespace = ModelHelper.CastAs<NMF.Interop.Cmof.INamespace>(oldParent);
            NMF.Interop.Cmof.INamespace newImportingNamespace = ModelHelper.CastAs<NMF.Interop.Cmof.INamespace>(newParent);
            if ((oldImportingNamespace != null))
            {
                oldImportingNamespace.PackageImport.Remove(this);
            }
            if ((newImportingNamespace != null))
            {
                newImportingNamespace.PackageImport.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldImportingNamespace, newImportingNamespace);
            this.OnPropertyChanged("ImportingNamespace", e, _importingNamespaceReference);
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
            if ((reference == "IMPORTEDPACKAGE"))
            {
                return this.ImportedPackage;
            }
            if ((reference == "IMPORTINGNAMESPACE"))
            {
                return this.ImportingNamespace;
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
            if ((attribute == "VISIBILITY"))
            {
                return this.Visibility;
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
            if ((feature == "IMPORTEDPACKAGE"))
            {
                this.ImportedPackage = ((IPackage)(value));
                return;
            }
            if ((feature == "IMPORTINGNAMESPACE"))
            {
                this.ImportingNamespace = ((NMF.Interop.Cmof.INamespace)(value));
                return;
            }
            if ((feature == "VISIBILITY"))
            {
                this.Visibility = ((VisibilityKind)(value));
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
            if ((attribute == "VISIBILITY"))
            {
                return Observable.Box(new VisibilityProxy(this));
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
            if ((reference == "IMPORTEDPACKAGE"))
            {
                return new ImportedPackageProxy(this);
            }
            if ((reference == "IMPORTINGNAMESPACE"))
            {
                return new ImportingNamespaceProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//PackageImport")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the PackageImport class
        /// </summary>
        public class PackageImportReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private PackageImport _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public PackageImportReferencedElementsCollection(PackageImport parent)
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
                    if ((this._parent.ImportedPackage != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.ImportingNamespace != null))
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
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
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
                if ((this._parent.ImportedPackage == null))
                {
                    IPackage importedPackageCasted = item.As<IPackage>();
                    if ((importedPackageCasted != null))
                    {
                        this._parent.ImportedPackage = importedPackageCasted;
                        return;
                    }
                }
                if ((this._parent.ImportingNamespace == null))
                {
                    NMF.Interop.Cmof.INamespace importingNamespaceCasted = item.As<NMF.Interop.Cmof.INamespace>();
                    if ((importingNamespaceCasted != null))
                    {
                        this._parent.ImportingNamespace = importingNamespaceCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.ImportedPackage = null;
                this._parent.ImportingNamespace = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.ImportedPackage))
                {
                    return true;
                }
                if ((item == this._parent.ImportingNamespace))
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
                if ((this._parent.ImportedPackage != null))
                {
                    array[arrayIndex] = this._parent.ImportedPackage;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.ImportingNamespace != null))
                {
                    array[arrayIndex] = this._parent.ImportingNamespace;
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
                if ((this._parent.ImportedPackage == item))
                {
                    this._parent.ImportedPackage = null;
                    return true;
                }
                if ((this._parent.ImportingNamespace == item))
                {
                    this._parent.ImportingNamespace = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.ImportedPackage).Concat(this._parent.ImportingNamespace).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the visibility property
        /// </summary>
        private sealed class VisibilityProxy : ModelPropertyChange<IPackageImport, VisibilityKind>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public VisibilityProxy(IPackageImport modelElement) : 
                    base(modelElement, "visibility")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override VisibilityKind Value
            {
                get
                {
                    return this.ModelElement.Visibility;
                }
                set
                {
                    this.ModelElement.Visibility = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the importedPackage property
        /// </summary>
        private sealed class ImportedPackageProxy : ModelPropertyChange<IPackageImport, IPackage>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ImportedPackageProxy(IPackageImport modelElement) : 
                    base(modelElement, "importedPackage")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IPackage Value
            {
                get
                {
                    return this.ModelElement.ImportedPackage;
                }
                set
                {
                    this.ModelElement.ImportedPackage = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the importingNamespace property
        /// </summary>
        private sealed class ImportingNamespaceProxy : ModelPropertyChange<IPackageImport, NMF.Interop.Cmof.INamespace>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ImportingNamespaceProxy(IPackageImport modelElement) : 
                    base(modelElement, "importingNamespace")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override NMF.Interop.Cmof.INamespace Value
            {
                get
                {
                    return this.ModelElement.ImportingNamespace;
                }
                set
                {
                    this.ModelElement.ImportingNamespace = value;
                }
            }
        }
    }
}

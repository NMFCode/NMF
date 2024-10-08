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
    /// An ExtensionPoint identifies a point in the behavior of a UseCase where that behavior can be extended by the behavior of some other (extending) UseCase, as specified by an Extend relationship.
    ///&lt;p&gt;From package UML::UseCases.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ExtensionPoint")]
    [DebuggerDisplayAttribute("ExtensionPoint {Name}")]
    public partial class ExtensionPoint : RedefinableElement, IExtensionPoint, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _must_have_nameOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveMust_have_nameOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _useCaseReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveUseCaseReference);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The UseCase that owns this ExtensionPoint.
        ///&lt;p&gt;From package UML::UseCases.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("useCase")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("extensionPoint")]
        public IUseCase UseCase
        {
            get
            {
                return ModelHelper.CastAs<IUseCase>(this.Parent);
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
                return base.ReferencedElements.Concat(new ExtensionPointReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ExtensionPoint")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// An ExtensionPoint must have a name.
        ///name-&gt;notEmpty ()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Must_have_name(object diagnostics, object context)
        {
            System.Func<IExtensionPoint, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IExtensionPoint, object, object, bool>>(_must_have_nameOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method must_have_name registered. Use the method b" +
                        "roker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _must_have_nameOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _must_have_nameOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _must_have_nameOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveMust_have_nameOperation()
        {
            return ClassInstance.LookupOperation("must_have_name");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveUseCaseReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ExtensionPoint.ClassInstance)).Resolve("useCase")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            IUseCase oldUseCase = ModelHelper.CastAs<IUseCase>(oldParent);
            IUseCase newUseCase = ModelHelper.CastAs<IUseCase>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldUseCase, newUseCase);
            this.OnPropertyChanging("UseCase", e, _useCaseReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            IUseCase oldUseCase = ModelHelper.CastAs<IUseCase>(oldParent);
            IUseCase newUseCase = ModelHelper.CastAs<IUseCase>(newParent);
            if ((oldUseCase != null))
            {
                oldUseCase.ExtensionPoint.Remove(this);
            }
            if ((newUseCase != null))
            {
                newUseCase.ExtensionPoint.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldUseCase, newUseCase);
            this.OnPropertyChanged("UseCase", e, _useCaseReference);
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
            if ((reference == "USECASE"))
            {
                return this.UseCase;
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
            if ((feature == "USECASE"))
            {
                this.UseCase = ((IUseCase)(value));
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
            if ((reference == "USECASE"))
            {
                return new UseCaseProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ExtensionPoint")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ExtensionPoint class
        /// </summary>
        public class ExtensionPointReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ExtensionPoint _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ExtensionPointReferencedElementsCollection(ExtensionPoint parent)
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
                    if ((this._parent.UseCase != null))
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
                if ((this._parent.UseCase == null))
                {
                    IUseCase useCaseCasted = item.As<IUseCase>();
                    if ((useCaseCasted != null))
                    {
                        this._parent.UseCase = useCaseCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.UseCase = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.UseCase))
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
                if ((this._parent.UseCase != null))
                {
                    array[arrayIndex] = this._parent.UseCase;
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
                if ((this._parent.UseCase == item))
                {
                    this._parent.UseCase = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.UseCase).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the useCase property
        /// </summary>
        private sealed class UseCaseProxy : ModelPropertyChange<IExtensionPoint, IUseCase>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public UseCaseProxy(IExtensionPoint modelElement) : 
                    base(modelElement, "useCase")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IUseCase Value
            {
                get
                {
                    return this.ModelElement.UseCase;
                }
                set
                {
                    this.ModelElement.UseCase = value;
                }
            }
        }
    }
}

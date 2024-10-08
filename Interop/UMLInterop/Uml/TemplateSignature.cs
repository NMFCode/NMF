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
    /// A Template Signature bundles the set of formal TemplateParameters for a template.
    ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//TemplateSignature")]
    public partial class TemplateSignature : Element, ITemplateSignature, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _own_elementsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveOwn_elementsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _unique_parametersOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveUnique_parametersOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _templateReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveTemplateReference);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedParameterReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedParameterReference);
        
        /// <summary>
        /// The backing field for the OwnedParameter property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private TemplateSignatureOwnedParameterCollection _ownedParameter;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public TemplateSignature()
        {
            this._ownedParameter = new TemplateSignatureOwnedParameterCollection(this);
            this._ownedParameter.CollectionChanging += this.OwnedParameterCollectionChanging;
            this._ownedParameter.CollectionChanged += this.OwnedParameterCollectionChanged;
        }
        
        /// <summary>
        /// The TemplateableElement that owns this TemplateSignature.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("template")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("ownedTemplateSignature")]
        public ITemplateableElement Template
        {
            get
            {
                return ModelHelper.CastAs<ITemplateableElement>(this.Parent);
            }
            set
            {
                this.Parent = value;
            }
        }
        
        /// <summary>
        /// The formal parameters that are owned by this TemplateSignature.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("ownedParameter")]
        [DescriptionAttribute("The formal parameters that are owned by this TemplateSignature.\n<p>From package U" +
            "ML::CommonStructure.</p>")]
        [CategoryAttribute("TemplateSignature")]
        [XmlElementNameAttribute("ownedParameter")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("signature")]
        [ConstantAttribute()]
        public IOrderedSetExpression<ITemplateParameter> OwnedParameter
        {
            get
            {
                return this._ownedParameter;
            }
        }
        
        IListExpression<ITemplateParameter> ITemplateSignature.Parameter
        {
            get
            {
                return new TemplateSignatureParameterCollection(this);
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new TemplateSignatureReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//TemplateSignature")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Parameters must own the ParameterableElements they parameter or those ParameterableElements must be owned by the TemplateableElement being templated.
        ///template.ownedElement-&gt;includesAll(parameter.parameteredElement-&gt;asSet() - parameter.ownedParameteredElement-&gt;asSet())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Own_elements(object diagnostics, object context)
        {
            System.Func<ITemplateSignature, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ITemplateSignature, object, object, bool>>(_own_elementsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method own_elements registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _own_elementsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _own_elementsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _own_elementsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveOwn_elementsOperation()
        {
            return ClassInstance.LookupOperation("own_elements");
        }
        
        /// <summary>
        /// The names of the parameters of a TemplateSignature are unique.
        ///parameter-&gt;forAll( p1, p2 | (p1 &lt;&gt; p2 and p1.parameteredElement.oclIsKindOf(NamedElement) and p2.parameteredElement.oclIsKindOf(NamedElement) ) implies
        ///   p1.parameteredElement.oclAsType(NamedElement).name &lt;&gt; p2.parameteredElement.oclAsType(NamedElement).name)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Unique_parameters(object diagnostics, object context)
        {
            System.Func<ITemplateSignature, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ITemplateSignature, object, object, bool>>(_unique_parametersOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method unique_parameters registered. Use the metho" +
                        "d broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _unique_parametersOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _unique_parametersOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _unique_parametersOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveUnique_parametersOperation()
        {
            return ClassInstance.LookupOperation("unique_parameters");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveTemplateReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.TemplateSignature.ClassInstance)).Resolve("template")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            ITemplateableElement oldTemplate = ModelHelper.CastAs<ITemplateableElement>(oldParent);
            ITemplateableElement newTemplate = ModelHelper.CastAs<ITemplateableElement>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldTemplate, newTemplate);
            this.OnPropertyChanging("Template", e, _templateReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            ITemplateableElement oldTemplate = ModelHelper.CastAs<ITemplateableElement>(oldParent);
            ITemplateableElement newTemplate = ModelHelper.CastAs<ITemplateableElement>(newParent);
            if ((oldTemplate != null))
            {
                oldTemplate.OwnedTemplateSignature = null;
            }
            if ((newTemplate != null))
            {
                newTemplate.OwnedTemplateSignature = this;
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldTemplate, newTemplate);
            this.OnPropertyChanged("Template", e, _templateReference);
            base.OnParentChanged(newParent, oldParent);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedParameterReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.TemplateSignature.ClassInstance)).Resolve("ownedParameter")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedParameter property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedParameterCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedParameter", e, _ownedParameterReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedParameter property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedParameter", e, _ownedParameterReference);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "TEMPLATE"))
            {
                return this.Template;
            }
            if ((reference == "OWNEDPARAMETER"))
            {
                if ((index < this.OwnedParameter.Count))
                {
                    return this.OwnedParameter[index];
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
            if ((feature == "OWNEDPARAMETER"))
            {
                return this._ownedParameter;
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
            if ((feature == "TEMPLATE"))
            {
                this.Template = ((ITemplateableElement)(value));
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
            if ((reference == "TEMPLATE"))
            {
                return new TemplateProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//TemplateSignature")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the TemplateSignature class
        /// </summary>
        public class TemplateSignatureReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private TemplateSignature _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public TemplateSignatureReferencedElementsCollection(TemplateSignature parent)
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
        /// Represents a proxy to represent an incremental access to the template property
        /// </summary>
        private sealed class TemplateProxy : ModelPropertyChange<ITemplateSignature, ITemplateableElement>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public TemplateProxy(ITemplateSignature modelElement) : 
                    base(modelElement, "template")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override ITemplateableElement Value
            {
                get
                {
                    return this.ModelElement.Template;
                }
                set
                {
                    this.ModelElement.Template = value;
                }
            }
        }
    }
}

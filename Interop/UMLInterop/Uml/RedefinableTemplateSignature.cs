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
    /// A RedefinableTemplateSignature supports the addition of formal template parameters in a specialization of a template classifier.
    ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//RedefinableTemplateSignature")]
    [DebuggerDisplayAttribute("RedefinableTemplateSignature {Name}")]
    public partial class RedefinableTemplateSignature : RedefinableElement, IRedefinableTemplateSignature, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _redefines_parentsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveRedefines_parentsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getInheritedParametersOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetInheritedParametersOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _extendedSignatureReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveExtendedSignatureReference);
        
        /// <summary>
        /// The backing field for the ExtendedSignature property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationSet<IRedefinableTemplateSignature> _extendedSignature;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _classifierReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveClassifierReference);
        
        /// <summary>
        /// The backing field for the Classifier property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IClassifier _classifier;
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _own_elementsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveOwn_elementsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _unique_parametersOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveUnique_parametersOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _parameterReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveParameterReference);
        
        /// <summary>
        /// The backing field for the Parameter property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationList<ITemplateParameter> _parameter;
        
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
        public RedefinableTemplateSignature()
        {
            this._extendedSignature = new ObservableAssociationSet<IRedefinableTemplateSignature>();
            this._extendedSignature.CollectionChanging += this.ExtendedSignatureCollectionChanging;
            this._extendedSignature.CollectionChanged += this.ExtendedSignatureCollectionChanged;
            this._parameter = new ObservableAssociationList<ITemplateParameter>();
            this._parameter.CollectionChanging += this.ParameterCollectionChanging;
            this._parameter.CollectionChanged += this.ParameterCollectionChanged;
            this._ownedParameter = new TemplateSignatureOwnedParameterCollection(this);
            this._ownedParameter.CollectionChanging += this.OwnedParameterCollectionChanging;
            this._ownedParameter.CollectionChanged += this.OwnedParameterCollectionChanged;
        }
        
        /// <summary>
        /// The signatures extended by this RedefinableTemplateSignature.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("extendedSignature")]
        [DescriptionAttribute("The signatures extended by this RedefinableTemplateSignature.\n<p>From package UML" +
            "::Classification.</p>")]
        [CategoryAttribute("RedefinableTemplateSignature")]
        [XmlElementNameAttribute("extendedSignature")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public ISetExpression<IRedefinableTemplateSignature> ExtendedSignature
        {
            get
            {
                return this._extendedSignature;
            }
        }
        
        /// <summary>
        /// The Classifier that owns this RedefinableTemplateSignature.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("classifier")]
        [DescriptionAttribute("The Classifier that owns this RedefinableTemplateSignature.\n<p>From package UML::" +
            "Classification.</p>")]
        [CategoryAttribute("RedefinableTemplateSignature")]
        [XmlElementNameAttribute("classifier")]
        [XmlAttributeAttribute(true)]
        public IClassifier Classifier
        {
            get
            {
                return this._classifier;
            }
            set
            {
                if ((this._classifier != value))
                {
                    IClassifier old = this._classifier;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Classifier", e, _classifierReference);
                    this._classifier = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetClassifier;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetClassifier;
                    }
                    this.OnPropertyChanged("Classifier", e, _classifierReference);
                }
            }
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
        
        /// <summary>
        /// The ordered set of all formal TemplateParameters for this TemplateSignature.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("parameter")]
        [DescriptionAttribute("The ordered set of all formal TemplateParameters for this TemplateSignature.\n<p>F" +
            "rom package UML::CommonStructure.</p>")]
        [CategoryAttribute("TemplateSignature")]
        [XmlElementNameAttribute("parameter")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IListExpression<ITemplateParameter> Parameter
        {
            get
            {
                return this._parameter;
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
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new RedefinableTemplateSignatureChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new RedefinableTemplateSignatureReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//RedefinableTemplateSignature")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If any of the parent Classifiers are a template, then the extendedSignature must include the signature of that Classifier.
        ///classifier.allParents()-&gt;forAll(c | c.ownedTemplateSignature-&gt;notEmpty() implies self-&gt;closure(extendedSignature)-&gt;includes(c.ownedTemplateSignature))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Redefines_parents(object diagnostics, object context)
        {
            System.Func<IRedefinableTemplateSignature, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableTemplateSignature, object, object, bool>>(_redefines_parentsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method redefines_parents registered. Use the metho" +
                        "d broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _redefines_parentsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _redefines_parentsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _redefines_parentsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveRedefines_parentsOperation()
        {
            return ClassInstance.LookupOperation("redefines_parents");
        }
        
        /// <summary>
        /// Derivation for RedefinableTemplateSignature::/inheritedParameter
        ///result = (if extendedSignature-&gt;isEmpty() then Set{} else extendedSignature.parameter-&gt;asSet() endif)
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        public ISetExpression<ITemplateParameter> GetInheritedParameters()
        {
            System.Func<IRedefinableTemplateSignature, ISetExpression<ITemplateParameter>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableTemplateSignature, ISetExpression<ITemplateParameter>>>(_getInheritedParametersOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getInheritedParameters registered. Use the " +
                        "method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getInheritedParametersOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getInheritedParametersOperation.Value, e));
            ISetExpression<ITemplateParameter> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getInheritedParametersOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetInheritedParametersOperation()
        {
            return ClassInstance.LookupOperation("getInheritedParameters");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveExtendedSignatureReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.RedefinableTemplateSignature.ClassInstance)).Resolve("extendedSignature")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the ExtendedSignature property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ExtendedSignatureCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("ExtendedSignature", e, _extendedSignatureReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the ExtendedSignature property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ExtendedSignatureCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("ExtendedSignature", e, _extendedSignatureReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveClassifierReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.RedefinableTemplateSignature.ClassInstance)).Resolve("classifier")));
        }
        
        /// <summary>
        /// Handles the event that the Classifier property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetClassifier(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Classifier))
            {
                this.Classifier = null;
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
        
        private static NMF.Models.Meta.ITypedElement RetrieveParameterReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.TemplateSignature.ClassInstance)).Resolve("parameter")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Parameter property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ParameterCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Parameter", e, _parameterReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Parameter property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Parameter", e, _parameterReference);
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
            if ((reference == "CLASSIFIER"))
            {
                return this.Classifier;
            }
            if ((reference == "PARAMETER"))
            {
                if ((index < this.Parameter.Count))
                {
                    return this.Parameter[index];
                }
                else
                {
                    return null;
                }
            }
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
            if ((feature == "EXTENDEDSIGNATURE"))
            {
                return this._extendedSignature;
            }
            if ((feature == "PARAMETER"))
            {
                return this._parameter;
            }
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
            if ((feature == "CLASSIFIER"))
            {
                this.Classifier = ((IClassifier)(value));
                return;
            }
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
            if ((reference == "CLASSIFIER"))
            {
                return new ClassifierProxy(this);
            }
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//RedefinableTemplateSignature")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the RedefinableTemplateSignature class
        /// </summary>
        public class RedefinableTemplateSignatureChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private RedefinableTemplateSignature _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public RedefinableTemplateSignatureChildrenCollection(RedefinableTemplateSignature parent)
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
        /// The collection class to to represent the children of the RedefinableTemplateSignature class
        /// </summary>
        public class RedefinableTemplateSignatureReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private RedefinableTemplateSignature _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public RedefinableTemplateSignatureReferencedElementsCollection(RedefinableTemplateSignature parent)
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
        /// Represents a proxy to represent an incremental access to the classifier property
        /// </summary>
        private sealed class ClassifierProxy : ModelPropertyChange<IRedefinableTemplateSignature, IClassifier>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ClassifierProxy(IRedefinableTemplateSignature modelElement) : 
                    base(modelElement, "classifier")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IClassifier Value
            {
                get
                {
                    return this.ModelElement.Classifier;
                }
                set
                {
                    this.ModelElement.Classifier = value;
                }
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

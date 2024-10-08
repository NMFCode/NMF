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
    /// An OpaqueAction is an Action whose functionality is not specified within UML.
    ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueAction")]
    [DebuggerDisplayAttribute("OpaqueAction {Name}")]
    public partial class OpaqueAction : Action, IOpaqueAction, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _language_body_sizeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveLanguage_body_sizeOperation);
        
        /// <summary>
        /// The backing field for the Body property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableList<string> _body;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _bodyAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveBodyAttribute);
        
        /// <summary>
        /// The backing field for the Language property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableOrderedSet<string> _language;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _languageAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveLanguageAttribute);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _inputValueReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveInputValueReference);
        
        /// <summary>
        /// The backing field for the InputValue property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IInputPin> _inputValue;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _outputValueReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOutputValueReference);
        
        /// <summary>
        /// The backing field for the OutputValue property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IOutputPin> _outputValue;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public OpaqueAction()
        {
            this._body = new ObservableList<string>();
            this._body.CollectionChanging += this.BodyCollectionChanging;
            this._body.CollectionChanged += this.BodyCollectionChanged;
            this._language = new ObservableOrderedSet<string>();
            this._language.CollectionChanging += this.LanguageCollectionChanging;
            this._language.CollectionChanged += this.LanguageCollectionChanged;
            this._inputValue = new ObservableCompositionOrderedSet<IInputPin>(this);
            this._inputValue.CollectionChanging += this.InputValueCollectionChanging;
            this._inputValue.CollectionChanged += this.InputValueCollectionChanged;
            this._outputValue = new ObservableCompositionOrderedSet<IOutputPin>(this);
            this._outputValue.CollectionChanging += this.OutputValueCollectionChanging;
            this._outputValue.CollectionChanged += this.OutputValueCollectionChanged;
        }
        
        /// <summary>
        /// Provides a textual specification of the functionality of the Action, in one or more languages other than UML.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("body")]
        [DescriptionAttribute("Provides a textual specification of the functionality of the Action, in one or mo" +
            "re languages other than UML.\n<p>From package UML::Actions.</p>")]
        [CategoryAttribute("OpaqueAction")]
        [XmlElementNameAttribute("body")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IListExpression<string> Body
        {
            get
            {
                return this._body;
            }
        }
        
        /// <summary>
        /// If provided, a specification of the language used for each of the body Strings.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("language")]
        [DescriptionAttribute("If provided, a specification of the language used for each of the body Strings.\n<" +
            "p>From package UML::Actions.</p>")]
        [CategoryAttribute("OpaqueAction")]
        [XmlElementNameAttribute("language")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IOrderedSetExpression<string> Language
        {
            get
            {
                return this._language;
            }
        }
        
        /// <summary>
        /// The InputPins providing inputs to the OpaqueAction.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("inputValue")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IInputPin> InputValue
        {
            get
            {
                return this._inputValue;
            }
        }
        
        /// <summary>
        /// The OutputPins on which the OpaqueAction provides outputs.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("outputValue")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IOutputPin> OutputValue
        {
            get
            {
                return this._outputValue;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new OpaqueActionChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new OpaqueActionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueAction")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If the language attribute is not empty, then the size of the body and language lists must be the same.
        ///language-&gt;notEmpty() implies (_&apos;body&apos;-&gt;size() = language-&gt;size())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Language_body_size(object diagnostics, object context)
        {
            System.Func<IOpaqueAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IOpaqueAction, object, object, bool>>(_language_body_sizeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method language_body_size registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _language_body_sizeOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _language_body_sizeOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _language_body_sizeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveLanguage_body_sizeOperation()
        {
            return ClassInstance.LookupOperation("language_body_size");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveBodyAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.OpaqueAction.ClassInstance)).Resolve("body")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Body property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void BodyCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Body", e, _bodyAttribute);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Body property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void BodyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Body", e, _bodyAttribute);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveLanguageAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.OpaqueAction.ClassInstance)).Resolve("language")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Language property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void LanguageCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Language", e, _languageAttribute);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Language property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void LanguageCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Language", e, _languageAttribute);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveInputValueReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.OpaqueAction.ClassInstance)).Resolve("inputValue")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the InputValue property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void InputValueCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("InputValue", e, _inputValueReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the InputValue property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void InputValueCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("InputValue", e, _inputValueReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOutputValueReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.OpaqueAction.ClassInstance)).Resolve("outputValue")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OutputValue property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OutputValueCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OutputValue", e, _outputValueReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OutputValue property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OutputValueCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OutputValue", e, _outputValueReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int inputValueIndex = ModelHelper.IndexOfReference(this.InputValue, element);
            if ((inputValueIndex != -1))
            {
                return ModelHelper.CreatePath("inputValue", inputValueIndex);
            }
            int outputValueIndex = ModelHelper.IndexOfReference(this.OutputValue, element);
            if ((outputValueIndex != -1))
            {
                return ModelHelper.CreatePath("outputValue", outputValueIndex);
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
            if ((reference == "INPUTVALUE"))
            {
                if ((index < this.InputValue.Count))
                {
                    return this.InputValue[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "OUTPUTVALUE"))
            {
                if ((index < this.OutputValue.Count))
                {
                    return this.OutputValue[index];
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
            if ((attribute == "BODY"))
            {
                if ((index < this.Body.Count))
                {
                    return this.Body[index];
                }
                else
                {
                    return null;
                }
            }
            if ((attribute == "LANGUAGE"))
            {
                if ((index < this.Language.Count))
                {
                    return this.Language[index];
                }
                else
                {
                    return null;
                }
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
            if ((feature == "INPUTVALUE"))
            {
                return this._inputValue;
            }
            if ((feature == "OUTPUTVALUE"))
            {
                return this._outputValue;
            }
            if ((feature == "BODY"))
            {
                return this._body;
            }
            if ((feature == "LANGUAGE"))
            {
                return this._language;
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
            if ((container == this._inputValue))
            {
                return "inputValue";
            }
            if ((container == this._outputValue))
            {
                return "outputValue";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueAction")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the OpaqueAction class
        /// </summary>
        public class OpaqueActionChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private OpaqueAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public OpaqueActionChildrenCollection(OpaqueAction parent)
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
                    count = (count + this._parent.InputValue.Count);
                    count = (count + this._parent.OutputValue.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.InputValue.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.OutputValue.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.InputValue.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.OutputValue.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IInputPin inputValueCasted = item.As<IInputPin>();
                if ((inputValueCasted != null))
                {
                    this._parent.InputValue.Add(inputValueCasted);
                }
                IOutputPin outputValueCasted = item.As<IOutputPin>();
                if ((outputValueCasted != null))
                {
                    this._parent.OutputValue.Add(outputValueCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.InputValue.Clear();
                this._parent.OutputValue.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.InputValue.Contains(item))
                {
                    return true;
                }
                if (this._parent.OutputValue.Contains(item))
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
                IEnumerator<IModelElement> inputValueEnumerator = this._parent.InputValue.GetEnumerator();
                try
                {
                    for (
                    ; inputValueEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = inputValueEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    inputValueEnumerator.Dispose();
                }
                IEnumerator<IModelElement> outputValueEnumerator = this._parent.OutputValue.GetEnumerator();
                try
                {
                    for (
                    ; outputValueEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = outputValueEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    outputValueEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IInputPin inputPinItem = item.As<IInputPin>();
                if (((inputPinItem != null) 
                            && this._parent.InputValue.Remove(inputPinItem)))
                {
                    return true;
                }
                IOutputPin outputPinItem = item.As<IOutputPin>();
                if (((outputPinItem != null) 
                            && this._parent.OutputValue.Remove(outputPinItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.InputValue).Concat(this._parent.OutputValue).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the OpaqueAction class
        /// </summary>
        public class OpaqueActionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private OpaqueAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public OpaqueActionReferencedElementsCollection(OpaqueAction parent)
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
                    count = (count + this._parent.InputValue.Count);
                    count = (count + this._parent.OutputValue.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.InputValue.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.OutputValue.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.InputValue.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.OutputValue.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IInputPin inputValueCasted = item.As<IInputPin>();
                if ((inputValueCasted != null))
                {
                    this._parent.InputValue.Add(inputValueCasted);
                }
                IOutputPin outputValueCasted = item.As<IOutputPin>();
                if ((outputValueCasted != null))
                {
                    this._parent.OutputValue.Add(outputValueCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.InputValue.Clear();
                this._parent.OutputValue.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.InputValue.Contains(item))
                {
                    return true;
                }
                if (this._parent.OutputValue.Contains(item))
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
                IEnumerator<IModelElement> inputValueEnumerator = this._parent.InputValue.GetEnumerator();
                try
                {
                    for (
                    ; inputValueEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = inputValueEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    inputValueEnumerator.Dispose();
                }
                IEnumerator<IModelElement> outputValueEnumerator = this._parent.OutputValue.GetEnumerator();
                try
                {
                    for (
                    ; outputValueEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = outputValueEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    outputValueEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IInputPin inputPinItem = item.As<IInputPin>();
                if (((inputPinItem != null) 
                            && this._parent.InputValue.Remove(inputPinItem)))
                {
                    return true;
                }
                IOutputPin outputPinItem = item.As<IOutputPin>();
                if (((outputPinItem != null) 
                            && this._parent.OutputValue.Remove(outputPinItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.InputValue).Concat(this._parent.OutputValue).GetEnumerator();
            }
        }
    }
}

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
    /// A CreateObjectAction is an Action that creates an instance of the specified Classifier.
    ///<p>From package UML::Actions.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//CreateObjectAction")]
    [DebuggerDisplayAttribute("CreateObjectAction {Name}")]
    public partial class CreateObjectAction : Action, ICreateObjectAction, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _classifier_not_abstractOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveClassifier_not_abstractOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _multiplicityOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveMultiplicityOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _classifier_not_association_classOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveClassifier_not_association_classOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _same_typeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSame_typeOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _classifierReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveClassifierReference);
        
        /// <summary>
        /// The backing field for the Classifier property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IClassifier _classifier;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _resultReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveResultReference);
        
        /// <summary>
        /// The backing field for the Result property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IOutputPin _result;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The Classifier to be instantiated.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [DisplayNameAttribute("classifier")]
        [DescriptionAttribute("The Classifier to be instantiated.\n<p>From package UML::Actions.</p>")]
        [CategoryAttribute("CreateObjectAction")]
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
        /// The OutputPin on which the newly created object is placed.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("result")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IOutputPin Result
        {
            get
            {
                return this._result;
            }
            set
            {
                if ((this._result != value))
                {
                    IOutputPin old = this._result;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Result", e, _resultReference);
                    this._result = value;
                    if ((old != null))
                    {
                        old.Parent = null;
                        old.ParentChanged -= this.OnResetResult;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetResult;
                    }
                    this.OnPropertyChanged("Result", e, _resultReference);
                }
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new CreateObjectActionChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new CreateObjectActionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//CreateObjectAction")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The classifier cannot be abstract.
        ///not classifier.isAbstract
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Classifier_not_abstract(object diagnostics, object context)
        {
            System.Func<ICreateObjectAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICreateObjectAction, object, object, bool>>(_classifier_not_abstractOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method classifier_not_abstract registered. Use the" +
                        " method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _classifier_not_abstractOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _classifier_not_abstractOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _classifier_not_abstractOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveClassifier_not_abstractOperation()
        {
            return ClassInstance.LookupOperation("classifier_not_abstract");
        }
        
        /// <summary>
        /// The multiplicity of the result OutputPin is 1..1.
        ///result.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Multiplicity(object diagnostics, object context)
        {
            System.Func<ICreateObjectAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICreateObjectAction, object, object, bool>>(_multiplicityOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method multiplicity registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _multiplicityOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _multiplicityOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _multiplicityOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveMultiplicityOperation()
        {
            return ClassInstance.LookupOperation("multiplicity");
        }
        
        /// <summary>
        /// The classifier cannot be an AssociationClass.
        ///not classifier.oclIsKindOf(AssociationClass)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Classifier_not_association_class(object diagnostics, object context)
        {
            System.Func<ICreateObjectAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICreateObjectAction, object, object, bool>>(_classifier_not_association_classOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method classifier_not_association_class registered" +
                        ". Use the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _classifier_not_association_classOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _classifier_not_association_classOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _classifier_not_association_classOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveClassifier_not_association_classOperation()
        {
            return ClassInstance.LookupOperation("classifier_not_association_class");
        }
        
        /// <summary>
        /// The type of the result OutputPin must be the same as the classifier of the CreateObjectAction.
        ///result.type = classifier
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Same_type(object diagnostics, object context)
        {
            System.Func<ICreateObjectAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ICreateObjectAction, object, object, bool>>(_same_typeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method same_type registered. Use the method broker" +
                        " to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _same_typeOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _same_typeOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _same_typeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSame_typeOperation()
        {
            return ClassInstance.LookupOperation("same_type");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveClassifierReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.CreateObjectAction.ClassInstance)).Resolve("classifier")));
        }
        
        /// <summary>
        /// Handles the event that the Classifier property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetClassifier(object sender, System.EventArgs eventArgs)
        {
            this.Classifier = null;
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveResultReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.CreateObjectAction.ClassInstance)).Resolve("result")));
        }
        
        /// <summary>
        /// Handles the event that the Result property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetResult(object sender, System.EventArgs eventArgs)
        {
            this.Result = null;
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Result))
            {
                return ModelHelper.CreatePath("result");
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
            if ((reference == "CLASSIFIER"))
            {
                return this.Classifier;
            }
            if ((reference == "RESULT"))
            {
                return this.Result;
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
            if ((feature == "CLASSIFIER"))
            {
                this.Classifier = ((IClassifier)(value));
                return;
            }
            if ((feature == "RESULT"))
            {
                this.Result = ((IOutputPin)(value));
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
            if ((reference == "RESULT"))
            {
                return new ResultProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//CreateObjectAction")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the CreateObjectAction class
        /// </summary>
        public class CreateObjectActionChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private CreateObjectAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public CreateObjectActionChildrenCollection(CreateObjectAction parent)
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
                    if ((this._parent.Result != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
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
                if ((this._parent.Result == null))
                {
                    IOutputPin resultCasted = item.As<IOutputPin>();
                    if ((resultCasted != null))
                    {
                        this._parent.Result = resultCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Result = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Result))
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
                if ((this._parent.Result != null))
                {
                    array[arrayIndex] = this._parent.Result;
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
                if ((this._parent.Result == item))
                {
                    this._parent.Result = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Result).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the CreateObjectAction class
        /// </summary>
        public class CreateObjectActionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private CreateObjectAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public CreateObjectActionReferencedElementsCollection(CreateObjectAction parent)
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
                    if ((this._parent.Classifier != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.Result != null))
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
                if ((this._parent.Classifier == null))
                {
                    IClassifier classifierCasted = item.As<IClassifier>();
                    if ((classifierCasted != null))
                    {
                        this._parent.Classifier = classifierCasted;
                        return;
                    }
                }
                if ((this._parent.Result == null))
                {
                    IOutputPin resultCasted = item.As<IOutputPin>();
                    if ((resultCasted != null))
                    {
                        this._parent.Result = resultCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Classifier = null;
                this._parent.Result = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Classifier))
                {
                    return true;
                }
                if ((item == this._parent.Result))
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
                if ((this._parent.Classifier != null))
                {
                    array[arrayIndex] = this._parent.Classifier;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.Result != null))
                {
                    array[arrayIndex] = this._parent.Result;
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
                if ((this._parent.Classifier == item))
                {
                    this._parent.Classifier = null;
                    return true;
                }
                if ((this._parent.Result == item))
                {
                    this._parent.Result = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Classifier).Concat(this._parent.Result).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the classifier property
        /// </summary>
        private sealed class ClassifierProxy : ModelPropertyChange<ICreateObjectAction, IClassifier>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ClassifierProxy(ICreateObjectAction modelElement) : 
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
        /// Represents a proxy to represent an incremental access to the result property
        /// </summary>
        private sealed class ResultProxy : ModelPropertyChange<ICreateObjectAction, IOutputPin>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ResultProxy(ICreateObjectAction modelElement) : 
                    base(modelElement, "result")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IOutputPin Value
            {
                get
                {
                    return this.ModelElement.Result;
                }
                set
                {
                    this.ModelElement.Result = value;
                }
            }
        }
    }
}

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
using System.Linq;


namespace NMF.AnyText.Metamodel
{
    
    
    /// <summary>
    /// The default implementation of the ParanthesisRule class
    /// </summary>
    [XmlNamespaceAttribute("http://github.com/NMFCode/NMF/AnyText")]
    [XmlNamespacePrefixAttribute("anytext")]
    [ModelRepresentationClassAttribute("http://github.com/NMFCode/NMF/AnyText#//ParanthesisRule")]
    [DebuggerDisplayAttribute("ParanthesisRule {Name}")]
    public partial class ParanthesisRule : ClassRule, IParanthesisRule, IModelElement
    {
        
        private static Lazy<ITypedElement> _openingParanthesisReference = new Lazy<ITypedElement>(RetrieveOpeningParanthesisReference);
        
        /// <summary>
        /// The backing field for the OpeningParanthesis property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IKeywordExpression _openingParanthesis;
        
        private static Lazy<ITypedElement> _innerRuleReference = new Lazy<ITypedElement>(RetrieveInnerRuleReference);
        
        /// <summary>
        /// The backing field for the InnerRule property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IClassRule _innerRule;
        
        private static Lazy<ITypedElement> _closingParanthesisReference = new Lazy<ITypedElement>(RetrieveClosingParanthesisReference);
        
        /// <summary>
        /// The backing field for the ClosingParanthesis property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IKeywordExpression _closingParanthesis;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The OpeningParanthesis property
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IKeywordExpression OpeningParanthesis
        {
            get
            {
                return this._openingParanthesis;
            }
            set
            {
                if ((this._openingParanthesis != value))
                {
                    IKeywordExpression old = this._openingParanthesis;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("OpeningParanthesis", e, _openingParanthesisReference);
                    this._openingParanthesis = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetOpeningParanthesis;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetOpeningParanthesis;
                    }
                    this.OnPropertyChanged("OpeningParanthesis", e, _openingParanthesisReference);
                }
            }
        }
        
        /// <summary>
        /// The InnerRule property
        /// </summary>
        [CategoryAttribute("ParanthesisRule")]
        [XmlAttributeAttribute(true)]
        public IClassRule InnerRule
        {
            get
            {
                return this._innerRule;
            }
            set
            {
                if ((this._innerRule != value))
                {
                    IClassRule old = this._innerRule;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("InnerRule", e, _innerRuleReference);
                    this._innerRule = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetInnerRule;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetInnerRule;
                    }
                    this.OnPropertyChanged("InnerRule", e, _innerRuleReference);
                }
            }
        }
        
        /// <summary>
        /// The ClosingParanthesis property
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IKeywordExpression ClosingParanthesis
        {
            get
            {
                return this._closingParanthesis;
            }
            set
            {
                if ((this._closingParanthesis != value))
                {
                    IKeywordExpression old = this._closingParanthesis;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("ClosingParanthesis", e, _closingParanthesisReference);
                    this._closingParanthesis = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetClosingParanthesis;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetClosingParanthesis;
                    }
                    this.OnPropertyChanged("ClosingParanthesis", e, _closingParanthesisReference);
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
                return base.Children.Concat(new ParanthesisRuleChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ParanthesisRuleReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://github.com/NMFCode/NMF/AnyText#//ParanthesisRule")));
                }
                return _classInstance;
            }
        }
        
        private static ITypedElement RetrieveOpeningParanthesisReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.AnyText.Metamodel.ParanthesisRule.ClassInstance)).Resolve("OpeningParanthesis")));
        }
        
        /// <summary>
        /// Handles the event that the OpeningParanthesis property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetOpeningParanthesis(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.OpeningParanthesis))
            {
                this.OpeningParanthesis = null;
            }
        }
        
        private static ITypedElement RetrieveInnerRuleReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.AnyText.Metamodel.ParanthesisRule.ClassInstance)).Resolve("InnerRule")));
        }
        
        /// <summary>
        /// Handles the event that the InnerRule property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetInnerRule(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.InnerRule))
            {
                this.InnerRule = null;
            }
        }
        
        private static ITypedElement RetrieveClosingParanthesisReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.AnyText.Metamodel.ParanthesisRule.ClassInstance)).Resolve("ClosingParanthesis")));
        }
        
        /// <summary>
        /// Handles the event that the ClosingParanthesis property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetClosingParanthesis(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.ClosingParanthesis))
            {
                this.ClosingParanthesis = null;
            }
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.OpeningParanthesis))
            {
                return ModelHelper.CreatePath("OpeningParanthesis");
            }
            if ((element == this.ClosingParanthesis))
            {
                return ModelHelper.CreatePath("ClosingParanthesis");
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
            if ((reference == "OPENINGPARANTHESIS"))
            {
                return this.OpeningParanthesis;
            }
            if ((reference == "INNERRULE"))
            {
                return this.InnerRule;
            }
            if ((reference == "CLOSINGPARANTHESIS"))
            {
                return this.ClosingParanthesis;
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
            if ((feature == "OPENINGPARANTHESIS"))
            {
                this.OpeningParanthesis = ((IKeywordExpression)(value));
                return;
            }
            if ((feature == "INNERRULE"))
            {
                this.InnerRule = ((IClassRule)(value));
                return;
            }
            if ((feature == "CLOSINGPARANTHESIS"))
            {
                this.ClosingParanthesis = ((IKeywordExpression)(value));
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
            if ((reference == "OPENINGPARANTHESIS"))
            {
                return new OpeningParanthesisProxy(this);
            }
            if ((reference == "INNERRULE"))
            {
                return new InnerRuleProxy(this);
            }
            if ((reference == "CLOSINGPARANTHESIS"))
            {
                return new ClosingParanthesisProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://github.com/NMFCode/NMF/AnyText#//ParanthesisRule")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ParanthesisRule class
        /// </summary>
        public class ParanthesisRuleChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ParanthesisRule _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ParanthesisRuleChildrenCollection(ParanthesisRule parent)
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
                    if ((this._parent.OpeningParanthesis != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.ClosingParanthesis != null))
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
                if ((this._parent.OpeningParanthesis == null))
                {
                    IKeywordExpression openingParanthesisCasted = item.As<IKeywordExpression>();
                    if ((openingParanthesisCasted != null))
                    {
                        this._parent.OpeningParanthesis = openingParanthesisCasted;
                        return;
                    }
                }
                if ((this._parent.ClosingParanthesis == null))
                {
                    IKeywordExpression closingParanthesisCasted = item.As<IKeywordExpression>();
                    if ((closingParanthesisCasted != null))
                    {
                        this._parent.ClosingParanthesis = closingParanthesisCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OpeningParanthesis = null;
                this._parent.ClosingParanthesis = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.OpeningParanthesis))
                {
                    return true;
                }
                if ((item == this._parent.ClosingParanthesis))
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
                if ((this._parent.OpeningParanthesis != null))
                {
                    array[arrayIndex] = this._parent.OpeningParanthesis;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.ClosingParanthesis != null))
                {
                    array[arrayIndex] = this._parent.ClosingParanthesis;
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
                if ((this._parent.OpeningParanthesis == item))
                {
                    this._parent.OpeningParanthesis = null;
                    return true;
                }
                if ((this._parent.ClosingParanthesis == item))
                {
                    this._parent.ClosingParanthesis = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OpeningParanthesis).Concat(this._parent.ClosingParanthesis).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ParanthesisRule class
        /// </summary>
        public class ParanthesisRuleReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ParanthesisRule _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ParanthesisRuleReferencedElementsCollection(ParanthesisRule parent)
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
                    if ((this._parent.OpeningParanthesis != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.InnerRule != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.ClosingParanthesis != null))
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
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
                this._parent.BubbledChange -= this.PropagateValueChanges;
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.OpeningParanthesis == null))
                {
                    IKeywordExpression openingParanthesisCasted = item.As<IKeywordExpression>();
                    if ((openingParanthesisCasted != null))
                    {
                        this._parent.OpeningParanthesis = openingParanthesisCasted;
                        return;
                    }
                }
                if ((this._parent.InnerRule == null))
                {
                    IClassRule innerRuleCasted = item.As<IClassRule>();
                    if ((innerRuleCasted != null))
                    {
                        this._parent.InnerRule = innerRuleCasted;
                        return;
                    }
                }
                if ((this._parent.ClosingParanthesis == null))
                {
                    IKeywordExpression closingParanthesisCasted = item.As<IKeywordExpression>();
                    if ((closingParanthesisCasted != null))
                    {
                        this._parent.ClosingParanthesis = closingParanthesisCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OpeningParanthesis = null;
                this._parent.InnerRule = null;
                this._parent.ClosingParanthesis = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.OpeningParanthesis))
                {
                    return true;
                }
                if ((item == this._parent.InnerRule))
                {
                    return true;
                }
                if ((item == this._parent.ClosingParanthesis))
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
                if ((this._parent.OpeningParanthesis != null))
                {
                    array[arrayIndex] = this._parent.OpeningParanthesis;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.InnerRule != null))
                {
                    array[arrayIndex] = this._parent.InnerRule;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.ClosingParanthesis != null))
                {
                    array[arrayIndex] = this._parent.ClosingParanthesis;
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
                if ((this._parent.OpeningParanthesis == item))
                {
                    this._parent.OpeningParanthesis = null;
                    return true;
                }
                if ((this._parent.InnerRule == item))
                {
                    this._parent.InnerRule = null;
                    return true;
                }
                if ((this._parent.ClosingParanthesis == item))
                {
                    this._parent.ClosingParanthesis = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OpeningParanthesis).Concat(this._parent.InnerRule).Concat(this._parent.ClosingParanthesis).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the OpeningParanthesis property
        /// </summary>
        private sealed class OpeningParanthesisProxy : ModelPropertyChange<IParanthesisRule, IKeywordExpression>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public OpeningParanthesisProxy(IParanthesisRule modelElement) : 
                    base(modelElement, "OpeningParanthesis")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IKeywordExpression Value
            {
                get
                {
                    return this.ModelElement.OpeningParanthesis;
                }
                set
                {
                    this.ModelElement.OpeningParanthesis = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the InnerRule property
        /// </summary>
        private sealed class InnerRuleProxy : ModelPropertyChange<IParanthesisRule, IClassRule>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public InnerRuleProxy(IParanthesisRule modelElement) : 
                    base(modelElement, "InnerRule")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IClassRule Value
            {
                get
                {
                    return this.ModelElement.InnerRule;
                }
                set
                {
                    this.ModelElement.InnerRule = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the ClosingParanthesis property
        /// </summary>
        private sealed class ClosingParanthesisProxy : ModelPropertyChange<IParanthesisRule, IKeywordExpression>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ClosingParanthesisProxy(IParanthesisRule modelElement) : 
                    base(modelElement, "ClosingParanthesis")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IKeywordExpression Value
            {
                get
                {
                    return this.ModelElement.ClosingParanthesis;
                }
                set
                {
                    this.ModelElement.ClosingParanthesis = value;
                }
            }
        }
    }
}
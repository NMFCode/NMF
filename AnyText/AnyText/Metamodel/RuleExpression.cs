//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
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
    /// The default implementation of the RuleExpression class
    /// </summary>
    [XmlNamespaceAttribute("https://github.com/NMFCode/NMF/AnyText")]
    [XmlNamespacePrefixAttribute("anytext")]
    [ModelRepresentationClassAttribute("https://github.com/NMFCode/NMF/AnyText#//RuleExpression")]
    public partial class RuleExpression : ParserExpression, IRuleExpression, IModelElement
    {
        
        private static Lazy<ITypedElement> _ruleReference = new Lazy<ITypedElement>(RetrieveRuleReference);
        
        /// <summary>
        /// The backing field for the Rule property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IRule _rule;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The Rule property
        /// </summary>
        [CategoryAttribute("RuleExpression")]
        [XmlAttributeAttribute(true)]
        public IRule Rule
        {
            get
            {
                return this._rule;
            }
            set
            {
                if ((this._rule != value))
                {
                    IRule old = this._rule;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Rule", e, _ruleReference);
                    this._rule = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetRule;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetRule;
                    }
                    this.OnPropertyChanged("Rule", e, _ruleReference);
                }
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new RuleExpressionReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("https://github.com/NMFCode/NMF/AnyText#//RuleExpression")));
                }
                return _classInstance;
            }
        }
        
        private static ITypedElement RetrieveRuleReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.AnyText.Metamodel.RuleExpression.ClassInstance)).Resolve("Rule")));
        }
        
        /// <summary>
        /// Handles the event that the Rule property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetRule(object sender, EventArgs eventArgs)
        {
            if ((sender == this.Rule))
            {
                this.Rule = null;
            }
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "RULE"))
            {
                return this.Rule;
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
            if ((feature == "RULE"))
            {
                this.Rule = ((IRule)(value));
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
            if ((reference == "RULE"))
            {
                return new RuleProxy(this);
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
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("https://github.com/NMFCode/NMF/AnyText#//RuleExpression")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the RuleExpression class
        /// </summary>
        public class RuleExpressionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private RuleExpression _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public RuleExpressionReferencedElementsCollection(RuleExpression parent)
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
                    if ((this._parent.Rule != null))
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
                if ((this._parent.Rule == null))
                {
                    IRule ruleCasted = item.As<IRule>();
                    if ((ruleCasted != null))
                    {
                        this._parent.Rule = ruleCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Rule = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Rule))
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
                if ((this._parent.Rule != null))
                {
                    array[arrayIndex] = this._parent.Rule;
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
                if ((this._parent.Rule == item))
                {
                    this._parent.Rule = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Rule).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the Rule property
        /// </summary>
        private sealed class RuleProxy : ModelPropertyChange<IRuleExpression, IRule>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public RuleProxy(IRuleExpression modelElement) : 
                    base(modelElement, "Rule")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IRule Value
            {
                get
                {
                    return this.ModelElement.Rule;
                }
                set
                {
                    this.ModelElement.Rule = value;
                }
            }
        }
    }
}

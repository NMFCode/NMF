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
    /// A RedefinableElement is an element that, when defined in the context of a Classifier, can be redefined more specifically or differently in the context of another Classifier that specializes (directly or indirectly) the context Classifier.
    ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//RedefinableElement")]
    [DebuggerDisplayAttribute("RedefinableElement {Name}")]
    public abstract partial class RedefinableElement : NamedElement, IRedefinableElement, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _redefinition_consistentOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveRedefinition_consistentOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _non_leaf_redefinitionOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNon_leaf_redefinitionOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _redefinition_context_validOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveRedefinition_context_validOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isConsistentWithOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsConsistentWithOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isRedefinitionContextValidOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsRedefinitionContextValidOperation);
        
        /// <summary>
        /// The backing field for the IsLeaf property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private bool _isLeaf = false;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _isLeafAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveIsLeafAttribute);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Indicates whether it is possible to further redefine a RedefinableElement. If the value is true, then it is not possible to further redefine the RedefinableElement.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isLeaf")]
        [DescriptionAttribute("Indicates whether it is possible to further redefine a RedefinableElement. If the" +
            " value is true, then it is not possible to further redefine the RedefinableEleme" +
            "nt.\n<p>From package UML::Classification.</p>")]
        [CategoryAttribute("RedefinableElement")]
        [XmlElementNameAttribute("isLeaf")]
        [XmlAttributeAttribute(true)]
        public bool IsLeaf
        {
            get
            {
                return this._isLeaf;
            }
            set
            {
                if ((this._isLeaf != value))
                {
                    bool old = this._isLeaf;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("IsLeaf", e, _isLeafAttribute);
                    this._isLeaf = value;
                    this.OnPropertyChanged("IsLeaf", e, _isLeafAttribute);
                }
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//RedefinableElement")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// A redefining element must be consistent with each redefined element.
        ///redefinedElement-&gt;forAll(re | re.isConsistentWith(self))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Redefinition_consistent(object diagnostics, object context)
        {
            System.Func<IRedefinableElement, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableElement, object, object, bool>>(_redefinition_consistentOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method redefinition_consistent registered. Use the" +
                        " method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _redefinition_consistentOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _redefinition_consistentOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _redefinition_consistentOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveRedefinition_consistentOperation()
        {
            return ClassInstance.LookupOperation("redefinition_consistent");
        }
        
        /// <summary>
        /// A RedefinableElement can only redefine non-leaf RedefinableElements.
        ///redefinedElement-&gt;forAll(re | not re.isLeaf)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Non_leaf_redefinition(object diagnostics, object context)
        {
            System.Func<IRedefinableElement, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableElement, object, object, bool>>(_non_leaf_redefinitionOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method non_leaf_redefinition registered. Use the m" +
                        "ethod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _non_leaf_redefinitionOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _non_leaf_redefinitionOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _non_leaf_redefinitionOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNon_leaf_redefinitionOperation()
        {
            return ClassInstance.LookupOperation("non_leaf_redefinition");
        }
        
        /// <summary>
        /// At least one of the redefinition contexts of the redefining element must be a specialization of at least one of the redefinition contexts for each redefined element.
        ///redefinedElement-&gt;forAll(re | self.isRedefinitionContextValid(re))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Redefinition_context_valid(object diagnostics, object context)
        {
            System.Func<IRedefinableElement, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableElement, object, object, bool>>(_redefinition_context_validOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method redefinition_context_valid registered. Use " +
                        "the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _redefinition_context_validOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _redefinition_context_validOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _redefinition_context_validOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveRedefinition_context_validOperation()
        {
            return ClassInstance.LookupOperation("redefinition_context_valid");
        }
        
        /// <summary>
        /// The query isConsistentWith() specifies, for any two RedefinableElements in a context in which redefinition is possible, whether redefinition would be logically consistent. By default, this is false; this operation must be overridden for subclasses of RedefinableElement to define the consistency conditions.
        ///redefiningElement.isRedefinitionContextValid(self)
        ///result = (false)
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        /// <param name="redefiningElement"></param>
        public bool IsConsistentWith(IRedefinableElement redefiningElement)
        {
            System.Func<IRedefinableElement, IRedefinableElement, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableElement, IRedefinableElement, bool>>(_isConsistentWithOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isConsistentWith registered. Use the method" +
                        " broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isConsistentWithOperation.Value, redefiningElement);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isConsistentWithOperation.Value, e));
            bool result = handler.Invoke(this, redefiningElement);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isConsistentWithOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsConsistentWithOperation()
        {
            return ClassInstance.LookupOperation("isConsistentWith");
        }
        
        /// <summary>
        /// The query isRedefinitionContextValid() specifies whether the redefinition contexts of this RedefinableElement are properly related to the redefinition contexts of the specified RedefinableElement to allow this element to redefine the other. By default at least one of the redefinition contexts of this element must be a specialization of at least one of the redefinition contexts of the specified element.
        ///result = (redefinitionContext-&gt;exists(c | c.allParents()-&gt;includesAll(redefinedElement.redefinitionContext)))
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        /// <param name="redefinedElement"></param>
        public bool IsRedefinitionContextValid(IRedefinableElement redefinedElement)
        {
            System.Func<IRedefinableElement, IRedefinableElement, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IRedefinableElement, IRedefinableElement, bool>>(_isRedefinitionContextValidOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isRedefinitionContextValid registered. Use " +
                        "the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isRedefinitionContextValidOperation.Value, redefinedElement);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isRedefinitionContextValidOperation.Value, e));
            bool result = handler.Invoke(this, redefinedElement);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isRedefinitionContextValidOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsRedefinitionContextValidOperation()
        {
            return ClassInstance.LookupOperation("isRedefinitionContextValid");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveIsLeafAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.RedefinableElement.ClassInstance)).Resolve("isLeaf")));
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "ISLEAF"))
            {
                return this.IsLeaf;
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
            if ((feature == "ISLEAF"))
            {
                this.IsLeaf = ((bool)(value));
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
            if ((attribute == "ISLEAF"))
            {
                return Observable.Box(new IsLeafProxy(this));
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//RedefinableElement")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the isLeaf property
        /// </summary>
        private sealed class IsLeafProxy : ModelPropertyChange<IRedefinableElement, bool>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public IsLeafProxy(IRedefinableElement modelElement) : 
                    base(modelElement, "isLeaf")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override bool Value
            {
                get
                {
                    return this.ModelElement.IsLeaf;
                }
                set
                {
                    this.ModelElement.IsLeaf = value;
                }
            }
        }
    }
}

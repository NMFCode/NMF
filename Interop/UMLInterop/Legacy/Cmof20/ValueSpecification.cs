//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMF.Interop.Legacy.Cmof
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    using NMF.Models.Repository;
    using System.Globalization;
    
    
    /// <summary>
    /// ValueSpecification is an abstract metaclass used to identify a value or values in a model. It may reference an instance or it may be an expression denoting an instance or instances when evaluated. It adds a specialization to Constructs::TypedElement.
    /// </summary>
    [XmlNamespaceAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//ValueSpecification")]
    [DebuggerDisplayAttribute("ValueSpecification {Name}")]
    public abstract partial class ValueSpecification : TypedElement, IValueSpecification, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isComputableOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsComputableOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _integerValueOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIntegerValueOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _booleanValueOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveBooleanValueOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _stringValueOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveStringValueOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _unlimitedValueOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveUnlimitedValueOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isNullOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsNullOperation);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new ValueSpecificationChildrenCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//ValueSpecification")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The query isComputable() determines whether a value specification can be computed in a model. This operation cannot be fully defined in OCL. A conforming implementation is expected to deliver true for this operation for all value specifications that it can compute, and to compute all of those for which the operation is true. A conforming implementation is expected to be able to compute the value of all literals.
        ///result = false
        /// </summary>
        public bool IsComputable()
        {
            System.Func<IValueSpecification, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IValueSpecification, bool>>(_isComputableOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isComputable registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isComputableOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isComputableOperation.Value, e));
            bool result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isComputableOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsComputableOperation()
        {
            return ClassInstance.LookupOperation("isComputable");
        }
        
        /// <summary>
        /// The query integerValue() gives a single Integer value when one can be computed.
        ///result = Set{}
        /// </summary>
        public int IntegerValue()
        {
            System.Func<IValueSpecification, int> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IValueSpecification, int>>(_integerValueOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method integerValue registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _integerValueOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _integerValueOperation.Value, e));
            int result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _integerValueOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIntegerValueOperation()
        {
            return ClassInstance.LookupOperation("integerValue");
        }
        
        /// <summary>
        /// The query booleanValue() gives a single Boolean value when one can be computed.
        ///result = Set{}
        /// </summary>
        public bool BooleanValue()
        {
            System.Func<IValueSpecification, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IValueSpecification, bool>>(_booleanValueOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method booleanValue registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _booleanValueOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _booleanValueOperation.Value, e));
            bool result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _booleanValueOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveBooleanValueOperation()
        {
            return ClassInstance.LookupOperation("booleanValue");
        }
        
        /// <summary>
        /// The query stringValue() gives a single String value when one can be computed.
        ///result = Set{}
        /// </summary>
        public string StringValue()
        {
            System.Func<IValueSpecification, string> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IValueSpecification, string>>(_stringValueOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method stringValue registered. Use the method brok" +
                        "er to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _stringValueOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _stringValueOperation.Value, e));
            string result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _stringValueOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveStringValueOperation()
        {
            return ClassInstance.LookupOperation("stringValue");
        }
        
        /// <summary>
        /// The query unlimitedValue() gives a single UnlimitedNatural value when one can be computed.
        ///result = Set{}
        /// </summary>
        public int UnlimitedValue()
        {
            System.Func<IValueSpecification, int> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IValueSpecification, int>>(_unlimitedValueOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method unlimitedValue registered. Use the method b" +
                        "roker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _unlimitedValueOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _unlimitedValueOperation.Value, e));
            int result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _unlimitedValueOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveUnlimitedValueOperation()
        {
            return ClassInstance.LookupOperation("unlimitedValue");
        }
        
        /// <summary>
        /// The query isNull() returns true when it can be computed that the value is null.
        ///result = false
        /// </summary>
        public bool IsNull()
        {
            System.Func<IValueSpecification, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IValueSpecification, bool>>(_isNullOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isNull registered. Use the method broker to" +
                        " register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isNullOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isNullOperation.Value, e));
            bool result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isNullOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsNullOperation()
        {
            return ClassInstance.LookupOperation("isNull");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//ValueSpecification")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ValueSpecification class
        /// </summary>
        public class ValueSpecificationChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ValueSpecification _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ValueSpecificationChildrenCollection(ValueSpecification parent)
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
    }
}
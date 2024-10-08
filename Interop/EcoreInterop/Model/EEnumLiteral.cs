//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMF.Interop.Ecore
{
    
    
    /// <summary>
    /// The default implementation of the EEnumLiteral class
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/emf/2002/Ecore")]
    [XmlNamespacePrefixAttribute("ecore")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/emf/2002/Ecore#//EEnumLiteral/")]
    [DebuggerDisplayAttribute("EEnumLiteral {Name}")]
    public class EEnumLiteral : ENamedElement, IEEnumLiteral, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Value property
        /// </summary>
        private Nullable<int> _value;
        
        /// <summary>
        /// The backing field for the Instance property
        /// </summary>
        private object _instance;
        
        /// <summary>
        /// The backing field for the Literal property
        /// </summary>
        private string _literal;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The value property
        /// </summary>
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(true)]
        public virtual Nullable<int> Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if ((this._value != value))
                {
                    Nullable<int> old = this._value;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnValueChanging(e);
                    this.OnPropertyChanging("Value", e);
                    this._value = value;
                    this.OnValueChanged(e);
                    this.OnPropertyChanged("Value", e);
                }
            }
        }
        
        /// <summary>
        /// The instance property
        /// </summary>
        [XmlElementNameAttribute("instance")]
        [XmlAttributeAttribute(true)]
        public virtual object Instance
        {
            get
            {
                return this._instance;
            }
            set
            {
                if ((this._instance != value))
                {
                    object old = this._instance;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnInstanceChanging(e);
                    this.OnPropertyChanging("Instance", e);
                    this._instance = value;
                    this.OnInstanceChanged(e);
                    this.OnPropertyChanged("Instance", e);
                }
            }
        }
        
        /// <summary>
        /// The literal property
        /// </summary>
        [XmlElementNameAttribute("literal")]
        [XmlAttributeAttribute(true)]
        public virtual string Literal
        {
            get
            {
                return this._literal;
            }
            set
            {
                if ((this._literal != value))
                {
                    string old = this._literal;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnLiteralChanging(e);
                    this.OnPropertyChanging("Literal", e);
                    this._literal = value;
                    this.OnLiteralChanged(e);
                    this.OnPropertyChanged("Literal", e);
                }
            }
        }
        
        /// <summary>
        /// The eEnum property
        /// </summary>
        [XmlElementNameAttribute("eEnum")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("eLiterals")]
        public virtual IEEnum EEnum
        {
            get
            {
                return ModelHelper.CastAs<IEEnum>(this.Parent);
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
                return base.ReferencedElements.Concat(new EEnumLiteralReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/emf/2002/Ecore#//EEnumLiteral/")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the Value property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> ValueChanging;
        
        /// <summary>
        /// Gets fired when the Value property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> ValueChanged;
        
        /// <summary>
        /// Gets fired before the Instance property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> InstanceChanging;
        
        /// <summary>
        /// Gets fired when the Instance property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> InstanceChanged;
        
        /// <summary>
        /// Gets fired before the Literal property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> LiteralChanging;
        
        /// <summary>
        /// Gets fired when the Literal property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> LiteralChanged;
        
        /// <summary>
        /// Gets fired before the EEnum property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> EEnumChanging;
        
        /// <summary>
        /// Gets fired when the EEnum property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> EEnumChanged;
        
        /// <summary>
        /// Raises the ValueChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnValueChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.ValueChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the ValueChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnValueChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.ValueChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the InstanceChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnInstanceChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.InstanceChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the InstanceChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnInstanceChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.InstanceChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the LiteralChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnLiteralChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.LiteralChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the LiteralChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnLiteralChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.LiteralChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the EEnumChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnEEnumChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.EEnumChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            IEEnum oldEEnum = ModelHelper.CastAs<IEEnum>(oldParent);
            IEEnum newEEnum = ModelHelper.CastAs<IEEnum>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldEEnum, newEEnum);
            this.OnEEnumChanging(e);
            this.OnPropertyChanging("EEnum");
        }
        
        /// <summary>
        /// Raises the EEnumChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnEEnumChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.EEnumChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            IEEnum oldEEnum = ModelHelper.CastAs<IEEnum>(oldParent);
            IEEnum newEEnum = ModelHelper.CastAs<IEEnum>(newParent);
            if ((oldEEnum != null))
            {
                oldEEnum.ELiterals.Remove(this);
            }
            if ((newEEnum != null))
            {
                newEEnum.ELiterals.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldEEnum, newEEnum);
            this.OnEEnumChanged(e);
            this.OnPropertyChanged("EEnum", e);
            base.OnParentChanged(newParent, oldParent);
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "VALUE"))
            {
                return this.Value;
            }
            if ((attribute == "INSTANCE"))
            {
                return this.Instance;
            }
            if ((attribute == "LITERAL"))
            {
                return this.Literal;
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
            if ((feature == "EENUM"))
            {
                this.EEnum = ((IEEnum)(value));
                return;
            }
            if ((feature == "VALUE"))
            {
                this.Value = ((int)(value));
                return;
            }
            if ((feature == "INSTANCE"))
            {
                this.Instance = ((object)(value));
                return;
            }
            if ((feature == "LITERAL"))
            {
                this.Literal = ((string)(value));
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
            if ((attribute == "EENUM"))
            {
                return new EEnumProxy(this);
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
            if ((reference == "EENUM"))
            {
                return new EEnumProxy(this);
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
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/emf/2002/Ecore#//EEnumLiteral/")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the EEnumLiteral class
        /// </summary>
        public class EEnumLiteralReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private EEnumLiteral _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public EEnumLiteralReferencedElementsCollection(EEnumLiteral parent)
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
                    if ((this._parent.EEnum != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }

            /// <inheritdoc />
            protected override void AttachCore()
            {
                this._parent.EEnumChanged += this.PropagateValueChanges;
            }

            /// <inheritdoc />
            protected override void DetachCore()
            {
                this._parent.EEnumChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.EEnum == null))
                {
                    IEEnum eEnumCasted = item.As<IEEnum>();
                    if ((eEnumCasted != null))
                    {
                        this._parent.EEnum = eEnumCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.EEnum = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.EEnum))
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
                if ((this._parent.EEnum != null))
                {
                    array[arrayIndex] = this._parent.EEnum;
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
                if ((this._parent.EEnum == item))
                {
                    this._parent.EEnum = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.EEnum).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the value property
        /// </summary>
        private sealed class ValueProxy : ModelPropertyChange<IEEnumLiteral, Nullable<int>>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ValueProxy(IEEnumLiteral modelElement) : 
                    base(modelElement, "value")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override Nullable<int> Value
            {
                get
                {
                    return this.ModelElement.Value;
                }
                set
                {
                    this.ModelElement.Value = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the instance property
        /// </summary>
        private sealed class InstanceProxy : ModelPropertyChange<IEEnumLiteral, object>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public InstanceProxy(IEEnumLiteral modelElement) : 
                    base(modelElement, "instance")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override object Value
            {
                get
                {
                    return this.ModelElement.Instance;
                }
                set
                {
                    this.ModelElement.Instance = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the literal property
        /// </summary>
        private sealed class LiteralProxy : ModelPropertyChange<IEEnumLiteral, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public LiteralProxy(IEEnumLiteral modelElement) : 
                    base(modelElement, "literal")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.Literal;
                }
                set
                {
                    this.ModelElement.Literal = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the eEnum property
        /// </summary>
        private sealed class EEnumProxy : ModelPropertyChange<IEEnumLiteral, IEEnum>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public EEnumProxy(IEEnumLiteral modelElement) : 
                    base(modelElement, "eEnum")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IEEnum Value
            {
                get
                {
                    return this.ModelElement.EEnum;
                }
                set
                {
                    this.ModelElement.EEnum = value;
                }
            }
        }
    }
}


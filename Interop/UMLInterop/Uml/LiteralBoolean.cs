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
    /// A LiteralBoolean is a specification of a Boolean value.
    ///<p>From package UML::Values.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//LiteralBoolean")]
    [DebuggerDisplayAttribute("LiteralBoolean {Name}")]
    public partial class LiteralBoolean : LiteralSpecification, ILiteralBoolean, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Value property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private bool _value = false;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _valueAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveValueAttribute);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The specified Boolean value.
        ///<p>From package UML::Values.</p>
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("value")]
        [DescriptionAttribute("The specified Boolean value.\n<p>From package UML::Values.</p>")]
        [CategoryAttribute("LiteralBoolean")]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(true)]
        public bool Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if ((this._value != value))
                {
                    bool old = this._value;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Value", e, _valueAttribute);
                    this._value = value;
                    this.OnPropertyChanged("Value", e, _valueAttribute);
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//LiteralBoolean")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveValueAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.LiteralBoolean.ClassInstance)).Resolve("value")));
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
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "VALUE"))
            {
                this.Value = ((bool)(value));
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
            if ((attribute == "VALUE"))
            {
                return Observable.Box(new ValueProxy(this));
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//LiteralBoolean")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the value property
        /// </summary>
        private sealed class ValueProxy : ModelPropertyChange<ILiteralBoolean, bool>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ValueProxy(ILiteralBoolean modelElement) : 
                    base(modelElement, "value")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override bool Value
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
    }
}

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
    /// The default implementation of the KeywordExpression class
    /// </summary>
    [XmlNamespaceAttribute("https://github.com/NMFCode/NMF/AnyText")]
    [XmlNamespacePrefixAttribute("anytext")]
    [ModelRepresentationClassAttribute("https://github.com/NMFCode/NMF/AnyText#//KeywordExpression")]
    public partial class KeywordExpression : ParserExpression, IKeywordExpression, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Keyword property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private string _keyword;
        
        private static Lazy<ITypedElement> _keywordAttribute = new Lazy<ITypedElement>(RetrieveKeywordAttribute);
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The Keyword property
        /// </summary>
        [CategoryAttribute("KeywordExpression")]
        [XmlAttributeAttribute(true)]
        public string Keyword
        {
            get
            {
                return this._keyword;
            }
            set
            {
                if ((this._keyword != value))
                {
                    string old = this._keyword;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Keyword", e, _keywordAttribute);
                    this._keyword = value;
                    this.OnPropertyChanged("Keyword", e, _keywordAttribute);
                }
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("https://github.com/NMFCode/NMF/AnyText#//KeywordExpression")));
                }
                return _classInstance;
            }
        }
        
        private static ITypedElement RetrieveKeywordAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMF.AnyText.Metamodel.KeywordExpression.ClassInstance)).Resolve("Keyword")));
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "KEYWORD"))
            {
                return this.Keyword;
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
            if ((feature == "KEYWORD"))
            {
                this.Keyword = ((string)(value));
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
            if ((attribute == "KEYWORD"))
            {
                return new KeywordProxy(this);
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("https://github.com/NMFCode/NMF/AnyText#//KeywordExpression")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the Keyword property
        /// </summary>
        private sealed class KeywordProxy : ModelPropertyChange<IKeywordExpression, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public KeywordProxy(IKeywordExpression modelElement) : 
                    base(modelElement, "Keyword")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.Keyword;
                }
                set
                {
                    this.ModelElement.Keyword = value;
                }
            }
        }
    }
}

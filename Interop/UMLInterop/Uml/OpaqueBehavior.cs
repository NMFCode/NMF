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
    /// An OpaqueBehavior is a Behavior whose specification is given in a textual language other than UML.
    ///<p>From package UML::CommonBehavior.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueBehavior")]
    [DebuggerDisplayAttribute("OpaqueBehavior {Name}")]
    public partial class OpaqueBehavior : Behavior, IOpaqueBehavior, IModelElement
    {
        
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
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        public OpaqueBehavior()
        {
            this._body = new ObservableList<string>();
            this._body.CollectionChanging += this.BodyCollectionChanging;
            this._body.CollectionChanged += this.BodyCollectionChanged;
            this._language = new ObservableOrderedSet<string>();
            this._language.CollectionChanging += this.LanguageCollectionChanging;
            this._language.CollectionChanged += this.LanguageCollectionChanged;
        }
        
        /// <summary>
        /// Specifies the behavior in one or more languages.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("body")]
        [DescriptionAttribute("Specifies the behavior in one or more languages.\n<p>From package UML::CommonBehav" +
            "ior.</p>")]
        [CategoryAttribute("OpaqueBehavior")]
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
        /// Languages the body strings use in the same order as the body strings.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("language")]
        [DescriptionAttribute("Languages the body strings use in the same order as the body strings.\n<p>From pac" +
            "kage UML::CommonBehavior.</p>")]
        [CategoryAttribute("OpaqueBehavior")]
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
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueBehavior")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveBodyAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.OpaqueBehavior.ClassInstance)).Resolve("body")));
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
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.OpaqueBehavior.ClassInstance)).Resolve("language")));
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
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueBehavior")));
            }
            return _classInstance;
        }
    }
}

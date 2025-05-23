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
    /// The public interface for LiteralRule
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(LiteralRule))]
    [XmlDefaultImplementationTypeAttribute(typeof(LiteralRule))]
    [ModelRepresentationClassAttribute("https://github.com/NMFCode/NMF/AnyText#//LiteralRule")]
    public partial interface ILiteralRule : IModelElement
    {
        
        /// <summary>
        /// The Literal property
        /// </summary>
        [CategoryAttribute("LiteralRule")]
        [XmlAttributeAttribute(true)]
        string Literal
        {
            get;
            set;
        }
        
        /// <summary>
        /// The Value property
        /// </summary>
        [CategoryAttribute("LiteralRule")]
        [XmlAttributeAttribute(true)]
        Nullable<int> Value
        {
            get;
            set;
        }
        
        /// <summary>
        /// The Keyword property
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IFormattedExpression Keyword
        {
            get;
            set;
        }
    }
}

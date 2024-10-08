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
using System.Globalization;
using System.Linq;


namespace NMF.Interop.Legacy.Cmof
{
    
    
    /// <summary>
    /// The public interface for Link
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Link))]
    [XmlDefaultImplementationTypeAttribute(typeof(Link))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Link")]
    public interface ILink : IModelElement
    {
        
        /// <summary>
        /// The secondElement property
        /// </summary>
        [DisplayNameAttribute("secondElement")]
        [CategoryAttribute("Link")]
        [XmlElementNameAttribute("secondElement")]
        [XmlAttributeAttribute(true)]
        IElement SecondElement
        {
            get;
            set;
        }
        
        /// <summary>
        /// The firstElement property
        /// </summary>
        [DisplayNameAttribute("firstElement")]
        [CategoryAttribute("Link")]
        [XmlElementNameAttribute("firstElement")]
        [XmlAttributeAttribute(true)]
        IElement FirstElement
        {
            get;
            set;
        }
        
        /// <summary>
        /// The association property
        /// </summary>
        [DisplayNameAttribute("association")]
        [CategoryAttribute("Link")]
        [XmlElementNameAttribute("association")]
        [XmlAttributeAttribute(true)]
        IAssociation Association
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherLink"></param>
        bool Equals(ILink otherLink);
        
        /// <summary>
        /// 
        /// </summary>
        IModelElement Delete();
    }
}

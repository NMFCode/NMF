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


namespace NMF.Interop.Cmof
{
    
    
    /// <summary>
    /// The public interface for Generalization
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Generalization))]
    [XmlDefaultImplementationTypeAttribute(typeof(Generalization))]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Generalization")]
    public interface IGeneralization : IModelElement, IDirectedRelationship
    {
        
        /// <summary>
        /// Indicates whether the specific classifier can be used wherever the general classifier can be used. If true, the execution traces of the specific classifier will be a superset of the execution traces of the general classifier.
        /// </summary>
        [DefaultValueAttribute(true)]
        [DisplayNameAttribute("isSubstitutable")]
        [DescriptionAttribute("Indicates whether the specific classifier can be used wherever the general classi" +
            "fier can be used. If true, the execution traces of the specific classifier will " +
            "be a superset of the execution traces of the general classifier.")]
        [CategoryAttribute("Generalization")]
        [XmlElementNameAttribute("isSubstitutable")]
        [XmlAttributeAttribute(true)]
        Nullable<bool> IsSubstitutable
        {
            get;
            set;
        }
        
        /// <summary>
        /// References the general classifier in the Generalization relationship.
        /// </summary>
        [DisplayNameAttribute("general")]
        [DescriptionAttribute("References the general classifier in the Generalization relationship.")]
        [CategoryAttribute("Generalization")]
        [XmlElementNameAttribute("general")]
        [XmlAttributeAttribute(true)]
        IClassifier General
        {
            get;
            set;
        }
        
        /// <summary>
        /// References the specializing classifier in the Generalization relationship.
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("specific")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("generalization")]
        IClassifier Specific
        {
            get;
            set;
        }
    }
}

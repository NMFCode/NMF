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
    /// The public interface for Generalization
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Generalization))]
    [XmlDefaultImplementationTypeAttribute(typeof(Generalization))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Generalization")]
    public interface IGeneralization : IModelElement, IDirectedRelationship
    {
        
        /// <summary>
        /// Indicates whether the specific Classifier can be used wherever the general Classifier can be used. If true, the execution traces of the specific Classifier shall be a superset of the execution traces of the general Classifier. If false, there is no such constraint on execution traces. If unset, the modeler has not stated whether there is such a constraint or not.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [DefaultValueAttribute(true)]
        [DisplayNameAttribute("isSubstitutable")]
        [DescriptionAttribute(@"Indicates whether the specific Classifier can be used wherever the general Classifier can be used. If true, the execution traces of the specific Classifier shall be a superset of the execution traces of the general Classifier. If false, there is no such constraint on execution traces. If unset, the modeler has not stated whether there is such a constraint or not.
<p>From package UML::Classification.</p>")]
        [CategoryAttribute("Generalization")]
        [XmlElementNameAttribute("isSubstitutable")]
        [XmlAttributeAttribute(true)]
        Nullable<bool> IsSubstitutable
        {
            get;
            set;
        }
        
        /// <summary>
        /// The general classifier in the Generalization relationship.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [DisplayNameAttribute("general")]
        [DescriptionAttribute("The general classifier in the Generalization relationship.\n<p>From package UML::C" +
            "lassification.</p>")]
        [CategoryAttribute("Generalization")]
        [XmlElementNameAttribute("general")]
        [XmlAttributeAttribute(true)]
        IClassifier General
        {
            get;
            set;
        }
        
        /// <summary>
        /// Represents a set of instances of Generalization.  A Generalization may appear in many GeneralizationSets.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("generalizationSet")]
        [DescriptionAttribute("Represents a set of instances of Generalization.  A Generalization may appear in " +
            "many GeneralizationSets.\n<p>From package UML::Classification.</p>")]
        [CategoryAttribute("Generalization")]
        [XmlElementNameAttribute("generalizationSet")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("generalization")]
        [ConstantAttribute()]
        ISetExpression<IGeneralizationSet> GeneralizationSet
        {
            get;
        }
        
        /// <summary>
        /// The specializing Classifier in the Generalization relationship.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("specific")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("generalization")]
        IClassifier Specific
        {
            get;
            set;
        }
    }
}

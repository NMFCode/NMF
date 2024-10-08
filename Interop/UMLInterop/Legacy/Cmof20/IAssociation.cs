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
    /// The public interface for Association
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Association))]
    [XmlDefaultImplementationTypeAttribute(typeof(Association))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Association")]
    public interface IAssociation : IModelElement, IRelationship, IClassifier
    {
        
        /// <summary>
        /// Specifies whether the association is derived from other model elements such as other associations or constraints. The default value is false.
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isDerived")]
        [DescriptionAttribute("Specifies whether the association is derived from other model elements such as ot" +
            "her associations or constraints. The default value is false.")]
        [CategoryAttribute("Association")]
        [XmlElementNameAttribute("isDerived")]
        [XmlAttributeAttribute(true)]
        bool IsDerived
        {
            get;
            set;
        }
        
        /// <summary>
        /// Each end represents participation of instances of the classifier connected to the end in links of the association. This is an ordered association.
        /// </summary>
        [LowerBoundAttribute(2)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("memberEnd")]
        [DescriptionAttribute("Each end represents participation of instances of the classifier connected to the" +
            " end in links of the association. This is an ordered association.")]
        [CategoryAttribute("Association")]
        [XmlElementNameAttribute("memberEnd")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("association")]
        [ConstantAttribute()]
        IOrderedSetExpression<IProperty> MemberEnd
        {
            get;
        }
        
        /// <summary>
        /// The navigable ends that are owned by the association itself.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("navigableOwnedEnd")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IProperty> NavigableOwnedEnd
        {
            get;
        }
        
        /// <summary>
        /// The ends that are owned by the association itself. This is an ordered association.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedEnd")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("owningAssociation")]
        [ConstantAttribute()]
        IListExpression<IProperty> OwnedEnd
        {
            get;
        }
        
        /// <summary>
        /// Association ends of associations with more than two ends must be owned by the association.
        ///if memberEnd-&gt;size() &gt; 2 then ownedEnd-&gt;includesAll(memberEnd)
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Association_ends(object diagnostics, object context);
    }
}

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
    /// The public interface for DataType
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(DataType))]
    [XmlDefaultImplementationTypeAttribute(typeof(DataType))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//DataType")]
    public interface IDataType : IModelElement, IClassifier
    {
        
        /// <summary>
        /// The Operations owned by the DataType. Subsets Classifier::feature and Element::ownedMember.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedOperation")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("datatype")]
        [ConstantAttribute()]
        IOrderedSetExpression<NMF.Interop.Legacy.Cmof.IOperation> OwnedOperation
        {
            get;
        }
        
        /// <summary>
        /// The Attributes owned by the DataType. Subsets Classifier::attribute and Element::ownedMember.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedAttribute")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("datatype")]
        [ConstantAttribute()]
        IOrderedSetExpression<IProperty> OwnedAttribute
        {
            get;
        }
    }
}
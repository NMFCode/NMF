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
    /// The public interface for MultiplicityElement
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(MultiplicityElement))]
    [XmlDefaultImplementationTypeAttribute(typeof(MultiplicityElement))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//MultiplicityElement")]
    public interface IMultiplicityElement : IModelElement, IElement
    {
        
        /// <summary>
        /// For a multivalued multiplicity, this attribute specifies whether the values in an instantiation of this element are sequentially ordered. Default is false.
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isOrdered")]
        [DescriptionAttribute("For a multivalued multiplicity, this attribute specifies whether the values in an" +
            " instantiation of this element are sequentially ordered. Default is false.")]
        [CategoryAttribute("MultiplicityElement")]
        [XmlElementNameAttribute("isOrdered")]
        [XmlAttributeAttribute(true)]
        bool IsOrdered
        {
            get;
            set;
        }
        
        /// <summary>
        /// For a multivalued multiplicity, this attributes specifies whether the values in an instantiation of this element are unique. Default is true.
        /// </summary>
        [DefaultValueAttribute(true)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isUnique")]
        [DescriptionAttribute("For a multivalued multiplicity, this attributes specifies whether the values in a" +
            "n instantiation of this element are unique. Default is true.")]
        [CategoryAttribute("MultiplicityElement")]
        [XmlElementNameAttribute("isUnique")]
        [XmlAttributeAttribute(true)]
        bool IsUnique
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies the lower bound of the multiplicity interval. Default is one.
        /// </summary>
        [DefaultValueAttribute(1)]
        [DisplayNameAttribute("lower")]
        [DescriptionAttribute("Specifies the lower bound of the multiplicity interval. Default is one.")]
        [CategoryAttribute("MultiplicityElement")]
        [XmlElementNameAttribute("lower")]
        [XmlAttributeAttribute(true)]
        Nullable<int> Lower
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies the upper bound of the multiplicity interval. Default is one.
        /// </summary>
        [DefaultValueAttribute(1)]
        [DisplayNameAttribute("upper")]
        [DescriptionAttribute("Specifies the upper bound of the multiplicity interval. Default is one.")]
        [CategoryAttribute("MultiplicityElement")]
        [XmlElementNameAttribute("upper")]
        [XmlAttributeAttribute(true)]
        Nullable<int> Upper
        {
            get;
            set;
        }
        
        /// <summary>
        /// A multiplicity must define at least one valid cardinality that is greater than zero.
        ///upperBound()-&gt;notEmpty() implies upperBound() &gt; 0
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Upper_gt_0(object diagnostics, object context);
        
        /// <summary>
        /// The lower bound must be a non-negative integer literal.
        ///lowerBound()-&gt;notEmpty() implies lowerBound() &gt;= 0
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Lower_ge_0(object diagnostics, object context);
        
        /// <summary>
        /// The upper bound must be greater than or equal to the lower bound.
        ///(upperBound()-&gt;notEmpty() and lowerBound()-&gt;notEmpty()) implies upperBound() &gt;= lowerBound()
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Upper_ge_lower(object diagnostics, object context);
        
        /// <summary>
        /// The query lowerBound() returns the lower bound of the multiplicity as an integer.
        ///result = if lower-&gt;notEmpty() then lower else 1 endif
        /// </summary>
        int LowerBound();
        
        /// <summary>
        /// The query upperBound() returns the upper bound of the multiplicity for a bounded multiplicity as an unlimited natural.
        ///result = if upper-&gt;notEmpty() then upper else 1 endif
        /// </summary>
        int UpperBound();
        
        /// <summary>
        /// The query isMultivalued() checks whether this multiplicity has an upper bound greater than one.
        ///upperBound()-&gt;notEmpty()
        ///result = upperBound() &gt; 1
        /// </summary>
        bool IsMultivalued();
        
        /// <summary>
        /// The query includesCardinality() checks whether the specified cardinality is valid for this multiplicity.
        ///upperBound()-&gt;notEmpty() and lowerBound()-&gt;notEmpty()
        ///result = (lowerBound() &lt;= C) and (upperBound() &gt;= C)
        /// </summary>
        /// <param name="c"></param>
        bool IncludesCardinality(int c);
        
        /// <summary>
        /// The query includesMultiplicity() checks whether this multiplicity includes all the cardinalities allowed by the specified multiplicity.
        ///self.upperBound()-&gt;notEmpty() and self.lowerBound()-&gt;notEmpty() and M.upperBound()-&gt;notEmpty() and M.lowerBound()-&gt;notEmpty()
        ///result = (self.lowerBound() &lt;= M.lowerBound()) and (self.upperBound() &gt;= M.upperBound())
        /// </summary>
        /// <param name="m"></param>
        bool IncludesMultiplicity(IMultiplicityElement m);
    }
}

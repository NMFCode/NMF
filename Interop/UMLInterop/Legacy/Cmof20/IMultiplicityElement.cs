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
        ///upperBound()->notEmpty() implies upperBound() > 0
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Upper_gt_0(object diagnostics, object context);
        
        /// <summary>
        /// The lower bound must be a non-negative integer literal.
        ///lowerBound()->notEmpty() implies lowerBound() >= 0
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Lower_ge_0(object diagnostics, object context);
        
        /// <summary>
        /// The upper bound must be greater than or equal to the lower bound.
        ///(upperBound()->notEmpty() and lowerBound()->notEmpty()) implies upperBound() >= lowerBound()
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Upper_ge_lower(object diagnostics, object context);
        
        /// <summary>
        /// The query lowerBound() returns the lower bound of the multiplicity as an integer.
        ///result = if lower->notEmpty() then lower else 1 endif
        /// </summary>
        int LowerBound();
        
        /// <summary>
        /// The query upperBound() returns the upper bound of the multiplicity for a bounded multiplicity as an unlimited natural.
        ///result = if upper->notEmpty() then upper else 1 endif
        /// </summary>
        int UpperBound();
        
        /// <summary>
        /// The query isMultivalued() checks whether this multiplicity has an upper bound greater than one.
        ///upperBound()->notEmpty()
        ///result = upperBound() > 1
        /// </summary>
        bool IsMultivalued();
        
        /// <summary>
        /// The query includesCardinality() checks whether the specified cardinality is valid for this multiplicity.
        ///upperBound()->notEmpty() and lowerBound()->notEmpty()
        ///result = (lowerBound() <= C) and (upperBound() >= C)
        /// </summary>
        /// <param name="c"></param>
        bool IncludesCardinality(int c);
        
        /// <summary>
        /// The query includesMultiplicity() checks whether this multiplicity includes all the cardinalities allowed by the specified multiplicity.
        ///self.upperBound()->notEmpty() and self.lowerBound()->notEmpty() and M.upperBound()->notEmpty() and M.lowerBound()->notEmpty()
        ///result = (self.lowerBound() <= M.lowerBound()) and (self.upperBound() >= M.upperBound())
        /// </summary>
        /// <param name="m"></param>
        bool IncludesMultiplicity(IMultiplicityElement m);
    }
}
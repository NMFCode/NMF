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
    /// The public interface for OpaqueExpression
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(OpaqueExpression))]
    [XmlDefaultImplementationTypeAttribute(typeof(OpaqueExpression))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//OpaqueExpression")]
    public interface IOpaqueExpression : IModelElement, IValueSpecification
    {
        
        /// <summary>
        /// A textual definition of the behavior of the OpaqueExpression, possibly in multiple languages.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("body")]
        [DescriptionAttribute("A textual definition of the behavior of the OpaqueExpression, possibly in multipl" +
            "e languages.\n<p>From package UML::Values.</p>")]
        [CategoryAttribute("OpaqueExpression")]
        [XmlElementNameAttribute("body")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        IListExpression<string> Body
        {
            get;
        }
        
        /// <summary>
        /// Specifies the languages used to express the textual bodies of the OpaqueExpression.  Languages are matched to body Strings by order. The interpretation of the body depends on the languages. If the languages are unspecified, they may be implicit from the expression body or the context.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("language")]
        [DescriptionAttribute(@"Specifies the languages used to express the textual bodies of the OpaqueExpression.  Languages are matched to body Strings by order. The interpretation of the body depends on the languages. If the languages are unspecified, they may be implicit from the expression body or the context.
<p>From package UML::Values.</p>")]
        [CategoryAttribute("OpaqueExpression")]
        [XmlElementNameAttribute("language")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        IOrderedSetExpression<string> Language
        {
            get;
        }
        
        /// <summary>
        /// Specifies the behavior of the OpaqueExpression as a UML Behavior.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("behavior")]
        [DescriptionAttribute("Specifies the behavior of the OpaqueExpression as a UML Behavior.\n<p>From package" +
            " UML::Values.</p>")]
        [CategoryAttribute("OpaqueExpression")]
        [XmlElementNameAttribute("behavior")]
        [XmlAttributeAttribute(true)]
        IBehavior Behavior
        {
            get;
            set;
        }
        
        /// <summary>
        /// If the language attribute is not empty, then the size of the body and language arrays must be the same.
        ///language-&gt;notEmpty() implies (_&apos;body&apos;-&gt;size() = language-&gt;size())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Language_body_size(object diagnostics, object context);
        
        /// <summary>
        /// The behavior must have exactly one return result parameter.
        ///behavior &lt;&gt; null implies
        ///   behavior.ownedParameter-&gt;select(direction=ParameterDirectionKind::return)-&gt;size() = 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool One_return_result_parameter(object diagnostics, object context);
        
        /// <summary>
        /// The behavior may only have return result parameters.
        ///behavior &lt;&gt; null implies behavior.ownedParameter-&gt;select(direction&lt;&gt;ParameterDirectionKind::return)-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Only_return_result_parameters(object diagnostics, object context);
        
        /// <summary>
        /// The query isIntegral() tells whether an expression is intended to produce an Integer.
        ///result = (false)
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        bool IsIntegral();
        
        /// <summary>
        /// The query isNonNegative() tells whether an integer expression has a non-negative value.
        ///self.isIntegral()
        ///result = (false)
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        bool IsNonNegative();
        
        /// <summary>
        /// The query isPositive() tells whether an integer expression has a positive value.
        ///self.isIntegral()
        ///result = (false)
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        bool IsPositive();
        
        /// <summary>
        /// Derivation for OpaqueExpression::/result
        ///result = (if behavior = null then
        ///	null
        ///else
        ///	behavior.ownedParameter-&gt;first()
        ///endif)
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        NMF.Interop.Uml.IParameter GetResult();
        
        /// <summary>
        /// The query value() gives an integer value for an expression intended to produce one.
        ///self.isIntegral()
        ///result = (0)
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        int Value();
    }
}

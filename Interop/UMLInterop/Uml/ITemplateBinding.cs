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
    /// The public interface for TemplateBinding
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(TemplateBinding))]
    [XmlDefaultImplementationTypeAttribute(typeof(TemplateBinding))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//TemplateBinding")]
    public interface ITemplateBinding : IModelElement, IDirectedRelationship
    {
        
        /// <summary>
        /// The TemplateParameterSubstitutions owned by this TemplateBinding.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("parameterSubstitution")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("templateBinding")]
        [ConstantAttribute()]
        IOrderedSetExpression<ITemplateParameterSubstitution> ParameterSubstitution
        {
            get;
        }
        
        /// <summary>
        /// The TemplateSignature for the template that is the target of this TemplateBinding.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("signature")]
        [DescriptionAttribute("The TemplateSignature for the template that is the target of this TemplateBinding" +
            ".\n<p>From package UML::CommonStructure.</p>")]
        [CategoryAttribute("TemplateBinding")]
        [XmlElementNameAttribute("signature")]
        [XmlAttributeAttribute(true)]
        ITemplateSignature Signature
        {
            get;
            set;
        }
        
        /// <summary>
        /// The TemplateableElement that is bound by this TemplateBinding.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("boundElement")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("templateBinding")]
        ITemplateableElement BoundElement
        {
            get;
            set;
        }
        
        /// <summary>
        /// Each parameterSubstitution must refer to a formal TemplateParameter of the target TemplateSignature.
        ///parameterSubstitution-&gt;forAll(b | signature.parameter-&gt;includes(b.formal))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Parameter_substitution_formal(object diagnostics, object context);
        
        /// <summary>
        /// A TemplateBiinding contains at most one TemplateParameterSubstitution for each formal TemplateParameter of the target TemplateSignature.
        ///signature.parameter-&gt;forAll(p | parameterSubstitution-&gt;select(b | b.formal = p)-&gt;size() &lt;= 1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool One_parameter_substitution(object diagnostics, object context);
    }
}

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
    /// The public interface for Expression
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Expression))]
    [XmlDefaultImplementationTypeAttribute(typeof(Expression))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Expression")]
    public interface IExpression : IModelElement, IValueSpecification
    {
        
        /// <summary>
        /// The symbol associated with this node in the expression tree.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("symbol")]
        [DescriptionAttribute("The symbol associated with this node in the expression tree.\n<p>From package UML:" +
            ":Values.</p>")]
        [CategoryAttribute("Expression")]
        [XmlElementNameAttribute("symbol")]
        [XmlAttributeAttribute(true)]
        string Symbol
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies a sequence of operand ValueSpecifications.
        ///&lt;p&gt;From package UML::Values.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("operand")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IValueSpecification> Operand
        {
            get;
        }
    }
}

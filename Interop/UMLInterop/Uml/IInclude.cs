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
    /// The public interface for Include
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Include))]
    [XmlDefaultImplementationTypeAttribute(typeof(Include))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Include")]
    public interface IInclude : IModelElement, IDirectedRelationship, INamedElement
    {
        
        /// <summary>
        /// The UseCase that is to be included.
        ///&lt;p&gt;From package UML::UseCases.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("addition")]
        [DescriptionAttribute("The UseCase that is to be included.\n<p>From package UML::UseCases.</p>")]
        [CategoryAttribute("Include")]
        [XmlElementNameAttribute("addition")]
        [XmlAttributeAttribute(true)]
        IUseCase Addition
        {
            get;
            set;
        }
        
        /// <summary>
        /// The UseCase which includes the addition and owns the Include relationship.
        ///&lt;p&gt;From package UML::UseCases.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("includingCase")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("include")]
        IUseCase IncludingCase
        {
            get;
            set;
        }
    }
}

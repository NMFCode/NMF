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
    /// The public interface for Dependency
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Dependency))]
    [XmlDefaultImplementationTypeAttribute(typeof(Dependency))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Dependency")]
    public interface IDependency : IModelElement, IDirectedRelationship, IPackageableElement
    {
        
        /// <summary>
        /// The Element(s) dependent on the supplier Element(s). In some cases (such as a trace Abstraction) the assignment of direction (that is, the designation of the client Element) is at the discretion of the modeler and is a stipulation.
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("client")]
        [DescriptionAttribute(@"The Element(s) dependent on the supplier Element(s). In some cases (such as a trace Abstraction) the assignment of direction (that is, the designation of the client Element) is at the discretion of the modeler and is a stipulation.
<p>From package UML::CommonStructure.</p>")]
        [CategoryAttribute("Dependency")]
        [XmlElementNameAttribute("client")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<INamedElement> Client
        {
            get;
        }
        
        /// <summary>
        /// The Element(s) on which the client Element(s) depend in some respect. The modeler may stipulate a sense of Dependency direction suitable for their domain.
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("supplier")]
        [DescriptionAttribute("The Element(s) on which the client Element(s) depend in some respect. The modeler" +
            " may stipulate a sense of Dependency direction suitable for their domain.\n<p>Fro" +
            "m package UML::CommonStructure.</p>")]
        [CategoryAttribute("Dependency")]
        [XmlElementNameAttribute("supplier")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<INamedElement> Supplier
        {
            get;
        }
    }
}

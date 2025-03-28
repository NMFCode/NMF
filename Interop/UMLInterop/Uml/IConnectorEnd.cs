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
    /// The public interface for ConnectorEnd
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ConnectorEnd))]
    [XmlDefaultImplementationTypeAttribute(typeof(ConnectorEnd))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ConnectorEnd")]
    public interface IConnectorEnd : IModelElement, IMultiplicityElement
    {
        
        /// <summary>
        /// Indicates the role of the internal structure of a Classifier with the Port to which the ConnectorEnd is attached.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("partWithPort")]
        [DescriptionAttribute("Indicates the role of the internal structure of a Classifier with the Port to whi" +
            "ch the ConnectorEnd is attached.\n<p>From package UML::StructuredClassifiers.</p>" +
            "")]
        [CategoryAttribute("ConnectorEnd")]
        [XmlElementNameAttribute("partWithPort")]
        [XmlAttributeAttribute(true)]
        IProperty PartWithPort
        {
            get;
            set;
        }
        
        /// <summary>
        /// The ConnectableElement attached at this ConnectorEnd. When an instance of the containing Classifier is created, a link may (depending on the multiplicities) be created to an instance of the Classifier that types this ConnectableElement.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("role")]
        [DescriptionAttribute(@"The ConnectableElement attached at this ConnectorEnd. When an instance of the containing Classifier is created, a link may (depending on the multiplicities) be created to an instance of the Classifier that types this ConnectableElement.
<p>From package UML::StructuredClassifiers.</p>")]
        [CategoryAttribute("ConnectorEnd")]
        [XmlElementNameAttribute("role")]
        [XmlAttributeAttribute(true)]
        IConnectableElement Role
        {
            get;
            set;
        }
        
        /// <summary>
        /// If a ConnectorEnd references a partWithPort, then the role must be a Port that is defined or inherited by the type of the partWithPort.
        ///partWithPort-&gt;notEmpty() implies 
        ///  (role.oclIsKindOf(Port) and partWithPort.type.oclAsType(Namespace).member-&gt;includes(role))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Role_and_part_with_port(object diagnostics, object context);
        
        /// <summary>
        /// If a ConnectorEnd is attached to a Port of the containing Classifier, partWithPort will be empty.
        ///(role.oclIsKindOf(Port) and role.owner = connector.owner) implies partWithPort-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Part_with_port_empty(object diagnostics, object context);
        
        /// <summary>
        /// The multiplicity of the ConnectorEnd may not be more general than the multiplicity of the corresponding end of the Association typing the owning Connector, if any.
        ///self.compatibleWith(definingEnd)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Multiplicity(object diagnostics, object context);
        
        /// <summary>
        /// The Property held in self.partWithPort must not be a Port.
        ///partWithPort-&gt;notEmpty() implies not partWithPort.oclIsKindOf(Port)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Self_part_with_port(object diagnostics, object context);
        
        /// <summary>
        /// Derivation for ConnectorEnd::/definingEnd : Property
        ///result = (if connector.type = null 
        ///then
        ///  null 
        ///else
        ///  let index : Integer = connector.end-&gt;indexOf(self) in
        ///    connector.type.memberEnd-&gt;at(index)
        ///endif)
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        IProperty GetDefiningEnd();
    }
}

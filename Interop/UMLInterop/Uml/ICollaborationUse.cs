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
    /// The public interface for CollaborationUse
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(CollaborationUse))]
    [XmlDefaultImplementationTypeAttribute(typeof(CollaborationUse))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//CollaborationUse")]
    public interface ICollaborationUse : IModelElement, INamedElement
    {
        
        /// <summary>
        /// A mapping between features of the Collaboration and features of the owning Classifier. This mapping indicates which ConnectableElement of the Classifier plays which role(s) in the Collaboration. A ConnectableElement may be bound to multiple roles in the same CollaborationUse (that is, it may play multiple roles).
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("roleBinding")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IDependency> RoleBinding
        {
            get;
        }
        
        /// <summary>
        /// The Collaboration which is used in this CollaborationUse. The Collaboration defines the cooperation between its roles which are mapped to ConnectableElements relating to the Classifier owning the CollaborationUse.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("type")]
        [DescriptionAttribute(@"The Collaboration which is used in this CollaborationUse. The Collaboration defines the cooperation between its roles which are mapped to ConnectableElements relating to the Classifier owning the CollaborationUse.
<p>From package UML::StructuredClassifiers.</p>")]
        [CategoryAttribute("CollaborationUse")]
        [XmlElementNameAttribute("type")]
        [XmlAttributeAttribute(true)]
        ICollaboration Type
        {
            get;
            set;
        }
        
        /// <summary>
        /// All the client elements of a roleBinding are in one Classifier and all supplier elements of a roleBinding are in one Collaboration.
        ///roleBinding-&gt;collect(client)-&gt;forAll(ne1, ne2 |
        ///  ne1.oclIsKindOf(ConnectableElement) and ne2.oclIsKindOf(ConnectableElement) and
        ///    let ce1 : ConnectableElement = ne1.oclAsType(ConnectableElement), ce2 : ConnectableElement = ne2.oclAsType(ConnectableElement) in
        ///      ce1.structuredClassifier = ce2.structuredClassifier)
        ///and
        ///  roleBinding-&gt;collect(supplier)-&gt;forAll(ne1, ne2 |
        ///  ne1.oclIsKindOf(ConnectableElement) and ne2.oclIsKindOf(ConnectableElement) and
        ///    let ce1 : ConnectableElement = ne1.oclAsType(ConnectableElement), ce2 : ConnectableElement = ne2.oclAsType(ConnectableElement) in
        ///      ce1.collaboration = ce2.collaboration)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Client_elements(object diagnostics, object context);
        
        /// <summary>
        /// Every collaborationRole in the Collaboration is bound within the CollaborationUse.
        ///type.collaborationRole-&gt;forAll(role | roleBinding-&gt;exists(rb | rb.supplier-&gt;includes(role)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Every_role(object diagnostics, object context);
        
        /// <summary>
        /// Connectors in a Collaboration typing a CollaborationUse must have corresponding Connectors between elements bound in the context Classifier, and these corresponding Connectors must have the same or more general type than the Collaboration Connectors.
        ///type.ownedConnector-&gt;forAll(connector |
        ///  let rolesConnectedInCollab : Set(ConnectableElement) = connector.end.role-&gt;asSet(),
        ///        relevantBindings : Set(Dependency) = roleBinding-&gt;select(rb | rb.supplier-&gt;intersection(rolesConnectedInCollab)-&gt;notEmpty()),
        ///        boundRoles : Set(ConnectableElement) = relevantBindings-&gt;collect(client.oclAsType(ConnectableElement))-&gt;asSet(),
        ///        contextClassifier : StructuredClassifier = boundRoles-&gt;any(true).structuredClassifier-&gt;any(true) in
        ///          contextClassifier.ownedConnector-&gt;exists( correspondingConnector | 
        ///              correspondingConnector.end.role-&gt;forAll( role | boundRoles-&gt;includes(role) )
        ///              and (connector.type-&gt;notEmpty() and correspondingConnector.type-&gt;notEmpty()) implies connector.type-&gt;forAll(conformsTo(correspondingConnector.type)) )
        ///)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Connectors(object diagnostics, object context);
    }
}

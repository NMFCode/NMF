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
    /// The public interface for Interface
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Interface))]
    [XmlDefaultImplementationTypeAttribute(typeof(Interface))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Interface")]
    public interface IInterface : IModelElement, IClassifier
    {
        
        /// <summary>
        /// References all the Classifiers that are defined (nested) within the Interface.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("nestedClassifier")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IClassifier> NestedClassifier
        {
            get;
        }
        
        /// <summary>
        /// The attributes (i.e., the Properties) owned by the Interface.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedAttribute")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("interface")]
        [ConstantAttribute()]
        IOrderedSetExpression<IProperty> OwnedAttribute
        {
            get;
        }
        
        /// <summary>
        /// Receptions that objects providing this Interface are willing to accept.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedReception")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IReception> OwnedReception
        {
            get;
        }
        
        /// <summary>
        /// References a ProtocolStateMachine specifying the legal sequences of the invocation of the BehavioralFeatures described in the Interface.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("protocol")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IProtocolStateMachine Protocol
        {
            get;
            set;
        }
        
        /// <summary>
        /// References all the Interfaces redefined by this Interface.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("redefinedInterface")]
        [DescriptionAttribute("References all the Interfaces redefined by this Interface.\n<p>From package UML::S" +
            "impleClassifiers.</p>")]
        [CategoryAttribute("Interface")]
        [XmlElementNameAttribute("redefinedInterface")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<IInterface> RedefinedInterface
        {
            get;
        }
        
        /// <summary>
        /// The Operations owned by the Interface.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedOperation")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("interface")]
        [ConstantAttribute()]
        IOrderedSetExpression<NMF.Interop.Uml.IOperation> OwnedOperation
        {
            get;
        }
        
        /// <summary>
        /// The visibility of all Features owned by an Interface must be public.
        ///feature->forAll(visibility = VisibilityKind::public)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Visibility(object diagnostics, object context);
        
        /// <summary>
        /// Creates a property with the specified name, type, lower bound, and upper bound as an owned attribute of this interface.
        /// </summary>
        /// <param name="name">The name for the new attribute, or null.</param>
        /// <param name="type">The type for the new attribute, or null.</param>
        /// <param name="lower">The lower bound for the new attribute.</param>
        /// <param name="upper">The upper bound for the new attribute.</param>
        IProperty CreateOwnedAttribute(string name, NMF.Interop.Uml.IType type, int lower, object upper);
        
        /// <summary>
        /// Creates an operation with the specified name, parameter names, parameter types, and return type (or null) as an owned operation of this interface.
        /// </summary>
        /// <param name="name">The name for the new operation, or null.</param>
        /// <param name="parameterNames">The parameter names for the new operation, or null.</param>
        /// <param name="parameterTypes">The parameter types for the new operation, or null.</param>
        /// <param name="returnType">The return type for the new operation, or null.</param>
        NMF.Interop.Uml.IOperation CreateOwnedOperation(string name, IEnumerable<string> parameterNames, IEnumerable<NMF.Interop.Uml.IType> parameterTypes, NMF.Interop.Uml.IType returnType);
    }
}

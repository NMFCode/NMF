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
    /// The public interface for InstanceSpecification
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(InstanceSpecification))]
    [XmlDefaultImplementationTypeAttribute(typeof(InstanceSpecification))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//InstanceSpecification")]
    public interface IInstanceSpecification : IModelElement, IDeployedArtifact, IPackageableElement, IDeploymentTarget
    {
        
        /// <summary>
        /// The Classifier or Classifiers of the represented instance. If multiple Classifiers are specified, the instance is classified by all of them.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("classifier")]
        [DescriptionAttribute("The Classifier or Classifiers of the represented instance. If multiple Classifier" +
            "s are specified, the instance is classified by all of them.\n<p>From package UML:" +
            ":Classification.</p>")]
        [CategoryAttribute("InstanceSpecification")]
        [XmlElementNameAttribute("classifier")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<IClassifier> Classifier
        {
            get;
        }
        
        /// <summary>
        /// A Slot giving the value or values of a StructuralFeature of the instance. An InstanceSpecification can have one Slot per StructuralFeature of its Classifiers, including inherited features. It is not necessary to model a Slot for every StructuralFeature, in which case the InstanceSpecification is a partial description.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("slot")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("owningInstance")]
        [ConstantAttribute()]
        IOrderedSetExpression<ISlot> Slot
        {
            get;
        }
        
        /// <summary>
        /// A specification of how to compute, derive, or construct the instance.
        ///<p>From package UML::Classification.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("specification")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        IValueSpecification Specification
        {
            get;
            set;
        }
        
        /// <summary>
        /// An InstanceSpecification can act as a DeployedArtifact if it represents an instance of an Artifact.
        ///deploymentForArtifact->notEmpty() implies classifier->exists(oclIsKindOf(Artifact))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Deployment_artifact(object diagnostics, object context);
        
        /// <summary>
        /// No more than one slot in an InstanceSpecification may have the same definingFeature.
        ///classifier->forAll(c | (c.allSlottableFeatures()->forAll(f | slot->select(s | s.definingFeature = f)->size() <= 1)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Structural_feature(object diagnostics, object context);
        
        /// <summary>
        /// The definingFeature of each slot is a StructuralFeature related to a classifier of the InstanceSpecification, including direct attributes, inherited attributes, private attributes in generalizations, and memberEnds of Associations, but excluding redefined StructuralFeatures.
        ///slot->forAll(s | classifier->exists (c | c.allSlottableFeatures()->includes (s.definingFeature)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Defining_feature(object diagnostics, object context);
        
        /// <summary>
        /// An InstanceSpecification can act as a DeploymentTarget if it represents an instance of a Node and functions as a part in the internal structure of an encompassing Node.
        ///deployment->notEmpty() implies classifier->exists(node | node.oclIsKindOf(Node) and Node.allInstances()->exists(n | n.part->exists(p | p.type = node)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Deployment_target(object diagnostics, object context);
    }
}

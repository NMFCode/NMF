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
    /// The public interface for Behavior
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Behavior))]
    [XmlDefaultImplementationTypeAttribute(typeof(Behavior))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Behavior")]
    public interface IBehavior : IModelElement, NMF.Interop.Uml.IClass
    {
        
        /// <summary>
        /// Tells whether the Behavior can be invoked while it is still executing from a previous invocation.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DefaultValueAttribute(true)]
        [DisplayNameAttribute("isReentrant")]
        [DescriptionAttribute("Tells whether the Behavior can be invoked while it is still executing from a prev" +
            "ious invocation.\n<p>From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("Behavior")]
        [XmlElementNameAttribute("isReentrant")]
        [XmlAttributeAttribute(true)]
        Nullable<bool> IsReentrant
        {
            get;
            set;
        }
        
        /// <summary>
        /// Designates a BehavioralFeature that the Behavior implements. The BehavioralFeature must be owned by the BehavioredClassifier that owns the Behavior or be inherited by it. The Parameters of the BehavioralFeature and the implementing Behavior must match. A Behavior does not need to have a specification, in which case it either is the classifierBehavior of a BehavioredClassifier or it can only be invoked by another Behavior of the Classifier.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DisplayNameAttribute("specification")]
        [DescriptionAttribute(@"Designates a BehavioralFeature that the Behavior implements. The BehavioralFeature must be owned by the BehavioredClassifier that owns the Behavior or be inherited by it. The Parameters of the BehavioralFeature and the implementing Behavior must match. A Behavior does not need to have a specification, in which case it either is the classifierBehavior of a BehavioredClassifier or it can only be invoked by another Behavior of the Classifier.
<p>From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("Behavior")]
        [XmlElementNameAttribute("specification")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("method")]
        IBehavioralFeature Specification
        {
            get;
            set;
        }
        
        /// <summary>
        /// References a list of Parameters to the Behavior which describes the order and type of arguments that can be given when the Behavior is invoked and of the values which will be returned when the Behavior completes its execution.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedParameter")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<NMF.Interop.Uml.IParameter> OwnedParameter
        {
            get;
        }
        
        /// <summary>
        /// The ParameterSets owned by this Behavior.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedParameterSet")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IParameterSet> OwnedParameterSet
        {
            get;
        }
        
        /// <summary>
        /// An optional set of Constraints specifying what is fulfilled after the execution of the Behavior is completed, if its precondition was fulfilled before its invocation.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("postcondition")]
        [DescriptionAttribute("An optional set of Constraints specifying what is fulfilled after the execution o" +
            "f the Behavior is completed, if its precondition was fulfilled before its invoca" +
            "tion.\n<p>From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("Behavior")]
        [XmlElementNameAttribute("postcondition")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<IConstraint> Postcondition
        {
            get;
        }
        
        /// <summary>
        /// An optional set of Constraints specifying what must be fulfilled before the Behavior is invoked.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("precondition")]
        [DescriptionAttribute("An optional set of Constraints specifying what must be fulfilled before the Behav" +
            "ior is invoked.\n<p>From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("Behavior")]
        [XmlElementNameAttribute("precondition")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<IConstraint> Precondition
        {
            get;
        }
        
        /// <summary>
        /// References the Behavior that this Behavior redefines. A subtype of Behavior may redefine any other subtype of Behavior. If the Behavior implements a BehavioralFeature, it replaces the redefined Behavior. If the Behavior is a classifierBehavior, it extends the redefined Behavior.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("redefinedBehavior")]
        [DescriptionAttribute(@"References the Behavior that this Behavior redefines. A subtype of Behavior may redefine any other subtype of Behavior. If the Behavior implements a BehavioralFeature, it replaces the redefined Behavior. If the Behavior is a classifierBehavior, it extends the redefined Behavior.
<p>From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("Behavior")]
        [XmlElementNameAttribute("redefinedBehavior")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        ISetExpression<IBehavior> RedefinedBehavior
        {
            get;
        }
        
        /// <summary>
        /// There may be at most one Behavior for a given pairing of BehavioredClassifier (as owner of the Behavior) and BehavioralFeature (as specification of the Behavior).
        ///specification <> null implies _'context'.ownedBehavior->select(specification=self.specification)->size() = 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Most_one_behavior(object diagnostics, object context);
        
        /// <summary>
        /// If a Behavior has a specification BehavioralFeature, then it must have the same number of ownedParameters as its specification. The Behavior Parameters must also "match" the BehavioralParameter Parameters, but the exact requirements for this matching are not formalized.
        ///specification <> null implies ownedParameter->size() = specification.ownedParameter->size()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Parameters_match(object diagnostics, object context);
        
        /// <summary>
        /// The specification BehavioralFeature must be a feature (possibly inherited) of the context BehavioredClassifier of the Behavior.
        ///_'context'.feature->includes(specification)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Feature_of_context_classifier(object diagnostics, object context);
        
        /// <summary>
        /// A Behavior that is directly owned as a nestedClassifier does not have a context. Otherwise, to determine the context of a Behavior, find the first BehavioredClassifier reached by following the chain of owner relationships from the Behavior, if any. If there is such a BehavioredClassifier, then it is the context, unless it is itself a Behavior with a non-empty context, in which case that is also the context for the original Behavior.
        ///result = (if nestingClass <> null then
        ///    null
        ///else
        ///    let b:BehavioredClassifier = self.behavioredClassifier(self.owner) in
        ///    if b.oclIsKindOf(Behavior) and b.oclAsType(Behavior)._'context' <> null then 
        ///        b.oclAsType(Behavior)._'context'
        ///    else 
        ///        b 
        ///    endif
        ///endif
        ///        )
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        IBehavioredClassifier GetContext();
        
        /// <summary>
        /// The first BehavioredClassifier reached by following the chain of owner relationships from the Behavior, if any.
        ///if from.oclIsKindOf(BehavioredClassifier) then
        ///    from.oclAsType(BehavioredClassifier)
        ///else if from.owner = null then
        ///    null
        ///else
        ///    self.behavioredClassifier(from.owner)
        ///endif
        ///endif
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        /// <param name="from"></param>
        IBehavioredClassifier BehavioredClassifier(IElement from);
        
        /// <summary>
        /// The in and inout ownedParameters of the Behavior.
        ///result = (ownedParameter->select(direction=ParameterDirectionKind::_'in' or direction=ParameterDirectionKind::inout))
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        IOrderedSetExpression<NMF.Interop.Uml.IParameter> InputParameters();
        
        /// <summary>
        /// The out, inout and return ownedParameters.
        ///result = (ownedParameter->select(direction=ParameterDirectionKind::out or direction=ParameterDirectionKind::inout or direction=ParameterDirectionKind::return))
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        IOrderedSetExpression<NMF.Interop.Uml.IParameter> OutputParameters();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.Identifier;
using NMFExamples.Pcm.Core;
using NMFExamples.Pcm.Core.Entity;
using NMFExamples.Pcm.Parameter;
using NMFExamples.Pcm.Reliability;
using NMFExamples.Pcm.Repository;
using NMFExamples.Pcm.Seff.Seff_performance;
using NMFExamples.Pcm.Seff.Seff_reliability;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using global::System.Collections;
using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using global::System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMFExamples.Pcm.Seff
{
    
    
    /// <summary>
    /// The public interface for ResourceDemandingBehaviour
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ResourceDemandingBehaviour))]
    [XmlDefaultImplementationTypeAttribute(typeof(ResourceDemandingBehaviour))]
    public interface IResourceDemandingBehaviour : IModelElement, IIdentifier
    {
        
        /// <summary>
        /// The abstractLoopAction_ResourceDemandingBehaviour property
        /// </summary>
        IAbstractLoopAction AbstractLoopAction_ResourceDemandingBehaviour
        {
            get;
            set;
        }
        
        /// <summary>
        /// The abstractBranchTransition_ResourceDemandingBehaviour property
        /// </summary>
        IAbstractBranchTransition AbstractBranchTransition_ResourceDemandingBehaviour
        {
            get;
            set;
        }
        
        /// <summary>
        /// The steps_Behaviour property
        /// </summary>
        IListExpression<IAbstractAction> Steps_Behaviour
        {
            get;
        }
        
        /// <summary>
        /// Gets fired before the AbstractLoopAction_ResourceDemandingBehaviour property changes its value
        /// </summary>
        event global::System.EventHandler<ValueChangedEventArgs> AbstractLoopAction_ResourceDemandingBehaviourChanging;
        
        /// <summary>
        /// Gets fired when the AbstractLoopAction_ResourceDemandingBehaviour property changed its value
        /// </summary>
        event global::System.EventHandler<ValueChangedEventArgs> AbstractLoopAction_ResourceDemandingBehaviourChanged;
        
        /// <summary>
        /// Gets fired before the AbstractBranchTransition_ResourceDemandingBehaviour property changes its value
        /// </summary>
        event global::System.EventHandler<ValueChangedEventArgs> AbstractBranchTransition_ResourceDemandingBehaviourChanging;
        
        /// <summary>
        /// Gets fired when the AbstractBranchTransition_ResourceDemandingBehaviour property changed its value
        /// </summary>
        event global::System.EventHandler<ValueChangedEventArgs> AbstractBranchTransition_ResourceDemandingBehaviourChanged;
    }
}


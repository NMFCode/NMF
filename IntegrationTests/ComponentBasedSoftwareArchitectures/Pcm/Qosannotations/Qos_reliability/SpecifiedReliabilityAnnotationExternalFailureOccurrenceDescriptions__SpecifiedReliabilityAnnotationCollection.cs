//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.Pcm.Qosannotations;
using NMFExamples.Pcm.Reliability;
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

namespace NMFExamples.Pcm.Qosannotations.Qos_reliability
{
    
    
    public class SpecifiedReliabilityAnnotationExternalFailureOccurrenceDescriptions__SpecifiedReliabilityAnnotationCollection : ObservableOppositeList<ISpecifiedReliabilityAnnotation, IExternalFailureOccurrenceDescription>
    {
        
        public SpecifiedReliabilityAnnotationExternalFailureOccurrenceDescriptions__SpecifiedReliabilityAnnotationCollection(ISpecifiedReliabilityAnnotation parent) : 
                base(parent)
        {
        }
        
        private void OnItemParentChanged(object sender, ValueChangedEventArgs e)
        {
            if ((e.NewValue != this.Parent))
            {
                this.Remove(((IExternalFailureOccurrenceDescription)(sender)));
            }
        }
        
        protected override void SetOpposite(IExternalFailureOccurrenceDescription item, ISpecifiedReliabilityAnnotation parent)
        {
            if ((parent != null))
            {
                item.ParentChanged += this.OnItemParentChanged;
                item.SpecifiedReliabilityAnnotation__ExternalFailureOccurrenceDescription = parent;
            }
            else
            {
                item.ParentChanged -= this.OnItemParentChanged;
                if ((item.SpecifiedReliabilityAnnotation__ExternalFailureOccurrenceDescription == this.Parent))
                {
                    item.SpecifiedReliabilityAnnotation__ExternalFailureOccurrenceDescription = parent;
                }
            }
        }
    }
}

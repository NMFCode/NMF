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
    /// A DurationObservation is a reference to a duration during an execution. It points out the NamedElement(s) in the model to observe and whether the observations are when this NamedElement is entered or when it is exited.
    ///<p>From package UML::Values.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//DurationObservation")]
    [DebuggerDisplayAttribute("DurationObservation {Name}")]
    public partial class DurationObservation : Observation, IDurationObservation, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _first_event_multiplicityOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveFirst_event_multiplicityOperation);
        
        /// <summary>
        /// The backing field for the FirstEvent property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableSet<bool> _firstEvent;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _firstEventAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveFirstEventAttribute);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _eventReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveEventReference);
        
        /// <summary>
        /// The backing field for the Event property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationOrderedSet<INamedElement> _event;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        public DurationObservation()
        {
            this._firstEvent = new ObservableSet<bool>();
            this._firstEvent.CollectionChanging += this.FirstEventCollectionChanging;
            this._firstEvent.CollectionChanged += this.FirstEventCollectionChanged;
            this._event = new ObservableAssociationOrderedSet<INamedElement>();
            this._event.CollectionChanging += this.EventCollectionChanging;
            this._event.CollectionChanged += this.EventCollectionChanged;
        }
        
        /// <summary>
        /// The value of firstEvent[i] is related to event[i] (where i is 1 or 2). If firstEvent[i] is true, then the corresponding observation event is the first time instant the execution enters event[i]. If firstEvent[i] is false, then the corresponding observation event is the time instant the execution exits event[i].
        ///<p>From package UML::Values.</p>
        /// </summary>
        [UpperBoundAttribute(2)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("firstEvent")]
        [DescriptionAttribute(@"The value of firstEvent[i] is related to event[i] (where i is 1 or 2). If firstEvent[i] is true, then the corresponding observation event is the first time instant the execution enters event[i]. If firstEvent[i] is false, then the corresponding observation event is the time instant the execution exits event[i].
<p>From package UML::Values.</p>")]
        [CategoryAttribute("DurationObservation")]
        [XmlElementNameAttribute("firstEvent")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public ISetExpression<bool> FirstEvent
        {
            get
            {
                return this._firstEvent;
            }
        }
        
        /// <summary>
        /// The DurationObservation is determined as the duration between the entering or exiting of a single event Element during execution, or the entering/exiting of one event Element and the entering/exiting of a second.
        ///<p>From package UML::Values.</p>
        /// </summary>
        [LowerBoundAttribute(1)]
        [UpperBoundAttribute(2)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("event")]
        [DescriptionAttribute("The DurationObservation is determined as the duration between the entering or exi" +
            "ting of a single event Element during execution, or the entering/exiting of one " +
            "event Element and the entering/exiting of a second.\n<p>From package UML::Values." +
            "</p>")]
        [CategoryAttribute("DurationObservation")]
        [XmlElementNameAttribute("event")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IOrderedSetExpression<INamedElement> Event
        {
            get
            {
                return this._event;
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new DurationObservationReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//DurationObservation")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The multiplicity of firstEvent must be 2 if the multiplicity of event is 2. Otherwise the multiplicity of firstEvent is 0.
        ///if (event->size() = 2)
        ///  then (firstEvent->size() = 2) else (firstEvent->size() = 0)
        ///endif
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool First_event_multiplicity(object diagnostics, object context)
        {
            System.Func<IDurationObservation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IDurationObservation, object, object, bool>>(_first_event_multiplicityOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method first_event_multiplicity registered. Use th" +
                        "e method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _first_event_multiplicityOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _first_event_multiplicityOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _first_event_multiplicityOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveFirst_event_multiplicityOperation()
        {
            return ClassInstance.LookupOperation("first_event_multiplicity");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveFirstEventAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.DurationObservation.ClassInstance)).Resolve("firstEvent")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the FirstEvent property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void FirstEventCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("FirstEvent", e, _firstEventAttribute);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the FirstEvent property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void FirstEventCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("FirstEvent", e, _firstEventAttribute);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveEventReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.DurationObservation.ClassInstance)).Resolve("event")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Event property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void EventCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Event", e, _eventReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Event property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void EventCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Event", e, _eventReference);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "EVENT"))
            {
                if ((index < this.Event.Count))
                {
                    return this.Event[index];
                }
                else
                {
                    return null;
                }
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "EVENT"))
            {
                return this._event;
            }
            if ((feature == "FIRSTEVENT"))
            {
                return this._firstEvent;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//DurationObservation")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the DurationObservation class
        /// </summary>
        public class DurationObservationReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private DurationObservation _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public DurationObservationReferencedElementsCollection(DurationObservation parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    count = (count + this._parent.Event.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.Event.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.Event.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                INamedElement eventCasted = item.As<INamedElement>();
                if ((eventCasted != null))
                {
                    if ((this._parent.Event.Count < 2))
                    {
                        this._parent.Event.Add(eventCasted);
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Event.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.Event.Contains(item))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
                IEnumerator<IModelElement> eventEnumerator = this._parent.Event.GetEnumerator();
                try
                {
                    for (
                    ; eventEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = eventEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    eventEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                INamedElement namedElementItem = item.As<INamedElement>();
                if (((namedElementItem != null) 
                            && this._parent.Event.Remove(namedElementItem)))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Event).GetEnumerator();
            }
        }
    }
}

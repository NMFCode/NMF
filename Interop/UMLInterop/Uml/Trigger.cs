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
    /// A Trigger specifies a specific point  at which an Event occurrence may trigger an effect in a Behavior. A Trigger may be qualified by the Port on which the Event occurred.
    ///<p>From package UML::CommonBehavior.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Trigger")]
    [DebuggerDisplayAttribute("Trigger {Name}")]
    public partial class Trigger : NamedElement, ITrigger, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _trigger_with_portsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveTrigger_with_portsOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _eventReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveEventReference);
        
        /// <summary>
        /// The backing field for the Event property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private NMF.Interop.Uml.IEvent _event;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _portReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrievePortReference);
        
        /// <summary>
        /// The backing field for the Port property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationSet<IPort> _port;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        public Trigger()
        {
            this._port = new ObservableAssociationSet<IPort>();
            this._port.CollectionChanging += this.PortCollectionChanging;
            this._port.CollectionChanged += this.PortCollectionChanged;
        }
        
        /// <summary>
        /// The Event that detected by the Trigger.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DisplayNameAttribute("event")]
        [DescriptionAttribute("The Event that detected by the Trigger.\n<p>From package UML::CommonBehavior.</p>")]
        [CategoryAttribute("Trigger")]
        [XmlElementNameAttribute("event")]
        [XmlAttributeAttribute(true)]
        public NMF.Interop.Uml.IEvent Event
        {
            get
            {
                return this._event;
            }
            set
            {
                if ((this._event != value))
                {
                    NMF.Interop.Uml.IEvent old = this._event;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Event", e, _eventReference);
                    this._event = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetEvent;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetEvent;
                    }
                    this.OnPropertyChanged("Event", e, _eventReference);
                }
            }
        }
        
        /// <summary>
        /// A optional Port of through which the given effect is detected.
        ///<p>From package UML::CommonBehavior.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("port")]
        [DescriptionAttribute("A optional Port of through which the given effect is detected.\n<p>From package UM" +
            "L::CommonBehavior.</p>")]
        [CategoryAttribute("Trigger")]
        [XmlElementNameAttribute("port")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public ISetExpression<IPort> Port
        {
            get
            {
                return this._port;
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new TriggerReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Trigger")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If a Trigger specifies one or more ports, the event of the Trigger must be a MessageEvent.
        ///port->notEmpty() implies event.oclIsKindOf(MessageEvent)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Trigger_with_ports(object diagnostics, object context)
        {
            System.Func<ITrigger, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ITrigger, object, object, bool>>(_trigger_with_portsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method trigger_with_ports registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _trigger_with_portsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _trigger_with_portsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _trigger_with_portsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveTrigger_with_portsOperation()
        {
            return ClassInstance.LookupOperation("trigger_with_ports");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveEventReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Trigger.ClassInstance)).Resolve("event")));
        }
        
        /// <summary>
        /// Handles the event that the Event property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetEvent(object sender, System.EventArgs eventArgs)
        {
            this.Event = null;
        }
        
        private static NMF.Models.Meta.ITypedElement RetrievePortReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Trigger.ClassInstance)).Resolve("port")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Port property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void PortCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Port", e, _portReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Port property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void PortCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Port", e, _portReference);
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
                return this.Event;
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
            if ((feature == "PORT"))
            {
                return this._port;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "EVENT"))
            {
                this.Event = ((NMF.Interop.Uml.IEvent)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given reference
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="reference">The requested reference in upper case</param>
        protected override NMF.Expressions.INotifyExpression<NMF.Models.IModelElement> GetExpressionForReference(string reference)
        {
            if ((reference == "EVENT"))
            {
                return new EventProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Trigger")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Trigger class
        /// </summary>
        public class TriggerReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Trigger _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public TriggerReferencedElementsCollection(Trigger parent)
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
                    if ((this._parent.Event != null))
                    {
                        count = (count + 1);
                    }
                    count = (count + this._parent.Port.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
                this._parent.Port.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
                this._parent.Port.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.Event == null))
                {
                    NMF.Interop.Uml.IEvent eventCasted = item.As<NMF.Interop.Uml.IEvent>();
                    if ((eventCasted != null))
                    {
                        this._parent.Event = eventCasted;
                        return;
                    }
                }
                IPort portCasted = item.As<IPort>();
                if ((portCasted != null))
                {
                    this._parent.Port.Add(portCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Event = null;
                this._parent.Port.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Event))
                {
                    return true;
                }
                if (this._parent.Port.Contains(item))
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
                if ((this._parent.Event != null))
                {
                    array[arrayIndex] = this._parent.Event;
                    arrayIndex = (arrayIndex + 1);
                }
                IEnumerator<IModelElement> portEnumerator = this._parent.Port.GetEnumerator();
                try
                {
                    for (
                    ; portEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = portEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    portEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                if ((this._parent.Event == item))
                {
                    this._parent.Event = null;
                    return true;
                }
                IPort portItem = item.As<IPort>();
                if (((portItem != null) 
                            && this._parent.Port.Remove(portItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Event).Concat(this._parent.Port).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the event property
        /// </summary>
        private sealed class EventProxy : ModelPropertyChange<ITrigger, NMF.Interop.Uml.IEvent>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public EventProxy(ITrigger modelElement) : 
                    base(modelElement, "event")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override NMF.Interop.Uml.IEvent Value
            {
                get
                {
                    return this.ModelElement.Event;
                }
                set
                {
                    this.ModelElement.Event = value;
                }
            }
        }
    }
}

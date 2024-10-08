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
    /// ActivityGroup is an abstract class for defining sets of ActivityNodes and ActivityEdges in an Activity.
    ///&lt;p&gt;From package UML::Activities.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityGroup")]
    [DebuggerDisplayAttribute("ActivityGroup {Name}")]
    public abstract partial class ActivityGroup : NamedElement, IActivityGroup, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _nodes_and_edgesOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNodes_and_edgesOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _not_containedOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNot_containedOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _containingActivityOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveContainingActivityOperation);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new ActivityGroupChildrenCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityGroup")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// All containedNodes and containeEdges of an ActivityGroup must be in the same Activity as the group.
        ///containedNode-&gt;forAll(activity = self.containingActivity()) and 
        ///containedEdge-&gt;forAll(activity = self.containingActivity())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Nodes_and_edges(object diagnostics, object context)
        {
            System.Func<IActivityGroup, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActivityGroup, object, object, bool>>(_nodes_and_edgesOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method nodes_and_edges registered. Use the method " +
                        "broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _nodes_and_edgesOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _nodes_and_edgesOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _nodes_and_edgesOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNodes_and_edgesOperation()
        {
            return ClassInstance.LookupOperation("nodes_and_edges");
        }
        
        /// <summary>
        /// No containedNode or containedEdge of an ActivityGroup may be contained by its subgroups or its superGroups, transitively.
        ///subgroup-&gt;closure(subgroup).containedNode-&gt;excludesAll(containedNode) and
        ///superGroup-&gt;closure(superGroup).containedNode-&gt;excludesAll(containedNode) and 
        ///subgroup-&gt;closure(subgroup).containedEdge-&gt;excludesAll(containedEdge) and 
        ///superGroup-&gt;closure(superGroup).containedEdge-&gt;excludesAll(containedEdge)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Not_contained(object diagnostics, object context)
        {
            System.Func<IActivityGroup, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActivityGroup, object, object, bool>>(_not_containedOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method not_contained registered. Use the method br" +
                        "oker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _not_containedOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _not_containedOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _not_containedOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNot_containedOperation()
        {
            return ClassInstance.LookupOperation("not_contained");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IActivity ContainingActivity()
        {
            System.Func<IActivityContent, IActivity> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActivityContent, IActivity>>(_containingActivityOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method containingActivity registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _containingActivityOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _containingActivityOperation.Value, e));
            IActivity result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _containingActivityOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveContainingActivityOperation()
        {
            return ClassInstance.LookupOperation("containingActivity");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityGroup")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ActivityGroup class
        /// </summary>
        public class ActivityGroupChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ActivityGroup _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ActivityGroupChildrenCollection(ActivityGroup parent)
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
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.ComponentBasedSystems;
using NMFExamples.ComponentBasedSystems.Assembly;
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMFExamples.ComponentBasedSystems.Deployment
{
    
    
    /// <summary>
    /// The default implementation of the Environment_MM06 class
    /// </summary>
    [XmlNamespaceAttribute("http://sdq.ipd.kit.edu/ComponentBasedSystem/Deployment/")]
    [XmlNamespacePrefixAttribute("deploy")]
    [ModelRepresentationClassAttribute("http://sdq.ipd.kit.edu/ComponentBasedSystem/#//Deployment/Environment_MM06")]
    public partial class Environment_MM06 : ModelElement, IEnvironment_MM06, IModelElement
    {
        
        private static Lazy<ITypedElement> _containersReference = new Lazy<ITypedElement>(RetrieveContainersReference);
        
        /// <summary>
        /// The backing field for the Containers property
        /// </summary>
        private Environment_MM06ContainersCollection _containers;
        
        private static Lazy<ITypedElement> _linksReference = new Lazy<ITypedElement>(RetrieveLinksReference);
        
        /// <summary>
        /// The backing field for the Links property
        /// </summary>
        private ObservableCompositionOrderedSet<ILink> _links;
        
        private static IClass _classInstance;
        
        public Environment_MM06()
        {
            this._containers = new Environment_MM06ContainersCollection(this);
            this._containers.CollectionChanging += this.ContainersCollectionChanging;
            this._containers.CollectionChanged += this.ContainersCollectionChanged;
            this._links = new ObservableCompositionOrderedSet<ILink>(this);
            this._links.CollectionChanging += this.LinksCollectionChanging;
            this._links.CollectionChanged += this.LinksCollectionChanged;
        }
        
        /// <summary>
        /// The Containers property
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("Environment")]
        [ConstantAttribute()]
        public virtual IOrderedSetExpression<IContainer_MM06> Containers
        {
            get
            {
                return this._containers;
            }
        }
        
        /// <summary>
        /// The Links property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public virtual IOrderedSetExpression<ILink> Links
        {
            get
            {
                return this._links;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new Environment_MM06ChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new Environment_MM06ReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.kit.edu/ComponentBasedSystem/#//Deployment/Environment_MM06")));
                }
                return _classInstance;
            }
        }
        
        private static ITypedElement RetrieveContainersReference()
        {
            return ((ITypedElement)(((ModelElement)(Environment_MM06.ClassInstance)).Resolve("Containers")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Containers property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ContainersCollectionChanging(object sender, NMF.Collections.ObjectModel.NotifyCollectionChangingEventArgs e)
        {
            this.OnCollectionChanging("Containers", e, _containersReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Containers property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ContainersCollectionChanged(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Containers", e, _containersReference);
        }
        
        private static ITypedElement RetrieveLinksReference()
        {
            return ((ITypedElement)(((ModelElement)(Environment_MM06.ClassInstance)).Resolve("Links")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Links property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void LinksCollectionChanging(object sender, NMF.Collections.ObjectModel.NotifyCollectionChangingEventArgs e)
        {
            this.OnCollectionChanging("Links", e, _linksReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Links property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void LinksCollectionChanged(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Links", e, _linksReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int containersIndex = ModelHelper.IndexOfReference(this.Containers, element);
            if ((containersIndex != -1))
            {
                return ModelHelper.CreatePath("Containers", containersIndex);
            }
            int linksIndex = ModelHelper.IndexOfReference(this.Links, element);
            if ((linksIndex != -1))
            {
                return ModelHelper.CreatePath("Links", linksIndex);
            }
            return base.GetRelativePathForNonIdentifiedChild(element);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "CONTAINERS"))
            {
                if ((index < this.Containers.Count))
                {
                    return this.Containers[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "LINKS"))
            {
                if ((index < this.Links.Count))
                {
                    return this.Links[index];
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
        protected override global::System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "CONTAINERS"))
            {
                return this._containers;
            }
            if ((feature == "LINKS"))
            {
                return this._links;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected override string GetCompositionName(object container)
        {
            if ((container == this._containers))
            {
                return "Containers";
            }
            if ((container == this._links))
            {
                return "Links";
            }
            return base.GetCompositionName(container);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.kit.edu/ComponentBasedSystem/#//Deployment/Environment_MM06")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Environment_MM06 class
        /// </summary>
        public class Environment_MM06ChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Environment_MM06 _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public Environment_MM06ChildrenCollection(Environment_MM06 parent)
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
                    count = (count + this._parent.Containers.Count);
                    count = (count + this._parent.Links.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.Containers.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.Links.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.Containers.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.Links.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IContainer_MM06 containersCasted = item.As<IContainer_MM06>();
                if ((containersCasted != null))
                {
                    this._parent.Containers.Add(containersCasted);
                }
                ILink linksCasted = item.As<ILink>();
                if ((linksCasted != null))
                {
                    this._parent.Links.Add(linksCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Containers.Clear();
                this._parent.Links.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.Containers.Contains(item))
                {
                    return true;
                }
                if (this._parent.Links.Contains(item))
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
                IEnumerator<IModelElement> containersEnumerator = this._parent.Containers.GetEnumerator();
                try
                {
                    for (
                    ; containersEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = containersEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    containersEnumerator.Dispose();
                }
                IEnumerator<IModelElement> linksEnumerator = this._parent.Links.GetEnumerator();
                try
                {
                    for (
                    ; linksEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = linksEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    linksEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IContainer_MM06 container_MM06Item = item.As<IContainer_MM06>();
                if (((container_MM06Item != null) 
                            && this._parent.Containers.Remove(container_MM06Item)))
                {
                    return true;
                }
                ILink linkItem = item.As<ILink>();
                if (((linkItem != null) 
                            && this._parent.Links.Remove(linkItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Containers).Concat(this._parent.Links).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Environment_MM06 class
        /// </summary>
        public class Environment_MM06ReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Environment_MM06 _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public Environment_MM06ReferencedElementsCollection(Environment_MM06 parent)
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
                    count = (count + this._parent.Containers.Count);
                    count = (count + this._parent.Links.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.Containers.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.Links.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.Containers.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.Links.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IContainer_MM06 containersCasted = item.As<IContainer_MM06>();
                if ((containersCasted != null))
                {
                    this._parent.Containers.Add(containersCasted);
                }
                ILink linksCasted = item.As<ILink>();
                if ((linksCasted != null))
                {
                    this._parent.Links.Add(linksCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Containers.Clear();
                this._parent.Links.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.Containers.Contains(item))
                {
                    return true;
                }
                if (this._parent.Links.Contains(item))
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
                IEnumerator<IModelElement> containersEnumerator = this._parent.Containers.GetEnumerator();
                try
                {
                    for (
                    ; containersEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = containersEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    containersEnumerator.Dispose();
                }
                IEnumerator<IModelElement> linksEnumerator = this._parent.Links.GetEnumerator();
                try
                {
                    for (
                    ; linksEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = linksEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    linksEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IContainer_MM06 container_MM06Item = item.As<IContainer_MM06>();
                if (((container_MM06Item != null) 
                            && this._parent.Containers.Remove(container_MM06Item)))
                {
                    return true;
                }
                ILink linkItem = item.As<ILink>();
                if (((linkItem != null) 
                            && this._parent.Links.Remove(linkItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Containers).Concat(this._parent.Links).GetEnumerator();
            }
        }
    }
}

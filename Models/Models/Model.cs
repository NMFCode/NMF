using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Models
{
    [XmlElementName("XMI")]
    [XmlNamespaceAttribute("http://www.omg.org/XMI")]
    [XmlNamespacePrefixAttribute("xmi")]
    [DebuggerDisplayAttribute("Model {ModelUri}")]
    [ModelRepresentationClass("http://nmf.codeplex.com/nmeta/#//Model/")]
    public class Model : ModelElement, IModel
    {
        /// <summary>
        /// The backing field for the ModelUri property
        /// </summary>
        private Uri _modelUri;

        private static readonly Lazy<ITypedElement> _modelUriAttribute = new Lazy<ITypedElement>(RetrieveModelUriAttribute);

        private static readonly Lazy<ITypedElement> _rootElementsReference = new Lazy<ITypedElement>(RetrieveRootElementsReference);

        /// <summary>
        /// The backing field for the RootElements property
        /// </summary>
        private readonly ObservableCompositionOrderedSet<NMF.Models.IModelElement> _rootElements;

        private static IClass _classInstance;

        public Model()
        {
            this._rootElements = new ObservableCompositionOrderedSet<NMF.Models.IModelElement>(this);
            this._rootElements.CollectionChanging += this.RootElementsCollectionChanging;
            this._rootElements.CollectionChanged += this.RootElementsCollectionChanged;
        }

        /// <summary>
        /// The ModelUri property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Uri ModelUri
        {
            get
            {
                return this._modelUri;
            }
            set
            {
                if ((this._modelUri != value))
                {
                    Uri old = this._modelUri;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnModelUriChanging(e);
                    this.OnPropertyChanging("ModelUri", e, _modelUriAttribute);
                    this._modelUri = value;
                    this.OnModelUriChanged(e);
                    this.OnPropertyChanged("ModelUri", e, _modelUriAttribute);
                }
            }
        }

        /// <summary>
        /// The RootElements property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<NMF.Models.IModelElement> RootElements
        {
            get
            {
                return this._rootElements;
            }
        }

        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<NMF.Models.IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new ModelChildrenCollection(this));
            }
        }

        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<NMF.Models.IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ModelReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Model")));
                }
                return _classInstance;
            }
        }

        /// <summary>
        /// Gets fired before the ModelUri property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> ModelUriChanging;

        /// <summary>
        /// Gets fired when the ModelUri property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> ModelUriChanged;

        private static ITypedElement RetrieveModelUriAttribute()
        {
            return ((ITypedElement)(((NMF.Models.ModelElement)(NMF.Models.Model.ClassInstance)).Resolve("ModelUri")));
        }

        /// <summary>
        /// Raises the ModelUriChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnModelUriChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.ModelUriChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Raises the ModelUriChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnModelUriChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.ModelUriChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }

        private static ITypedElement RetrieveRootElementsReference()
        {
            return ((ITypedElement)(((NMF.Models.ModelElement)(NMF.Models.Model.ClassInstance)).Resolve("RootElements")));
        }

        /// <summary>
        /// Forwards CollectionChanging notifications for the RootElements property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void RootElementsCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("RootElements", e, _rootElementsReference);
        }

        /// <summary>
        /// Forwards CollectionChanged notifications for the RootElements property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void RootElementsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("RootElements", e, _rootElementsReference);
            if (PromoteSingleRootElement && ModelUri != null && IsFlagSet(ModelElementFlag.RequireUris) && 
                ((RootElements.Count == 1 && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) || 
                 (e.Action == NotifyCollectionChangedAction.Add && RootElements.Count - e.NewItems.Count == 1)))
            {
                IModelElement oldRoot;
                string fragment;
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (e.NewStartingIndex == 0)
                    {
                        oldRoot = RootElements[RootElements.Count - 1];
                    }
                    else
                    {
                        oldRoot = RootElements[0];
                    }
                    fragment = "//";
                }
                else
                {
                    oldRoot = RootElements[0];
                    fragment = "/0/";
                }
                var builder = new UriBuilder(ModelUri);
                foreach (ModelElement element in oldRoot.Descendants())
                {
                    var frag = element.CreateUriWithFragment(null, false, oldRoot);
                    builder.Fragment = fragment + frag;
                    element.OnUriChanged(builder.Uri);
                }
            }

        }

        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "MODELURI"))
            {
                return this.ModelUri;
            }
            return base.GetAttributeValue(attribute, index);
        }

        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "ROOTELEMENTS"))
            {
                return this._rootElements;
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
            if ((feature == "MODELURI"))
            {
                this.ModelUri = ((Uri)(value));
                return;
            }
            base.SetFeature(feature, value);
        }

        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected internal override string GetCompositionName(object container)
        {
            if ((container == this._rootElements))
            {
                return null;
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
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Model")));
            }
            return _classInstance;
        }

        /// <summary>
        /// The collection class to to represent the children of the Model class
        /// </summary>
        public class ModelChildrenCollection : ReferenceCollection, ICollectionExpression<NMF.Models.IModelElement>, ICollection<NMF.Models.IModelElement>
        {

            private readonly Model _parent;

            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ModelChildrenCollection(Model parent)
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
                    count = (count + this._parent.RootElements.Count);
                    return count;
                }
            }

            protected override void AttachCore()
            {
                this._parent.RootElements.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }

            protected override void DetachCore()
            {
                this._parent.RootElements.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }

            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(NMF.Models.IModelElement item)
            {
                NMF.Models.IModelElement rootElementsCasted = item;
                if ((rootElementsCasted != null))
                {
                    this._parent.RootElements.Add(rootElementsCasted);
                }
            }

            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.RootElements.Clear();
            }

            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(NMF.Models.IModelElement item)
            {
                if (this._parent.RootElements.Contains(item))
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
            public override void CopyTo(NMF.Models.IModelElement[] array, int arrayIndex)
            {
                IEnumerator<NMF.Models.IModelElement> rootElementsEnumerator = this._parent.RootElements.GetEnumerator();
                try
                {
                    for (
                    ; rootElementsEnumerator.MoveNext();
                    )
                    {
                        array[arrayIndex] = rootElementsEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    rootElementsEnumerator.Dispose();
                }
            }

            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(NMF.Models.IModelElement item)
            {
                IModelElement modelElementItem = item;
                if (((modelElementItem != null)
                            && this._parent.RootElements.Remove(modelElementItem)))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<NMF.Models.IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<NMF.Models.IModelElement>().Concat(this._parent.RootElements).GetEnumerator();
            }
        }

        /// <summary>
        /// The collection class to to represent the children of the Model class
        /// </summary>
        public class ModelReferencedElementsCollection : ReferenceCollection, ICollectionExpression<NMF.Models.IModelElement>, ICollection<NMF.Models.IModelElement>
        {

            private readonly Model _parent;

            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ModelReferencedElementsCollection(Model parent)
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
                    count = (count + this._parent.RootElements.Count);
                    return count;
                }
            }

            protected override void AttachCore()
            {
                this._parent.RootElements.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }

            protected override void DetachCore()
            {
                this._parent.RootElements.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }

            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(NMF.Models.IModelElement item)
            {
                IModelElement rootElementsCasted = item;
                if ((rootElementsCasted != null))
                {
                    this._parent.RootElements.Add(rootElementsCasted);
                }
            }

            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.RootElements.Clear();
            }

            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(NMF.Models.IModelElement item)
            {
                if (this._parent.RootElements.Contains(item))
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
            public override void CopyTo(NMF.Models.IModelElement[] array, int arrayIndex)
            {
                IEnumerator<NMF.Models.IModelElement> rootElementsEnumerator = this._parent.RootElements.GetEnumerator();
                try
                {
                    for (
                    ; rootElementsEnumerator.MoveNext();
                    )
                    {
                        array[arrayIndex] = rootElementsEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    rootElementsEnumerator.Dispose();
                }
            }

            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(NMF.Models.IModelElement item)
            {
                var modelElementItem = item;
                if (((modelElementItem != null)
                            && this._parent.RootElements.Remove(modelElementItem)))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<NMF.Models.IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<NMF.Models.IModelElement>().Concat(this._parent.RootElements).GetEnumerator();
            }
        }

        /// <summary>
        /// Represents a proxy to represent an incremental access to the ModelUri property
        /// </summary>
        private sealed class ModelUriProxy : ModelPropertyChange<NMF.Models.Model, Uri>
        {

            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ModelUriProxy(NMF.Models.Model modelElement) :
                    base(modelElement, "ModelUri")
            {
            }

            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override Uri Value
            {
                get
                {
                    return this.ModelElement.ModelUri;
                }
                set
                {
                    this.ModelElement.ModelUri = value;
                }
            }
        }


        private Dictionary<string, ModelElement> IdStore;

        static Model()
        {
            PromoteSingleRootElement = true;
        }

        /// <summary>
        /// This configuration sets whether a model should be identified with its single root model if such an element exists
        /// </summary>
        public static bool PromoteSingleRootElement { get; set; }

        /// <summary>
        /// The repository that manages this model
        /// </summary>
        public IModelRepository Repository { get; internal set; }

        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if (reference == "#" && index < RootElements.Count)
            {
                return RootElements[index];
            }
            else
            {
                int num;
                if (int.TryParse(reference, out num) && num > 0 && num < RootElements.Count)
                {
                    return RootElements[num];
                }
                return null;
            }
        }


        protected override string GetRelativePathForNonIdentifiedChild(IModelElement child)
        {
            if (RootElements.Count == 1 && PromoteSingleRootElement)
            {
                return null;
            }
            else
            {
                var index = RootElements.IndexOf(child);
                if (index != -1)
                {
                    return "/" + index.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        protected internal override Uri CreateUriWithFragment(string fragment, bool absolute, IModelElement baseElement = null)
        {
            if (fragment != null)
            {
                if (!absolute)
                {
                    return new Uri("#/" + fragment, UriKind.Relative);
                }
                else if (ModelUri != null && ModelUri.IsAbsoluteUri)
                {
                    return new Uri(ModelUri.AbsoluteUri + "#/" + fragment);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (absolute)
                {
                    return ModelUri;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Registers the given model element with the given id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="element">That element to register with the id</param>
        /// <returns>True, if the registration process was successful. Otherwise, False denotes that already an element with this identifier existed.</returns>
        public bool RegisterId(string id, ModelElement element)
        {
            if (id == null) return false;
            var idStore = IdStore;
            if (idStore == null)
            {
                idStore = Interlocked.CompareExchange(ref IdStore, new Dictionary<string, ModelElement>(), null) ?? IdStore;
            }
            if (idStore.ContainsKey(id))
            {
                return false;
            }
            else
            {
                idStore.Add(id, element);
                return true;
            }
        }

        /// <summary>
        /// Unregister the given identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>True, if the identifier is removed. False denotes that this identifier was not registered</returns>
        public bool UnregisterId(string id)
        {
            if (id == null) return false;
            if (IdStore == null)
            {
                return false;
            }
            else
            {
                return IdStore.Remove(id);
            }
        }

        public virtual Uri CreateUriForElement(IModelElement element)
        {
            if (element == null) return null;
            if (element.Model == this)
            {
                return element.RelativeUri;
            }
            else
            {
                var target = element.AbsoluteUri;
                return SimplifyUri(target);
            }
        }

        protected Uri SimplifyUri(Uri target)
        {
            var current = ModelUri;
            if (target == null || !target.IsAbsoluteUri) return target;
            if (!target.IsFile || current == null) return target;
            if (target.Scheme != current.Scheme) return target;
            if (target.Host != current.Host) return target;
            for (int i = 0; i < target.Segments.Length; i++)
            {
                if (i >= current.Segments.Length || target.Segments[i] != current.Segments[i])
                {
                    var relative = Path.Combine(Enumerable.Repeat("..", current.Segments.Length - i - 1).Concat(target.Segments.Skip(i)).ToArray());
                    if (Path.IsPathRooted(relative)) return target;
                    return new Uri(relative.Replace(Path.DirectorySeparatorChar, '/') + target.Fragment, UriKind.Relative);
                }
            }
            return target;
        }

        /// <summary>
        /// Resolves the given global ID
        /// </summary>
        /// <param name="id">The given global id</param>
        /// <returns>The model element with the given id or null, if no such element is found</returns>
        public IModelElement ResolveGlobal(string id)
        {
            if (id == null) throw new ArgumentOutOfRangeException(nameof(id));
            if (IdStore != null && IdStore.TryGetValue(id, out ModelElement element))
            {
                return element;
            }
            return null;
        }

        public override IModelElement Resolve(string path)
        {
            if (string.IsNullOrEmpty(path)) return this;
            if (path.StartsWith("\"") && path.EndsWith("\"/"))
            {
                path = path.Substring(1, path.Length - 3);
            }
            if (path.StartsWith("#"))
            {
                path = path.Substring(1);
            }
            if (!path.StartsWith("/"))
            {
                var index = path.IndexOf('/');
                if (index == 0)
                {
                    return ResolveNonIdentified(path.Substring(1));
                }
                if (IdStore != null)
                {
                    ModelElement element;
                    if (IdStore.TryGetValue(index == -1 ? path : path.Substring(0, index), out element))
                    {
                        if (index == -1)
                        {
                            return element;
                        }
                        else
                        {
                            return element.Resolve(path.Substring(index + 1));
                        }
                    }
                }
                return null;
            }
            else
            {
                return ResolveNonIdentified(path.TrimStart('/'));
            }
        }

        private IModelElement ResolveNonIdentified(string path)
        {
            if (RootElements.Count == 1 && PromoteSingleRootElement)
            {
                if (RootElements[0] is ModelElement root)
                {
                    var resolved = root.Resolve(path);
                    if (resolved != null) return resolved;
                }
            }
            var baseResolve = base.Resolve(path);
            if (baseResolve != null || PromoteSingleRootElement || RootElements.Count != 1) return baseResolve;
            if (RootElements[0] is ModelElement rootCasted)
            {
                return rootCasted.Resolve(path);
            }
            else
            {
                return null;
            }
        }

        protected override string GetRelativePathForChild(IModelElement child)
        {
            if (PromoteSingleRootElement && RootElements.Count == 1 && child == RootElements[0])
            {
                return string.Empty;
            }
            return base.GetRelativePathForChild(child);
        }

        protected internal virtual void EnsureAllElementsContained()
        {
            foreach (var element in this.Descendants())
            {
                foreach (var referenced in element.ReferencedElements)
                {
                    var ancestor = referenced;
                    while (ancestor.Parent != null)
                    {
                        ancestor = ancestor.Parent;
                    }
                    if (ancestor.Model == null)
                    {
                        RootElements.Add(ancestor);
                    }
                }
            }
        }

        protected internal virtual bool SerializeAsReference(IModelElement element)
        {
            return element != null && element.Model != this;
        }

        public event EventHandler<UnlockEventArgs> UnlockRequested;

        protected override void OnBubbledChange(BubbledChangeEventArgs e)
        {
            base.OnBubbledChange(e);
            if (e.ChangeType == ChangeType.UnlockRequest)
            {
                UnlockRequested?.Invoke(this, e.OriginalEventArgs as UnlockEventArgs);
            }
        }
    }
}

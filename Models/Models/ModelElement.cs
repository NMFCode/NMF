using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using NMF.Serialization;
using NMF.Models.Repository;
using NMF.Expressions;
using NMF.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web;
using System.Collections;
using NMF.Models.Collections;
using NMF.Models.Meta;

namespace NMF.Models
{
    /// <summary>
    /// Defines the base class for a model element implementation
    /// </summary>
    [ModelRepresentationClassAttribute("http://nmf.codeplex.com/nmeta/#//ModelElement/")]
    public abstract class ModelElement : IModelElement, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ModelElement parent;
        private ObservableCompositionSet<ModelElementExtension> extensions;
        private ModelElementFlag flag;
        private EventHandler<BubbledChangeEventArgs> bubbledChange;

        [Flags]
        internal enum ModelElementFlag : byte
        {
            Deleting = 1,
            RaiseBubbledChanges = 2,
            RequireUris = 4,
            Locked = 8,
            Frozen = 24
        }

        internal bool IsFlagSet(ModelElementFlag flag)
        {
            return (this.flag & flag) == flag;
        }

        private void SetFlag(ModelElementFlag flag)
        {
            this.flag |= flag;
        }

        private void UnsetFlag(ModelElementFlag flag)
        {
            this.flag &= ~flag;
        }

        /// <summary>
        /// Gets the model that contains the current model element
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Model Model
        {
            get
            {
                IModelElement current = this;
                while (current != null)
                {
                    if (current is Model model)
                    {
                        return model;
                    }
                    else
                    {
                        current = current.Parent;
                    }
                }
                return null;
            }
        }

        internal void RequestUris()
        {
            if (!IsFlagSet(ModelElementFlag.RequireUris))
            {
                SetFlag(ModelElementFlag.RequireUris);
                foreach (var child in Children)
                {
                    if (child is ModelElement childME)
                    {
                        childME.RequestUris();
                    }
                }
            }
        }

        internal void UnregisterUriRequest()
        {
            if (IsFlagSet(ModelElementFlag.RequireUris))
            {
                UnsetFlag(ModelElementFlag.RequireUris);
                if (bubbledChange == null)
                {
                    foreach (var child in Children)
                    {
                        if (child is ModelElement childME)
                        {
                            childME.UnregisterUriRequest();
                        }
                    }
                }
            }
        }

        internal void RequestBubbledChanges()
        {
            if (!IsFlagSet(ModelElementFlag.RaiseBubbledChanges))
            {
                SetFlag(ModelElementFlag.RaiseBubbledChanges);
                foreach (var child in Children)
                {
                    if (child is ModelElement childME)
                    {
                        childME.RequestBubbledChanges();
                    }
                }
            }
        }

        internal void UnregisterBubbledChangeRequest()
        {
            if (IsFlagSet(ModelElementFlag.RaiseBubbledChanges))
            {
                UnsetFlag(ModelElementFlag.RaiseBubbledChanges);
                if (bubbledChange == null)
                {
                    foreach (var child in Children)
                    {
                        if (child is ModelElement childME)
                        {
                            childME.UnregisterBubbledChangeRequest();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Freezes this model element such that it becomes immutable.
        /// </summary>
        public void Freeze()
        {
            if (!IsFlagSet(ModelElementFlag.Frozen))
            {
                SetFlag(ModelElementFlag.Frozen);
                // free event handlers because there will no longer be changes
                PropertyChanged = null;
                bubbledChange = null;
                PropertyChanging = null;
                foreach (var child in Children)
                {
                    child.Freeze();
                }
            }
        }

        /// <summary>
        /// Locks this model element against any changes (can be undone)
        /// </summary>
        public void Lock()
        {
            if (!IsFlagSet(ModelElementFlag.Locked))
            {
                SetFlag(ModelElementFlag.Locked);
                foreach (var child in Children)
                {
                    child.Lock();   
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the model element is closed for any future modifications
        /// </summary>
        public bool IsFrozen => IsFlagSet(ModelElementFlag.Frozen);

        /// <summary>
        /// Gets a value indicating whether the model element is temporarily locked
        /// </summary>
        public bool IsLocked => IsFlagSet(ModelElementFlag.Locked);

        /// <summary>
        /// Unlocks this model element.
        /// </summary>
        /// <exception cref="LockedException">thrown if the model element could not be unlocked</exception>
        public void Unlock()
        {
            if (!TryUnlock()) throw new LockedException();
        }

        /// <summary>
        /// Tries to unlock the current model element in order to make changes possible
        /// </summary>
        /// <returns>True, if unlocking the model element succeeds, otherwise False</returns>
        public bool TryUnlock()
        {
            if (IsFlagSet(ModelElementFlag.Locked))
            {
                if (IsFlagSet(ModelElementFlag.Frozen))
                {
                    return false;
                }

                var unlockEventArgs = new UnlockEventArgs(this);
                OnBubbledChange(BubbledChangeEventArgs.Unlock(this, unlockEventArgs));

                if (unlockEventArgs.MayUnlock)
                {
                    UnlockInternal();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void UnlockInternal()
        {
            var current = this;
            while (current != null)
            {
                current.UnsetFlag(ModelElementFlag.Locked);
                current = current.Parent as ModelElement;
            }
        }

        /// <summary>
        /// Sets the parent for the current model element to the given element
        /// </summary>
        /// <param name="newParent">The new parent for the given element</param>
        private void SetParent(IModelElement newParent)
        {
            Unlock();
            var newParentME = newParent as ModelElement;
            if (newParentME != parent)
            {
                var oldParent = parent;
                if (newParentME != null)
                {
                    OnParentChanging(newParentME, parent);
                    Uri oldUri = null;
                    if (IsFlagSet(ModelElementFlag.RequireUris))
                    {
                        oldUri = AbsoluteUri;
                    }
                    parent = newParentME;
                    var newModel = newParentME.Model;
                    var oldModel = oldParent != null ? oldParent.Model : null;
                    if (oldParent != null)
                    {
                        if (EnforceModels && oldModel != null && newModel == null)
                        {
                            oldModel.RootElements.Add(newParentME);
                        }
                    }
                    if (oldParent != null && oldParent.IsFlagSet(ModelElementFlag.RequireUris) && oldUri != null)
                    {
                        oldParent.OnBubbledChange(BubbledChangeEventArgs.UriChanged(this, new UriChangedEventArgs(oldUri)));
                    }
                    else if (bubbledChange == null)
                    {
                        UnregisterBubbledChangeRequest();
                    }
                    if (newParentME.IsFlagSet(ModelElementFlag.RaiseBubbledChanges) || newParentME.bubbledChange != null)
                    {
                        RequestBubbledChanges();
                    }
                    if (newParentME.IsFlagSet(ModelElementFlag.RequireUris))
                    {
                        RequestUris();
                        OnUriChanged(oldUri);
                    }
                    newParentME.OnChildCreated(this);
                    if (newModel != oldModel)
                    {
                        PropagateNewModel(newModel, oldModel, this);
                    }
                    OnParentChanged(newParentME, oldParent);
                }
                else
                {
                    parent = null;
                    var oldModel = oldParent.Model;
                    if (bubbledChange == null)
                    {
                        UnregisterBubbledChangeRequest();
                    }
                    if (oldModel != null)
                    {
                        PropagateNewModel(null, oldModel, this);
                    }
                    OnParentChanged(null, oldParent);
                }
            }
        }

        private IReference GetContainerReference(IModelElement child, IClass scope, out int index)
        {
            foreach (var r in scope.References)
            {
                if (r.IsContainment)
                {
                    if (r.UpperBound == 1)
                    {
                        if (GetReferencedElement(r) == child)
                        {
                            index = 0;
                            return r;
                        }
                    }
                    else
                    {
                        if (r.IsOrdered)
                        {
                            index = GetReferencedElements(r).IndexOf(child);
                            if (index != -1) return r;
                        }
                        else
                        {
                            if (GetReferencedElements(r).Contains(child))
                            {
                                index = -1;
                                return r;
                            }
                        }
                    }
                }
            }
            foreach (var baseClass in scope.BaseTypes)
            {
                var r = GetContainerReference(child, baseClass, out index);
                if (r != null) return r;
            }
            index = -1;
            return null;
        }

        /// <summary>
        /// Gets the container reference for the given child element
        /// </summary>
        /// <param name="child">The child element</param>
        /// <param name="index">The index of the child in the returned reference</param>
        /// <returns>A composition reference or null, if the child is not contained in the model element</returns>
        public virtual IReference GetContainerReference(IModelElement child, out int index)
        {
            if (child.Parent == this)
            {
                return GetContainerReference(child, GetClass(), out index);
            }
            else
            {
                index = -1;
                return null;
            }
        }

        /// <summary>
        /// Propagates through the composition hierarchy that an entire subtree has been added to a new model
        /// </summary>
        /// <param name="newModel">The new model that will host the subtree</param>
        /// <param name="oldModel">The old model of the subtree</param>
        /// <param name="subtreeRoot">The root element of the inserted subtree</param>
        protected virtual void PropagateNewModel(Model newModel, Model oldModel, IModelElement subtreeRoot)
        {
            foreach (var child in Children.OfType<ModelElement>())
            {
                child.PropagateNewModel(newModel, oldModel, subtreeRoot);
            }
        }

        /// <summary>
        /// Gets called when a new model element is added as a child of the current model element
        /// </summary>
        /// <param name="child">The child element</param>
        /// <remarks>This method is not called if an existing model element is moved in the composition hierarchy</remarks>
        protected virtual void OnChildCreated(IModelElement child)
        {
            if (bubbledChange != null || IsFlagSet(ModelElementFlag.RaiseBubbledChanges))
                OnBubbledChange(BubbledChangeEventArgs.ElementCreated(child, new UriChangedEventArgs(null)));
        }

        /// <summary>
        /// Gets called when the parent element of the current element changes
        /// </summary>
        /// <param name="newParent">The new parent element</param>
        /// <param name="oldParent">The old parent element</param>
        protected virtual void OnParentChanging(IModelElement newParent, IModelElement oldParent) { }

        /// <summary>
        /// Gets called when the parent element of the current element changes
        /// </summary>
        /// <param name="newParent">The new parent element</param>
        /// <param name="oldParent">The old parent element</param>
        protected virtual void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            ParentChanged?.Invoke(this, new ValueChangedEventArgs(oldParent, newParent));
        }

        /// <summary>
        /// Gets or sets the parent element for the current model element
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModelElement Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (value == null)
                {
                    Delete();
                }
                else
                {
                    SetParent(value);
                }
            }
        }


        /// <summary>
        /// Gets a collection with the children of the current model element
        /// </summary>
        [Constant]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return Extensions;
            }
        }


        /// <summary>
        /// Gets the relative Uri for the current model element
        /// </summary>
        [Category("General")]
        [Description("The relative Uri of this model element from its model")]
        public Uri RelativeUri
        {
            get
            {
                return CreateUriWithFragment(null, false);
            }
        }

        /// <summary>
        /// Gets the abolute Uri for the current model element
        /// </summary>
        [Category("General")]
        [Description("The absolute Uri of this model element, i.e., including the Uri of its containing model")]
        public Uri AbsoluteUri
        {
            get
            {
                return CreateUriWithFragment(null, true);
            }
        }


        /// <summary>
        /// Creates the uri with the given fragment starting from the current model element
        /// </summary>
        /// <param name="fragment">The fragment starting from this element</param>
        /// <param name="absolute">True, if an absolute Uri is desired, otherwise false</param>
        /// <param name="baseElement">The element from which the uri shall be created or null</param>
        /// <returns>A uri (relative or absolute)</returns>
        protected internal virtual Uri CreateUriWithFragment(string fragment, bool absolute, IModelElement baseElement = null)
        {
            if (Parent is not ModelElement parent) return null;
            if (baseElement == this)
            {
                if (fragment == null)
                {
                    return null;
                }
                else
                {
                    return new Uri(fragment, UriKind.Relative);
                }
            }
            string path = parent.GetRelativePathForChild(this);
            Uri result = null;
            if (path != null)
            {
                if (fragment != null)
                {
                    fragment = path + "/" + fragment;
                }
                else
                {
                    fragment = path;
                }
                result = parent.CreateUriWithFragment(fragment, absolute, baseElement);
            }
            if (result == null)
            {
                if (IsIdentified)
                {
                    result = CreateUriFromGlobalIdentifier(fragment, absolute);
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a uri with the given fragment
        /// </summary>
        /// <param name="fragment">The fragment</param>
        /// <param name="absolute">True when the Uri should be absolute, otherwise False</param>
        /// <returns>A uri with the given fragment</returns>
        protected Uri CreateUriFromGlobalIdentifier(string fragment, bool absolute)
        {
            var id = ToIdentifierString();
            if (id == null) return null;
            if (fragment != null)
            {
                id += "/" + fragment;
            }
            if (absolute)
            {
                var model = Model;
                if (model == null) return null;
                if (model == Parent && fragment != null)
                {
                    string path = model.GetRelativePathForChild(this);
                    if (path != null)
                    {
                        if (fragment != null)
                        {
                            fragment = path + "/" + fragment;
                        }
                        else
                        {
                            fragment = path;
                        }
                        return model.CreateUriWithFragment(fragment, absolute);
                    }
                }
                if (model != null && model.ModelUri != null && model.ModelUri.IsAbsoluteUri)
                {
                    return new Uri(model.ModelUri.AbsoluteUri + "#" + id);
                }
                else
                {
                    return null;
                }
            }
            return new Uri(id, UriKind.Relative);
        }

        /// <summary>
        /// Informs the model that the current model element has a new id
        /// </summary>
        /// <param name="e">The event data for the value change of the identifier</param>
        protected void PropagateNewId(ValueChangedEventArgs e)
        {
            var model = Model;
            if (model == null) return;
            if (e.OldValue != null) model.UnregisterId(e.OldValue.ToString());
            if (e.NewValue != null) model.RegisterId(e.NewValue.ToString(), this);
        }

        /// <summary>
        /// Gets or sets a value indicating whether a correct model containment should be enforced
        /// </summary>
        public static bool EnforceModels { get; set; }

        /// <summary>
        /// Gets or sets whether identifiers should be preferred in the serialization
        /// </summary>
        public static bool PreferIdentifiers { get; set; }

        /// <summary>
        /// Gets a value indicating whether this item can be identified through its ToString value
        /// </summary>
        [Browsable(false)]
        public virtual bool IsIdentified
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the identifier for this model element
        /// </summary>
        public virtual string ToIdentifierString()
        {
            return null;
        }


        /// <summary>
        /// Gets a string representation of the current model element
        /// </summary>
        /// <returns>A string representation of the current model element</returns>
        public override string ToString()
        {
            if (IsIdentified)
            {
                return string.Format("{0} '{1}'", GetType().Name, ToIdentifierString());
            }
            else
            {
                return GetType().Name;
            }
        }


        /// <summary>
        /// Gets fired when the identifier of the current model element changes
        /// </summary>
        public event EventHandler KeyChanged;


        /// <summary>
        /// Fires the <see cref="KeyChanged"/> event
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnKeyChanged(EventArgs e)
        {
            KeyChanged?.Invoke(this, e);
        }


        /// <summary>
        /// Resolves the given relative Uri from the current model element
        /// </summary>
        /// <param name="relativeUri">A relative uri describing the path to the desired child element</param>
        /// <returns>The corresponding child element or null, if no such was found</returns>
        public IModelElement Resolve(Uri relativeUri)
        {
            if (relativeUri == null) return this;
            if (relativeUri.IsAbsoluteUri) throw new ArgumentException("The uri is not a relative Uri", "relativeUri");
            return Resolve(relativeUri.OriginalString);
        }


        /// <summary>
        /// Resolves the given path starting from the current element
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The element corresponding to the given path or null, if no such element could be found</returns>
        public virtual IModelElement Resolve(string path)
        {
            if (string.IsNullOrEmpty(path)) return this;
            path = path.Trim('/', ' ', '\n', '\r', '\t');
            var segments = path.Split('/');
            var current = this;
            for (int i = 0; i < segments.Length; i++)
            {
                if (current == null) return null;
                var segmentDecoded = Uri.UnescapeDataString(segments[i]);
                if (!string.IsNullOrEmpty(segmentDecoded))
                {
                    current = current.GetModelElementForPathSegment(segmentDecoded) as ModelElement;
                }
            }
            return current;
        }


        /// <summary>
        /// Gets the relative Uri for the given child element
        /// </summary>
        /// <param name="child">The child element</param>
        /// <returns>A relative Uri to resolve the child element</returns>
        protected virtual string GetRelativePathForChild(IModelElement child)
        {
            if (child == null) return null;
            if (child is not ModelElement childModelElement || PreferIdentifiers)
            {
                if (child.IsIdentified)
                {
                    return GetIdentifierFragment(child) ?? GetRelativePathForNonIdentifiedChild(child);
                }
                return GetRelativePathForNonIdentifiedChild(child);
            }
            else
            {
                var fragment = GetRelativePathForNonIdentifiedChild(child);
                if (fragment == null && child.IsIdentified)
                {
                    fragment = GetIdentifierFragment(child);
                }
                return fragment;
            }
        }

        private static string GetIdentifierFragment(IModelElement child)
        {
            var id = child.ToIdentifierString();
            if (!string.IsNullOrWhiteSpace(id))
            {
                return Uri.EscapeDataString(id);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the relative Uri for the given child element that is not identified
        /// </summary>
        /// <param name="child">The child element</param>
        /// <returns>A relative Uri to resolve the child element</returns>
        protected virtual string GetRelativePathForNonIdentifiedChild(IModelElement child)
        {
            return null;
        }

        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected internal virtual string GetCompositionName(object container)
        {
            return container as string;
        }


        /// <summary>
        /// Gets the model element for the given relative Uri
        /// </summary>
        /// <param name="segment">The relative Uri</param>
        /// <returns>The model element that corresponds to the given Uri</returns>
        protected IModelElement GetModelElementForPathSegment(string segment)
        {
            if (segment == null) return null;
            var qString = segment.ToString().ToUpperInvariant();
            if (qString.StartsWith("#"))
            {
                qString = qString.Substring(1);
                foreach (var child in Children)
                {
                    if (!child.IsIdentified) continue;
                    var childId = child.ToIdentifierString();
                    if (childId != null && childId.ToUpperInvariant() == qString) return child;
                }
                return null;
            }
            else if (!qString.StartsWith("@"))
            {
                foreach (var child in Children)
                {
                    if (child.IsIdentified) {
                        var id = child.ToIdentifierString();
                        if (id != null && id.ToUpperInvariant() == qString) return child;
                    }
                }
            }

            if( ModelHelper.ParseSegment( segment, out var reference, out var index ) )
            {
                return GetModelElementForReference( reference, index );
            }

            if( ModelHelper.ParseIdentifierSegment( segment, out reference, out var identifierReference, out var identifier ) )
            {
                var collection = GetCollectionForFeature( reference );
                foreach( var element in collection.OfType<ModelElement>() )
                {
                    if( string.Equals(element.GetAttributeValue( identifierReference, 0 )?.ToString(), identifier) )
                    {
                        return element;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Gets the Model element for the given reference and index
        /// </summary>
        /// <param name="reference">The reference name in upper case</param>
        /// <param name="index">The index of the element within the reference</param>
        /// <returns>The model element at the given reference</returns>
        protected virtual IModelElement GetModelElementForReference(string reference, int index)
        {
            return null;
        }

        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <param name="feature">The features name in upper case</param>
        /// <returns>A non-generic list of elements</returns>
        protected virtual IList GetCollectionForFeature(string feature)
        {
            throw new ArgumentOutOfRangeException(nameof(feature));
        }

        /// <summary>
        /// Gets the attribute value for the given attribute
        /// </summary>
        /// <param name="attribute">The attributes name in upper case</param>
        /// <param name="index">The attributes index</param>
        /// <returns>The attribute value</returns>
        protected virtual object GetAttributeValue(string attribute, int index)
        {
            if (attribute == "ABSOLUTEURI")
            {
                return AbsoluteUri;
            }
            else if (attribute == "RELATIVEURI")
            {
                return RelativeUri;
            }
            throw new ArgumentOutOfRangeException(nameof(attribute));
        }

        /// <summary>
        /// Sets the given feature to the given value
        /// </summary>
        /// <param name="feature">The name of the feature that should be set</param>
        /// <param name="value">The value that should be set</param>
        protected virtual void SetFeature(string feature, object value)
        {
            throw new ArgumentOutOfRangeException(nameof(feature));
        }

        /// <summary>
        /// Calls the given operation
        /// </summary>
        /// <param name="operation">The operation that should be called</param>
        /// <param name="arguments">The arguments used to call the operation</param>
        /// <returns>The operation result or null, if the operation does not return any value</returns>
        public virtual object CallOperation(IOperation operation, object[] arguments)
        {
            throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary>
        /// Gets a property expression for the given reference
        /// </summary>
        /// <param name="reference">The name of the requested reference in upper case</param>
        /// <returns>A property expression</returns>
        protected virtual INotifyExpression<IModelElement> GetExpressionForReference(string reference)
        {
            throw new ArgumentOutOfRangeException(nameof(reference));
        }

        /// <summary>
        /// Gets a property expression for the given attribute
        /// </summary>
        /// <param name="attribute">The requested attribute in upper case</param>
        /// <returns>A property expression</returns>
        protected virtual INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            throw new ArgumentOutOfRangeException(nameof(attribute));
        }


        /// <summary>
        /// Gets a collection of model element extensions that have been applied to this model element
        /// </summary>
        [Category("General")]
        [Description("The extensions applied to this model element")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ICollectionExpression<ModelElementExtension> Extensions
        {
            get
            {
                return new ModelElementExtensionsProxy(this);
            }
        }  

        /// <summary>
        /// Gets the extension with the given extension type
        /// </summary>
        /// <typeparam name="T">The model element extension type</typeparam>
        /// <returns>The extension of the given extension type or null, if no such exists</returns>
        public T GetExtension<T>() where T : ModelElementExtension
        {
            return extensions?.OfType<T>().FirstOrDefault();
        }


        /// <summary>
        /// Gets a collection of model elements referenced from this element.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerableExpression<IModelElement> ReferencedElements => Extensions;


        /// <summary>
        /// Gets called when the PropertyChanged event is fired
        /// </summary>
        /// <param name="propertyName">The name of the changed property</param>
        /// <param name="valueChangedEvent">The original event data</param>
        /// <param name="feature">The feature</param>
        protected virtual void OnPropertyChanged(string propertyName, ValueChangedEventArgs valueChangedEvent, Lazy<ITypedElement> feature = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (!IsFlagSet(ModelElementFlag.Deleting))
                OnBubbledChange(BubbledChangeEventArgs.PropertyChanged(this, propertyName, valueChangedEvent, IsFlagSet(ModelElementFlag.RequireUris), feature));
        }


        /// <summary>
        /// Gets called when the PropertyChanging event is fired
        /// </summary>
        /// <param name="propertyName">The name of the changed property</param>
        /// <param name="feature">The feature</param>
        /// <param name="e">The event data</param>
        protected virtual void OnPropertyChanging(string propertyName, ValueChangedEventArgs e = null, Lazy<ITypedElement> feature = null)
        {
            Unlock();
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            if (!IsFlagSet(ModelElementFlag.Deleting))
                OnBubbledChange(BubbledChangeEventArgs.PropertyChanging(this, propertyName, e, IsFlagSet(ModelElementFlag.RequireUris), feature));
        }


        /// <summary>
        /// Deletes the current model element
        /// </summary>
        public virtual void Delete()
        {
            if (!IsFlagSet(ModelElementFlag.Deleting))
            {
                SetFlag(ModelElementFlag.Deleting);
                // do not create event args when nobody listens
                if (IsFlagSet(ModelElementFlag.RaiseBubbledChanges) || Deleted != null || Deleting != null)
                {
                    Uri originalAbsoluteUri = null;
                    if (IsFlagSet(ModelElementFlag.RequireUris))
                    {
                        originalAbsoluteUri = AbsoluteUri;
                    }
                    var e = new UriChangedEventArgs(originalAbsoluteUri);
                    OnDeleting(e);
                    OnDeleted(e);
                    // only bubble deletion for deleted root
                    OnBubbledChange(BubbledChangeEventArgs.ElementDeleted(this, e));
                }
                SetParent(null);
            }
        }

        
        /// <summary>
        /// Gets called before the model element gets deleted
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDeleting(UriChangedEventArgs e)
        {
            Deleting?.Invoke(this, e);
        }

        /// <summary>
        /// Gets called when the model element gets deleted
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnDeleted(UriChangedEventArgs e)
        {
            foreach (var child in Children.Reverse())
            {
                if (child is ModelElement childME)
                {
                    Uri oldChildUri = null;
                    if (e.OldUri != null)
                    {
                        var uriBuilder = new UriBuilder(e.OldUri);
                        uriBuilder.Fragment = uriBuilder.Fragment.Substring(1) + '/' + childME.CreateUriWithFragment(null, false, this).OriginalString;
                        oldChildUri = uriBuilder.Uri;
                    }
                    var args = new UriChangedEventArgs(oldChildUri);
                    childME.OnDeleting(args);
                    childME.OnDeleted(args);
                }
            }
            Deleted?.Invoke(this, e);
            UnsetFlag(ModelElementFlag.Deleting);
        }


        /// <summary>
        /// Gets fired when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Gets fired before a property value changes
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;


        /// <summary>
        /// Gets fired after the model element has been deleted
        /// </summary>
        public event EventHandler<UriChangedEventArgs> Deleted;


        /// <summary>
        /// Gets fired before the model element gets deleted
        /// </summary>
        public event EventHandler<UriChangedEventArgs> Deleting;


        /// <summary>
        /// Gets fired when the Uri of this element changes
        /// </summary>
        public event EventHandler<UriChangedEventArgs> UriChanged;


        internal void OnUriChanged(Uri oldUri)
        {
            var e = new UriChangedEventArgs(oldUri);
            UriChanged?.Invoke(this, e);
            OnBubbledChange(BubbledChangeEventArgs.UriChanged(this, e));
        }


        /// <summary>
        /// Gets the class of the current model element
        /// </summary>
        /// <returns>The class of the current model element</returns>
        public abstract Meta.IClass GetClass();


        /// <summary>
        /// Gets the NMeta class object for this type
        /// </summary>
        public static Meta.IClass ClassInstance => (Meta.IClass)MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//ModelElement/");

        /// <summary>
        /// Gets the referenced element of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="index">The index of the desired model element, if multi-valued reference</param>
        /// <returns>The referenced element for the given reference</returns>
        [ObservableProxy(typeof(ModelElementProxy), "GetReferencedElement")]
        public IModelElement GetReferencedElement(Meta.IReference reference, int index = 0)
        {
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            return GetModelElementForReference(reference.Name.ToUpperInvariant(), 0);
        }

        /// <summary>
        /// Sets the referenced element of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="element">The element that should be set</param>
        public void SetReferencedElement(Meta.IReference reference, IModelElement element)
        {
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            SetFeature(reference.Name.ToUpperInvariant(), element);
        }

        /// <summary>
        /// Gets the referenced elements of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <returns>A collection of referenced elements</returns>
        public IList GetReferencedElements(Meta.IReference reference)
        {
            return GetCollectionForFeature(reference.Name.ToUpperInvariant());
        }

        /// <summary>
        /// Gets the value of the current model element under the given attribute
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="index">The index of the desired value, if multi-valued attribute</param>
        /// <returns>The attributes value</returns>
        [ObservableProxy(typeof(ModelElementProxy), "GetAttributeValue")]
        public object GetAttributeValue(Meta.IAttribute attribute, int index = 0)
        {
            if (attribute == null) throw new ArgumentOutOfRangeException("attribute");
            return GetAttributeValue(attribute.Name.ToUpperInvariant(), index);
        }

        /// <summary>
        /// Sets the value of the current model element for the given attribute
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="value">The value that should be set</param>
        public void SetAttributeValue(Meta.IAttribute attribute, object value)
        {
            if (attribute == null) throw new ArgumentOutOfRangeException("attribute");
            SetFeature(attribute.Name.ToUpperInvariant(), value);
        }

        /// <summary>
        /// Gets the values for the given attribute
        /// </summary>
        /// <param name="attribute">The attribute whose value is queried</param>
        /// <returns>The attribute value collection</returns>
        public IList GetAttributeValues(Meta.IAttribute attribute)
        {
            if (attribute == null) throw new ArgumentOutOfRangeException("attribute");
            return GetCollectionForFeature(attribute.Name.ToUpperInvariant());
        }

        /// <summary>
        /// Raises the Bubbled Change event for the given collection change
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        /// <param name="e">The event data</param>
        /// <param name="feature">The feature that is changing</param>
        protected void OnCollectionChanged(string propertyName, NotifyCollectionChangedEventArgs e, Lazy<ITypedElement> feature = null)
        {
            if (!IsFlagSet(ModelElementFlag.Deleting))
                OnBubbledChange(BubbledChangeEventArgs.CollectionChanged(this, propertyName, e, IsFlagSet(ModelElementFlag.RequireUris), feature));
        }

        /// <summary>
        /// Raises the Bubbled Change event for the given upcoming collection change
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        /// <param name="e">The event data</param>
        /// <param name="feature">The feature that is changing</param>
        protected void OnCollectionChanging(string propertyName, NotifyCollectionChangedEventArgs e, Lazy<ITypedElement> feature = null)
        {
            Unlock();
            if (!IsFlagSet(ModelElementFlag.Deleting))
                OnBubbledChange(BubbledChangeEventArgs.CollectionChanging(this, propertyName, e, IsFlagSet(ModelElementFlag.RequireUris), feature));
        }

        /// <summary>
        /// Fires the BubbledChange event
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnBubbledChange(BubbledChangeEventArgs e)
        {
            bubbledChange?.Invoke(this, e);
            if (IsFlagSet(ModelElementFlag.RaiseBubbledChanges) || e.ChangeType == ChangeType.UnlockRequest)
            {
                if (Parent is ModelElement parent)
                {
                    parent.OnBubbledChange(e);
                }
            }
        }

        /// <summary>
        /// Is fired when an element in the below containment hierarchy has changed
        /// </summary>
        public event EventHandler<BubbledChangeEventArgs> BubbledChange
        {
            add
            {
                var isAttached = bubbledChange != null;
                bubbledChange += value;
                if (!isAttached && (bubbledChange != null)) RequestBubbledChanges();
            }
            remove
            {
                var isAttached = bubbledChange != null;
                bubbledChange -= value;
                if (isAttached && bubbledChange == null) UnregisterBubbledChangeRequest();
            }
        }

        /// <summary>
        /// Gets fired when the container of the current model element has changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ParentChanged;

        private class ModelElementProxy
        {
            public static INotifyExpression<IModelElement> GetReferencedElement(ModelElement element, Meta.IReference reference, int index)
            {
                if (reference == null) throw new ArgumentOutOfRangeException("reference");
                if (reference.UpperBound == 1)
                {
                    return element.GetExpressionForReference(reference.Name.ToUpperInvariant());
                }
                throw new NotSupportedException();
            }

            public static INotifyExpression<object> GetAttributeValue(ModelElement element, Meta.IAttribute attribute, int index)
            {
                if (attribute == null) throw new ArgumentOutOfRangeException("attribute");
                if (attribute.UpperBound == 1)
                {
                    return element.GetExpressionForAttribute(attribute.Name.ToUpperInvariant());
                }
                throw new NotSupportedException();
            }
        }

        private class ModelElementExtensionsProxy : ICollectionExpression<ModelElementExtension>
        {
            private readonly ModelElement element;

            public ModelElementExtensionsProxy(ModelElement element)
            {
                this.element = element;
            }

            public int Count
            {
                get
                {
                    if (element.extensions == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return element.extensions.Count;
                    }
                }
            }

            private ICollectionExpression<ModelElementExtension> CreateCollection()
            {
                if (element.extensions == null)
                {
                    Interlocked.CompareExchange(ref element.extensions, new ObservableCompositionSet<ModelElementExtension>(element), null);
                }
                return element.extensions;
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public void Add(ModelElementExtension item)
            {
                CreateCollection().Add(item);
            }

            public INotifyCollection<ModelElementExtension> AsNotifiable()
            {
                return CreateCollection().AsNotifiable();
            }

            public void Clear()
            {
                var extensions = element.extensions;
                if (extensions != null)
                {
                    extensions.Clear();
                }
            }

            public bool Contains(ModelElementExtension item)
            {
                var extensions = element.extensions;
                if (extensions == null)
                {
                    return false;
                }
                else
                {
                    return extensions.Contains(item);
                }
            }

            public void CopyTo(ModelElementExtension[] array, int arrayIndex)
            {
                var extensions = element.extensions;
                if (extensions != null)
                {
                    extensions.CopyTo(array, arrayIndex);
                }
            }

            public IEnumerator<ModelElementExtension> GetEnumerator()
            {
                var extensions = element.extensions;
                if (extensions == null)
                {
                    return Enumerable.Empty<ModelElementExtension>().GetEnumerator();
                }
                else
                {
                    return extensions.GetEnumerator();
                }
            }

            public bool Remove(ModelElementExtension item)
            {
                var extensions = element.extensions;
                if (extensions == null)
                {
                    return false;
                }
                else
                {
                    return extensions.Remove(item);
                }
            }

            INotifyEnumerable<ModelElementExtension> IEnumerableExpression<ModelElementExtension>.AsNotifiable()
            {
                return AsNotifiable();
            }

            INotifyEnumerable IEnumerableExpression.AsNotifiable()
            {
                return AsNotifiable();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

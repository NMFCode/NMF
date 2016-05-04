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

namespace NMF.Models
{
    /// <summary>
    /// Defines the base class for a model element implementation
    /// </summary>
    public abstract class ModelElement : IModelElement, INotifyPropertyChanged
    {
        private IModelElement parent;
        private ObservableList<ModelElementExtension> extensions;

        /// <summary>
        /// Gets the model that contains the current model element
        /// </summary>
        public Model Model
        {
            get
            {
                IModelElement current = this;
                while (current != null)
                {
                    var model = current as Model;
                    if (model != null)
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


        /// <summary>
        /// Sets the parent for the current model element to the given element
        /// </summary>
        /// <param name="newParent">The new parent for the given element</param>
        private void SetParent(IModelElement newParent)
        {
            if (newParent != parent)
            {
                var oldParent = parent;
                parent = newParent;

                if (oldParent != null)
                {
                    oldParent.Deleted -= CascadeDelete;

                    if (newParent == null)
                    {
                        var oldModel = oldParent.Model;
                        if (oldModel != null)
                        {
                            if (oldParent != oldModel)
                            {
                                if (EnforceModels)
                                {
                                    oldModel.RootElements.Add(this);
                                }
                            }
                            else
                            {
                                oldModel.RootElements.Remove(this);
                            }
                        }
                    }
                }
                else
                {
                    var newParentME = newParent as ModelElement;
                    if (newParentME != null)
                    {
                        newParentME.OnChildCreated(this);
                    }
                }
                if (newParent != null)
                {
                    if (EnforceModels && newParent.Model == null && oldParent != null)
                    {
                        var oldModel = oldParent.Model;
                        if (oldModel != null)
                        {
                            oldModel.RootElements.Add(newParent);
                        }
                    }
                    newParent.Deleted += CascadeDelete;
                }

                OnParentChanged(newParent, oldParent);
            }
        }

        /// <summary>
        /// Gets called when a new model element is added as a child of the current model element
        /// </summary>
        /// <param name="child">The child element</param>
        /// <remarks>This method is not called if an existing model element is moved in the composition hierarchy</remarks>
        protected virtual void OnChildCreated(IModelElement child)
        {
            OnBubbledChange(new BubbledChangeEventArgs(child));
        }

        /// <summary>
        /// Gets called when the parent element of the current element changes
        /// </summary>
        /// <param name="newParent">The new parent element</param>
        /// <param name="oldParent">The old parent element</param>
        protected virtual void OnParentChanged(IModelElement newParent, IModelElement oldParent) { }

        /// <summary>
        /// Gets or sets the parent element for the current model element
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModelElement Parent
        {
            get
            {
                return parent;
            }
            set
            {
                SetParent(value);
            }
        }


        /// <summary>
        /// Gets a collection with the children of the current model element
        /// </summary>
        [Constant]
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
        /// <returns>A uri (relative or absolute)</returns>
        protected internal virtual Uri CreateUriWithFragment(string fragment, bool absolute)
        {
            var parent = Parent as ModelElement;
            if (parent == null) return null;
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
                result = parent.CreateUriWithFragment(fragment, absolute);
            }
            if (result == null)
            {
                if (IsIdentified)
                {
                    var model = Model;
                    if (model != null)
                    {
                        var newFragment = GetIdentifierFragment(this);

                        if (fragment != null)
                        {
                            newFragment += "/" + fragment;
                        }

                        if (absolute)
                        {
                            result = model.ModelUri != null
                                ? new Uri(model.ModelUri, "#" + newFragment)
                                : null;
                        }
                        else
                        {
                            result = new Uri("#" + newFragment, UriKind.Relative);
                        }
                    }
                }
            }
            return result;
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
            var handler = KeyChanged;
            if (handler != null) handler(this, e);
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
            var childModelElement = child as ModelElement;
            if (childModelElement == null || PreferIdentifiers)
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
                return Uri.EscapeDataString(id) + "/";
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
                    if (child.IsIdentified && child.ToIdentifierString().ToUpperInvariant() == qString) return child;
                }
                return null;
            }
            else if (!qString.StartsWith("@"))
            {
                foreach (var child in Children)
                {
                    if (child.IsIdentified && child.ToIdentifierString().ToUpperInvariant() == qString) return child;
                }
            }
            string reference;
            int index;
            ModelHelper.ParseSegment(segment, out reference, out index);
            return GetModelElementForReference(reference, index);
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
            throw new ArgumentOutOfRangeException("feature");
        }

        /// <summary>
        /// Gets the attribute value for the given attribute
        /// </summary>
        /// <param name="attribute">The attributes name in upper case</param>
        /// <param name="index">The attributes index</param>
        /// <returns>The attribute value</returns>
        protected virtual object GetAttributeValue(string attribute, int index)
        {
            throw new ArgumentOutOfRangeException("attribute");
        }

        /// <summary>
        /// Sets the given feature to the given value
        /// </summary>
        /// <param name="feature">The name of the feature that should be set</param>
        /// <param name="value">The value that should be set</param>
        protected virtual void SetFeature(string feature, object value)
        {
            throw new ArgumentOutOfRangeException("feature");
        }

        /// <summary>
        /// Gets a property expression for the given reference
        /// </summary>
        /// <param name="reference">The name of the requested reference in upper case</param>
        /// <returns>A property expression</returns>
        protected virtual INotifyExpression<IModelElement> GetExpressionForReference(string reference)
        {
            throw new ArgumentOutOfRangeException("reference");
        }

        /// <summary>
        /// Gets a property expression for the given attribute
        /// </summary>
        /// <param name="attribute">The requested attribute in upper case</param>
        /// <returns>A property expression</returns>
        protected virtual INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            throw new ArgumentOutOfRangeException("attribute");
        }


        /// <summary>
        /// Gets a collection of model element extensions that have been applied to this model element
        /// </summary>
        public ICollectionExpression<ModelElementExtension> Extensions
        {
            get
            {
                if (extensions == null)
                {
                    Interlocked.CompareExchange(ref extensions, new ObservableList<ModelElementExtension>(), null);
                }
                return extensions;
            }
        }


        /// <summary>
        /// Gets the extension with the given extension type
        /// </summary>
        /// <typeparam name="T">The model element extension type</typeparam>
        /// <returns>The extension of the given extension type or null, if no such exists</returns>
        public T GetExtension<T>() where T : ModelElementExtension
        {
            if (extensions == null)
            {
                return null;
            }
            else
            {
                return extensions.OfType<T>().FirstOrDefault();
            }
        }


        /// <summary>
        /// Gets a collection of model elements referenced from this element.
        /// </summary>
        public virtual IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return Extensions;
            }
        }


        /// <summary>
        /// Gets called when the PropertyChanged event is fired
        /// </summary>
        /// <param name="propertyName">The name of the changed property</param>
        /// <param name="valueChangedEvent">The original event data</param>
        protected virtual void OnPropertyChanged(string propertyName, ValueChangedEventArgs valueChangedEvent)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            OnBubbledChange(new BubbledChangeEventArgs(this, propertyName, valueChangedEvent));
        }


        /// <summary>
        /// Deletes the current model element
        /// </summary>
        public virtual void Delete()
        {
            OnDeleted(EventArgs.Empty);
        }


        /// <summary>
        /// Gets called when the model element gets deleted
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnDeleted(EventArgs e)
        {
            var handler = Interlocked.Exchange(ref Deleted, null);
            if (handler != null)
            {
                handler(this, e);
            }
            SetParent(null);
        }

        internal void CascadeDelete(object sender, EventArgs e)
        {
            Delete();
        }


        /// <summary>
        /// Gets fired when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Gets fired when the model element gets deleted
        /// </summary>
        public event EventHandler Deleted;


        /// <summary>
        /// Gets the class of the current model element
        /// </summary>
        /// <returns>The class of the current model element</returns>
        public abstract Meta.IClass GetClass();


        /// <summary>
        /// Gets the NMeta class object for this type
        /// </summary>
        public static Meta.IClass ClassInstance
        {
            get
            {
                return MetaRepository.Instance.ResolveClass("http://nmf.codeplex.com/nmeta/#//ModelElement/");
            }
        }


        /// <summary>
        /// Gets the value for the given attribute
        /// </summary>
        /// <param name="attribute">The attribute whose value is queried</param>
        /// <returns>The attribute value</returns>
        public virtual object GetAttributeValue(Meta.IAttribute attribute)
        {
            throw new ArgumentOutOfRangeException("attribute");
        }

        /// <summary>
        /// Gets the referenced element of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="index">The index of the desired model element, if multi-valued reference</param>
        /// <returns>The referenced element for the given reference</returns>
        [ObservableProxy(typeof(ModelElementProxy), "GetReferencedElement")]
        public IModelElement GetReferencedElement(Meta.IReference reference, int index = 0)
        {
            if (reference == null) throw new ArgumentNullException("reference");
            return GetModelElementForReference(reference.Name.ToUpperInvariant(), 0);
        }

        /// <summary>
        /// Sets the referenced element of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="element">The element that should be set</param>
        public void SetReferencedElement(Meta.IReference reference, IModelElement element)
        {
            if (reference == null) throw new ArgumentNullException("reference");
            SetFeature(reference.Name.ToUpperInvariant(), element);
        }

        /// <summary>
        /// Gets the referenced elements of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <returns>A collection of referenced elements</returns>
        public IList GetReferencedElements(Meta.IReference reference)
        {
            throw new ArgumentOutOfRangeException("reference");
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
        protected void OnCollectionChanged(string propertyName, NotifyCollectionChangedEventArgs e)
        {
            OnBubbledChange(new BubbledChangeEventArgs(this, propertyName, e));
        }

        /// <summary>
        /// Fires the BubbledChange event
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnBubbledChange(BubbledChangeEventArgs e)
        {
            var handler = BubbledChange;
            if (handler != null)
            {
                handler(this, e);
            }
            var parent = Parent as ModelElement;
            if (parent != null)
            {
                parent.OnBubbledChange(e);
            }
        }

        /// <summary>
        /// Is fired when an element in the below containment hierarchy has changed
        /// </summary>
        public event EventHandler<BubbledChangeEventArgs> BubbledChange;

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
    }
}

using NMF.Models.Repository;
using NMF.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models.Evolution
{
    [XmlConstructor(2)]
    public abstract class PropertyChangeBase<T> : IModelChange
    {
        [XmlAttribute(true)]
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [XmlAttribute(true)]
        [XmlConstructorParameter(1)]
        public string PropertyName { get; set; }

        public PropertyChangeBase(Uri absoluteUri, string propertyName)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            AbsoluteUri = absoluteUri;
            PropertyName = propertyName;
        }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var newValue = GetNewValue(repository);
            parent?.GetType().GetProperty(PropertyName)?.SetValue(parent, newValue, null);
        }

        public void Undo()
        {
            //TODO
            throw new NotImplementedException();
        }

        protected abstract T GetNewValue(IModelRepository repository);
    }

    [XmlConstructor(2)]
    public class PropertyChangeAttribute<T> : PropertyChangeBase<T>
    {
        public T NewValue { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyChangeAttribute(Uri absoluteUri, string propertyName) : base(absoluteUri, propertyName) { }
        
        public PropertyChangeAttribute(Uri absoluteUri, string propertyName, T newValue)
            : base(absoluteUri, propertyName)
        {
            NewValue = newValue;
        }

        protected override T GetNewValue(IModelRepository repository)
        {
            return NewValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as PropertyChangeAttribute<T>;
            if (other == null)
                return false;
            else
                return Equals(this.AbsoluteUri, other.AbsoluteUri)
                    && Equals(this.PropertyName, other.PropertyName)
                    && Equals(this.NewValue, other.NewValue);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ NewValue?.GetHashCode() ?? 0
                ^ PropertyName?.GetHashCode() ?? 0;
        }
    }

    [XmlConstructor(3)]
    public class PropertyChangeReference<T> : PropertyChangeBase<T> where T : class, IModelElement
    {
        [XmlConstructorParameter(2)]
        [XmlAttribute(true)]
        public Uri ReferenceUri { get; set; }
        
        public PropertyChangeReference(Uri absoluteUri, string propertyName, Uri referenceUri)
            : base(absoluteUri, propertyName)
        {
            ReferenceUri = referenceUri;
        }

        protected override T GetNewValue(IModelRepository repository)
        {
            if (ReferenceUri == null)
                return null;
            return (T)repository.Resolve(ReferenceUri);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as PropertyChangeReference<T>;
            if (other == null)
                return false;
            else
                return Equals(this.AbsoluteUri, other.AbsoluteUri)
                    && Equals(this.PropertyName, other.PropertyName)
                    && Equals(this.ReferenceUri, other.ReferenceUri);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ ReferenceUri?.GetHashCode() ?? 0
                ^ PropertyName?.GetHashCode() ?? 0;
        }
    }
}

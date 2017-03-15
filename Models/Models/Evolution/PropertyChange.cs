using System;
using System.ComponentModel;
using NMF.Models.Repository;
using NMF.Serialization;

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

        public void Invert(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var oldValue = GetOldValue(repository);
            parent?.GetType().GetProperty(PropertyName)?.SetValue(parent, oldValue, null);
        }

        protected abstract T GetNewValue(IModelRepository repository);

        protected abstract T GetOldValue(IModelRepository repository);
    }

    [XmlConstructor(2)]
    public class PropertyChangeAttribute<T> : PropertyChangeBase<T>
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private readonly T _oldValue;
        public T NewValue { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyChangeAttribute(Uri absoluteUri, string propertyName) : base(absoluteUri, propertyName) { }
        
        public PropertyChangeAttribute(Uri absoluteUri, string propertyName, T oldValue, T newValue)
            : base(absoluteUri, propertyName)
        {
            _oldValue = oldValue;
            NewValue = newValue;
        }

        protected override T GetNewValue(IModelRepository repository)
        {
            return NewValue;
        }

        protected override T GetOldValue(IModelRepository repository)
        {
            return _oldValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as PropertyChangeAttribute<T>;
            if (other == null)
                return false;
            return Equals(AbsoluteUri, other.AbsoluteUri)
                   && Equals(PropertyName, other.PropertyName)
                   && Equals(NewValue, other.NewValue);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ NewValue?.GetHashCode() ?? 0
                ^ PropertyName?.GetHashCode() ?? 0;
        }
    }

    [XmlConstructor(4)]
    public class PropertyChangeReference<T> : PropertyChangeBase<T> where T : class, IModelElement
    {
        [XmlConstructorParameter(2)]
        [XmlAttribute(true)]
        public Uri OldUri { get; set; }

        [XmlConstructorParameter(3)]
        [XmlAttribute(true)]
        public Uri ReferenceUri { get; set; }
        
        public PropertyChangeReference(Uri absoluteUri, string propertyName, Uri oldUri, Uri referenceUri)
            : base(absoluteUri, propertyName)
        {
            ReferenceUri = referenceUri;
            OldUri = oldUri;
        }

        protected override T GetNewValue(IModelRepository repository)
        {
            if (ReferenceUri == null)
                return null;
            return (T)repository.Resolve(ReferenceUri);
        }

        protected override T GetOldValue(IModelRepository repository)
        {
            if (OldUri == null)
                return null;
            return (T) repository.Resolve(OldUri);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as PropertyChangeReference<T>;
            if (other == null)
                return false;
            return Equals(AbsoluteUri, other.AbsoluteUri)
                   && Equals(PropertyName, other.PropertyName)
                   && Equals(ReferenceUri, other.ReferenceUri);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ ReferenceUri?.GetHashCode() ?? 0
                ^ PropertyName?.GetHashCode() ?? 0;
        }
    }
}

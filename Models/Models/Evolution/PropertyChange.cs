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
    public class PropertyChange<T> : IModelChange
    {
        [XmlAttribute(true)]
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [XmlAttribute(true)]
        [XmlConstructorParameter(1)]
        public string PropertyName { get; set; }

        public T OldValue { get; set; }

        public T NewValue { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyChange(Uri absoluteUri, string propertyName)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            AbsoluteUri = absoluteUri;
            PropertyName = propertyName;
        }

        public PropertyChange(Uri absoluteUri, string propertyName, T oldValue, T newValue)
            : this(absoluteUri, propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        
        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            parent?.GetType().GetProperty(PropertyName)?.SetValue(parent, NewValue, null);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as PropertyChange<T>;
            if (other == null)
                return false;
            else
                return Equals(this.AbsoluteUri, other.AbsoluteUri)
                    && Equals(this.NewValue, other.NewValue)
                    && Equals(this.OldValue, other.OldValue)
                    && Equals(this.PropertyName, other.PropertyName);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ NewValue?.GetHashCode() ?? 0
                ^ OldValue?.GetHashCode() ?? 0
                ^ PropertyName?.GetHashCode() ?? 0;
        }
    }
}

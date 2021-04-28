using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Diagnostics;

namespace NMF.Serialization
{
    internal abstract class XmlPropertySerializationInfo : IPropertySerializationInfo
    {
        protected XmlPropertySerializationInfo(PropertyInfo property)
        {
            Property = property;
        }

        public PropertyInfo Property { get; private set; }

        public TypeConverter Converter
        {
            get;
            set;
        }

        public bool ShallCreateInstance
        {
            get;
            set;
        }

        public string ElementName
        {
            get;
            set;
        }

        public string Namespace
        {
            get;
            set;
        }

        public string NamespacePrefix
        {
            get;
            set;
        }

        public XmlPropertySerializationInfo Opposite
        {
            get;
            set;
        }

        public abstract void SetDefaultValue(object obj);

        public abstract object GetValue(object obj, XmlSerializationContext context);

        public abstract void SetValue(object obj, object value, XmlSerializationContext context);

        public abstract bool ShouldSerializeValue(object obj, object value);

        public bool IsIdentifier
        {
            get;
            set;
        }

        public XmlIdentificationMode IdentificationMode
        {
            get;
            set;
        }

        public bool IsStringConvertible
        {
            get
            {
                if (Converter != null)
                {
                    return Converter.CanConvertFrom(typeof(string)) && Converter.CanConvertTo(typeof(string));
                }
                else
                {
                    return PropertyType.IsStringConvertible;
                }
            }
        }

        public object ConvertFromString(string text)
        {
            if (Converter != null)
            {
                return Converter.ConvertFromInvariantString(text);
            }
            else
            {
                return PropertyType.ConvertFromString(text);
            }
        }

        public string ConvertToString(object input)
        {
            if (Converter != null)
            {
                return Converter.ConvertToInvariantString(input);
            }
            else
            {
                return PropertyType.ConvertToString(input);
            }
        }

        public void AddToCollection(object input, object item, XmlSerializationContext context)
        {
            if (!context.IsBlocked(input, this))
            {
                try
                {
                    var collection = GetValue(input, context);
                    PropertyType.AddToCollection(collection, item);
                    if (Opposite != null)
                    {
                        context.BlockProperty(item, Opposite);
                    }
                }
                catch (InvalidCastException e)
                {
                    throw new Exception($"The element {item} cannot be added to the property {ElementName} of {input} because the types do not match.", e);
                }
            }
        }

        public abstract bool IsReadOnly { get; }


        public ITypeSerializationInfo PropertyType
        {
            get;
            set;
        }

        IPropertySerializationInfo IPropertySerializationInfo.Opposite
        {
            get
            {
                return Opposite;
            }
        }

        public Type PropertyMinType
        {
            get
            {
                return Property.PropertyType;
            }
        }
    }

    internal class XmlPropertySerializationInfo<TComponent, TProperty> : XmlPropertySerializationInfo
    {
        private readonly Func<TComponent, TProperty> getter;
        private readonly Action<TComponent, TProperty> setter;
        private TProperty defaultValue = default(TProperty);

        public XmlPropertySerializationInfo(PropertyInfo property) : base(property)
        {
            try
            {
                getter = (Func<TComponent, TProperty>)Delegate.CreateDelegate(typeof(Func<TComponent, TProperty>), property.GetGetMethod());
                var setMethod = property.GetSetMethod(false);
                if (setMethod != null)
                {
                    setter = (Action<TComponent, TProperty>)Delegate.CreateDelegate(typeof(Action<TComponent, TProperty>), setMethod);
                }
            }
            catch (ArgumentException ex)
            {
                getter = _ => default(TProperty);
                Debug.WriteLine(ex.Message);
            }
        }

        public override object GetValue(object obj, XmlSerializationContext context)
        {
            return getter((TComponent)obj);
        }

        public override void SetValue(object obj, object value, XmlSerializationContext context)
        {
            if (context.IsBlocked(obj, this))
            {
                return;
            }
            setter((TComponent)obj, (TProperty)value);
            if (Opposite != null)
            {
                context.BlockProperty(value, Opposite);
            }
        }

        public override bool ShouldSerializeValue(object obj, object value)
        {
            if (!PropertyType.IsCollection)
            {
                return !EqualityComparer<TProperty>.Default.Equals((TProperty)value, defaultValue);
            }
            else
            {
                if (value is not IEnumerable collection) return false;
                var enumerator = collection.GetEnumerator();
                var result = enumerator != null && enumerator.MoveNext();
                enumerator.Reset();
                return result;
            }
        }

        public override bool IsReadOnly
        {
            get { return setter == null; }
        }

        public override void SetDefaultValue(object obj)
        {
            try
            {
                defaultValue = (TProperty)obj;
            }
            catch (Exception)
            {
            }
        }
    }

}

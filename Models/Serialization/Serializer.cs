using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes the base class for a serializer
    /// </summary>
    public class Serializer
    {
        private readonly Dictionary<Type, ITypeSerializationInfo> types = new Dictionary<Type, ITypeSerializationInfo>();
        private readonly SerializerTypeCollection typesWrapper;
        private readonly Dictionary<string, Dictionary<string, ITypeSerializationInfo>> typesByQualifier = new Dictionary<string, Dictionary<string, ITypeSerializationInfo>>();

        private static readonly Type genericCollection = typeof(ICollection<>);

        private readonly XmlSerializationSettings settings;

        /// <summary>
        /// Creates a new XmlSerializer with default settings and no preloaded types
        /// </summary>
        public Serializer() : this(XmlSerializationSettings.Default) { }

        /// <summary>
        /// Creates a new XmlSerializer with default settings
        /// </summary>
        /// <param name="additionalTypes">Set of types to preload into the serializer</param>
        /// <remarks>Types will be loaded with default settings</remarks>
        public Serializer(IEnumerable<Type> additionalTypes) : this(XmlSerializationSettings.Default, additionalTypes) { }

        /// <summary>
        /// Creates a new XmlSerializer with the specified settings
        /// </summary>
        /// <param name="settings">Serializer-settings for the serializer. Can be null or Nothing in Visual Basic. In this case, the default settings will be used.</param>
        public Serializer(XmlSerializationSettings settings) : this(settings, null)
        {
        }

        /// <summary>
        /// Creates a new XmlSerializer with the specified settings and the given preloaded types
        /// </summary>
        /// <param name="additionalTypes">Set of types to load into the serializer</param>
        /// <param name="settings">The settings to use for the serializer</param>
        /// <remarks>The types will be loaded with the specified settings</remarks>
        public Serializer(XmlSerializationSettings settings, IEnumerable<Type> additionalTypes) : this(null, settings, additionalTypes)
        {
        }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public Serializer(Serializer parent) : this(parent, null) { }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">New settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public Serializer(Serializer parent, XmlSerializationSettings settings) : this(parent, settings, null)
        {
        }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">New settings</param>
        /// <param name="additionalTypes">Set of types to load into the serializer</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public Serializer(Serializer parent, XmlSerializationSettings settings, IEnumerable<Type> additionalTypes)
        {
            if (settings == null && parent != null)
            {
                this.settings = parent.settings;
            }
            else
            {
                this.settings = (settings ?? XmlSerializationSettings.Default).Fix();
            }
            typesWrapper = new SerializerTypeCollection(this);
            if (parent != null)
            {
                foreach (var typeEntry in parent.types)
                {
                    types.Add(typeEntry.Key, typeEntry.Value);
                }
                foreach (var typeByQualifier in parent.typesByQualifier)
                {
                    typesByQualifier.Add(typeByQualifier.Key, new Dictionary<string, ITypeSerializationInfo>(typeByQualifier.Value));
                }
            }
            if (additionalTypes != null)
            {
                foreach (Type t in additionalTypes)
                {
                    GetSerializationInfo(t, true);
                }
            }
        }


        /// <summary>
        /// Gets the serialization root element
        /// </summary>
        /// <param name="graph">The base element that should be serialized</param>
        /// <param name="fragment">A value indicating whether only a fragment should be written</param>
        /// <returns>The root element for serialization</returns>
        protected virtual object SelectRoot(object graph, bool fragment)
        {
            return graph;
        }

        /// <summary>
        /// Creates a serialization context for the given root element
        /// </summary>
        /// <param name="root">The root element of the serialization</param>
        /// <returns>A serialization context</returns>
        protected virtual XmlSerializationContext CreateSerializationContext(object root)
        {
            return new XmlSerializationContext(root);
        }

        /// <summary>
        /// The settings to be used in the serializer
        /// </summary>
        public XmlSerializationSettings Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// The set of types that are known to the serializer
        /// </summary>
        public ICollection<Type> KnownTypes
        {
            get
            {
                return typesWrapper;
            }
        }

        internal Dictionary<Type, ITypeSerializationInfo> Types
        {
            get { return types; }
        }

        private Queue<Action> initializationQueue;

        private ITypeSerializationInfo AddType(Type type)
        {
            ITypeSerializationInfo info = CreateTypeSerializationInfoFor(type);
            types.Add(type, info);

            CheckCollection(type, info);

            if (initializationQueue != null)
            {
                EnqueueBaseTypes(type, info);
            }
            else
            {
                initializationQueue = new Queue<Action>();
                EnqueueBaseTypes(type, info);

                while (initializationQueue.Count > 0)
                {
                    var initializationAction = initializationQueue.Dequeue();
                    initializationAction();
                }

                initializationQueue = null;
            }

            return info;
        }

        private void EnqueueBaseTypes(Type type, ITypeSerializationInfo info)
        {
            if (type.BaseType != null)
            {
                GetSerializationInfo(type.BaseType, true);
            }
            initializationQueue.Enqueue(() => InitializeTypeSerializationInfo(type, info));
        }

        /// <summary>
        /// Registers the given type serialization info for a namespace lookup
        /// </summary>
        /// <param name="info">the type serialization info</param>
        protected void RegisterNamespace(ITypeSerializationInfo info)
        {
            if (!typesByQualifier.TryGetValue(info.Namespace ?? "", out Dictionary<string, ITypeSerializationInfo> typesOfNamespace))
            {
                typesOfNamespace = new Dictionary<string, ITypeSerializationInfo>();
                if (info.Namespace != null)
                {
                    var ns = info.Namespace;
                    string alternate;
                    if (ns.EndsWith("/"))
                    {
                        alternate = ns.Substring(0, ns.Length - 1);
                    }
                    else
                    {
                        alternate = ns + "/";
                    }
                    typesByQualifier.Add(ns, typesOfNamespace);
                    typesByQualifier.Add(alternate, typesOfNamespace);
                }
                else
                {
                    typesByQualifier.Add("", typesOfNamespace);
                }
            }
            var elName = Settings.CaseSensitive ? info.ElementName : info.ElementName.ToUpperInvariant();
            if (!typesOfNamespace.ContainsKey(elName))
                typesOfNamespace.Add(elName, info);
        }

        /// <summary>
        /// Gets the type serialization info corresponding to the provided pair of namespace and local name
        /// </summary>
        /// <param name="ns">the namespace of the type</param>
        /// <param name="localName">the local name of the type</param>
        /// <returns>the type serialization info</returns>
        public ITypeSerializationInfo GetTypeInfo(string ns, string localName)
        {
            if (typesByQualifier.TryGetValue(ns ?? "", out Dictionary<string, ITypeSerializationInfo> typesOfNs))
            {
                if (!Settings.CaseSensitive) localName = localName.ToUpperInvariant();
                if (typesOfNs.TryGetValue(localName, out ITypeSerializationInfo info))
                {
                    return info;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates the type serialization information for the given type
        /// </summary>
        /// <param name="type">the system type for which the serialization information should be created</param>
        /// <returns>a type serialization info</returns>
        protected virtual ITypeSerializationInfo CreateTypeSerializationInfoFor(Type type)
        {
            return new XmlTypeSerializationInfo(type);
        }

        /// <summary>
        /// Initializes the given type serialization information for the given type
        /// </summary>
        /// <param name="type">the system type</param>
        /// <param name="serializationInfo">the serialization information object</param>
        protected virtual void InitializeTypeSerializationInfo(Type type, ITypeSerializationInfo serializationInfo)
        {
            if (serializationInfo is not XmlTypeSerializationInfo info) throw new NotSupportedException("Cannot initialize other serialization info types");

            InitializeElementName(type, info);
            string identifier = FetchIdentifier(type);
            IPropertySerializationInfo[] constructorInfos = FetchConstructorInfos(type, info);
            List<string> ignoredProperties = FetchIgnoredProperties(type);

            Type constructorType = type;
            if (type.BaseType != null)
            {
                RegisterBaseType(type, info, constructorInfos);
            }
            foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var indexParams = pi.GetIndexParameters();
                if ((indexParams == null || indexParams.Length == 0) && !ignoredProperties.Contains(pi.Name))
                {
                    CreatePropertySerializationInfo(info, identifier, constructorInfos, pi);
                }
            }
            info.Constructor = FindConstructor(constructorInfos, constructorType);
            foreach (object att in type.GetCustomAttributes(typeof(XmlKnownTypeAttribute), false))
            {
                var t = (att as XmlKnownTypeAttribute).Type;
                if (t != null) GetSerializationInfo(t, true);
            }

            RegisterNamespace(info);
        }

        private static ConstructorInfo FindConstructor(IPropertySerializationInfo[] constructorInfos, Type constructorType)
        {
            if (constructorInfos != null)
            {
                Type[] ts = new Type[constructorInfos.GetLength(0)];
                for (int i = 0; i < constructorInfos.GetLength(0); i++)
                {
                    ts[i] = constructorInfos[i] == null ? typeof(object) : constructorInfos[i].PropertyType.MappedType;
                }
                return constructorType.GetConstructor(ts) ?? throw new InvalidOperationException("No suitable constructor found for type " + constructorType.FullName);
            }
            else
            {
                return constructorType.GetConstructor(Type.EmptyTypes);
            }
        }

        private void RegisterBaseType(Type type, XmlTypeSerializationInfo info, IPropertySerializationInfo[] constructorInfos)
        {
            var parentTsi = GetSerializationInfo(type.BaseType, true);
            info.BaseTypes.Add(parentTsi);
            if (!info.IsIdentified && parentTsi.IsIdentified && parentTsi.IdentifierProperty is XmlPropertySerializationInfo identifierProperty)
            {
                info.IdentifierProperty = identifierProperty;
            }
            if (parentTsi.ConstructorProperties != null && constructorInfos != null)
            {
                Array.Copy(parentTsi.ConstructorProperties, constructorInfos, Math.Min(parentTsi.ConstructorProperties.Length, constructorInfos.Length));
            }
        }

        private static List<string> FetchIgnoredProperties(Type type)
        {
            List<string> ignoredProperties = new List<string>();
            foreach (object att in type.GetCustomAttributes(typeof(XmlIgnorePropertyAttribute), false))
            {
                ignoredProperties.Add((att as XmlIgnorePropertyAttribute).Property);
            }

            return ignoredProperties;
        }

        private void InitializeElementName(Type type, XmlTypeSerializationInfo info)
        {
            if (type.IsGenericType)
            {
                var genericTypes = type.GetGenericArguments().Select(t => t.Name).Aggregate((a, b) => a + "-" + b);
                var sanitizedTypeName = type.Name.Substring(0, type.Name.IndexOf('`'));
                info.ElementName = sanitizedTypeName + "_" + genericTypes + "_";
            }
            else
            {
                info.ElementName = Settings.GetPersistanceString(type.Name);
            }
            info.Namespace = Settings.DefaultNamespace;

            foreach (object att in type.GetCustomAttributes(typeof(XmlElementNameAttribute), false))
            {
                info.ElementName = Settings.GetPersistanceString((att as XmlElementNameAttribute).ElementName);
            }

            foreach (object att in type.GetCustomAttributes(typeof(XmlNamespaceAttribute), false))
            {
                info.Namespace = (att as XmlNamespaceAttribute).Namespace;
            }

            foreach (object att in type.GetCustomAttributes(typeof(XmlNamespacePrefixAttribute), false))
            {
                info.NamespacePrefix = (att as XmlNamespacePrefixAttribute).NamespacePrefix;
            }
        }

        private static string FetchIdentifier(Type type)
        {
            string identifier = null;
            foreach (object att in type.GetCustomAttributes(typeof(XmlIdentifierAttribute), false))
            {
                identifier = (att as XmlIdentifierAttribute).Identifier;
            }

            return identifier;
        }

        private static IPropertySerializationInfo[] FetchConstructorInfos(Type type, XmlTypeSerializationInfo info)
        {
            IPropertySerializationInfo[] constructorInfos = null;
            foreach (object att in type.GetCustomAttributes(typeof(XmlConstructorAttribute), false))
            {
                constructorInfos = new XmlPropertySerializationInfo[(att as XmlConstructorAttribute).ParameterCount];
                info.ConstructorProperties = constructorInfos;
            }

            return constructorInfos;
        }

        private void CheckCollection(Type type, ITypeSerializationInfo info)
        {
            if (info is XmlTypeSerializationInfo serializationInfo && typeof(IEnumerable).IsAssignableFrom(type))
            {
                AssignCollectionTypeFromInterfaces(type, serializationInfo);

                if (serializationInfo.CollectionType == null && type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == genericCollection)
                {
                    AssignCollectionTypeFromInterface(serializationInfo, type);
                }

                if (serializationInfo.CollectionType != null)
                {
                    serializationInfo.CreateCollectionAddMethod();
                }
            }
        }

        private void AssignCollectionTypeFromInterfaces(Type type, XmlTypeSerializationInfo serializationInfo)
        {
            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == genericCollection)
                {
                    AssignCollectionTypeFromInterface(serializationInfo, i);
                    break;
                }
            }
        }

        private void AssignCollectionTypeFromInterface(XmlTypeSerializationInfo serializationInfo, Type i)
        {
            Type collType = i.GetGenericArguments()[0];
            serializationInfo.CollectionType = i;
            var converter = TypeConversion.GetTypeConverter(collType);
            if (converter == null || !converter.CanConvertFrom(typeof(string)) || !converter.CanConvertTo(typeof(string)))
            {
                serializationInfo.CollectionItemType = GetSerializationInfo(collType, true);
            }
            else
            {
                serializationInfo.CollectionItemType = new StringConvertibleType(converter, collType);
            }
            serializationInfo.CollectionItemRawType = collType;
        }

        private XmlPropertySerializationInfo CreatePropertySerializationInfo(PropertyInfo pd)
        {
            return Activator.CreateInstance(typeof(XmlPropertySerializationInfo<,>).MakeGenericType(pd.DeclaringType, pd.PropertyType), pd)
                as XmlPropertySerializationInfo;
        }

        private void CreatePropertySerializationInfo(XmlTypeSerializationInfo typeSerializationInfo, string identifier, IPropertySerializationInfo[] constructorInfos, PropertyInfo pd)
        {
            var isId = Settings.TreatAsEqual(pd.Name, identifier);
            var cParam = FetchAttribute<XmlConstructorParameterAttribute>(pd, true);

            if (!typeof(IEnumerable).IsAssignableFrom(pd.PropertyType) && !pd.CanWrite && !isId && cParam != null) return;

            XmlPropertySerializationInfo p = CreatePropertySerializationInfo(pd);

            if (!IsRelevantProperty(pd, p, cParam != null)) return;

            if (isId)
            {
                p.IsIdentifier = true;
                typeSerializationInfo.IdentifierProperty = p;
            }

            InitializePropertyTypeConverter(pd, p);
            InitializePropertyDefaultValue(pd, p);

            RegisterProperty(typeSerializationInfo, constructorInfos, pd, cParam, p);

            AssignPropertyName(pd, p);

            p.IdentificationMode = Fetch(FetchAttribute<XmlIdentificationModeAttribute>(pd, true), att => att.Mode);

            FindOpposite(pd, p);
        }

        private void FindOpposite(PropertyInfo property, XmlPropertySerializationInfo prop)
        {
            // find opposite
            var oppositeAtt = FetchAttribute<XmlOppositeAttribute>(property, true);
            if (oppositeAtt != null)
            {
                var oppositeType = prop.PropertyType;
                if (oppositeAtt.OppositeType != null)
                {
                    oppositeType = GetSerializationInfo(oppositeAtt.OppositeType, true);
                }
                var oppositeProperty = oppositeType.AttributeProperties.OfType<XmlPropertySerializationInfo>().FirstOrDefault(prop => prop.ElementName == oppositeAtt.OppositeProperty);
                if (oppositeProperty == null && oppositeType.IsCollection && oppositeType.CollectionItemType != null)
                {
                    oppositeType = oppositeType.CollectionItemType;
                    oppositeProperty = oppositeType.AttributeProperties.OfType<XmlPropertySerializationInfo>().FirstOrDefault(prop => prop.ElementName == oppositeAtt.OppositeProperty);
                }
                if (oppositeProperty != null)
                {
                    prop.Opposite = oppositeProperty;
                    oppositeProperty.Opposite = prop;
                }
            }
        }

        private void AssignPropertyName(PropertyInfo property, XmlPropertySerializationInfo prop)
        {
            // default settings for element name and namespace
            prop.ElementName = Settings.GetPersistanceString(property.Name);
            prop.Namespace = Settings.DefaultNamespace;
            // override element name settings
            var elementName = Fetch(FetchAttribute<XmlElementNameAttribute>(property, true), att => att.ElementName);
            if (elementName != null) prop.ElementName = Settings.GetPersistanceString(elementName);
            var ns = Fetch(FetchAttribute<XmlNamespaceAttribute>(property, true), att => att.Namespace);
            if (ns != null) prop.Namespace = Settings.GetPersistanceString(ns);
            var nsPrefix = Fetch(FetchAttribute<XmlNamespacePrefixAttribute>(property, true), att => att.NamespacePrefix);
            if (nsPrefix != null) prop.NamespacePrefix = Settings.GetPersistanceString(nsPrefix);
        }

        private static void RegisterProperty(XmlTypeSerializationInfo typeSerializationInfo, IPropertySerializationInfo[] constructorInfos, PropertyInfo property, XmlConstructorParameterAttribute cParam, XmlPropertySerializationInfo prop)
        {
            var defaultAttribute = FetchAttribute<XmlDefaultPropertyAttribute>(property, true);
            //control serialization through an attribute
            if (cParam != null && constructorInfos != null)
            {
                if (defaultAttribute != null && defaultAttribute.IsDefault)
                {
                    throw new InvalidOperationException("Default properties must not be used as constructor parameters.");
                }
                if (cParam.Index >= 0 || cParam.Index < constructorInfos.GetLength(0))
                {
                    constructorInfos[cParam.Index] = prop;
                }
                else
                {
                    RegisterStandardProperty(typeSerializationInfo, property, prop);
                }
            }
            else
            {
                if (defaultAttribute != null && defaultAttribute.IsDefault)
                {
                    RegisterDefaultProperty(typeSerializationInfo, prop);
                }
                else
                {
                    RegisterStandardProperty(typeSerializationInfo, property, prop);
                }
            }
        }

        private static void RegisterDefaultProperty(XmlTypeSerializationInfo typeSerializationInfo, XmlPropertySerializationInfo prop)
        {
            if (typeSerializationInfo.DefaultProperty != null)
            {
                throw new InvalidOperationException("Only one default property allowed for type " + typeSerializationInfo.Type.FullName);
            }
            typeSerializationInfo.DefaultProperty = prop;
        }

        private static void RegisterStandardProperty(XmlTypeSerializationInfo typeSerializationInfo, PropertyInfo property, XmlPropertySerializationInfo prop)
        {
            var asAttribute = FetchAttribute<XmlAttributeAttribute>(property, true);
            if (asAttribute == null || !asAttribute.SerializeAsAttribute)
            {
                typeSerializationInfo.DeclaredElementProperties.Add(prop);
            }
            else
            {
                typeSerializationInfo.DeclaredAttributeProperties.Add(prop);
            }
        }

        private static void InitializePropertyDefaultValue(PropertyInfo property, XmlPropertySerializationInfo prop)
        {
            var defaultValue = Fetch(FetchAttribute<DefaultValueAttribute>(property, true), dva => dva.Value);
            if (defaultValue != null)
            {
                prop.SetDefaultValue(defaultValue);
            }
        }

        private void InitializePropertyTypeConverter(PropertyInfo property, XmlPropertySerializationInfo prop)
        {
            //property might be using its own type converter
            prop.Converter = GetTypeConverter(property);
            if (prop.Converter == null || !prop.Converter.CanConvertFrom(typeof(string)) || !prop.Converter.CanConvertTo(typeof(string)))
            {
                prop.PropertyType = GetSerializationInfo(property.PropertyType, true);
            }
            else
            {
                prop.PropertyType = new StringConvertibleType(prop.Converter, property.PropertyType);
            }
        }

        private static bool IsRelevantProperty(PropertyInfo property, XmlPropertySerializationInfo prop, bool isRequiredForConstructor)
        {
            DesignerSerializationVisibilityAttribute des = FetchAttribute<DesignerSerializationVisibilityAttribute>(property, true);
            if ((des == null || des.Visibility == DesignerSerializationVisibility.Visible) && !prop.IsReadOnly)
            {
                prop.ShallCreateInstance = true;
            }
            else if (des != null && des.Visibility == DesignerSerializationVisibility.Content)
            {
                prop.ShallCreateInstance = false;
            }
            else
            {
                if (!isRequiredForConstructor || (des != null && des.Visibility == DesignerSerializationVisibility.Hidden))
                {
                    return false;
                }
                else
                {
                    prop.ShallCreateInstance = false;
                }
            }

            return true;
        }

        private static TypeConverter GetTypeConverter(PropertyInfo pd)
        {
            var converterType = Fetch(FetchAttribute<XmlTypeConverterAttribute>(pd, true), att => att.Type);
            if (converterType != null)
            {
                return Activator.CreateInstance(converterType) as TypeConverter;
            }
            var converterTypeString = Fetch(FetchAttribute<TypeConverterAttribute>(pd, true), att => att.ConverterTypeName);
            if (converterTypeString != null)
            {
                return Activator.CreateInstance(Type.GetType(converterTypeString)) as TypeConverter;
            }

            return TypeConversion.GetTypeConverter(pd.PropertyType);
        }

        internal static T FetchAttribute<T>(MemberInfo reflectedItem, bool inherit) where T : Attribute
        {
            var results = reflectedItem.GetCustomAttributes(typeof(T), inherit);
            if (results == null || results.Length == 0) return null;
            return results[0] as T;
        }

        internal static TValue Fetch<T, TValue>(T obj, Func<T, TValue> func) where T : class
        {
            if (obj == null) return default;
            return func(obj);
        }

        internal static string CStr(object obj)
        {
            return obj?.ToString();
        }

        /// <summary>
        /// Converts the given string to a value
        /// </summary>
        /// <param name="text">the string that needs to be parsed</param>
        /// <param name="info">the property for which the string is parsed</param>
        /// <param name="context">the context in which the string is parsed</param>
        /// <returns>the parsed object</returns>
        protected object ConvertString(string text, IPropertySerializationInfo info, XmlSerializationContext context)
        {
            try
            {
                return info.ConvertFromString(text);
            }
            catch (Exception ex)
            {
                var eventArgs = new ConverterExceptionEventArgs(text, null, ex, context, info.PropertyType);
                OnConverterException(eventArgs);
                if (eventArgs.Handled)
                {
                    return eventArgs.Value;
                }
                throw;
            }
        }


        /// <summary>
        /// Gets the serialization of the given attribute value
        /// </summary>
        /// <param name="value">The value of the attribute</param>
        /// <param name="info">The serialization information of the type</param>
        /// <param name="isCollection">True, if the value is added to a collection, otherwise false</param>
        /// <param name="context">The serialization context</param>
        /// <returns>The serialized value of the attribute</returns>
        protected virtual string GetAttributeValue(object value, ITypeSerializationInfo info, bool isCollection, XmlSerializationContext context)
        {
            if (info.IsStringConvertible)
            {
                try
                {
                    return info.ConvertToString(value);
                }
                catch (Exception ex)
                {
                    var eventArgs = new ConverterExceptionEventArgs(null, value, ex, context, info);
                    OnConverterException(eventArgs);
                    if (eventArgs.Handled)
                    {
                        return eventArgs.TextValue;
                    }
                    throw;
                }
            }
            else if (info.IsIdentified)
            {
                return CStr(info.IdentifierProperty.GetValue(value, context));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets called when the <see cref="ConverterException"/> event should be raised
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnConverterException(ConverterExceptionEventArgs eventArgs)
        {
            ConverterException?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Handles the case that the type of the element is not known
        /// </summary>
        /// <param name="property">The property for which the type was requested</param>
        /// <param name="ns">The namespace at the current position</param>
        /// <param name="localName">The local name of the type</param>
        /// <returns>The type serialization information received for this type or null, if no type could be resolved</returns>
        protected virtual ITypeSerializationInfo HandleUnknownType(IPropertySerializationInfo property, string ns, string localName)
        {
            var e = new UnknownTypeEventArgs(property, ns, localName);
            OnUnknownType(e);
            if (e.Type != null)
            {
                return e.Type;
            }
            throw new InvalidOperationException($"The type {localName} in namespace {ns} could not be resolved.");
        }

        /// <summary>
        /// Fires the UnknownElement event
        /// </summary>
        /// <param name="e">the event data</param>
        protected virtual void OnUnknownElement(UnknownElementEventArgs e)
        {
            UnknownElement?.Invoke(this, e);
        }

        /// <summary>
        /// Fires the UnknownAttribute event
        /// </summary>
        /// <param name="e">the event data</param>
        protected virtual void OnUnknownAttribute(UnknownAttributeEventArgs e)
        {
            UnknownAttribute?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the UnknownType event
        /// </summary>
        /// <param name="e">the event data</param>
        protected virtual void OnUnknownType(UnknownTypeEventArgs e)
        {
            UnknownType?.Invoke(this, e);
        }

        /// <summary>
        /// Gets raised when the serializer finds an element that is not known
        /// </summary>
        public event EventHandler<UnknownElementEventArgs> UnknownElement;

        /// <summary>
        /// Get raised when the serializer finds an attribute that is not known
        /// </summary>
        public event EventHandler<UnknownAttributeEventArgs> UnknownAttribute;

        /// <summary>
        /// Gets raised when the serializer finds a type that is not known
        /// </summary>
        public event EventHandler<UnknownTypeEventArgs> UnknownType;

        /// <summary>
        /// Gets raised when a converter used by the serializer ran into an exception
        /// </summary>
        public event EventHandler<ConverterExceptionEventArgs> ConverterException;

        /// <summary>
        /// Gets the serialization information for the provided instance
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="createIfNecessary">If true, the serialization information is added if missing</param>
        /// <returns>The type serialization information</returns>
        public virtual ITypeSerializationInfo GetSerializationInfoForInstance(object instance, bool createIfNecessary)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return GetSerializationInfo(instance.GetType(), createIfNecessary);
        }

        /// <summary>
        /// Gets the serialization information for the given type
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="createIfNecessary">If true, the serialization information is added if missing</param>
        /// <returns>The type serialization information</returns>
        public ITypeSerializationInfo GetSerializationInfo(Type type, bool createIfNecessary)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetSerializationInfo(type.GetGenericArguments()[0], createIfNecessary);
            }
            if (type.IsInterface)
            {
                var att = FetchAttribute<XmlDefaultImplementationTypeAttribute>(type, false);
                if (att != null)
                {
                    return GetSerializationInfo(att.DefaultImplementationType, createIfNecessary);
                }
            }
            if (!types.TryGetValue(type, out ITypeSerializationInfo info))
            {
                if (createIfNecessary)
                {
                    info = AddType(type);
                }
                else
                {
                    info = FindLeastCommonTypeInfo(type);
                    types.Add(type, info);
                }
            }
            return info;
        }

        private ITypeSerializationInfo FindLeastCommonTypeInfo(Type type)
        {
            ITypeSerializationInfo info = null;
            foreach (XmlTypeSerializationInfo tmp in types.Values.OfType<XmlTypeSerializationInfo>())
            {
                if (tmp.Type.IsAssignableFrom(type) && (info == null || info.IsAssignableFrom(tmp)))
                {
                    info = tmp;
                }
            }

            return info;
        }

        /// <summary>
        /// Adds a delay to add an item to a property collection
        /// </summary>
        /// <param name="property">the property for which the item is added</param>
        /// <param name="obj">the target object</param>
        /// <param name="text">the original text in the JSON document</param>
        /// <param name="context">the context in which the deserialization is done</param>
        protected static void CreateAddToPropertyDelay(IPropertySerializationInfo property, object obj, string text, XmlSerializationContext context)
        {
            context.LostProperties.Enqueue(new AddToPropertyDelay(property) { Target = obj, Identifier = text });
        }

        /// <summary>
        /// Adds a delay to set a property directly from a resolved text
        /// </summary>
        /// <param name="property">the property that should be set, </param>
        /// <param name="obj">the target object</param>
        /// <param name="text">the original text in the JSON document</param>
        /// <param name="context">the context in which the deserialization is done</param>
        protected static void CreateSetPropertyDelay(IPropertySerializationInfo property, object obj, string text, XmlSerializationContext context)
        {
            context.LostProperties.Enqueue(new SetPropertyDelay() { Identifier = text, Target = obj, Property = property });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Serialization;
using NMF.Serialization.Xmi;
using NMF.Utilities;

namespace NMF.Models.Dynamic.Serialization
{
    /// <summary>
    /// Denotes a serializer that works dynamically
    /// </summary>
    public class DynamicModelSerializer : ModelSerializer
    {
        private readonly Dictionary<IReferenceType, DynamicModelElementSerializationInfo> _serializationInfos = new Dictionary<IReferenceType, DynamicModelElementSerializationInfo>();

        /// <summary>
        /// Creates a dynamic serializer supporting the provided namespaces
        /// </summary>
        /// <param name="namespaces">The namespaces known by this serializer</param>
        public DynamicModelSerializer(params INamespace[] namespaces) : this((IEnumerable<INamespace>)namespaces) { }

        /// <summary>
        /// Creates a dynamic serializer supporting the provided namespaces
        /// </summary>
        /// <param name="namespaces">The namespaces known by this serializer</param>
        public DynamicModelSerializer(IEnumerable<INamespace> namespaces) : base(MetaRepository.Instance.Serializer)
        {
            foreach (var ns in namespaces)
            {
                foreach (var cl in ns.Types.OfType<IClass>())
                {
                    _serializationInfos.Add(cl, new DynamicModelElementSerializationInfo(cl));
                }
            }
            _serializationInfos.Add(ModelElement.ClassInstance, new DynamicModelElementSerializationInfo(ModelElement.ClassInstance));

            foreach (var item in _serializationInfos.Values)
            {
                item.BaseTypes = item.Class.Closure(c => c.BaseTypes).Except(item.Class).Select(baseType => _serializationInfos[baseType]).ToList();
                item.DeclaredAttributeProperties = item.Class.Attributes.Select(CreateAttributeSerializationInfo)
                    .Concat(item.Class.References.Where(r => !r.IsContainment).Select(CreateReferenceSerializationInfo)).ToList();
                item.DeclaredElementProperties = item.Class.References.Where(r => r.IsContainment)
                    .Select(CreateReferenceSerializationInfo).ToList();

                RegisterNamespace(item, item.Namespace);
            }
        }

        /// <inheritdoc />
        public override ITypeSerializationInfo GetSerializationInfoForInstance(object instance, bool createIfNecessary)
        {
            if (instance is DynamicModelElement dynamicElement)
            {
                return _serializationInfos[dynamicElement.GetClass()];
            }
            return base.GetSerializationInfoForInstance(instance, createIfNecessary);
        }

        private IPropertySerializationInfo CreateReferenceSerializationInfo(IReference arg)
        {
            var actualType = arg.ReferenceType ?? ModelElement.ClassInstance;
            var mapping = actualType.GetExtension<MappedType>();
            ITypeSerializationInfo type;
            if (mapping != null)
            {
                type = GetSerializationInfo(mapping.SystemType, true);
            }
            else
            {
                type = _serializationInfos[actualType];
            }
            if (arg.UpperBound != 1)
            {
                type = new CollectionTypeSerializationInfo(type);
            }
            var referenceInfo = new DynamicReferenceSerializationInfo(arg, type);
            if (arg.Opposite != null && _serializationInfos.TryGetValue(arg.Opposite.DeclaringType, out var oppositeType) && oppositeType.DeclaredElementProperties != null)
            {
                var oppositeSerializationInfo = oppositeType.DeclaredElementProperties
                    .Concat(oppositeType.DeclaredAttributeProperties)
                    .OfType<DynamicReferenceSerializationInfo>()
                    .Single(r => r.Reference == arg.Opposite);

                oppositeSerializationInfo.Opposite = referenceInfo;
                referenceInfo.Opposite = oppositeSerializationInfo;
            }
            return referenceInfo;
        }

        private IPropertySerializationInfo CreateAttributeSerializationInfo(IAttribute arg)
        {
            ITypeSerializationInfo type = GetAttributeType(arg);
            if (arg.UpperBound != 1)
            {
                type = new CollectionTypeSerializationInfo(type);
            }
            return new DynamicAttributeSerializationInfo(arg, type);
        }

        private ITypeSerializationInfo GetAttributeType(IAttribute arg)
        {
            var mappedType = arg.Type.GetExtension<MappedType>();
            if (mappedType != null)
            {
                var type = GetSerializationInfo(mappedType.SystemType, true);
                return type;
            }
            if (arg.Type is IEnumeration enumeration)
            {
                return XmiStringSerializationInfo.Instance;
            }
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void HandleUnknownAttribute(XmlReader reader, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (reader.Name == "xsi:type")
            {
                return;
            }
            base.HandleUnknownAttribute(reader, obj, info, context);
        }
    }
}

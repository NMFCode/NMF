using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Serialization;
using NMF.Serialization.Xmi;

namespace NMF.Models.Dynamic.Serialization
{
    public class DynamicModelSerializer : ModelSerializer
    {
        private Dictionary<IReferenceType, DynamicModelElementSerializationInfo> _serializationInfos = new Dictionary<IReferenceType, DynamicModelElementSerializationInfo>();

        public DynamicModelSerializer(params INamespace[] namespaces) : this((IEnumerable<INamespace>)namespaces) { }

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
                item.BaseTypes = item.Class.BaseTypes.Select(baseType => _serializationInfos[baseType]).ToList();
                item.AttributeProperties = item.Class.Attributes.Select(CreateAttributeSerializationInfo)
                    .Concat(item.Class.References.Where(r => !r.IsContainment).Select(CreateReferenceSerializationInfo)).ToList();
                item.ElementProperties = item.Class.References.Where(r => r.IsContainment)
                    .Select(CreateReferenceSerializationInfo).ToList();
                RegisterNamespace(item);
            }
        }

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
            return new DynamicReferenceSerializationInfo(arg, type);
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
    }
}

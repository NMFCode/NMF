using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Models.Tests.Railway;
using NMF.Serialization;
using NMF.Serialization.Xmi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Tests
{
    [TestClass]
    public class CustomSerializerTest
    {
        [TestMethod]
        public void CustomSerializer_Works()
        {
            var serializer = new CustomSerializer(MetaRepository.Instance.Serializer.KnownTypes);
            var repo = new ModelRepository(MetaRepository.Instance, serializer, FileLocator.Instance);

            var model = repo.Resolve("railway.railway");
            repo.Save(model, "railwayWithCustomSerializer.railway");
        }


        /// <summary>
        /// Serialize default values, but no XMI IDs
        /// </summary>
        private class CustomSerializer : ModelSerializer
        {
            public CustomSerializer(IEnumerable<Type> types) : base(new XmlSerializationSettings
            {
                CaseSensitive = true,
                SerializeDefaultValues = true,
            }, types)
            {
            }

            protected override IPropertySerializationInfo IdAttribute => CustomIdAttribute.Instance;
        }

        private class CustomIdAttribute : XmiArtificialIdAttribute, IPropertySerializationInfo
        {
            internal static new readonly CustomIdAttribute Instance = new CustomIdAttribute();

            public override bool ShouldSerializeValue(object obj, object value)
            {
                return false;
            }

            public override object GetValue(object input, XmlSerializationContext context)
            {
                return null;
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Security;
using NMF.Models.Services;
using NMF.Models.Tests.Railway;
using NMF.Serialization;
using System;
using System.IO;

namespace NMF.Models.Tests
{
    [TestClass]
    public class JsonResolveTests
    {
        private static readonly string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestMethod]
        public void Json_RailwayModelRoundtrip_DoesNotChangeSignature()
        {
            var repository = new ModelRepository();
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway");
            var railwayContainer = railwayModel.Model.RootElements[0] as IRailwayContainer;
            Assert.IsNotNull(railwayModel);
            Assert.IsInstanceOfType(railwayContainer, typeof(RailwayContainer));
            var xmlSerializer = MetaRepository.Instance.Serializer as XmlSerializer;
            var jsonSerializer = new JsonModelSerializer(xmlSerializer);

            var hash = Convert.ToBase64String(ModelHasher.CreateHash(railwayContainer));

            jsonSerializer.Serialize(railwayContainer, "railway.json");

            using (var fs = File.OpenRead("railway.json"))
            {
                var deserializedModel = jsonSerializer.Deserialize(fs, new Uri("foo:bar"), repository, false);
                Assert.IsNotNull(deserializedModel);

                var deserializedContainer = deserializedModel.RootElements[0] as IRailwayContainer;
                Assert.IsNotNull(deserializedContainer);

                var deserializedHash = Convert.ToBase64String(ModelHasher.CreateHash(deserializedContainer));

                Assert.AreEqual(hash, deserializedHash);
            }
        }
    }
}

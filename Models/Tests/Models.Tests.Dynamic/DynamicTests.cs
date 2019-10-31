using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Dynamic.Serialization;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Security;

namespace NMF.Models.Tests.Dynamic
{
    [TestClass]
    public class DynamicTests
    {
        [TestMethod]
        public void DynamicDeserialize_DebugModel_Succeeds()
        {
            TestLoadAndSerializeSucceeds("debug");
        }

        [TestMethod]
        public void DynamicDeserialize_RailwayModel_Succeeds()
        {
            TestLoadAndSerializeSucceeds("railway");
        }

        private void TestLoadAndSerializeSucceeds(string baseName)
        {
            var repository = new ModelRepository();
            var metamodel = repository.Resolve($"{baseName}.nmf");
            var serializer = new DynamicModelSerializer(metamodel.RootElements[0] as INamespace);
            var instance = serializer.Deserialize($"{baseName}.{baseName}") as IModelElement;

            var originalHash = Convert.ToBase64String(ModelHasher.CreateHash(instance));

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(instance, ms);
                ms.Position = 0;
                var newInstance = serializer.Deserialize(ms) as IModelElement;
                var newHash = Convert.ToBase64String(ModelHasher.CreateHash(newInstance));

                Assert.AreEqual(originalHash, newHash);
            }
        }
    }
}

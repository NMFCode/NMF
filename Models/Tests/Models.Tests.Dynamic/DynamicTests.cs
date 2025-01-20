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

        [TestMethod]
        public void DynamicDeserializeAndHash_RailwayModel_SameHashcodeAsStronglyTypedModel()
        {
            var baseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

            var repository = new ModelRepository();
            var metamodel = repository.Resolve($"railway.nmf");
            var serializer = new DynamicModelSerializer(metamodel.RootElements[0] as INamespace);
            var instance = serializer.Deserialize($"railway.railway") as IModelElement;
            var model = new Model();
            model.ModelUri = new Uri(baseUri);
            model.RootElements.Add(instance);

            AssertElementHash(model, "#//@invalids.0/@definedBy.24/@elements.5", "iEjxOMgHlUQ4DVraE6OLqtB6YejmpRQBnzUJoYCLyENqTrjX8Grpln2C38v0VGppvwSGcWy2ORQdAdTiEl4Wng==");
            AssertElementHash(model, "#//@invalids.0/@definedBy.24", "JIxRgzBllkPg7Y3sYxRSP8NQ40xwLuRVhfFUNw8tNN7ngOjVT/km6nT0KGhitqX61txCKjmN/469hD14Ewb3TA==");
            AssertElementHash(model, "#//@invalids.0/@follows.1", "trtBI8OljhfVs4oLQ/ZQwsSs5sxxhe4kbx1ABXOEc+PYjj8pWWxwqpysvcJEeq+nPGGvvNUJULNNAm2FMu+ThA==");
            AssertElementHash(model, "#//@invalids.0", "28ycfeifjclgD25XnW/BKpka6ueUbBHZxWW8/E+i4EMf7AtylXqAgHvfIj3V2pQg06mHjfUhKj2XT1TjlSiPLg==");

            var hash = Convert.ToBase64String(ModelHasher.CreateHash(instance));
            Assert.AreEqual("9AveIiH2st2d6elHogWwTPQQXGtPVrtkRcSffb9J+LGtXjJIS+EoKxCiTDKD2QjZpLd/cLX1DIS2lFYYEfZiVg==", hash);
        }

        private void AssertElementHash(Model model, string relativeUri, string expectedHash)
        {
            var element = model.Resolve(new Uri(relativeUri, UriKind.Relative));
            var elementHash = Convert.ToBase64String(ModelHasher.CreateHash(element));
            Assert.AreEqual(expectedHash, elementHash);
        }

        [TestMethod]
        public void DynamicDeserializeAndHash_DebugModel_SameHashcodeAsStronglyTypedModel()
        {
            var baseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

            var repository = new ModelRepository();
            var metamodel = repository.Resolve($"debug.nmf");
            var serializer = new DynamicModelSerializer(metamodel.RootElements[0] as INamespace);
            var instance = serializer.Deserialize($"debug.debug") as IModelElement;
            var model = new Model();
            model.ModelUri = new Uri(baseUri);
            model.RootElements.Add(instance);

            var hash = Convert.ToBase64String(ModelHasher.CreateHash(instance));

            Assert.AreEqual("Mvm7t+x8xMt23q8LYGI1UXtCUjb3toQVYQpzDcn0OZewW77JcTLyWb0X9qt8sgC1660s22VluIADXZ45AKx4Xw==", hash);
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

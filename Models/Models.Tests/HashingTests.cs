using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Security;
using NMF.Models.Tests.Debug;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests
{
    [TestClass]
    public class HashingTests
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestMethod]
        public void Hash_ComputedCorrectly()
        {
            var model = LoadRailwayModel(new ModelRepository());
            var referenceModel = LoadRailwayModel(new ModelRepository());

            var hash1 = Convert.ToBase64String(ModelHasher.CreateHash(model));
            var hash2 = Convert.ToBase64String(ModelHasher.CreateHash(referenceModel));

            Assert.AreEqual(hash1, hash2);
            Assert.AreEqual(hash1, "9AveIiH2st2d6elHogWwTPQQXGtPVrtkRcSffb9J+LGtXjJIS+EoKxCiTDKD2QjZpLd/cLX1DIS2lFYYEfZiVg==");


            AssertElementHash(model.Model, "#//@invalids.0/@definedBy.24/@elements.5", "iEjxOMgHlUQ4DVraE6OLqtB6YejmpRQBnzUJoYCLyENqTrjX8Grpln2C38v0VGppvwSGcWy2ORQdAdTiEl4Wng==");
            AssertElementHash(model.Model, "#//@invalids.0/@definedBy.24", "JIxRgzBllkPg7Y3sYxRSP8NQ40xwLuRVhfFUNw8tNN7ngOjVT/km6nT0KGhitqX61txCKjmN/469hD14Ewb3TA==");
            AssertElementHash(model.Model, "#//@invalids.0/@follows.1", "trtBI8OljhfVs4oLQ/ZQwsSs5sxxhe4kbx1ABXOEc+PYjj8pWWxwqpysvcJEeq+nPGGvvNUJULNNAm2FMu+ThA==");
            AssertElementHash(model.Model, "#//@invalids.0", "28ycfeifjclgD25XnW/BKpka6ueUbBHZxWW8/E+i4EMf7AtylXqAgHvfIj3V2pQg06mHjfUhKj2XT1TjlSiPLg==");
            
            var sw = model.Invalids.OfType<Switch>().FirstOrDefault();
            sw.CurrentPosition = sw.CurrentPosition + 1;

            var hash3 = Convert.ToBase64String(ModelHasher.CreateHash(model));
            Assert.AreNotEqual(hash1, hash3);
        }

        private void AssertElementHash(Model model, string relativeUri, string expectedHash)
        {
            var element = model.Resolve(new Uri(relativeUri, UriKind.Relative));
            var elementHash = Convert.ToBase64String(ModelHasher.CreateHash(element));
            Assert.AreEqual(expectedHash, elementHash);
        }

        private RailwayContainer LoadRailwayModel(ModelRepository repository)
        {
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            var railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
            return railway;
        }

        private Test LoadDebugModel(ModelRepository repository)
        {
            var debugModel = repository.Resolve(new Uri(BaseUri), "debug.debug").Model;
            Assert.IsNotNull(debugModel);
            var debug = debugModel.RootElements.Single() as Test;
            Assert.IsNotNull(debug);
            return debug;
        }

        [TestMethod]
        public void Hash_Debug_ComputedCorrectly()
        {
            var model = LoadDebugModel(new ModelRepository());
            var referenceModel = LoadDebugModel(new ModelRepository());

            var hash1 = Convert.ToBase64String(ModelHasher.CreateHash(model));
            var hash2 = Convert.ToBase64String(ModelHasher.CreateHash(referenceModel));

            Assert.AreEqual(hash1, hash2);
            Assert.AreEqual(hash1, "Mvm7t+x8xMt23q8LYGI1UXtCUjb3toQVYQpzDcn0OZewW77JcTLyWb0X9qt8sgC1660s22VluIADXZ45AKx4Xw==");
        }
    }
}

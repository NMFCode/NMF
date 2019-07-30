using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Security;
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
            Assert.AreEqual(hash1, "xekHQlYh/mVWcd8BkeG5Pm2QHEGOcbz3iRQQWP5gQTO21CGnKNnykPxHjyFV9j8e/Gp4fVbopBhxXpueEoXPTA==");

            var sw = model.Invalids.OfType<Switch>().FirstOrDefault();
            sw.CurrentPosition = sw.CurrentPosition + 1;

            var hash3 = Convert.ToBase64String(ModelHasher.CreateHash(model));
            Assert.AreNotEqual(hash1, hash3);
        }

        private RailwayContainer LoadRailwayModel(ModelRepository repository)
        {
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            var railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
            return railway;
        }
    }
}

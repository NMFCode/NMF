using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests
{
    [TestClass]
    public class ModelCollectionInsertionTests
    {
        private RailwayContainer railway;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestInitialize]
        public void LoadRailwayModel()
        {
            var repository = new ModelRepository(MetaRepository.Instance);
            var railwayModel = repository.Resolve(new Uri(BaseUri), "..\\..\\railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }

        [TestMethod]
        public void ModelCollectionInsertionSucceeds()
        {
            var toInsert = new Route();
            var change = new ModelCollectionInsertion(railway.AbsoluteUri, "Routes", toInsert, 0);

            change.Apply();
            
            Assert.AreEqual(toInsert, railway.Routes.First());
        }
    }
}

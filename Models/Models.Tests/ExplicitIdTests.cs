using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Models.Tests.Railway;

namespace NMF.Models.Tests
{
    [TestClass]
    public class ExplicitIDTests
    {
        private ModelRepository repository;
        private Model railwayModel;
        private RailwayContainer railway;

        private static readonly string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestInitialize]
        public void LoadRailwayModel()
        {
            Model.PromoteSingleRootElement = true;
            repository = new ModelRepository();
            railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }

        [TestMethod]
        public void ExplicitIdsRenderedCorrectly()
        {
            var serializer = new ExplicitIdSerializer();
            using (var ms = new MemoryStream()) 
            {
                serializer.Serialize(railwayModel, ms);
                ms.Position = 0;
                using (var reader = new StreamReader(ms))
                {
                    var result = reader.ReadToEnd();
                    var idPattern = new Regex("xmi:id", RegexOptions.Compiled);
                    var counts = idPattern.Matches(result).Count;
                    Assert.AreEqual(railwayModel.Descendants().Count(), counts);
                }
            } 
        }
    }
}

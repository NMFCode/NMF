using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class TempTests
    {
        private ModelRepository repository;
        private RailwayContainer railway;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestInitialize]
        public void LoadRailwayModel()
        {
            repository = new ModelRepository();
            var railwayModel = repository.Resolve(new Uri(BaseUri), "..\\..\\railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }

        [TestMethod]
        public void DeletedEventBubblesLast()
        {
            var args = new List<BubbledChangeEventArgs>();
            railway.BubbledChange += (obj, e) => args.Add(e);

            railway.Routes[0].Entry.Delete();

            Assert.AreEqual(ChangeType.ModelElementDeleting, args.First().ChangeType);
            Assert.AreEqual(ChangeType.ModelElementDeleted, args.Last().ChangeType);
        }
    }
}

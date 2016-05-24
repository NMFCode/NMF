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
            var allUris = railway.Descendants().Select(e => e.RelativeUri).ToList();

            foreach (var uri in allUris)
            {
                LoadRailwayModel();
                var args = new List<BubbledChangeEventArgs>();
                railway.BubbledChange += (obj, e) => args.Add(e);
                var element = railway.Resolve(uri);
                element.Delete();

                Assert.AreEqual(ChangeType.ModelElementDeleting, args.First().ChangeType, "URI: " + uri.ToString());
                Assert.AreEqual(ChangeType.ModelElementDeleted, args.Last().ChangeType, "URI: " + uri.ToString());
                Assert.AreEqual(args.First().Element.RelativeUri, args.Last().Element.RelativeUri);
            }
        }

        [TestMethod]
        public void SettingParentToNullCausesAllProperEvents()
        {
            var args = new List<BubbledChangeEventArgs>();
            railway.BubbledChange += (obj, e) => args.Add(e);
            railway.Routes[0].DefinedBy[0].Elements[0].Sensor = null;

            CollectionAssert.Contains(args.Select(e => e.ChangeType).ToList(), ChangeType.PropertyChanging);
            CollectionAssert.Contains(args.Select(e => e.ChangeType).ToList(), ChangeType.PropertyChanged);
            CollectionAssert.Contains(args.Select(e => e.ChangeType).ToList(), ChangeType.ModelElementDeleting);
            CollectionAssert.Contains(args.Select(e => e.ChangeType).ToList(), ChangeType.ModelElementDeleted);
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Utilities;

namespace NMF.Models.Tests
{
    [TestClass]
    public class ResolveTests
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

        [TestCleanup]
        public void ResetGlobalSettings()
        {
            Model.PromoteSingleRootElement = true;
        }

        [TestMethod]
        public void ModelUriCanBeResolved()
        {
            Assert.IsNull(railwayModel.RelativeUri);
            Assert.AreEqual(BaseUri, railwayModel.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(railwayModel, railwayModel.Resolve(railwayModel.RelativeUri));
            Assert.AreSame(railwayModel, repository.Resolve(railwayModel.AbsoluteUri));
        }

        [TestMethod]
        public void RootUriCanBeResolved()
        {
            Assert.AreEqual("#//", railway.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//", railway.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(railway, railway.Model.Resolve(railway.RelativeUri));
            Assert.AreSame(railway, repository.Resolve(railway.AbsoluteUri));
        }

        [TestMethod]
        public void RouteCanBeResolved()
        {
            var route = railway.Routes[0];
            Assert.AreEqual("#//@routes.0", route.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//@routes.0", route.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(route, railwayModel.Resolve(route.RelativeUri));
            Assert.AreSame(route, repository.Resolve(route.AbsoluteUri));
        }

        [TestMethod]
        public void SensorCanBeResolved()
        {
            var sensor = railway.Routes[0].DefinedBy[0];
            Assert.AreEqual("#//@routes.0/@definedBy.0", sensor.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//@routes.0/@definedBy.0", sensor.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(sensor, railwayModel.Resolve(sensor.RelativeUri));
            Assert.AreSame(sensor, repository.Resolve(sensor.AbsoluteUri));
        }

        [TestMethod]
        public void RootUriCanBeResolvedIfTwoRootsPresent()
        {
            railwayModel.RootElements.Add(new Segment());
            Assert.AreEqual("#//0", railway.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0", railway.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(railway, railway.Model.Resolve(railway.RelativeUri));
            Assert.AreSame(railway, repository.Resolve(railway.AbsoluteUri));
        }

        [TestMethod]
        public void RouteCanBeResolvedIfTwoRootsPresent()
        {
            railwayModel.RootElements.Add(new Segment());
            var route = railway.Routes[0];
            Assert.AreEqual("#//0/@routes.0", route.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0/@routes.0", route.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(route, railwayModel.Resolve(route.RelativeUri));
            Assert.AreSame(route, repository.Resolve(route.AbsoluteUri));
        }

        [TestMethod]
        public void RootUriCanBeResolvedWhenRootPromotionDisabled()
        {
            Model.PromoteSingleRootElement = false;
            Assert.AreEqual("#//0", railway.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0", railway.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(railway, railway.Model.Resolve(railway.RelativeUri));
            Assert.AreSame(railway, repository.Resolve(railway.AbsoluteUri));
        }

        [TestMethod]
        public void RouteCanBeResolvedWhenRootPromotionDisabled()
        {
            Model.PromoteSingleRootElement = false;
            var route = railway.Routes[0];
            Assert.AreEqual("#//0/@routes.0", route.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0/@routes.0", route.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(route, railwayModel.Resolve(route.RelativeUri));
            Assert.AreSame(route, repository.Resolve(route.AbsoluteUri));
        }

        [TestMethod]
        public void SensorCanBeResolvedWhenRootPromotionDisabled()
        {
            Model.PromoteSingleRootElement = false;
            var sensor = railway.Routes[0].DefinedBy[0];
            Assert.AreEqual("#//0/@routes.0/@definedBy.0", sensor.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0/@routes.0/@definedBy.0", sensor.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(sensor, railwayModel.Resolve(sensor.RelativeUri));
            Assert.AreSame(sensor, repository.Resolve(sensor.AbsoluteUri));
        }

        [TestMethod]
        public void RootUriCanBeResolvedIfTwoRootsPresentWhenRootPromotionDisabled()
        {
            Model.PromoteSingleRootElement = false;
            railwayModel.RootElements.Add(new Segment());
            Assert.AreEqual("#//0", railway.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0", railway.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(railway, railway.Model.Resolve(railway.RelativeUri));
            Assert.AreSame(railway, repository.Resolve(railway.AbsoluteUri));
        }

        [TestMethod]
        public void RouteCanBeResolvedIfTwoRootsPresentWhenRootPromotionDisabled()
        {
            Model.PromoteSingleRootElement = false;
            railwayModel.RootElements.Add(new Segment());
            var route = railway.Routes[0];
            Assert.AreEqual("#//0/@routes.0", route.RelativeUri.ToString());
            Assert.AreEqual(BaseUri + "#//0/@routes.0", route.AbsoluteUri.AbsoluteUri);
            Assert.AreSame(route, railwayModel.Resolve(route.RelativeUri));
            Assert.AreSame(route, repository.Resolve(route.AbsoluteUri));
        }

        [TestMethod]
        public void ToXmlTest()
        {
            var element = railwayModel;
            var serializer = MetaRepository.Instance.Serializer;

            ModelElement.EnforceModels = true;
            
            var stream = new MemoryStream();
            serializer.SerializeFragment(element, stream);

            var switchToUpdate = railwayModel.RootElements.Single().As<RailwayContainer>().Descendants().OfType<ISwitch>().First(sw => sw.Sensor == null);
            switchToUpdate.Sensor = new Sensor() { Id = 0815 };

            stream = new MemoryStream();
            serializer.SerializeFragment(element, stream);
        }

        [TestMethod]
        public void ToXmlTestWhenRootPromotionDisabled()
        {
            Model.PromoteSingleRootElement = false;

            var element = railwayModel;
            var serializer = MetaRepository.Instance.Serializer;

            ModelElement.EnforceModels = true;

            var tempFile = Path.GetTempFileName();
            using (var fs = new FileStream(tempFile, FileMode.Create))
            {
                serializer.SerializeFragment(element, fs);
            }

            AssertFileContentsMatch("RailwayModelWithXmi.xmi", tempFile);

            var test = File.ReadAllText(tempFile);
            
            var switchToUpdate = railwayModel.RootElements.Single().As<RailwayContainer>().Descendants().OfType<ISwitch>().First(sw => sw.Sensor == null);
            switchToUpdate.Sensor = new Sensor() { Id = 0815 };

            var stream = new MemoryStream();
            serializer.SerializeFragment(element, stream);
        }

        private void AssertFileContentsMatch(string path1, string path2)
        {
            var file1Contents = File.ReadAllLines(path1);
            var file2Contents = File.ReadAllLines(path2);

            Assert.AreEqual(file1Contents.Length, file2Contents.Length);
            for (int i = 0; i < file1Contents.Length; i++)
            {
                Assert.AreEqual(file1Contents[i], file2Contents[i], string.Format("Error is in line {0}.", i));
            }
        }

        [TestMethod]
        public void LoadCrashTest()
        {
            var repository = new ModelRepository();
            var rootModelElement = repository.Resolve(new Uri(BaseUri), "railway.railway");

            ModelElement.EnforceModels = true;

            repository = new ModelRepository();
            rootModelElement = repository.Resolve(new Uri(BaseUri), "railway.railway");
        }

    }
}

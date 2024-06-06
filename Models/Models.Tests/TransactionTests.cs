﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Tests.Debug;
using NMF.Models.Tests.Railway;

namespace NMF.Models.Tests
{
    [TestClass]
    public class TransactionTests
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestMethod]
        public void RollbackTest()
        {
            var model = LoadRailwayModel(new ModelRepository());
            var referenceModel = LoadRailwayModel(new ModelRepository());

            using (var trans = new ModelTransaction(model))
            {
                var route = new Route() { Id = 42 };
                model.Routes.Add(route);
                model.Semaphores[0].Signal = Signal.FAILURE;
                model.Routes[0].DefinedBy.RemoveAt(0);
                model.Routes[0].DefinedBy[0].Elements.RemoveAt(0);
            }

            Assert.AreEqual(model.Routes.Count, referenceModel.Routes.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy.Count, referenceModel.Routes[0].DefinedBy.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy[0].Elements.Count, referenceModel.Routes[0].DefinedBy[0].Elements.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy[0].Elements[0].ConnectsTo.Count, referenceModel.Routes[0].DefinedBy[0].Elements[0].ConnectsTo.Count);
            Assert.AreEqual(model.Semaphores[0].Signal, referenceModel.Semaphores[0].Signal);
        }

        [TestMethod]
        public void RestoreReferencesTest()
        {
            var repository = new ModelRepository();
            var model = repository.Resolve("debug.debug").Model;
            Assert.IsNotNull(model);
            var root = model.RootElements.Single() as Test;
            Assert.IsNotNull(root);

            using (var trans = new ModelTransaction(root))
            {
                var aElem = root.Invalids[0] as AClass;
                aElem.Cont1[0].Delete();
                aElem.Cont1[0].Delete();
                root.Invalids[1].Delete();
            }
            Assert.AreEqual((root.Invalids[1] as BClass).Ref1[0], (root.Invalids[0] as AClass).Cont1[0]);
            Assert.AreEqual((root.Invalids[1] as BClass).Ref2[0], (root.Invalids[0] as AClass).Cont1[1]);

            using (var trans = new ModelTransaction(root))
            {
                var aElem = root.Invalids[0] as AClass;
                aElem.Cont1[0].Delete();
                aElem.Cont1[0].Delete();
                root.Invalids[1].Delete();
            }
            Assert.AreEqual((root.Invalids[1] as BClass).Ref1[0], (root.Invalids[0] as AClass).Cont1[0]);
            Assert.AreEqual((root.Invalids[1] as BClass).Ref2[0], (root.Invalids[0] as AClass).Cont1[1]);

            using (var trans = new ModelTransaction(root))
            {
                root.Invalids[1].Delete();
            }
            Assert.AreEqual((root.Invalids[1] as BClass).Ref1[0], (root.Invalids[0] as AClass).Cont1[0]);
            Assert.AreEqual((root.Invalids[1] as BClass).Ref2[0], (root.Invalids[0] as AClass).Cont1[1]);
        }
        

        [TestMethod]
        public void CommitTest()
        {
            var model = LoadRailwayModel(new ModelRepository());
            var referenceModel = LoadRailwayModel(new ModelRepository());

            using (var trans = new ModelTransaction(model))
            {
                var route = new Route() { Id = 42 };
                model.Routes.Add(route);
                model.Routes[0].DefinedBy.RemoveAt(0);
                model.Semaphores[0].Signal = Signal.FAILURE;
                trans.Commit();
            }

            Assert.AreNotEqual(model.Routes.Count, referenceModel.Routes.Count);
            Assert.AreNotEqual(model.Routes[0].DefinedBy.Count, referenceModel.Routes[0].DefinedBy.Count);
            Assert.AreNotEqual(model.Semaphores[0].Signal, referenceModel.Semaphores[0].Signal);
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

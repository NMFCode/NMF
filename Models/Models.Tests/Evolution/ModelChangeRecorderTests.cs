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
    public class ModelChangeRecorderTests
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
        public void PropertyChange()
        {
            var semaphore = railway.Semaphores[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            semaphore.Signal = Signal.FAILURE;

            var expected = new PropertyChange(semaphore.AbsoluteUri, "Signal", Signal.FAILURE, Signal.GO);
            CollectionAssert.Contains(rec.RecordedChanges, expected);
        }

        [TestMethod]
        public void CollectionDeletion()
        {
            var semaphore = railway.Semaphores[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.RemoveAt(0);

            var expected = new CollectionDeletion(railway.AbsoluteUri, "Semaphores", semaphore, 0);
            CollectionAssert.Contains(rec.RecordedChanges, expected);
        }

        [TestMethod]
        public void CollectionInsertion()
        {
            var semaphore = new Semaphore { Signal = Signal.STOP };
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.Insert(0, semaphore);

            var expected = new CollectionInsertion(railway.AbsoluteUri, "Semaphores", semaphore, 0);
            CollectionAssert.Contains(rec.RecordedChanges, expected);
        }
    }
}

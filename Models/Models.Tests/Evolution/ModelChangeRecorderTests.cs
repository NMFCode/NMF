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
        public void RecordPropertyChange()
        {
            var semaphore = railway.Semaphores[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            semaphore.Signal = Signal.FAILURE;

            var expected = new PropertyChange<Signal>(semaphore.AbsoluteUri, "Signal", Signal.GO, Signal.FAILURE);
            var actual = rec.GetModelChanges().Changes[0];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordListDeletion()
        {
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.RemoveAt(0);

            var expected = new ListDeletion(railway.AbsoluteUri, "Semaphores", 0, 1);
            var actual = rec.GetModelChanges().Changes[0];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordListInsertion()
        {
            var semaphore = new Semaphore { Signal = Signal.STOP };
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.Insert(0, semaphore);

            var expected = new List<IModelChange>()
            {
                new ElementCreation(semaphore),
                new ListInsertion<ISemaphore>(railway.AbsoluteUri, "Semaphores", 0, new[] { semaphore })
            };
            var actual = rec.GetModelChanges().Changes;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordElementDeletion()
        {
            var toDelete = railway.Routes[0];
            var expected = new ElementDeletion(toDelete.AbsoluteUri);
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            toDelete.Delete();

            var actual = ((ChangeTransaction)rec.GetModelChanges().Changes[0]).SourceChange;
            Assert.AreEqual(expected, actual);
        }
    }
}
